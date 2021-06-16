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

          var category = await _db.Category.FindAsync(id); //find the category to delete

          if (category == null)
          {
              return NotFound();
          }

          _db.Category.Remove(category); //remove the category from the Category table and save changes

          await _db.SaveChangesAsync();
          await InitCategories();
          return RedirectToPage();
          
        }

        public async Task<IActionResult> OnPostCreateCategory(string categoryName){

               if(categoryName!=null){

                  Category category = new Category(){CategoryName=categoryName}; //create a new category with the categroy name
                  await _db.Category.AddAsync(category); //add and save the category to the db
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
                categoryToEdit.CategoryName = category.CategoryName; //find the category and edit the category name

                await _db.SaveChangesAsync();  //save the changes
                
                return new JsonResult("Success");

              }
              else
              {
                return new JsonResult("Category Name is null");
              }
               
        }

          public async Task InitCategories(){

            CategoryList = await _db.Category.ToListAsync();  //find all categories
            SelectList categories = null;

            if(CategoryList!=null){
              categories = new SelectList(CategoryList, "CategoryId", "CategoryName"); //save the categories to the select list
            }else{
              categories = new SelectList(new List<string>());
            }

            CategoryViewModel = new CreateCategoryViewModel{ //init the viewmodel with the category list and category select list
              Categories=CategoryList,
              CategoriesDropDownList = categories
            };
          

          }
            
        }


}
