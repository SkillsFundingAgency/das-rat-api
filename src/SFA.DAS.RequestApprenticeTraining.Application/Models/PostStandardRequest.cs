using System;
namespace SFA.DAS.RequestApprenticeTraining.Application.Models
{
    public class PostStandardRequest
    {
        public string StandardReference { get; set; }
        public string StandardTitle { get; set; }
        public int StandardLevel { get; set; }
        public string StandardSector { get; set; }
    }
}
