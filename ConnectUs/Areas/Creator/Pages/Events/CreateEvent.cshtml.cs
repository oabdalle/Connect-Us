using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ConnectUs.Model;
using ConnectUs.Model.Entities;
using ConnectUs.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace ConnectUs.Areas.Creator.Pages.Events
{
    [Authorize(Roles = "Admin, Creator")]
    public class CreateEventModel : PageModel
    {
        private readonly ILogger<CreateEventModel> _logger;
        private readonly ApplicationDbContext _db;

        private readonly IImageService _is;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<IdentityUser> _userManager;
        [Required]
        [BindProperty] //this assumes then onPost you will be getting this event here that the user modified so no need for onPost(Event eventObj)
        public Event Event { get; set; }

        public IEnumerable<Category> Categories { get; set; }

        public SelectList CategoriesDropDownList { get; set; }

        public string userId {get; set;}

        public CreateEventModel(ILogger<CreateEventModel> logger, ApplicationDbContext db, IWebHostEnvironment env, UserManager<IdentityUser> um, IImageService iS)
        {
            _logger = logger;
            _db = db;
            _env=env;
            _userManager = um;
            _is = iS;
        }

        public async Task OnGet()
        {
           await InitItems();

        }

        public async Task<IActionResult> OnPost() //create an event form on post
        { 
            Event.EventGallery = new List<EventImage>();
            
            if(Event.GalleryFiles != null){
                int index = 0;
              foreach (var file in Event.GalleryFiles) //for each IFormFile from form, create file to filesystem and save path to event gallery 
              {   

                var filename = _is.Create(file).Result;
                if(filename!=null){
                  var isCover = false;
                  if(index==0){
                      isCover=true;
                  }
                  index++;
         
                  Event.EventGallery.Add(new EventImage()
                  {
                      ImagePath = filename,
                      Cover = isCover,
                      Event = Event
                  });
          
                }
                
              }
            }else{
                await InitItems();
                return Page();
            }

            if (ModelState.IsValid)
            {   
                await _db.Event.AddAsync(Event);
                foreach (EventImage img in Event.EventGallery){
                    await _db.EventImage.AddAsync(img); //add each image from the event to the db with the event id as a secondary key
                }
                await _db.SaveChangesAsync();
                return RedirectToPage("/Index", new { area = "Events" });
            }
            else
            {     
                var errors = ModelState.Select(x => x.Value.Errors)
                                    .Where(y=>y.Count>0)
                                    .ToList();
                await InitItems();
                return Page();
            }

        }
    
        public IActionResult OnPostUpload(IFormFile file){
            return Page();
        }

        public async Task InitItems(){
           var user = await _userManager.GetUserAsync(HttpContext.User);
           userId = user.Id;
           Categories = await _db.Category.ToListAsync();
           CategoriesDropDownList = new SelectList(Categories, "CategoryId", "CategoryName");
        }

    }
}