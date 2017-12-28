using System.Web.Mvc;
using AuthenticateIMS.Models;
using System.Linq;
using System;
using AuthenticateIMS.Extensions;
using System.Data.Entity;
using System.Net;

namespace IMS.Controllers
{
    [Authorize]
    public class IssuesController : Controller
    {

        StockManagementEntities db;

        public IssuesController()
        {
            db = new StockManagementEntities();

        }
        // GET: Issues
        public ActionResult Index()
        {
            var user = User.Identity;
            
            if (!user.IsNormalUser())
            {
                var model = db.getAllIssues();
                return View(model);
            }
           
            return View("UnAuthorizedError");
            
        }

        
        [HttpPost]
        public void CreateIssue(int id)
        {
            Random r = new Random();
            int rInt = r.Next(0, 10000);
            int returnStatus = 4;
            int workflowId = 8;

            var request = db.Request_Details.Include(x => x.Stock_Details).Where(x => x.id == id).SingleOrDefault();
           // var request_Details = db.Request_Details.Include(x => x.Stock_Details).Include(x => x.Employee_Details).Where(x => (x.request_ID == id && x.approval_status == 2));

            string movement_ID = "ISS" + rInt;
            string request_ID = request.request_ID;
            string stock_code = request.stock_code;
            string compartmentId = request.compartment_ID;
            int quantity = request.quantity;
            string issuer = User.Identity.GetMineNumber();
            string recipient = request.mine_number;
            //string type = "issue";
            DateTime date = DateTime.Now;
            //DateTime? return_date = null;

            if (request.Stock_Details.stock_type == "Retn" && ModelState.IsValid)
            {
                returnStatus = 0;
                DateTime? expected = request.expected_date;

                db.insertIssue(
                    movement_ID,
                    stock_code,
                    compartmentId,
                    request_ID,
                    quantity,
                    issuer,
                    recipient,
                    date,
                    returnStatus,
                    expected
                );

            }
            else if(request.Stock_Details.stock_type == "Cons" && ModelState.IsValid)
            {
                DateTime? expected = null;

                db.insertIssue(
                   movement_ID,
                   stock_code,
                   compartmentId,
                   request_ID,
                   quantity,
                   issuer,
                   recipient,
                   date,
                   returnStatus,
                   expected
               );

            }

            //Change approval status to issued
            request.approval_status = 3;
            request.workflow_ID = workflowId;
            db.Entry(request).State = EntityState.Modified;
            db.SaveChanges();

        }

        public PartialViewResult _ConfirmReverse(string id)
        {
            var reverse = db.Movements.Where(x => x.movement_ID == id).SingleOrDefault();
            return PartialView(reverse);
        }

        public ActionResult UserCollections(string mineNumber)
        {
            var user = User.Identity;

            if (user.IsNormalUser() || user.IsSupervisorApprover())
            {
                if (mineNumber == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var model = db.getUserCollections(mineNumber);
                return View(model);
            }

            return View("UnAuthorizedError");
        }

        public ActionResult Details(string id)
        {
            var user = User.Identity;
            var today = Convert.ToDateTime(DateTime.Today).ToString("MMMM dd, yyyy");

            
                string check = "issue";
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var issues = db.Movements.Include(x => x.Stock_Details).Where(x => (x.request_ID == id && x.transaction_type_ID == check));

                var content = db.Movements.Include(x => x.Stock_Details).Include(x => x.Employee_Details).Include(x => x.Employee_Details1).Include(x => x.Return_Status1).Where(x => (x.request_ID == id && x.transaction_type_ID == check)).FirstOrDefault();

                ViewBag.requestId = content.request_ID;
                ViewBag.firstName = content.Employee_Details.firstname;
                ViewBag.surname = content.Employee_Details.surname;
                ViewBag.position = content.Employee_Details.position;
                ViewBag.site = content.Employee_Details.site;
                ViewBag.issuer = content.Employee_Details1.firstname + " " + content.Employee_Details1.surname;
                ViewBag.dateToday = today;

                if (content == null)
                {
                    return HttpNotFound();
                }
               
                return View(issues);
            

            //return View("UnAuthorizedError");

        }

        //POST: To reverse an issue
        //[HttpPost]
        public ActionResult ReverseIssue(string id, string reqComment)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            string check = "issue";
            var issue = db.Movements.Where(x => (x.movement_ID == id && x.transaction_type_ID == check)).FirstOrDefault();
            var request = db.Request_Details.Where(x => (x.request_ID == issue.request_ID && x.stock_code == issue.stock_code)).FirstOrDefault();
            var stock = db.Stock_Details.Where(x => x.stock_code == issue.stock_code).FirstOrDefault();

            if(issue == null || request == null || stock == null)
            {
                return HttpNotFound();
            }
            //Change transaction type to reversal
            issue.transaction_type_ID = "reversal";
            db.Entry(issue).State = EntityState.Modified;
            db.SaveChanges();

            ////Change approval status in request details table
            request.approval_status = 0;
            request.comment = reqComment;
            db.Entry(request).State = EntityState.Modified;
            db.SaveChanges();

            //Change quantity of stock
            stock.quantity_available += issue.quantity_supplied;
            db.Entry(stock).State = EntityState.Modified;


            return View("Index");
        }

        //Post: Return an item
        public void ReturnItem(string id, string returnComment)
        {
            var stock = db.Movements.Find(id);
            DateTime today = DateTime.Today;

            if(today > stock.expected_return_date)
            {
                stock.return_status = 2;
                stock.comment = returnComment;
                db.Entry(stock).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                stock.return_status = 1;
                stock.comment = returnComment;
                db.Entry(stock).State = EntityState.Modified;
                db.SaveChanges();
            }
           
        }
    } 

}
