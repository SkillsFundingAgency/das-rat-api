﻿using MediatR;
using SFA.DAS.RequestApprenticeTraining.Application.Models;
using System;
using static SFA.DAS.RequestApprenticeTraining.Domain.Models.Enums;

namespace SFA.DAS.RequestApprenticeTraining.Application.Commands.SubmitEmployerRequest
{
    public class SubmitEmployerRequestCommand : IRequest<SubmitEmployerRequestCommandResponse>
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

        public static implicit operator SubmitEmployerRequestCommand(SubmitEmployerRequestRequest source)
        {
            return new SubmitEmployerRequestCommand
            {
                OriginalLocation = source.OriginalLocation,
                RequestType = source.RequestType,
                StandardReference = source.StandardReference,
                NumberOfApprentices = source.NumberOfApprentices,
                SameLocation = source.SameLocation,
                SingleLocation = source.SingleLocation,
                SingleLocationLatitude = source.SingleLocationLatitude,
                SingleLocationLongitude = source.SingleLocationLongitude,
                MultipleLocations = source.MultipleLocations,
                AtApprenticesWorkplace = source.AtApprenticesWorkplace,
                DayRelease = source.DayRelease,
                BlockRelease = source.BlockRelease,
                RequestedBy = source.RequestedBy,
                ModifiedBy = source.ModifiedBy
            };
        }
    }
}