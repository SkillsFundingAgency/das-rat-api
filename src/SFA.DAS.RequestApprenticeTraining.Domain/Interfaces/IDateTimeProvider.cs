using System;

namespace SFA.DAS.RequestApprenticeTraining.Domain.Interfaces
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
    }

    public class UtcDateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => DateTime.UtcNow;
    }

    public class SpecifiedDateTimeProvider : IDateTimeProvider
    {
        public DateTime Now { get; set; }

        public SpecifiedDateTimeProvider(DateTime time)
        {
            Now = time;
        }

        public void Advance(TimeSpan timeSpan)
        {
            Now = Now.Add(timeSpan);
        }
    }
}
