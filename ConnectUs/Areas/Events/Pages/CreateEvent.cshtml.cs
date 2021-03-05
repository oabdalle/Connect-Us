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


        public EventImage EventImage {get; set;}

        public IEnumerable<Category> Categories { get; set; }

        public SelectList CategoriesDropDownList { get; set; }


        [Required]
        [BindProperty] 
        [Display(Name = "Event Image")]
        public IFormFile Image { get; set; }

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

            if (ModelState.IsValid)
            {  
                await _db.Event.AddAsync(Event);
                await _db.SaveChangesAsync();
                return RedirectToPage("/Index", new { area = "Home" });

            }
            else
            {
                await InitCategories();
                return Page();
            }

        }
    public async Task<IActionResult> OnPostUpload(IFormFile file)
    {

        var FileDic = "EventImages";
        var my= _env;
        ViewData["MyString"] = file.FileName;

        string FilePath = Path.Combine(_env.WebRootPath, FileDic);

        if (!Directory.Exists(FilePath))

            Directory.CreateDirectory(FilePath);

        var fileName = file.FileName;

        var filePath = Path.Combine(FilePath, fileName);

        using (FileStream fs = System.IO.File.Create(filePath))

        {

            file.CopyTo(fs);

        }

        return Page();

    }



        public IActionResult OnPostAddImage(string obj)
            {


             return new JsonResult(obj);

            }

        public async Task InitCategories(){

           Categories = await _db.Category.ToListAsync();
           CategoriesDropDownList = new SelectList(Categories, "CategoryId", "CategoryName");
        }
        private byte[] GetByteArrayFromImage(IFormFile file){
          using (var target = new MemoryStream())
           {
           file.CopyTo(target);
           return target.ToArray();
           }
        }
    }
}