using AuthenticateIMS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web;
using System.Web.Mvc;
using System.Security.Principal;

namespace AuthenticateIMS.Extensions
{
    public static class RoleExtensions
    {

        static ApplicationDbContext context;
        static IIdentity user = HttpContext.Current.User.Identity;

        static RoleExtensions()
        {
            context = new ApplicationDbContext();
        }

        public static bool IsAdminUser(this ControllerBase controller)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
               
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
                if (s[0].ToString() == "Admin")
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

        public static bool IsSupervisorApprover(this ControllerBase controller)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {

                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
                if (s[0].ToString() == "SRA")
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


        public static bool IsManagerApprover(this ControllerBase controller)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {

                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
                if (s[0].ToString() == "MRA")
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

        public static bool IsNormalUser(this ControllerBase controller)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {

                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
                if (s[0].ToString() == "NU")
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

        public static string PrintRole(this ControllerBase controller)
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