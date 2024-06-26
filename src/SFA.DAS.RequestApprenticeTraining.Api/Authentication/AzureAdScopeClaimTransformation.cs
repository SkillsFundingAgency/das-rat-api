﻿using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Api.Authentication
{
    [ExcludeFromCodeCoverage]
    public class AzureAdScopeClaimTransformation : IClaimsTransformation
    {
        public const string ScopeClaimType = "http://schemas.microsoft.com/identity/claims/scope";

        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var scopeClaims = principal.FindAll(ScopeClaimType).ToList();
            if (scopeClaims.Count != 1 || !scopeClaims[0].Value.Contains(' '))
            {
                // Caller has no scopes or has multiple scopes (already split)
                // or they have only one scope
                return Task.FromResult(principal);
            }

            Claim claim = scopeClaims[0];
            string[] scopes = claim.Value.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            IEnumerable<Claim> claims = scopes.Select(s => new Claim(ScopeClaimType, s));

            return Task.FromResult(new ClaimsPrincipal(new ClaimsIdentity(principal.Identity, claims)));
        }
    }
}
