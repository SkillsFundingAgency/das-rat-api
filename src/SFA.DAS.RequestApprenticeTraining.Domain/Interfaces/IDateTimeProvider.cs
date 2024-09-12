﻿using System;

namespace SFA.DAS.RequestApprenticeTraining.Domain.Interfaces
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
    }

    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => DateTime.Now;
    }
}
