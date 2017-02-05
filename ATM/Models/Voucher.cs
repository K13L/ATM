using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ATM.Models
{
    public class Voucher
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Date Issued")]
        public DateTime dateIssued { get; set; }

        [Required]
        [Display(Name = "Expiry Date")]
        public DateTime expiryDate { get; set; }

        [Required]
        public int CheckingAccountId { get; set; }
        public virtual CheckingAccount CheckingAccount { get; set; }
    }
}