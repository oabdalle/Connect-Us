using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ConnectUs.Hubs;
using ConnectUs.Model;
using ConnectUs.Model.Entities;
using ConnectUs.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Stripe;
using Stripe.Checkout;
namespace ConnectUs.Controllers
{
   [ApiController]
    [Route("api/[controller]/[action]")]
    public class ChargeController : Controller
    {

        public ChargeController(){
     
        }

        [HttpPost]
        public async Task<IActionResult> ChargeCard(string stripeEmail, string stripeToken)
        {    
          var customerService = new CustomerService();
          var chargeService = new ChargeService();

          var customer = customerService.Create(new CustomerCreateOptions{
              Email = stripeEmail,
              Source = stripeToken
          });
          var charge = chargeService.Create(new ChargeCreateOptions{
            Amount = 500,
            Description = "ASP.NET Core Stripe Tutorial",
            Currency = "usd",
            Customer = customer.Id
          });
           return RedirectToPage("Index");
        }

    }
}
























