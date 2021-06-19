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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Stripe;
using Stripe.Checkout;
using System.Collections;
using System.Collections.Generic;

namespace ConnectUs.Controllers
{
   [ApiController]
    [Route("[controller]/[action]")]
    public class ChargeController : Controller
    {

        public ChargeController(){
    
        }

        [HttpPost]
        public ActionResult CreateCheckoutSession()
        {
            var domain = "http://" + HttpContext.Request.Host;

            var cart = CartSessionService.GetObjectFromJson<List<ItemViewModel>>(HttpContext.Session, "cart");
            List<SessionLineItemOptions> LineItemsToAdd = new List<SessionLineItemOptions>();
            foreach(var item in cart){
                LineItemsToAdd.Add(new SessionLineItemOptions{
                    
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                      UnitAmountDecimal = item.Product.Price*100,
                      Currency = "usd",
                      ProductData = new SessionLineItemPriceDataProductDataOptions
                      {
                        Name = item.Product.Name,
                      },
                    },
                    Quantity = item.Quantity,
                   });             
             }
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                  "card",
                },
                LineItems = LineItemsToAdd,
                Mode = "payment",
                SuccessUrl = domain + "/success",
                CancelUrl = domain + "/cancel",
            };
            var service = new SessionService();
            Session session = service.Create(options);
            return Json(new { id = session.Id });
        }

    }
}
























