using MediatR;
using SFA.DAS.RequestApprenticeTraining.Application.Models;
using System;
using static SFA.DAS.RequestApprenticeTraining.Domain.Models.Enums;

namespace SFA.DAS.RequestApprenticeTraining.Application.Commands.CacheStandard
{
    public class CacheStandardCommand : IRequest<CacheStandardCommandResponse>
    {
        public string StandardReference { get; set; }
        public string StandardTitle { get; set; }
        public int StandardLevel { get; set; }
        public string StandardSector { get; set; }

        public static implicit operator CacheStandardCommand(CacheStandardRequest source)
        {
            return new CacheStandardCommand
            {
                StandardReference = source.StandardReference,
                StandardTitle = source.StandardTitle,
                StandardLevel = source.StandardLevel,
                StandardSector = source.StandardSector
            };
        }
    }
}