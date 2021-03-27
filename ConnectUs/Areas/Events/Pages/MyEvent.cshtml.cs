using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using ConnectUs.Model;
using ConnectUs.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace ConnectUs.Areas.Events.Pages
{
    public class MyEventModel : PageModel
    { 
        private readonly ILogger<MyEventModel> _logger;

        private readonly ApplicationDbContext _db;
        
        public EventListingViewModel EventCategory {get; set;}
        public MyEventModel(ILogger<MyEventModel> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public async Task OnGet()
        {
            EventCategory = new EventListingViewModel{
                EventImages= await _db.EventImage.ToListAsync(), 
                Events=await _db.Event.ToListAsync(),
                Categories= await _db.Category.ToListAsync()
            };
        }
    }
}