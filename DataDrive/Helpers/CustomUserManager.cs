using DataDrive.DAO.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace DataDrive.Helpers
{
    public class CustomUserManager : UserManager<ApplicationUser>
    {
        public CustomUserManager(IUserStore<ApplicationUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<ApplicationUser> passwordHasher, IEnumerable<IUserValidator<ApplicationUser>> userValidators, IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<ApplicationUser>> logger) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }

        public override string GetUserName(ClaimsPrincipal user)
        {
            string username = base.GetUserName(user);

            if (username == null)
            {
                Claim claim = user.FindFirst(ClaimTypes.NameIdentifier);
                if (claim != null)
                {
                    string nameIdentifier = claim.Value;
                    username = FindByIdAsync(nameIdentifier).Result?.UserName;
                }
            }

            return username;
        }
    }
}
