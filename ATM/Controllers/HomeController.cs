using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ATM.Models;
using Microsoft.AspNet.Identity.Owin;
using ATM.Services;

namespace ATM.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var checkingAccountId = db.CheckingAccounts.Where(c => c.ApplicationUserId == userId).First().Id;
            ViewBag.CheckingAccountId = checkingAccountId;
            var manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = manager.FindById(userId);
            ViewBag.Pin = user.Pin;
            var checkingAccount = db.CheckingAccounts.Find(checkingAccountId);
            ViewBag.DailyLimit = checkingAccount.DailyLimit;
            var service = new CheckingAccountService(db);
            service.DeleteSubscription(checkingAccountId);
            service.ExpiredVoucher(checkingAccountId);
            service.UpdateDailyLimit(checkingAccount);
     

            return View();
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
        [HttpPost]
        public ActionResult Contact(string message)
        {
            ViewBag.Message = "Message recieved!";

            return PartialView("_ContactThanks");
        }

        public ActionResult Serial(string letterCase)
        {
            var serial = "ASP12345";

            if (letterCase == "lower")
                return Content(serial.ToLower());

            return Content(serial);
        }
    }
}