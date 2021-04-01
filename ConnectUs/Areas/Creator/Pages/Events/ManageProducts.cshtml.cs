using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ConnectUs.Model;
using ConnectUs.Model.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace ConnectUs.Areas.Creator.Pages.Events
{
  
    public class ManageProducts : PageModel
    {
        private readonly ApplicationDbContext _db;
        public ManageProducts(ApplicationDbContext db){
            _db = db;
        } 
        [BindProperty]
        public Event eventEntry {get; set;}
        [BindProperty]
        public Category categoryEntry {get; set;}
       
        public List<EventImage> eventsImages {get; set;}

        public IEnumerable<Product> ProductList {get; set;}


        public async Task OnGet(int id)
        {
          await InitProducts(id);
        } 
        
        public async Task InitProducts(int id){

            eventEntry  = await _db.Event.FindAsync(id);
            eventsImages = await _db.EventImage.Where(x => x.EventId == id).ToListAsync();
            if(eventEntry is not null){
            categoryEntry = await _db.Category.FindAsync(eventEntry.CategoryId);
            }

            ProductList = await _db.Product.Where(x => x.EventId == eventEntry.EventId).ToListAsync();
        }

        public async Task<IActionResult> OnPostEditProduct([FromBody]Product product){


              if (product != null)
              {
                var productToEdit = await _db.Product.FindAsync(product.ProductId);
                productToEdit.Name = product.Name;
                productToEdit.Price = product.Price;
                if(product.ImageBase64 !=null)
                  productToEdit.ImageBase64 = product.ImageBase64;

                await _db.SaveChangesAsync(); 
                
                return new JsonResult("Success");

              }
              else
              {

         
                return new JsonResult("fail");
              }
               
        }

        public async Task<IActionResult> OnPostDeleteProduct(int id, int eventId)
        {

          var product = await _db.Product.FindAsync(id);

          if (product == null)
          {
              return NotFound();
          }

          _db.Product.Remove(product);
         
          await _db.SaveChangesAsync();
          return RedirectToPage(new {id = eventId});
          
        }
         public async Task<IActionResult> OnPostProductDetails(int id){

            if(id!=null){
              Product product = await _db.Product.FindAsync(id);

              return new JsonResult(product);

            }else{

              return new JsonResult("Category Name is null");
            }        
        
        }

        public async Task<IActionResult> OnPostRemoveImage(int Item1){
            var imageToDelete = await _db.EventImage.FindAsync(Item1);
            
            if(imageToDelete.Cover==true){
               var setCoverImage = await _db.EventImage.FindAsync(Item1+1);
               setCoverImage.Cover=true;
            }
            _db.EventImage.Remove(imageToDelete);
            await _db.SaveChangesAsync();
            eventsImages = await _db.EventImage.Where(x => x.EventId == imageToDelete.EventId).ToListAsync();
            return Partial("_EditImages", eventsImages);
        }

          public async Task<IActionResult> OnPostSetCover(int Item1){
            
            var imageToCover = await _db.EventImage.FindAsync(Item1);
            var imageToReset = await _db.EventImage.FirstOrDefaultAsync(x => x.EventId == imageToCover.EventId && x.Cover == true);
            imageToCover.Cover = true;
            imageToReset.Cover = false;
            await _db.SaveChangesAsync();

            eventsImages = await _db.EventImage.Where(x => x.EventId == imageToCover.EventId).ToListAsync();
           
            return Partial("_EditImages", eventsImages);
        }

        public async Task<IActionResult> OnPost()
        {
            if(ModelState.IsValid)
            {
        
                await _db.SaveChangesAsync();
                return RedirectToPage("/Index",  new { area = "Events" });
            }
            return RedirectToPage();
        }
      
      
    }
}