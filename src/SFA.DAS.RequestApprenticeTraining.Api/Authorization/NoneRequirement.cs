using Microsoft.AspNetCore.Authorization;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.RequestApprenticeTraining.Api.Authorization
{
    [ExcludeFromCodeCoverage]
    public class NoneRequirement : IAuthorizationRequirement
    {
    }
}
