using Microsoft.AspNetCore.Authorization;
using MSLunches.Data.Enums;
using System;

namespace MSLunches.Api.Authorization
{
    public class AuthorizationPolicies : IAuthorizationPolicies
    {
        public Action<AuthorizationPolicyBuilder> AdminOnly { get; } = builder => builder.RequireRole(AuthorizationRoles.Admin.ToString());
    }
}
