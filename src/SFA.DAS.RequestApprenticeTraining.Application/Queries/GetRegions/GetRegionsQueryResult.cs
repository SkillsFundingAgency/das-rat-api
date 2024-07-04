using System.Collections.Generic;
using SFA.DAS.RequestApprenticeTraining.Domain.Models;

namespace SFA.DAS.RequestApprenticeTraining.Application.Queries.GetRegions
{
    public class GetRegionsQueryResult
    {
        public List<Region> Regions { get; set; }
    }
}
