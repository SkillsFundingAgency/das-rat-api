using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Domain.Entities
{
    public class Course
    {
        public string Reference { get; set; }
        public string Title { get; set; }    
        public int Level { get; set; }
        public string Sector { get; set; }

    }
}
