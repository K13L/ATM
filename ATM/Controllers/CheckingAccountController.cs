using ATM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
namespace ATM.Controllers
{
    [Authorize]
    public class CheckingAccountController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: CheckingAccount
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DailyLimit(int checkingAccountId)
        {
            var checkingAccount = db.CheckingAccounts.Find(checkingAccountId);
            ViewBag.DailyLimit = checkingAccount.DailyLimit;
            return PartialView("_DailyLimit");
        }


        // GET: CheckingAccount/Details
        public ActionResult Details()
        {

            var userId = User.Identity.GetUserId();
            var checkingAccountId = db.CheckingAccounts.Where(c => c.ApplicationUserId == userId).First();

            return View(checkingAccountId);
        }

        [Authorize(Roles="Admin")]
        public ActionResult DetailsForAdmin(int Id)
        {

            var checkA = db.CheckingAccounts.Find(Id); ;
            return View("Details", checkA);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult List()
        {
            return View(db.CheckingAccounts.ToList());
        }

        public ActionResult Statement(int checkingAccountId)
        {
            var checkingAccount = db.CheckingAccounts.Find(checkingAccountId);
            return View(checkingAccount.Transactions.ToList());
        }

        // GET: CheckingAccount/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CheckingAccount/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: CheckingAccount/Edit/5
        public ActionResult Edit(int id)
        {
            var checkingAccount = db.CheckingAccounts.Find(id);
            return View(checkingAccount);
        }

        // POST: CheckingAccount/Edit/5
        [HttpPost]
        public ActionResult Edit(int id,  CheckingAccount checkingAccount)
        {
            try
            {
                var check = db.CheckingAccounts.Where(x => x.Id == id).First();
                check.FirstName = checkingAccount.FirstName;
                check.LastName = checkingAccount.LastName;
                check.DailyLimit = checkingAccount.DailyLimit;
                check.Balance = checkingAccount.Balance;
                db.Entry(check).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("List","CheckingAccount");
            }
            catch
            {
               return RedirectToAction("Index", "Home");
                //return View();
            }
        }

        // GET: CheckingAccount/Delete/5
        public ActionResult Delete(int id)
        {
            var checkingAccount = db.CheckingAccounts.Find(id);
            return View(checkingAccount);
        }

        // POST: CheckingAccount/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, CheckingAccount checkingAccount)
        {
            try
            {
                // TODO: Add delete logic here
                var check = db.CheckingAccounts.Where(x=>x.Id == id).First();
                db.CheckingAccounts.Remove(check);
                db.SaveChanges();

                return RedirectToAction("Index","Home");
            }
            catch
            {
                return View();
            }
        }
    }
}
