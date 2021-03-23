using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ConnectUs.Model;
using ConnectUs.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ConnectUs.Areas.Admin.Pages
{
    [Authorize(Roles = "Admin")]
    public class ManageCategoriesModel : PageModel
    {
        private readonly ILogger<ManageCategoriesModel> _logger;
        private readonly ApplicationDbContext _db;
        public IEnumerable<Category> CategoryList {get; set;}

        public CreateCategoryViewModel CategoryViewModel {get; set;}
        
        public ManageCategoriesModel(ILogger<ManageCategoriesModel> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;

        }

        public async Task<IActionResult> OnGet()
        { 
          await InitCategories();
          return Page();
        }

        public async Task<IActionResult> OnPostDeleteCategory(int id)
        {

          var category = await _db.Category.FindAsync(id);

          if (category == null)
          {
              return NotFound();
          }

          _db.Category.Remove(category);

          await _db.SaveChangesAsync();
          await InitCategories();
          return RedirectToPage();
          
        }

        public async Task<IActionResult> OnPostCreateCategory(string categoryName){

               if(categoryName!=null){

                  Category category = new Category(){CategoryName=categoryName};
                  await _db.Category.AddAsync(category);
                  await _db.SaveChangesAsync();
    
                  return new JsonResult("Success");

               }else{

                  return new JsonResult("Category Name is null");
               }        
        
        }
        public async Task<IActionResult> OnPostEditCategory([FromBody]Category category){


              if (ModelState.IsValid)
              {
                var categoryToEdit = await _db.Category.FindAsync(category.CategoryId);
                categoryToEdit.CategoryName = category.CategoryName;

                await _db.SaveChangesAsync(); 
                
                return new JsonResult("Success");

              }
              else
              {
                return new JsonResult("Category Name is null");
              }
               
        }

          public async Task InitCategories(){

            CategoryList = await _db.Category.ToListAsync(); 
            SelectList categories = null;

            if(CategoryList!=null){
              categories = new SelectList(CategoryList, "CategoryId", "CategoryName");
            }else{
              categories = new SelectList(new List<string>());
            }

            CategoryViewModel = new CreateCategoryViewModel{
              Categories=CategoryList,
              CategoriesDropDownList = categories
            };
          

          }
            
        }


}
