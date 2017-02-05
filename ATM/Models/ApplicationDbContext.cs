using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ATM.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            //this.Users, this.Roles
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<CheckingAccount> CheckingAccounts { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<SubscriptionModel> SubscriptionModels { get; set; }

        public DbSet<Voucher> Vouchers { get; set; }

        //public System.Data.Entity.DbSet<ATM.Models.SubscriptionModel> SubscriptionModels { get; set; }
    }
    
}