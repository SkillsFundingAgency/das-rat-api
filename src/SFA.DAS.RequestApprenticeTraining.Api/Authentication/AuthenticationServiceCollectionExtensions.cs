using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.RequestApprenticeTraining.Domain.Configuration;
using System.Collections.Generic;

namespace SFA.DAS.RequestApprenticeTraining.Api.Authentication
{
    public static class AuthenticationServiceCollectionExtensions
    {
        public static IServiceCollection AddApiAuthentication(this IServiceCollection services, ApplicationSettings applicationSettings, bool isDevelopment)
        {
            if (!isDevelopment)
            {
                services.AddAuthentication(auth =>
                {
                    auth.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(auth =>
                {
                    auth.Authority =
                        $"https://login.microsoftonline.com/{applicationSettings.AzureAd.Tenant}";
                    auth.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidAudiences = new List<string>
                        {
                            applicationSettings.AzureAd.Audiences
                        }
                    };
                });
                services.AddSingleton<IClaimsTransformation, AzureAdScopeClaimTransformation>();
            }

            return services;
        }
    }
}
