using MediatR;
using SFA.DAS.RequestApprenticeTraining.Application.Models;
using System;
using static SFA.DAS.RequestApprenticeTraining.Domain.Models.Enums;

namespace SFA.DAS.RequestApprenticeTraining.Application.Commands.PostStandard
{
    public class PostStandardCommand : IRequest<PostStandardCommandResponse>
    {
        public string StandardReference { get; set; }
        public string StandardTitle { get; set; }
        public int StandardLevel { get; set; }
        public string StandardSector { get; set; }

        public static implicit operator PostStandardCommand(PostStandardRequest source)
        {
            return new PostStandardCommand
            {
                StandardReference = source.StandardReference,
                StandardTitle = source.StandardTitle,
                StandardLevel = source.StandardLevel,
                StandardSector = source.StandardSector
            };
        }
    }
}