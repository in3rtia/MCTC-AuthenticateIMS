using AuthenticateIMS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace AuthenticateIMS.Extensions
{
    public static class IdentityExtensions
    {

        static ApplicationDbContext context = new ApplicationDbContext();
       // static IIdentity user = HttpContext.Current.User.Identity;

        public static string GetMineNumber(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("MineNumber");
            // Test for null to avoidz issues during local testing
            return (claim != null) ? claim.Value : string.Empty;
        }

        public static bool IsAdminUser(this IIdentity identity)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                IIdentity user = HttpContext.Current.User.Identity;

                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
                if (s[0].ToString() == "System Administrator")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        public static bool IsSupervisorApprover(this IIdentity identity)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                IIdentity user = HttpContext.Current.User.Identity;

                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
                if (s[0].ToString() == "Supervisor to Approve Requests")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        public static bool IsManagerApprover(this IIdentity identity)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                IIdentity user = HttpContext.Current.User.Identity;

                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
                if (s[0].ToString() == "Store Manager to Approve Requests")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        public static bool IsNormalUser(this IIdentity identity)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                IIdentity user = HttpContext.Current.User.Identity;

                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
                if (s[0].ToString() == "Normal User")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        public static string PrintRole(this IIdentity identity)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                IIdentity user = HttpContext.Current.User.Identity;

                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
                string role = s[0].ToString();

                return role;
            }

            return "User is not authenticated";
        }

    }
}