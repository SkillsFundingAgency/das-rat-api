using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Domain.Entities
{
    public class Standard
    {
        public string StandardReference { get; set; }
        public string StandardTitle { get; set; }    
        public int StandardLevel { get; set; }
        public string StandardSector { get; set; }

    }
}
