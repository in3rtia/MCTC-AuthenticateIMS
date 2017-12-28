using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AuthenticateIMS.Controllers;
using Newtonsoft.Json;
using AuthenticateIMS.Models;
using AuthenticateIMS.Extensions;

namespace IMS.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private StockManagementEntities _db = new StockManagementEntities();

        public ActionResult Index()
        {
            var model = _db.Stock_Details.ToList();
            var user = User.Identity.GetMineNumber();
            var person = _db.Employee_Details.Where(x => x.mine_number == user).SingleOrDefault();
            ViewBag.UserName = person.firstname + " " + person.surname;
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public JsonResult DataFromDataBase()
        {
            try
            {
                var result = _db.Stock_Details.Select(x => new { quantity = x.quantity_available, description = x.description_of_items }).ToList();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (System.Data.Entity.Core.EntityException)
            {
                return Json(new { Error = "Error" }, JsonRequestBehavior.AllowGet);
            }
            catch (System.Data.SqlClient.SqlException)
            {
                return Json(new { Error = "Error" }, JsonRequestBehavior.AllowGet);

            }
        }
        JsonSerializerSettings _jsonSetting = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };
    }
}