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
        public int EmployerRemovedAfterExpiryNoResponsesMonths { get; set; }
        public int EmployerRemovedAfterExpiryResponsesMonths { get; set; }
        public int ProviderRemovedAfterExpiryNotRespondedMonths { get; set; }
        public int ProviderRemovedAfterExpiryRespondedMonths { get; set; }

        /// <summary>
        /// The GOV.UK Notify email template configuration
        /// </summary>
        public List<NotificationTemplate> NotificationTemplates { get; set; }
    }
}
