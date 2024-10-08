﻿using System;
using System.Collections.Generic;

namespace SFA.DAS.RequestApprenticeTraining.Domain.Entities
{
    public class EmployerRequestReviewModel
    {
        public Guid EmployerRequestId { get; set; }
        public string StandardReference { get; set; }
        public string StandardTitle { get; set; }
        public int StandardLevel { get; set; }
        public DateTime DateOfRequest { get; set; }
        public int NumberOfApprentices { get; set; }
        public bool DayRelease { get; set; }
        public bool BlockRelease { get; set; }
        public bool AtApprenticesWorkplace { get; set; }
        public List<string> Locations { get; set; }
    }
}
