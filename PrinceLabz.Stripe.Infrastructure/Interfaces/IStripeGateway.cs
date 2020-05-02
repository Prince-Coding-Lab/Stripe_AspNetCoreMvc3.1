using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using StripeObj = Stripe;
namespace PrinceLabz.Stripe.Infrastructure.Interfaces
{
    public interface IStripeGateway
    {
        Task<string> SetApiKey();
        Task<StripeObj.Token> GetTokenAsync(StripeObj.CreditCardOptions card);
        Task<StripeObj.Customer> CreateCustomerAsync(StripeObj.CustomerCreateOptions customer);
        Task<StripeObj.Charge> CreateChargeAsync(StripeObj.ChargeCreateOptions charge);
    }
}
