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
namespace ConnectUs.Areas.Admin.Pages
{
    [Authorize(Roles = "Admin")]
    public class RolesModel : PageModel
    {
        private readonly ILogger<RolesModel> _logger;

        private readonly ApplicationDbContext _db;
      
    
        
        public RolesModel(ILogger<RolesModel> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public async Task OnGet()
        { 
  
        }
        
    }
}
