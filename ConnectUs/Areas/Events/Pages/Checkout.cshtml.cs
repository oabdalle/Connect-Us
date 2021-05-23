using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ConnectUs.Services;
using ConnectUs.Model;
using ConnectUs.Model.Entities;

namespace ConnectUs.Areas.Events.Pages
{

    public class CheckoutModel : PageModel
    {

        private readonly ILogger<CheckoutModel> _logger;

        public CheckoutModel(ILogger<CheckoutModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            var cart = CartSessionService.GetObjectFromJson<List<ItemViewModel>>(HttpContext.Session, "cart");
            if(cart != null){
              ViewData["cart"] = cart;
              ViewData["total"] = cart.Sum(item => item.Product.Price * item.Quantity);
            }
        }
    }
}
