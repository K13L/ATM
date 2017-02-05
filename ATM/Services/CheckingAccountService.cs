using ATM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATM.Services
{
    public class CheckingAccountService
    {
        private ApplicationDbContext db;

        public CheckingAccountService(ApplicationDbContext dbContext)
        {
            db = dbContext;
        }

        public void CreateCheckingAccount(string f, string l, string u, decimal b, decimal dl)
        {
           
            var accno = (123456 + db.CheckingAccounts.Count()).ToString().PadLeft(10, '0');
            var checkingAccount = new CheckingAccount { FirstName = f, LastName = l, AccountNumber = accno, Balance = b, DailyLimit = dl, lastDate= DateTime.Now.Date,  ApplicationUserId = u};
            db.CheckingAccounts.Add(checkingAccount);
            db.SaveChanges();
        }

        public void UpdateBalance(int checkingAccountId)
        {
            var checkingAccount = db.CheckingAccounts.Where(x => x.Id == checkingAccountId).First();
            checkingAccount.Balance = db.Transactions.Where(x => x.CheckingAccountId == checkingAccountId).Sum(c => c.Amount);
            db.SaveChanges();
        }

        public void UpdateDailyLimit(int checkingAccountId)
        {
            var checkingAccount = db.CheckingAccounts.Where(x => x.Id == checkingAccountId).First();
            checkingAccount.DailyLimit = checkingAccount.DailyLimit - 10; 
            db.SaveChanges();
        }

        public void UpdateSubscriptionAmount(int checkingAccountId)
        {
            var checkingAccount = db.SubscriptionModels.Where(x => x.CheckingAccountId == checkingAccountId).First();
            checkingAccount.Amount = checkingAccount.Amount - 10; 
            db.SaveChanges();
        }

        public void DeleteVoucher(int checkingAccountId)
        {
           
            var voucher = db.Vouchers.Where(x=>x.CheckingAccountId == checkingAccountId).First();
            if(voucher != null)
            {
                db.Vouchers.Remove(voucher);
                db.SaveChanges();
            }
            
        }

        public void ExpiredVoucher(int checkingAccountId)
        {
            var voucher = db.Vouchers.Find(checkingAccountId);
            if(voucher != null)
            {
                if (voucher.expiryDate == DateTime.Now.Date)
                {
                    db.Vouchers.Remove(voucher);
                    db.SaveChanges();
                }
            }
           
            
        }

        public void UpdateDailyLimit(CheckingAccount checkingAccount)
        {
            while (DateTime.Now.Date != checkingAccount.lastDate)
            {
                checkingAccount.lastDate = DateTime.Now.Date;
                checkingAccount.DailyLimit = 100;
                db.Entry(checkingAccount).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void CreateSubscription(SubscriptionModel subscriptionModel)
        {
            subscriptionModel.Amount = 500;
            subscriptionModel.dateSubscribed = DateTime.Now;
            db.SubscriptionModels.Add(subscriptionModel);
            db.SaveChanges();
        }

        public void DeleteSubscription(int checkingAccountId)
        {
            var subscriber = db.SubscriptionModels.Find(checkingAccountId);

            if(subscriber != null)
            {
                if (subscriber.dateSubscribed == (DateTime.Now.AddMonths(-1)))
                {
                    db.SubscriptionModels.Remove(subscriber);
                    db.SaveChanges();

                    //CreateSubscription(subscriber);
                }
            }

            
        }

        public string BankCharges(int checkingAccountId)
        {
            var sourceCheckingAccount = db.CheckingAccounts.Find(checkingAccountId);
            var limit = sourceCheckingAccount.DailyLimit;
            if (limit < 10)
            {
                return "Cannot complete action, You have reached the daily limit of transactions for today";
            }


          
            UpdateDailyLimit(checkingAccountId);

           try
           {
               var voucher = db.Vouchers.Where(x => x.CheckingAccountId == checkingAccountId).First();

               if (voucher != null && voucher.CheckingAccountId == checkingAccountId)
               {
                   DeleteVoucher(checkingAccountId);
                   return "A Voucher has been used";
               }
               return null;
               
           }
           catch (Exception e)
           {
               try
               {
                   var subscribe = db.SubscriptionModels.Where(x=>x.CheckingAccountId == checkingAccountId).First();
                   if (subscribe != null && subscribe.CheckingAccountId == checkingAccountId)
                    {

                        UpdateSubscriptionAmount(checkingAccountId);
                        return "Subscription has paid";
                    }
               }
               catch
               {
                   if (sourceCheckingAccount.Balance < 10)
                   {
                       return "You have insufficient funds to carry out this action, please proceed to the bank to deposit money";
                   }
                   else
                   {
                       db.Transactions.Add(new Transaction { CheckingAccountId = checkingAccountId, Amount = -10 });
                       db.SaveChanges();
                       return "R10 removed from balance";
                   }
               }
               return e.Message;
           }
            
          
         

           
        }
    }
}