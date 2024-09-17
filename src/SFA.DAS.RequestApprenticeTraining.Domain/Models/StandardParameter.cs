using System.Collections.Generic;

namespace SFA.DAS.RequestApprenticeTraining.Domain.Models
{
    public class RefreshStandardsParameters
    {
        public  List<RefreshStandardsParameter> Standards { get; set; }
    }
    
    public class RefreshStandardsParameter
    {
        public string StandardReference { get; set; }
        public string StandardTitle { get; set; }
        public int StandardLevel { get; set; }
        public string StandardSector { get; set; }
    }
}
