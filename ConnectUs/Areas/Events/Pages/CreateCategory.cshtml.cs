using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using ConnectUs.Model;
using Microsoft.EntityFrameworkCore;

namespace ConnectUs.Areas.Events.Pages
{
    public class CreateCategoryModel : PageModel
    {
        private readonly ILogger<CreateCategoryModel> _logger;
        private readonly ApplicationDbContext _db;

        
        [BindProperty] //this assumes then onPost you will be getting this event here that the user modified so no need for onPost(Event eventObj)
        public Category Category { get; set; }



        public IEnumerable<Category> Categories { get; set; }
        public SelectList CategoriesDropDownList { get; set; }

        public CreateCategoryModel(ILogger<CreateCategoryModel> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public async Task OnGet()
        {
           await InitCategories();

        }


        public async Task<IActionResult> OnPost() //we use <IActionResult> to direct to new page
        {
            if (ModelState.IsValid)
            {
                await _db.Category.AddAsync(Category);
                await _db.SaveChangesAsync();
                return RedirectToPage("/Index", new { area = "Home" });

            }
            else
            {
                await InitCategories();
                return Page();
            }

        }

        public async Task InitCategories(){

           Categories = await _db.Category.ToListAsync();
           CategoriesDropDownList = new SelectList(Categories, "CategoryId", "CategoryName");
        }
    }
}