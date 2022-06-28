using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineStore.Validators
{
    public class AuthAgeHandler : AuthorizationHandler<AuthAgeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthAgeRequirement requirement)
        {
            //var dateOfBirthClaim = context.User.FindFirst(c => c.Type == ClaimTypes.DateOfBirth);
            if(requirement.MinimumAge < 15)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
