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
using Microsoft.AspNetCore.Identity;

namespace ConnectUs.Areas.Creator.Pages.Events
{
    public class VideoUploadViewModel
    {

        public IFormFile fileupload { get; set; }

        public int productId { get; set; }
        public int eventId { get; set; } 
    }

    public class ManageProducts : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<IdentityUser> _userManager;
        public ManageProducts(ApplicationDbContext db, IWebHostEnvironment env, UserManager<IdentityUser> userManager){
            _db = db;
            _env = env;
            _userManager = userManager;
        } 
        [BindProperty]
        public Event eventEntry {get; set;}
        [BindProperty]
        public Category categoryEntry {get; set;}
       
        public List<EventImage> eventsImages {get; set;}

        public IEnumerable<Product> ProductList {get; set;}
        public string _dir;
        [BindProperty]
        public VideoUploadViewModel UploadModel { get; set; }
        public async Task<IActionResult> OnGet(int id)
        {
          eventEntry  = await _db.Event.FindAsync(id);
          var user = await _userManager.GetUserAsync(User);
          var roles = await _userManager.GetRolesAsync(user);
          eventEntry  = await _db.Event.FindAsync(id);
          if(roles[0] != "Creator" && user.Id != eventEntry.HostId ){
            return RedirectToPage("/AccessDenied",  new { area = "Account" });
          }
          eventsImages = await _db.EventImage.Where(x => x.EventId == id).ToListAsync();
          if(eventEntry is not null){
            categoryEntry = await _db.Category.FindAsync(eventEntry.CategoryId);
          }

          ProductList = await _db.Product.Where(x => x.EventId == eventEntry.EventId).ToListAsync();
          return Page();
        } 
        
        public async Task<IActionResult> OnPostCreateProduct([FromBody]Product product){

               if(product!=null){

                  Product productToAdd = new Product(){
                    Name=product.Name,
                    Price=product.Price,
                    EventId=product.EventId,
                    ImageBase64=product.ImageBase64,
                    VideoBase64=product.VideoBase64
                    };
                  await _db.Product.AddAsync(productToAdd);
                  await _db.SaveChangesAsync();
    
                  return new JsonResult("Success");

               }else{

                  return new JsonResult(product);
               }        
        
        }
        public async Task<IActionResult> OnPostUploadVideo()    
        {   
            _dir =  _env.WebRootPath;
            var fileupload =  UploadModel.fileupload;
            var productId =  UploadModel.productId;
            var eventId = UploadModel.eventId;
            if ( fileupload != null)    
            {   
                var uploads = Path.Combine(_dir, "uploads");
                var uniqueFileName = GetUniqueFileName(fileupload.FileName);
                using(var fileStream =
                    new FileStream(Path.Combine(uploads, uniqueFileName), FileMode.Create, FileAccess.Write)){
                        fileupload.CopyTo(fileStream);
                    }
                var productToEdit = await _db.Product.FindAsync(productId);
                productToEdit.VideoBase64 = uniqueFileName;
                await _db.SaveChangesAsync(); 
            }  
            return RedirectToPage(new {id = eventId}); 
        }    
        private string GetUniqueFileName(string fileName)
            {
                fileName = Path.GetFileName(fileName);
                return  Path.GetFileNameWithoutExtension(fileName)
                        + "_" 
                        + Guid.NewGuid().ToString().Substring(0, 4) 
                        + Path.GetExtension(fileName);
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
          _dir =  _env.WebRootPath;
          var vidPath = Path.Combine(_dir, "uploads", product.VideoBase64);
          try    
          {    
          // Check if file exists with its full path    
          if (System.IO.File.Exists(vidPath))    
          {    
          // If file found, delete it    
             System.IO.File.Delete(vidPath);    
            Console.WriteLine("File deleted.");    
          }    
          else Console.WriteLine("File not found");    
          }    
          catch (IOException ioExp)    
          {    
            Console.WriteLine(ioExp.Message);    
          }    
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
      
      
    }
}