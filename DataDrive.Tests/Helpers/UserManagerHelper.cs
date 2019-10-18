using DataDrive.DAO.Models;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace DataDrive.Tests.Helpers
{
    public static class UserManagerHelper
    {
        public static UserManager<ApplicationUser> GetUserManager(string username)
        {
            Mock<IUserStore<ApplicationUser>> store = new Mock<IUserStore<ApplicationUser>>();
            Mock<UserManager<ApplicationUser>> userManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            userManager.Object.UserValidators.Add(new UserValidator<ApplicationUser>());
            userManager.Object.PasswordValidators.Add(new PasswordValidator<ApplicationUser>());

            userManager.Setup(_ => _.GetUserName(It.IsAny<ClaimsPrincipal>()))
                .Returns(username);

            return userManager.Object;




            /*Mock<UserManager<ApplicationUser>> userManager = new Mock<UserManager<ApplicationUser>>();
            userManager.Setup(_ => _.GetUserName(It.IsAny<ClaimsPrincipal>()))
                .Returns(username);

            return userManager.O;*/
        }
    }
}
