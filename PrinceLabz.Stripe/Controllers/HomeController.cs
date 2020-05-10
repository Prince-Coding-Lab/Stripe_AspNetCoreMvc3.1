using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PrinceLabz.Stripe.Infrastructure.Interfaces;
using PrinceLabz.Stripe.Models;
using Stripe1 = Stripe;

namespace PrinceLabz.Stripe.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStripeGateway _stripeGateway;
        public HomeController(IStripeGateway stripeGateway)
        {
            _stripeGateway = stripeGateway;
            _stripeGateway.SetApiKey();
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult OrderDetail()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(Customer customer)
        {
            Stripe1.CreditCardOptions card = new Stripe1.CreditCardOptions();
            card.Name = customer.FirstName + " " + customer.LastName;
            card.Number = customer.CardNumber;// testing card number "4242424242424242";
            card.ExpYear = customer.ExpYear; ;
            card.ExpMonth = customer.ExpMonth; ;
            card.Cvc = customer.Cvc;
            //Assign Card to Token Object and create Token  
            Stripe1.TokenCreateOptions token = new Stripe1.TokenCreateOptions();
            token.Card = card;

            Stripe1.Token tokenresult = await _stripeGateway.GetTokenAsync(card);

            Stripe1.CustomerCreateOptions myCustomer = new Stripe1.CustomerCreateOptions();
            myCustomer.Email = customer.Email;


            Stripe1.Customer stripeCustomer = await _stripeGateway.CreateCustomerAsync(myCustomer);
            customer.Amount = customer.Amount;
            var options = new Stripe1.ChargeCreateOptions
            {
                Amount = Convert.ToInt32(customer.Amount),
                Currency = customer.CurrencyId == 1 ? "ILS" : "USD",
                ReceiptEmail = customer.Email,
                Source = tokenresult.Id
                // CustomerId = stripeCustomer.Id,
                // Description = Convert.ToString(tParams.TransactionId), //Optional  
            };

            Stripe1.Charge charge = await _stripeGateway.CreateChargeAsync(options);

            return RedirectToAction("OrderDetail", "Home");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
