using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AuthenticateIMS.Models;

namespace IMS.Controllers
{
    [Authorize]
    public class RequestDetailsController : Controller
    {
        private StockManagementEntities db = new StockManagementEntities();

        // GET: RequestDetails
        public ActionResult Index()
        {
            var request_Details = db.Request_Details.Include(r => r.Approval_Status1).Include(r => r.Employee_Details).Include(r => r.Stock_Details);
            return View(request_Details.ToList());
        }

        // GET: RequestDetails/Details/5
        public ActionResult MyOpenRequestsDetails(string id)
        {
            int check = 1;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var request_Details = db.Request_Details.Include(x => x.Stock_Details).Include(x => x.Employee_Details).Where(x => (x.request_ID == id && x.approval_status == check));

            var content = db.Request_Details.Include(x => x.Employee_Details).Include(x => x.Approval_Status1).Where(x => (x.request_ID == id) && (x.approval_status == check)).FirstOrDefault();
            //var approver = db.Approvals.Include(a => a.Employee_Details).Where(a => a.request_ID == id).FirstOrDefault();

            ViewBag.requestId = content.request_ID;
            ViewBag.firstName = content.Employee_Details.firstname;
            ViewBag.surname = content.Employee_Details.surname;
            ViewBag.position = content.Employee_Details.position;
            ViewBag.site = content.Employee_Details.site;
            ViewBag.dateOfRequest = content.date_of_request;
            ViewBag.status = content.Approval_Status1.status;
            if (content == null)
            {
                return HttpNotFound();
            }
            //if (approver.mine_number == null)
            //{
            //    ViewBag.approverName = "Currently not available";
            //}
            //else
            //{
            //    ViewBag.approverName = approver.Employee_Details.firstname + " " + approver.Employee_Details.surname;
            //}
            return View(request_Details);
        }

        //GET: Open approved requests details
        public ActionResult ApprovedRequestsDetails (string id)
        {
            int check = 2;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var request_Details = db.Request_Details.Include(x => x.Stock_Details).Include(x => x.Employee_Details).Where(x => x.request_ID == id);

            var content = db.Request_Details.Include(x => x.Employee_Details).Where(x => (x.request_ID == id) && (x.approval_status == check)).FirstOrDefault();
            var approver = db.Approvals.Include(a => a.Employee_Details).Where(a => a.request_ID == id).FirstOrDefault();

            ViewBag.requestId = content.request_ID;
            ViewBag.firstName = content.Employee_Details.firstname;
            ViewBag.surname = content.Employee_Details.surname;
            ViewBag.position = content.Employee_Details.position;
            ViewBag.site = content.Employee_Details.site;
            ViewBag.dateOfRequest = content.date_of_request;

            if (content == null)
            {
                return HttpNotFound();
            }
            if (approver.mine_number == null)
            {
                ViewBag.approverName = "Currently not available";
            }
            else
            {
                ViewBag.approverName = approver.Employee_Details.firstname + " " + approver.Employee_Details.surname;
            }
            return View(request_Details);
        }

        //GET: Rejected requests details
        public ActionResult RejectedRequestsDetails (string id)
        {
            int check = 4;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var request_Details = db.Request_Details.Include(x => x.Stock_Details).Include(x => x.Employee_Details).Where(x => x.request_ID == id);

            var content = db.Request_Details.Include(x => x.Employee_Details).Where(x => (x.request_ID == id) && (x.approval_status == check)).FirstOrDefault();
            var approver = db.Approvals.Include(a => a.Employee_Details).Where(a => a.request_ID == id).FirstOrDefault();

            ViewBag.requestId = content.request_ID;
            ViewBag.firstName = content.Employee_Details.firstname;
            ViewBag.surname = content.Employee_Details.surname;
            ViewBag.position = content.Employee_Details.position;
            ViewBag.site = content.Employee_Details.site;
            ViewBag.dateOfRequest = content.date_of_request;

            if (content == null)
            {
                return HttpNotFound();
            }
            if (approver.mine_number == null)
            {
                ViewBag.approverName = "Currently not available";
            }
            else
            {
                ViewBag.approverName = approver.Employee_Details.firstname + " " + approver.Employee_Details.surname;
            }
            return View(request_Details);
        }

