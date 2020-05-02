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
        [HttpPost]
        public async Task<IActionResult> Index(Customer customer)
        {
            Stripe1.CreditCardOptions card = new Stripe1.CreditCardOptions();
            card.Name = "Prince Kumar";
            card.Number = "4242424242424242";
            card.ExpYear = 2025;
            card.ExpMonth = 12;
            card.Cvc = "123";
            //Assign Card to Token Object and create Token  
            Stripe1.TokenCreateOptions token = new Stripe1.TokenCreateOptions();
            token.Card = card;

            Stripe1.Token tokenresult = await _stripeGateway.GetTokenAsync(card);

            Stripe1.CustomerCreateOptions myCustomer = new Stripe1.CustomerCreateOptions();
            myCustomer.Email = "princesharma78@gmail.com";
            // myCustomer.SourceToken = tokenresult.Id;

            Stripe1.Customer stripeCustomer = await _stripeGateway.CreateCustomerAsync(myCustomer);
            customer.Amount = 50;
            var options = new Stripe1.ChargeCreateOptions
            {
                Amount = Convert.ToInt32(customer.Amount),
                Currency = customer.CurrencyId == 1 ? "ILS" : "USD",
                ReceiptEmail = "princesharma78@gmail.com",
                Source = tokenresult.Id
                // CustomerId = stripeCustomer.Id,
                // Description = Convert.ToString(tParams.TransactionId), //Optional  
            };

            Stripe1.Charge charge = await _stripeGateway.CreateChargeAsync(options);

            return RedirectToAction("Index", "Home");
        }
        //[HttpPost]
        //public async Task<IActionResult> Index(Customer customer)
        //{
        //    Stripe1.CreditCardOptions card = new Stripe1.CreditCardOptions();
        //    card.Name = customer.FirstName + " " + customer.LastName;
        //    card.Number = customer.CardNumber;
        //    card.ExpYear = customer.ExpYear;
        //    card.ExpMonth = customer.ExpMonth;
        //    card.Cvc = customer.Cvc;
        //    //Assign Card to Token Object and create Token  
        //    Stripe1.TokenCreateOptions token = new Stripe1.TokenCreateOptions();
        //    token.Card = card;

        //    Stripe1.Token tokenresult=await _stripeGateway.GetTokenAsync(card);

        //    Stripe1.CustomerCreateOptions myCustomer = new Stripe1.CustomerCreateOptions();
        //    myCustomer.Email = customer.Email;
        //   // myCustomer.SourceToken = tokenresult.Id;

        //    Stripe1.Customer stripeCustomer =await _stripeGateway.CreateCustomerAsync(myCustomer);

        //    var options = new Stripe1.ChargeCreateOptions
        //    {
        //        Amount = Convert.ToInt32(customer.Amount),
        //        Currency = customer.CurrencyId == 1 ? "ILS" : "USD",
        //        ReceiptEmail = customer.Email,
        //       // CustomerId = stripeCustomer.Id,
        //       // Description = Convert.ToString(tParams.TransactionId), //Optional  
        //    };

        //    Stripe1.Charge charge = await _stripeGateway.CreateChargeAsync(options);

        //    return RedirectToAction("Index", "Home");
        //}

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
