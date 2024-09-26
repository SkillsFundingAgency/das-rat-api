using MediatR;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;
using SFA.DAS.RequestApprenticeTraining.Domain.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Application.Commands.RefreshStandards
{
    public class RefreshStandardsCommandHandler : IRequestHandler<RefreshStandardsCommand, Unit>
    {
        private readonly IStandardEntityContext _standardEntityContext;

        public RefreshStandardsCommandHandler(IStandardEntityContext standardEntityContext)
        {
            _standardEntityContext = standardEntityContext;
        }
        public async Task<Unit> Handle(RefreshStandardsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var standardsInDb = await _standardEntityContext.GetAll();

                var newStandards = request.Standards
                    .Where(apiItem => !standardsInDb.Any(dbItem => dbItem.StandardReference == apiItem.StandardReference))
                    .Select(standard => new Standard
                    {
                        StandardReference = standard.StandardReference,
                        StandardTitle = standard.StandardTitle,
                        StandardLevel = standard.StandardLevel,
                        StandardSector = standard.StandardSector,
                    }).ToList();

                await _standardEntityContext.AddRange(newStandards);

                var updatedItems = request.Standards
                    .Where(apiItem => standardsInDb.Any(dbItem => dbItem.StandardReference == apiItem.StandardReference &&
                        (dbItem.StandardTitle != apiItem.StandardTitle ||
                        dbItem.StandardLevel != apiItem.StandardLevel ||
                        dbItem.StandardSector != apiItem.StandardSector)))
                    .ToList();

                foreach (var standard in updatedItems)
                {
                    var toUpdate = standardsInDb.FirstOrDefault(dbItem => dbItem.StandardReference == standard.StandardReference);

                    if (toUpdate != null)
                    {
                        toUpdate.StandardTitle = standard.StandardTitle;
                        toUpdate.StandardLevel = standard.StandardLevel;
                        toUpdate.StandardSector = standard.StandardSector;
                    }
                }

                await _standardEntityContext.SaveChangesAsync();
            }
            catch (Exception ex)
            { 
                Console.WriteLine(ex.StackTrace);
            }

            return Unit.Value;
        }
    }
}