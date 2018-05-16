using Microsoft.AspNetCore.Authorization;
using MSLaunches.Data.Enums;
using System;

namespace MSLaunches.Api.Authorization
{
    public class AuthorizationPolicies : IAuthorizationPolicies
    {
        public Action<AuthorizationPolicyBuilder> AdminOnly { get; } = builder => builder.RequireRole(AuthorizationRoles.Admin.ToString());
    }
}
