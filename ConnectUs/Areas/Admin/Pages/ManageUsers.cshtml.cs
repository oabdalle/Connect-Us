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
    public class ManageUsersModel : PageModel
    {
        private readonly ILogger<ManageUsersModel> _logger;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public Dictionary<IdentityUser, IdentityRole> Users {get; set;} 
        public EditRoleViewModel RoleViewModel {get; set;}

        public ManageUsersModel(ILogger<ManageUsersModel> logger, RoleManager<IdentityRole> roleManager, ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task OnGet()
        {
            await InitUsers();
            await InitRoles();
        } 

        public async Task InitUsers(){
            Users = new Dictionary<IdentityUser, IdentityRole>(){};
            var users = await _userManager.Users.ToListAsync();
            foreach (var user in users){
                IList <string> roles =await _userManager.GetRolesAsync(user);
 
                if(roles[0]!=null){
                  IdentityRole role = await _roleManager.FindByNameAsync(roles[0]); 
                  if(role!=null){
                    Users.Add(user, role);
                  }
                }
            }



        }

        public async Task InitRoles(){
         
            
            List<SelectListItem> roleList =  _roleManager.Roles.Select(x => new SelectListItem { Text = x.Name, Value = x.NormalizedName }).ToList();
            IdentityRole role = new IdentityRole();
            RoleViewModel = new EditRoleViewModel{
              Role = role,
              RoleList = roleList 
            };
            
        }

        public async Task<IActionResult> OnPostActivateUser(string email){
          
            if(email != null){
              var user = await _userManager.FindByEmailAsync(email);
              var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
              var result = await _userManager.ConfirmEmailAsync(user, code);

              if(result.Succeeded){
                  await InitUsers();
                  await InitRoles();
                  return Page();
              }
            }
              await InitUsers();
              await InitRoles();
              return Page();
            
        }
        public async Task<IActionResult> OnPostEditUserRole(string roleName, string userId){
          
            if(roleName != null){
    
              var user = await _userManager.FindByIdAsync(userId);
              var role = await _roleManager.FindByNameAsync(roleName);
              var currentRoles= await _userManager.GetRolesAsync(user);
              if(user!=null && role!=null && currentRoles!=null){
                
                   await _userManager.RemoveFromRolesAsync(user, currentRoles.ToList());
                  var result = await _userManager.AddToRoleAsync(user, role.Name);

                  if(result.Succeeded){
                     return new JsonResult("Success");
                  }else{
                    return new JsonResult("Fail");
                  }

              }
              
            }
            
            return new JsonResult("Fail");
            
        }
        
        public async Task<IActionResult> OnPostDeleteUser(string id){
            var user = await _userManager.FindByIdAsync(id);
            if(user == null){
              await InitUsers();
              await InitRoles();
              return Page();
            }else{
              var result = await _userManager.DeleteAsync(user);
              if (result.Succeeded)
              {      
                  await InitUsers();
                  await InitRoles();
                  return RedirectToPage();
              }else{
                  foreach (var error in result.Errors)
                  {
                      ModelState.AddModelError("", error.Description);
                  }  
              }
          }
              await InitUsers();
              await InitRoles();
              return Page();
        }



    }

    public class EditRoleViewModel{

         public IdentityRole Role { get; set; }
    
         public List<SelectListItem> RoleList {get; set;}

         public EditRoleViewModel()
        {
        }
    }

}