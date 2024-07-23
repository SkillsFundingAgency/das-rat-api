﻿using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;

namespace SFA.DAS.RequestApprenticeTraining.Domain.Entities
{
    public class SelectEmployerRequest
    {

        public Guid EmployerRequestId { get; set; }
        public string StandardReference { get; set; }
        public string StandardTitle { get; set; }
        public int StandardLevel { get; set; }
        public string SingleLocation { get; set; }
        public DateTime DateOfRequest { get; set; }
        public int NumberOfApprentices { get; set; }
        public bool DayRelease { get; set; }
        public bool BlockRelease { get; set; }
        public bool AtApprenticesWorkplace { get; set; }
        public bool IsNew { get; set; }
        public bool IsContacted { get; set; }
        public List<string> Locations { get; set; }
    }
}
