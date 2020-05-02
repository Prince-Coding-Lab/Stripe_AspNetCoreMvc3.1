using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrinceLabz.Stripe.Models
{
    public class Customer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string CardNumber { get; set; }
        public int ExpYear { get; set; }
        public int ExpMonth { get; set; }
        public string Cvc { get; set; }
        public int Amount { get; set; }
        public int CurrencyId { get; set; }
    }
}
