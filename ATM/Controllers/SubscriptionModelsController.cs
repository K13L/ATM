using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ATM.Models;
using ATM.Services;
using Microsoft.AspNet.Identity;
namespace ATM.Controllers
{
    public class SubscriptionModelsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: SubscriptionModels
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var checkingAccountId = db.CheckingAccounts.Where(c => c.ApplicationUserId == userId).First().Id;
            ViewBag.CheckingAccountId = checkingAccountId;
            var subscriptionModels = db.SubscriptionModels.Include(x => x.CheckingAccount);
            return View(subscriptionModels.ToList());
        }

        // GET: SubscriptionModels/Details/5
        public ActionResult Details()
        {
            //var userId = User.Identity.GetUserId();
            var userId = User.Identity.GetUserId();
            var checkingAccountId = db.CheckingAccounts.Where(c => c.ApplicationUserId == userId).First().Id;
            ViewBag.CheckingAccountId = checkingAccountId;

            try
                {
                    var checkingAccount = db.SubscriptionModels.Where(x => x.CheckingAccountId == checkingAccountId).First();

                    if (checkingAccount != null)
                    {
                        return View(checkingAccount);
                    }
                    else
                    {
                        return RedirectToAction("Create", "SubscriptionModels");
                    }
                }
                catch(Exception e )
            {
                string message = e.Message;
                return  RedirectToAction("Create", "SubscriptionModels", new {checkingAccountId = checkingAccountId });
                }
     
        }

        // GET: SubscriptionModels/Create
        public ActionResult Create(int checkingAccountId)
        {
            //ViewBag.CheckingAccountId = new SelectList(db.CheckingAccounts, "Id", "AccountNumber");
            return View();
        }

        // POST: SubscriptionModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( SubscriptionModel subscriptionModel)
        {
            var checkingAccount = db.CheckingAccounts.Find(subscriptionModel.CheckingAccountId);
            if(checkingAccount.Balance < 250)
            {
                ViewBag.message = "Insufficient funds to subscribe";
                ModelState.AddModelError("Amount","Insufficent funds to subscribe");
            }
            
            if (ModelState.IsValid)
            {
                var trans = db.Transactions.Find(subscriptionModel.CheckingAccountId);
                trans.Amount = -250;
                trans.CheckingAccountId = subscriptionModel.CheckingAccountId;
                db.Transactions.Add(trans);
                db.SaveChanges();

                subscriptionModel.Amount = 500;
                subscriptionModel.dateSubscribed = DateTime.Now;
                db.SubscriptionModels.Add(subscriptionModel);
                db.SaveChanges();

                var service = new CheckingAccountService(db);
                service.UpdateBalance(trans.CheckingAccountId);

                return RedirectToAction("Index","Home");
            }

            //ViewBag.CheckingAccountId = new SelectList(db.CheckingAccounts, "Id", "AccountNumber", subscriptionModel.CheckingAccountId);
            return View();
        }

        // GET: SubscriptionModels/Edit/5
        public ActionResult Edit(int checkingAccountId)
        {
            if (checkingAccountId < 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubscriptionModel subscriptionModel = db.SubscriptionModels.Find(checkingAccountId);
            if (subscriptionModel == null)
            {
                return HttpNotFound();
            }
            ViewBag.CheckingAccountId = new SelectList(db.CheckingAccounts, "Id", "AccountNumber", subscriptionModel.CheckingAccountId);
            return View(subscriptionModel);
        }

        // POST: SubscriptionModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SubscriptionModel subscriptionModel)
        {
            if (ModelState.IsValid)
            {
                //subscriptionModel.CheckingAccountId = 
                db.Entry(subscriptionModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.CheckingAccountId = new SelectList(db.CheckingAccounts, "Id", "AccountNumber", subscriptionModel.CheckingAccountId);
            return View();
        }

        // GET: SubscriptionModels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubscriptionModel subscriptionModel = db.SubscriptionModels.Find(id);
            if (subscriptionModel == null)
            {
                return HttpNotFound();
            }
            return View(subscriptionModel);
        }

        // POST: SubscriptionModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SubscriptionModel subscriptionModel = db.SubscriptionModels.Find(id);
            db.SubscriptionModels.Remove(subscriptionModel);
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
