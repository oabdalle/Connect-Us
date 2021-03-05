using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ConnectUs.Model;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Hosting;

namespace ConnectUs.Areas.Events.Pages
{
    public class CreateEventModel : PageModel
    {
        private readonly ILogger<CreateEventModel> _logger;
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _env;

        [Required]
        [BindProperty] //this assumes then onPost you will be getting this event here that the user modified so no need for onPost(Event eventObj)
        public Event Event { get; set; }

        public IEnumerable<Category> Categories { get; set; }

        public SelectList CategoriesDropDownList { get; set; }

        public CreateEventModel(ILogger<CreateEventModel> logger, ApplicationDbContext db, IWebHostEnvironment env)
        {
            _logger = logger;
            _db = db;
            _env=env;
        }

        public async Task OnGet()
        {
           await InitCategories();

        }

        public async Task<IActionResult> OnPost() //we use <IActionResult> to direct to new page
        { 
            Event.EventGallery = new List<EventImage>();

            if(Event.GalleryFiles != null){
                foreach (var file in Event.GalleryFiles){
                {
                    using (var ms = new MemoryStream())
                    {
                        await file.CopyToAsync(ms);
                        var fileBytes = ms.ToArray();
                        string s = Convert.ToBase64String(fileBytes);
                        Event.EventGallery.Add(new EventImage()
                        {
                            Content = s,
                            Event = Event
                        });
                    }
                
                }
                }
            }else{
                await InitCategories();
                return Page();
            }

            if (ModelState.IsValid)
            {   
                await _db.Event.AddAsync(Event);
                foreach (EventImage img in Event.EventGallery){
                    await _db.EventImage.AddAsync(img);
                }
                await _db.SaveChangesAsync();
                return RedirectToPage("/Index", new { area = "Home" });
            }
            else
            {     
                var errors = ModelState.Select(x => x.Value.Errors)
                                    .Where(y=>y.Count>0)
                                    .ToList();
                await InitCategories();
                return Page();
            }

        }
    
        public IActionResult OnPostUpload(IFormFile file){
            return Page();
        }

        public async Task InitCategories(){

           Categories = await _db.Category.ToListAsync();
           CategoriesDropDownList = new SelectList(Categories, "CategoryId", "CategoryName");
        }

    }
}