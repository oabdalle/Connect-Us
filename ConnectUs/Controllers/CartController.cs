using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConnectUs.Model;
using ConnectUs.Model.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ConnectUs.Services;
using Microsoft.EntityFrameworkCore;
namespace ConnectUs.Controllers
{
     [ApiController]
    [Route("api/[controller]/[action]")]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CartController(ApplicationDbContext db){

           _db = db;
        }

  

        [HttpPost]
        public IActionResult Buy([FromBody]CartInfo productDetails)
        {
      
            if(productDetails!=null){
              Product product =  _db.Product.Where(x => x.ProductId == productDetails.productId).FirstOrDefault();
                              
              if (CartSessionService.GetObjectFromJson<List<ItemViewModel>>(HttpContext.Session, "cart") == null)
              {
                  List<ItemViewModel> cart = new List<ItemViewModel>();
                  cart.Add(new ItemViewModel { Product = product, Quantity = productDetails.quantity });
                  CartSessionService.SetObjectAsJson(HttpContext.Session, "cart", cart);
         
              }
              else
              {
                  List<ItemViewModel> cart = CartSessionService.GetObjectFromJson<List<ItemViewModel>>(HttpContext.Session, "cart");
                  int index = isExist(productDetails.productId);
                  if (index != -1)
                  {
                      cart[index].Quantity=productDetails.quantity;
                  }
                  else
                  {
                      cart.Add(new ItemViewModel { Product = product, Quantity = productDetails.quantity });

                  }
                  CartSessionService.SetObjectAsJson(HttpContext.Session, "cart", cart);
              }
              bool reload=false;
              if(productDetails.atCheckout==true){
                reload=true;
              }
              return Json(new { result = "Success", quantity=productDetails.quantity, reloadPage = reload });
            }
          return Json(new { result = "Not Found" });
        }


        [Route("remove/{id}")]
        public IActionResult Remove(int id)
        { 
            if(id>0){
              List<ItemViewModel> cart = CartSessionService.GetObjectFromJson<List<ItemViewModel>>(HttpContext.Session, "cart");
              int index = isExist(id);
              cart.RemoveAt(index);
              CartSessionService.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            return RedirectToPage("/Checkout", new { area = "Events"});
        }

        private int isExist(int id)
        {
            List<ItemViewModel> cart = CartSessionService.GetObjectFromJson<List<ItemViewModel>>(HttpContext.Session, "cart");
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Product.ProductId.Equals(id))
                {
                    return i;
                }
            }
            return -1;
        }
    }
    public class CartInfo{
      public int productId { get; set; }
      public int quantity { get; set; }
      public bool atCheckout { get; set; }
    }
}