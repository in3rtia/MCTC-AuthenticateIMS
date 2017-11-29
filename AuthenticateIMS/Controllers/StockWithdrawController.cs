using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AuthenticateIMS.Models;

namespace IMS.Controllers
{
    [Authorize]
    public class StockWithdrawController : Controller
    {
        private StockManagementEntities db = new StockManagementEntities();

        // GET: StockWithdraw
        public ActionResult Index()
        {
            var stock_Withdraw = db.Stock_Withdraw.Include(s => s.Employee_Details).Include(s => s.Shelf_Compartment).Include(s => s.Stock_Category).Include(s => s.Stock_Details);
            return View(stock_Withdraw.ToList());
        }

        // GET: StockWithdraw/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stock_Withdraw stock_Withdraw = db.Stock_Withdraw.Find(id);
            if (stock_Withdraw == null)
            {
                return HttpNotFound();
            }
            return View(stock_Withdraw);
        }

        //GET: Stock category of specific stock
        public string getCategory(string id)
        {
            var category = db.Stock_Details.Where(x => x.stock_code == id).Select(x => x.category_ID).SingleOrDefault();
            return category;
        }

        //GET: Stock type
        public string getStockType(string id)
        {
            var type = db.Stock_Details.Where(x => x.stock_code == id).Select(x => x.stock_type).SingleOrDefault();
            return type;
        }

        // GET: StockWithdraw/Create
        public ActionResult Create()
        {
            Random r = new Random();
            int rInt = r.Next(0, 10000);
            string withdrawId = "WTD" + rInt;

            ViewBag.withdrawId = withdrawId;
            ViewBag.stock_type = new SelectList(db.Stock_Type, "type_ID", "description");
            ViewBag.compartment_ID = new SelectList(db.Shelf_Compartment, "compartment_ID", "compartment_ID");
            ViewBag.category_ID = new SelectList(db.Stock_Category, "category_ID", "description");
            ViewBag.stock_code = new SelectList(db.Stock_Details, "stock_code", "description_of_items");
            return View();
        }

        // POST: StockWithdraw/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public void CreateWithdraw(string withdraw_ID,string stock_code,string quantity,string unit_of_withdraw,string withdrawer, string compartment_ID, string date_of_withdraw, string stock_type, DateTime expiry_date, string category_ID, string comment)
        {
            int quant = Int32.Parse(quantity);
            DateTime withdrawDate = DateTime.Parse(date_of_withdraw);

            var stock = db.Stock_Details.Where(x => x.stock_code == stock_code).FirstOrDefault();
   
            if (ModelState.IsValid)
            {
                Stock_Withdraw withdraw = new Stock_Withdraw
                {
                    withdraw_ID = withdraw_ID,
                    stock_code = stock_code,
                    quantity = quant,
                    unit_of_withdraw = unit_of_withdraw,
                    withdrawer = withdrawer,
                    compartment_ID = compartment_ID,
                    date_of_withdraw = withdrawDate,
                    stock_type = stock_type,
                    expiry_date = expiry_date,
                    category_ID = category_ID,
                    comment = comment
                };

                //Save withdraw to database
                db.Stock_Withdraw.Add(withdraw);
                db.SaveChanges();

                //Increase stock quantity in stock details table
                stock.quantity_available += quant;
                db.Entry(stock).State = EntityState.Modified;
                db.SaveChanges();
            }
      
        }

        // GET: StockWithdraw/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stock_Withdraw stock_Withdraw = db.Stock_Withdraw.Find(id);
            if (stock_Withdraw == null)
            {
                return HttpNotFound();
            }
            ViewBag.withdrawer = new SelectList(db.Employee_Details, "mine_number", "surname", stock_Withdraw.withdrawer);
            ViewBag.compartment_ID = new SelectList(db.Shelf_Compartment, "compartment_ID", "shelf_ID", stock_Withdraw.compartment_ID);
            ViewBag.category_ID = new SelectList(db.Stock_Category, "category_ID", "description", stock_Withdraw.category_ID);
            ViewBag.stock_code = new SelectList(db.Stock_Details, "stock_code", "unit_of_withdraw", stock_Withdraw.stock_code);
            return View(stock_Withdraw);
        }

        // POST: StockWithdraw/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,withdraw_ID,stock_code,quantity,unit_of_withdraw,withdrawer,date_of_withdraw,compartment_ID,stock_type,expiry_date,category_ID")] Stock_Withdraw stock_Withdraw)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stock_Withdraw).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.withdrawer = new SelectList(db.Employee_Details, "mine_number", "surname", stock_Withdraw.withdrawer);
            ViewBag.compartment_ID = new SelectList(db.Shelf_Compartment, "compartment_ID", "shelf_ID", stock_Withdraw.compartment_ID);
            ViewBag.category_ID = new SelectList(db.Stock_Category, "category_ID", "description", stock_Withdraw.category_ID);
            ViewBag.stock_code = new SelectList(db.Stock_Details, "stock_code", "unit_of_withdraw", stock_Withdraw.stock_code);
            return View(stock_Withdraw);
        }

        // GET: StockWithdraw/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stock_Withdraw stock_Withdraw = db.Stock_Withdraw.Find(id);
            if (stock_Withdraw == null)
            {
                return HttpNotFound();
            }
            return View(stock_Withdraw);
        }

        // POST: StockWithdraw/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Stock_Withdraw stock_Withdraw = db.Stock_Withdraw.Find(id);
            db.Stock_Withdraw.Remove(stock_Withdraw);
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
