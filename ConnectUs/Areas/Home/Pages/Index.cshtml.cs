using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ConnectUs.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
namespace ConnectUs.Areas.Home.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private readonly ApplicationDbContext _db;
        
        public IEnumerable<Event> Events { get; set; }
        
        public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public async Task OnGet()
        { 
            Events = await _db.Event.ToListAsync();
            List<Event> EventList = new List<Event>(Events);
            for(int i = 0; i < Events.Count(); i++){
                var imgHolder = await _db.EventImage.Where(x => x.EventId == (EventList[i].EventId)).ToListAsync();
                EventList[i].EventGallery = imgHolder;
            }
        }
        
    }
}
