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
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IImageService _is;
        public ManageProducts(ApplicationDbContext db, IWebHostEnvironment env, UserManager<IdentityUser> userManager, IImageService iS){
            _db = db;
            _env = env;
            _userManager = userManager;
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
        
        public async Task<IActionResult> OnPostCreateProduct(){ //create a new product for the event
            var httpRequest = HttpContext.Request;

            if(httpRequest != null){
              var path = _is.Create(httpRequest.Form.Files["ImagePath"]).Result; //get the path of the IFormFile passed
              if(path != null){

              Product productToAdd = new Product(){ //init a new product using values from the ajax form data
                Name=httpRequest.Form["Name"], 
                Price=decimal.Parse(httpRequest.Form["Price"]),
                EventId=Int32.Parse(httpRequest.Form["EventId"]),
                ImagePath=path
                // VideoPath="0"
                };
              await _db.Product.AddAsync(productToAdd); //add product to db
              await _db.SaveChangesAsync();

              return new JsonResult("Success");
              }
            }

              return new JsonResult("Fail");
                       
        
        }
        public async Task<IActionResult> OnPostUploadVideo()    //upload a video of a product 
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
                        fileupload.CopyTo(fileStream); //save the video to the server
                    }
                var productToEdit = await _db.Product.FindAsync(productId);
                if(productToEdit.VideoPath != null){
                  _is.Delete(productToEdit.VideoPath); //delete the previous video from the server
                }
                productToEdit.VideoPath = uniqueFileName; //save the video path to the db
                
                await _db.SaveChangesAsync(); 
                
            }  
            return RedirectToPage(new {id = eventId}); 
        }    
        private string GetUniqueFileName(string fileName) //get a unique file name for the video
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
                var path = _is.Create(httpRequest.Form.Files["ImagePath"]).Result; //get the path from the IFormFile and save it to the server

                if(path != null){
                  var productToEdit = await _db.Product.FindAsync(Int32.Parse(httpRequest.Form["ProductId"])); //retrieve the product to edit form the db
                  productToEdit.Name = httpRequest.Form["Name"];
                  productToEdit.Price = decimal.Parse(httpRequest.Form["Price"]);
                  _is.Delete(productToEdit.ImagePath);
                  productToEdit.ImagePath = path;

                  await _db.SaveChangesAsync(); //save the changes made to the existing product
                  
                  return new JsonResult("Success");

                }
              }
          
         
                return new JsonResult("fail");
              
               
        }

        public async Task<IActionResult> OnPostDeleteProduct(int id, int eventId)
        {

          var product = await _db.Product.FindAsync(id); //find the product to delete
          if (product == null)
          {
              return NotFound();
          }
          _db.Product.Remove(product); //remove product from db context
          if(product.VideoPath!=null){
            _is.Delete(product.VideoPath);
          }
          if(_is.Delete(product.ImagePath)){ //delete all images and videos of the product
            await _db.SaveChangesAsync(); //save the changes to db
          }
          return RedirectToPage(new {id = eventId});
        }         
        public async Task<IActionResult> OnPostProductDetails(int? id){

            if(id!=null){
              Product product = await _db.Product.FindAsync(id);

              return new JsonResult(product); //pass the product details as a json

            }else{

              return new JsonResult("Category Name is null");
            }        
        
        }
      
      
    }
}