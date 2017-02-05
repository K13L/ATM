using ATM.Models;
using ATM.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ATM.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        //private SubscriptionModel sub = new SubscriptionModel();
        
        // GET: Transaction
        public ActionResult Deposit(int checkingAccountId)
        {
            
            return View();
            
        }

        [HttpPost]
        public ActionResult Deposit(Transaction transaction)
        {
            var checkingAccount = db.CheckingAccounts.Find(transaction.CheckingAccountId);
            if (checkingAccount.DailyLimit < 10)
            {
                ModelState.AddModelError("Amount", "You have reached the daily limit of transactions. Please try again tomorrow");
               
            }

            if(checkingAccount.Balance < 10)
            {
                ModelState.AddModelError("Amount", "Insufficientfunds to carry out action");
            }
            
            if (ModelState.IsValid)
            {
                  
                    db.Transactions.Add(transaction);
                    db.SaveChanges();

                    var service = new CheckingAccountService(db);
                    ViewBag.message = service.BankCharges(transaction.CheckingAccountId);
                    service.UpdateBalance(transaction.CheckingAccountId);
                
                    return View();
                
            }
            return View();
        }

    public ActionResult Withdrawal (int checkingAccountId)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Withdrawal(Transaction transaction)
        {
            var checkingAccount = db.CheckingAccounts.Find(transaction.CheckingAccountId);
        if(checkingAccount.Balance < transaction.Amount )
        {
            ModelState.AddModelError("Amount", "You have insifficient funds!");
        }
            if(checkingAccount.DailyLimit < 10)
            {
                ModelState.AddModelError("Amount", "You have reached the daily limit of transactions. Please try again tomorrow");
            }

        if(ModelState.IsValid)
        {
            transaction.Amount = -transaction.Amount;
            db.Transactions.Add(transaction);
            db.SaveChanges();

            var service = new CheckingAccountService(db);
            ViewBag.message = service.BankCharges(transaction.CheckingAccountId);
            service.UpdateBalance(transaction.CheckingAccountId);
            //return RedirectToAction("Index", "Home");
            return View();
        }

        return View();

        }

      

    }
}