using System.Collections.Generic;

namespace SFA.DAS.RequestApprenticeTraining.Domain.Configuration
{
    public class ApplicationSettings
    {
        public AzureActiveDirectoryApiConfiguration AzureAd { get; set; }

        public string DbConnectionString { get; set; }
        public string NServiceBusConnectionString { get; set; }
        public string NServiceBusLicense { get; set; }

        public int ExpiryAfterMonths { get; set; }
        public int RemovedAfterExpiryNotContactedMonths { get; set; }
        public int RemovedAfterExpiryContactedMonths { get; set; }

        /// <summary>
        /// The GOV.UK Notify email template configuration
        /// </summary>
        public List<NotificationTemplate> NotificationTemplates { get; set; }
    }
}
