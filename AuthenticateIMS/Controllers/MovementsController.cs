using System.Data.Entity;
using System.Net;
using System.Web.Mvc;
using AuthenticateIMS.Models;
using System;
using System.Linq;
using AuthenticateIMS.Extensions;
using System.Threading.Tasks;

namespace IMS.Controllers
{
    [Authorize]
    public class MovementsController : Controller
    {
        private StockManagementEntities db = new StockManagementEntities();

        //// GET: Movements
        //public ActionResult Index()
        //{
        //    var movements = db.Movements.Include(m => m.Employee_Details).Include(m => m.Employee_Details1).Include(m => m.Return_Status1).Include(m => m.Request_Details).Include(m => m.Stock_Details).Include(m => m.Type_of_Transaction);
        //    return View(movements.ToList());
        //}

        // GET: Movements/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movement movement = db.Movements.Find(id);
            if (movement == null)
            {
                return HttpNotFound();
            }
            return View(movement);
        }

        // GET: All Reversals
        public ActionResult Reversals()
        {
            var user = User.Identity;

            if (user.IsAdminUser() || user.IsManagerApprover())
            {
                var model = db.getMovements("reversal");
                return View(model);
            }

            return View("UnAuthorizedError");
            
        }

        //GET: All receipts
        public ActionResult Receipts()
        {
            var user = User.Identity;

            if (user.IsAdminUser() || user.IsManagerApprover())
            {
                var model = db.getMovements("receipt");
                return View(model);
            }

            return View("UnAuthorizedError");
            
        }

        //Get: Partial view for create 
        public PartialViewResult _CreateReceipt()
        {
            var user = User.Identity.GetMineNumber();
            var requester = db.Employee_Details.Where(x => x.mine_number == user).SingleOrDefault();
            Random r = new Random();
            int rInt = r.Next(0, 10000);
            string receipt = "RCT" + rInt;

            ViewBag.firstName = requester.firstname;
            ViewBag.lastName = requester.surname;
            ViewBag.position = requester.position;
            ViewBag.site = requester.site;
            ViewBag.department = requester.department;

            ViewBag.receiptId = receipt;
            ViewBag.stock_type = new SelectList(db.Stock_Type, "type_ID", "description");
            ViewBag.compartment = new SelectList(db.Shelf_Compartment, "compartment_ID", "compartment_ID");
            ViewBag.category_ID = new SelectList(db.Stock_Category, "category_ID", "description");
            ViewBag.stock = new SelectList(db.Stock_Details, "stock_code", "description_of_items");

            return PartialView();
        }

