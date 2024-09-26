using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.RequestApprenticeTraining.Domain.Models
{
    public class Standard
    { 
        public string StandardReference { get; set; }
        public string StandardTitle { get; set; }
        public int StandardLevel { get; set; }
        public string StandardSector { get; set; }

        public static implicit operator Standard(Entities.Standard source)
        {
            if (source == null)
            {
                return null;
            }

            return new Standard
            {
                StandardReference = source.StandardReference,
                StandardTitle = source.StandardTitle,
                StandardLevel = source.StandardLevel,
                StandardSector = source.StandardSector,
            };
        }
    }
}
