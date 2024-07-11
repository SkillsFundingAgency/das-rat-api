using MediatR;
using System;
using static SFA.DAS.RequestApprenticeTraining.Domain.Models.Enums;

namespace SFA.DAS.RequestApprenticeTraining.Application.Commands.CreateEmployerRequest
{
    public class CreateEmployerRequestCommand : IRequest<CreateEmployerRequestCommandResponse>
    {
        public string OriginalLocation { get; set; }
        public RequestType RequestType { get; set; }
        public long AccountId { get; set; }
        public string StandardReference { get; set; }
        public int NumberOfApprentices { get; set; }
        public string SameLocation { get; set; }
        public string SingleLocation { get; set; }
        public double SingleLocationLatitude { get; set; }
        public double SingleLocationLongitude { get; set; }
        public string[] MultipleLocations { get; set; }
        public bool AtApprenticesWorkplace { get; set; }
        public bool DayRelease { get; set; }
        public bool BlockRelease { get; set; }
        public Guid RequestedBy { get; set; }
        public Guid ModifiedBy { get; set; }
    }
}