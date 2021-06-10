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
using ConnectUs.Services;
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
        private readonly IImageService _is;

        public ManageProducts(ApplicationDbContext db, IWebHostEnvironment env, IImageService iS){
            _db = db;
            _env = env;
            _is = iS;
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
        public async Task<IActionResult> OnPostCreateProduct(){
            var httpRequest = HttpContext.Request;

            if(httpRequest != null){
              var path = _is.Create(httpRequest.Form.Files["ImagePath"]).Result;
              if(path != null){

              Product productToAdd = new Product(){
                Name=httpRequest.Form["Name"],
                Price=decimal.Parse(httpRequest.Form["Price"]),
                EventId=Int32.Parse(httpRequest.Form["EventId"]),
                ImagePath=path
                // VideoPath="0"
                };
              await _db.Product.AddAsync(productToAdd);
              await _db.SaveChangesAsync();

              return new JsonResult("Success");
              }
            }

              return new JsonResult("Fail");
                  // return new JsonResult(product);
                       
        
        }
        public async Task<IActionResult> OnPostUploadVideo()    
        {   
            _dir =  _env.WebRootPath;
            var fileupload =  UploadModel.fileupload;
            var productId =  UploadModel.productId;
            var eventId = UploadModel.eventId;
            if (fileupload != null)    
            {   
                var uploads = Path.Combine(_dir, "uploads");
                var uniqueFileName = GetUniqueFileName(fileupload.FileName);
                using(var fileStream =
                    new FileStream(Path.Combine(uploads, uniqueFileName), FileMode.Create, FileAccess.Write)){
                        fileupload.CopyTo(fileStream);
                    }
                var productToEdit = await _db.Product.FindAsync(productId);
                if(productToEdit.VideoPath != null){
                  _is.Delete(productToEdit.VideoPath);
                }
                productToEdit.VideoPath = uniqueFileName;
                
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
        public async Task<IActionResult> OnPostEditProduct(){
              var httpRequest = HttpContext.Request;

              if(httpRequest != null){
                var path = _is.Create(httpRequest.Form.Files["ImagePath"]).Result;

                if(path != null){
                  var productToEdit = await _db.Product.FindAsync(Int32.Parse(httpRequest.Form["ProductId"]));
                  productToEdit.Name = httpRequest.Form["Name"];
                  productToEdit.Price = decimal.Parse(httpRequest.Form["Price"]);
                  _is.Delete(productToEdit.ImagePath);
                  productToEdit.ImagePath = path;

                  await _db.SaveChangesAsync(); 
                  
                  return new JsonResult("Success");

                }
              }
          
         
                return new JsonResult("fail");
              
               
        }

        public async Task<IActionResult> OnPostDeleteProduct(int id, int eventId)
        {

          var product = await _db.Product.FindAsync(id);

          if (product == null)
          {
              return NotFound();
          }
          
          _db.Product.Remove(product);

          if(_is.Delete(product.ImagePath).Result && _is.Delete(product.VideoPath).Result){

            await _db.SaveChangesAsync();
          }
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