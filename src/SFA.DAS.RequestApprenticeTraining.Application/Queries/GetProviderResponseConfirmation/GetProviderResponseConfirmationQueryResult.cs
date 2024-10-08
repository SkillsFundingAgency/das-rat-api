﻿using SFA.DAS.RequestApprenticeTraining.Domain.Models;
using System.Collections.Generic;

namespace SFA.DAS.RequestApprenticeTraining.Application.Queries.GetProviderResponseConfirmation
{
    public class GetProviderResponseConfirmationQueryResult
    {
        public long Ukprn { get; set; }
        public List<EmployerRequestReviewModel> EmployerRequests { get; set; } = new List<EmployerRequestReviewModel>();
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
    }
}
