using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DataDrive.Tests.Helpers
{
    internal static class ControllerExtensions
    {
        public static void Authenticate(this Controller controller, string username, string authenticationType = "authentication")
        {
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = new ClaimsPrincipal(
                        new ClaimsIdentity(
                            new Claim[]
                        {
                            new Claim(ClaimTypes.Name, username)
                        }, authenticationType))
                }
            };
        }
    }
}
