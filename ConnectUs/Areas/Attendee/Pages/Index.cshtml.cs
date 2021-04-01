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
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace ConnectUs.Areas.Attendee.Pages
{
    [Authorize(Roles = "Attendee")]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private readonly ApplicationDbContext _db;
        
        private readonly UserManager<IdentityUser> _userManager;
      
        public EventListingViewModel EventListing {get; set;}
        
        public List<Event> BookedEvents = new List<Event>();


        public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _db = db;
            _userManager=userManager;
        }

        public async Task OnGet()
        { 

          var user = await _userManager.GetUserAsync(User);
          if(user != null){
            var userBookings = await _db.Booking.Where(x => x.UserId == user.Id).ToListAsync();
            for(int i = 0; i < userBookings.Count; i++){
              var eventToAdd = await _db.Event.FindAsync(userBookings[i].EventId);
              BookedEvents.Add(eventToAdd);
          }
        }
          EventListing = new EventListingViewModel{
                EventImages= await _db.EventImage.ToListAsync(), 
                Events=await _db.Event.ToListAsync(),
                Categories= await _db.Category.ToListAsync(),
                BookedEvents = BookedEvents


            };
        }
        
    }
}
