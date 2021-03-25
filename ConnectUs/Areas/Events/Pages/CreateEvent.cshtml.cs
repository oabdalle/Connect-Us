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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
namespace ConnectUs.Areas.Events.Pages
{
    [Authorize(Roles = "Creator, Admin")]
    public class CreateEventModel : PageModel
    {
        private readonly ILogger<CreateEventModel> _logger;
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<IdentityUser> _userManager;
        [Required]
        [BindProperty] //this assumes then onPost you will be getting this event here that the user modified so no need for onPost(Event eventObj)
        public Event Event { get; set; }

        public IEnumerable<Category> Categories { get; set; }

        public SelectList CategoriesDropDownList { get; set; }

        public string userId {get; set;}

        public CreateEventModel(ILogger<CreateEventModel> logger, ApplicationDbContext db, IWebHostEnvironment env, UserManager<IdentityUser> um)
        {
            _logger = logger;
            _db = db;
            _env=env;
            _userManager = um;
        }

        public async Task OnGet()
        {
           await InitItems();

        }

        public async Task<IActionResult> OnPost() //we use <IActionResult> to direct to new page
        { 
            Event.EventGallery = new List<EventImage>();
            
            if(Event.GalleryFiles != null){
                int index = 0;
              foreach (var file in Event.GalleryFiles){
              {   
                    var isCover = false;
                    if(index==0){
                        isCover=true;
                    }
                    index++;
                    using (var ms = new MemoryStream())
                    {
                        await file.CopyToAsync(ms);
                        var fileBytes = ms.ToArray();
                        string s = Convert.ToBase64String(fileBytes);
                        Event.EventGallery.Add(new EventImage()
                        {
                            Content = s,
                            Cover = isCover,
                            Event = Event
                        });
                    }
                
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
                    await _db.EventImage.AddAsync(img);
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