        //POST: Create a receipt entry
        public void CreateReceipt(string receipt_ID, string stock_code, string quantity, string units, string acceptor, string compartment, string date_of_receipt, string stock_type, DateTime? expiry_date, string category_ID, string comment)
        {
            int quant = Int32.Parse(quantity);
            DateTime receiptDate = DateTime.Parse(date_of_receipt);


            var stock = db.Stock_Details.Where(x => x.stock_code == stock_code).FirstOrDefault();

            if (ModelState.IsValid)
            {

                Receipt receipt = new Receipt
                {
                    receipt_ID = receipt_ID,
                    stock_code = stock_code,
                    quantity = quant,
                    units = units,
                    acceptor = acceptor,
                    compartment_ID = compartment,
                    date_of_receipt = receiptDate,
                    stock_type = stock_type,
                    expiry_date = expiry_date,
                    category_ID = category_ID,
                    comment = comment
                };

                //Save withdraw to database
                db.Receipts.Add(receipt);
                db.SaveChanges();

                //Increase stock quantity in stock details table
                stock.quantity_available += quant;
                db.Entry(stock).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        // GET: Movements/Create
        public ActionResult Create()
        {
            ViewBag.received_by = new SelectList(db.Employee_Details, "mine_number", "surname");
            ViewBag.issued_by = new SelectList(db.Employee_Details, "mine_number", "surname");
            ViewBag.return_status = new SelectList(db.Return_Status, "status_ID", "status");
            ViewBag.request_ID = new SelectList(db.Request_Details, "request_ID", "stock_code");
            ViewBag.stock_code = new SelectList(db.Stock_Details, "stock_code", "unit_of_withdraw");
            ViewBag.transaction_type_ID = new SelectList(db.Type_of_Transaction, "transaction_type_ID", "type_description");
            return View();
        }

        // POST: Movements/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,movement_ID,stock_code,compartment_ID,request_ID,quantity_supplied,issued_by,received_by,transaction_type_ID,date_received,return_status,expected_return_date,return_date")] Movement movement)
        {
            if (ModelState.IsValid)
            {
                db.Movements.Add(movement);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.received_by = new SelectList(db.Employee_Details, "mine_number", "surname", movement.received_by);
            ViewBag.issued_by = new SelectList(db.Employee_Details, "mine_number", "surname", movement.issued_by);
            ViewBag.return_status = new SelectList(db.Return_Status, "status_ID", "status", movement.return_status);
            ViewBag.request_ID = new SelectList(db.Request_Details, "request_ID", "stock_code", movement.request_ID);
            ViewBag.stock_code = new SelectList(db.Stock_Details, "stock_code", "unit_of_withdraw", movement.stock_code);
            ViewBag.transaction_type_ID = new SelectList(db.Type_of_Transaction, "transaction_type_ID", "type_description", movement.transaction_type_ID);
            return View(movement);
        }

        // GET: Movements/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movement movement = db.Movements.Find(id);
            if (movement == null)
            {
                return HttpNotFound();
            }
            ViewBag.received_by = new SelectList(db.Employee_Details, "mine_number", "surname", movement.received_by);
            ViewBag.issued_by = new SelectList(db.Employee_Details, "mine_number", "surname", movement.issued_by);
            ViewBag.return_status = new SelectList(db.Return_Status, "status_ID", "status", movement.return_status);
            ViewBag.request_ID = new SelectList(db.Request_Details, "request_ID", "stock_code", movement.request_ID);
            ViewBag.stock_code = new SelectList(db.Stock_Details, "stock_code", "unit_of_withdraw", movement.stock_code);
            ViewBag.transaction_type_ID = new SelectList(db.Type_of_Transaction, "transaction_type_ID", "type_description", movement.transaction_type_ID);
            return View(movement);
        }

        // POST: Movements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,movement_ID,stock_code,compartment_ID,request_ID,quantity_supplied,issued_by,received_by,transaction_type_ID,date_received,return_status,expected_return_date,return_date")] Movement movement)
        {
            if (ModelState.IsValid)
            {
                db.Entry(movement).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.received_by = new SelectList(db.Employee_Details, "mine_number", "surname", movement.received_by);
            ViewBag.issued_by = new SelectList(db.Employee_Details, "mine_number", "surname", movement.issued_by);
            ViewBag.return_status = new SelectList(db.Return_Status, "status_ID", "status", movement.return_status);
            ViewBag.request_ID = new SelectList(db.Request_Details, "request_ID", "stock_code", movement.request_ID);
            ViewBag.stock_code = new SelectList(db.Stock_Details, "stock_code", "unit_of_withdraw", movement.stock_code);
            ViewBag.transaction_type_ID = new SelectList(db.Type_of_Transaction, "transaction_type_ID", "type_description", movement.transaction_type_ID);
            return View(movement);
        }

        // GET: Movements/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movement movement = db.Movements.Find(id);
            if (movement == null)
            {
                return HttpNotFound();
            }
            return View(movement);
        }

        //GET: All new stock
        public ActionResult NewStock()
        {
            ////var orderDay = DateTime.Now.AddDays(-7);
            ////var timeSpan = DateTime.Now.Subtract(orderDay);

            var newstock = db.Stock_Withdraw.OrderByDescending(x => x.date_of_withdraw).Include(x => x.Stock_Details).Include(x => x.Employee_Details);
            return View(newstock);
        }

        // POST: Movements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Movement movement = db.Movements.Find(id);
            db.Movements.Remove(movement);
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