        //GET: A users open requests
        public ActionResult MyOpenRequests(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //var number = db.CustomUsers.Where(x => x.email == name).Select(x => x.mine_number).SingleOrDefault();
            var model = db.getMyOpenRequests(id, 1);
            return View(model);
        }

        // GET: All approved requests
        public ActionResult Approved()
        {
            var model = db.getAllApprovedORDeniedRequests(2);
            return View(model);
        }

        //GET: All approved details
        public ActionResult ApprovedDetails(string id)
        {
            if (id == null)
            {
                return View("Approved");
            }
            int check = 2;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var request_Details = db.Request_Details.Include(x => x.Stock_Details).Include(x => x.Employee_Details).Where(x => (x.request_ID == id && x.approval_status == check));

            var content = db.Request_Details.Include(x => x.Employee_Details).Where(x => (x.request_ID == id) && (x.approval_status == check)).FirstOrDefault();
            var approver = db.Approvals.Include(a => a.Employee_Details).Where(a => a.request_ID == id).FirstOrDefault();

            ViewBag.requestId = content.request_ID;
            ViewBag.firstName = content.Employee_Details.firstname;
            ViewBag.surname = content.Employee_Details.surname;
            ViewBag.position = content.Employee_Details.position;
            ViewBag.site = content.Employee_Details.site;
            ViewBag.dateOfRequest = content.date_of_request;

            if (content == null)
            {
                return HttpNotFound();
            }
            if (approver.mine_number == null)
            {
                ViewBag.approverName = "Currently not available";
            }
            else
            {
                ViewBag.approverName = approver.Employee_Details.firstname + " " + approver.Employee_Details.surname;
            }
            return View(request_Details);
        }

        // GET: All rejected requests
        public ActionResult Rejected()
        {
            var model = db.getAllApprovedORDeniedRequests(4);
            return View(model);
        }

        //GET: All open requests
        public ActionResult Open()
        {
            var model = db.getAllApprovedORDeniedRequests(1);
            return View(model);
        }

        //GET: All poen requests details
        public ActionResult OpenDetails(string id)
        {
            int check = 1;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var request_Details = db.Request_Details.Include(x => x.Stock_Details).Include(x => x.Employee_Details).Where(x => (x.request_ID == id && x.approval_status == check));

            var content = db.Request_Details.Include(x => x.Employee_Details).Where(x => (x.request_ID == id) && (x.approval_status == check)).FirstOrDefault();
            var approver = db.Approvals.Include(a => a.Employee_Details).Where(a => a.request_ID == id).FirstOrDefault();

            ViewBag.requestId = content.request_ID;
            ViewBag.firstName = content.Employee_Details.firstname;
            ViewBag.surname = content.Employee_Details.surname;
            ViewBag.position = content.Employee_Details.position;
            ViewBag.site = content.Employee_Details.site;
            ViewBag.dateOfRequest = content.date_of_request;

            if (content == null)
            {
                return HttpNotFound();
            }
            //if (approver.mine_number == null)
            //{
            //    ViewBag.approverName = "Currently not available";
            //}
            //else
            //{
            //    ViewBag.approverName = approver.Employee_Details.firstname + " " + approver.Employee_Details.surname;
            //}
            return View(request_Details);
        }

        // GET: RequestDetails/Create
        public ActionResult Create()
        {
            Random r = new Random();
            int rInt = r.Next(0, 10000);
            string requestId = "REQ" + rInt;
            TempData["Success"] = "Requests made successfully.";

            ViewBag.request_ID = requestId;
            ViewBag.approval_status = new SelectList(db.Approval_Status, "approval_ID", "status");
            ViewBag.mine_number = new SelectList(db.Employee_Details, "mine_number", "surname");
            ViewBag.approver = new SelectList(db.Employee_Details, "mine_number", "surname");
            ViewBag.stock_name = new SelectList(db.Stock_Details, "stock_code", "description_of_items");
            return View();
        }

        public string getNumber(string name)
        {
            var number = db.CustomUsers.Where(x => x.email == name).Select(x => x.mine_number).SingleOrDefault();
            return number;
        }

