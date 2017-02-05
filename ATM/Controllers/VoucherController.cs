using ATM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace ATM.Controllers
{
    public class VoucherController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Voucher
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var checkingAccountId = db.CheckingAccounts.Where(c => c.ApplicationUserId == userId).First().Id;
            ViewBag.CheckingAccountId = checkingAccountId;
            return View(db.Vouchers.ToList());
        }

        // GET: Voucher/Details/5
        public ActionResult Details(int checkingAccountId)
        {
            var checkingAccount = db.Vouchers.Where(c => c.CheckingAccountId == checkingAccountId).First();
            return View(checkingAccount);
        }

        // GET: Voucher/Create
        public ActionResult Create(int checkingAccountId)
        {
            return View();
        }

        // POST: Voucher/Create
        [HttpPost]
        public ActionResult Create(Voucher voucher)
        {
            try
            {
               if(ModelState.IsValid)
               {
                   voucher.dateIssued = DateTime.Now;
                   DateTime nextMonth = voucher.dateIssued.AddDays(0).AddMonths(1);
                   voucher.expiryDate = nextMonth;
                   db.Vouchers.Add(voucher);
                   db.SaveChanges();
               }

               return RedirectToAction("Details", "Voucher", new {checkingAccountId = voucher.CheckingAccountId });
            }
            catch
            {
                return View();
            }
        }

        // GET: Voucher/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Voucher/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Voucher/Delete/5
        public ActionResult Delete(int checkingAccountId)
        {
            return View();
        }

        // POST: Voucher/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int checkingAccountId)
        {
            try
            {
                var voucher = db.Vouchers.Where(x => x.CheckingAccountId == checkingAccountId).First();
                db.Vouchers.Remove(voucher);
                db.SaveChanges();

                return RedirectToAction("Index", "Voucher");
            }
            catch
            {
                return View();
            }
        }
    }
}
