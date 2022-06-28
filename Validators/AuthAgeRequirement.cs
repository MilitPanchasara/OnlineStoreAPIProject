using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Validators
{
    public class AuthAgeRequirement : IAuthorizationRequirement
    {
        public int MinimumAge { get; }

        public AuthAgeRequirement(int minAge)
        {
            MinimumAge = minAge;
        }
    }
}