        public string getCompartment(string id)
        {
            var compartment = db.Stock_Details.Where(x => x.stock_code == id).Select(x => x.compartment_ID).SingleOrDefault();
            return compartment;
        }

        public string getUnit(string id)
        {
            var unit = db.Stock_Details.Where(x => x.stock_code == id).Select(x => x.unit_of_issue).SingleOrDefault();
            return unit;
        }

        // POST: Custom method for creating a request
        public void createRequest(string request_ID, string stock_code, string compartment_ID, string purpose_of_item, string mine_number, string date_of_request, string quant,string unit_of_issue, string status, string approver)
        {
            var stock = db.Stock_Details.Where(x => x.stock_code == stock_code).FirstOrDefault();
            int quantity = Int32.Parse(quant);
            //int minimum = Int32.Parse(stock.minimum_level);
            int difference = stock.quantity_available - stock.minimum_level;
            int approval_status = Int32.Parse(status);

            if(stock.quantity_available > stock.minimum_level && ModelState.IsValid)
            {
                if(quantity < stock.quantity_available && quantity <= difference)
                {
                    db.insertRequest(
                       request_ID,
                       stock_code,
                       compartment_ID,
                       purpose_of_item,
                       mine_number,
                       date_of_request,
                       quantity,
                       unit_of_issue,
                       approval_status,
                       approver
                   );   

                    TempData["Success"] = "Requests made successfully.";
                }
                else
                {
                    ViewData["Error"] = "Error creating requests.";
                }

            }
           
            
        }

        // POST: RequestDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,request_ID,stock_code,compartment_ID,purpose_of_item,mine_number,date_of_request,quantity,approval_status,approver")] Request_Details request_Details)
        {
            if (ModelState.IsValid)
            {
                db.Request_Details.Add(request_Details);
                db.SaveChanges();

                return RedirectToAction("Index");

            }

            ViewBag.approval_status = new SelectList(db.Approval_Status, "approval_ID", "status", request_Details.approval_status);
            ViewBag.mine_number = new SelectList(db.Employee_Details, "mine_number", "surname", request_Details.mine_number);
            ViewBag.approver = new SelectList(db.Employee_Details, "mine_number", "surname", request_Details.approver);
            ViewBag.stock_code = new SelectList(db.Stock_Details, "stock_code", "unit_of_withdraw", request_Details.stock_code);
            return View(request_Details);
        }

        // GET: RequestDetails/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request_Details request_Details = db.Request_Details.Find(id);
            if (request_Details == null)
            {
                return HttpNotFound();
            }
            ViewBag.approval_status = new SelectList(db.Approval_Status, "approval_ID", "status", request_Details.approval_status);
            ViewBag.mine_number = new SelectList(db.Employee_Details, "mine_number", "surname", request_Details.mine_number);
            ViewBag.approver = new SelectList(db.Employee_Details, "mine_number", "surname", request_Details.approver);
            ViewBag.stock_code = new SelectList(db.Stock_Details, "stock_code", "unit_of_withdraw", request_Details.stock_code);
            return View(request_Details);
        }

        // POST: RequestDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,request_ID,stock_code,compartment_ID,purpose_of_item,mine_number,date_of_request,quantity,approval_status,approver")] Request_Details request_Details)
        {
            if (ModelState.IsValid)
            {
                db.Entry(request_Details).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.approval_status = new SelectList(db.Approval_Status, "approval_ID", "status", request_Details.approval_status);
            ViewBag.mine_number = new SelectList(db.Employee_Details, "mine_number", "surname", request_Details.mine_number);
            ViewBag.approver = new SelectList(db.Employee_Details, "mine_number", "surname", request_Details.approver);
            ViewBag.stock_code = new SelectList(db.Stock_Details, "stock_code", "unit_of_withdraw", request_Details.stock_code);
            return View(request_Details);
        }

        // GET: RequestDetails/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request_Details request_Details = db.Request_Details.Find(id);
            if (request_Details == null)
            {
                return HttpNotFound();
            }
            return View(request_Details);
        }

        // POST: RequestDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Request_Details request_Details = db.Request_Details.Find(id);
            db.Request_Details.Remove(request_Details);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
