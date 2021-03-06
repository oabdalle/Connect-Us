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
       public class EventDescription : PageModel
    {
         private readonly ApplicationDbContext _db;
        public EventDescription(ApplicationDbContext db){
            _db = db;
        } 
        [BindProperty]
        public Event eventEntry {get; set;}
        public async Task OnGet(int id)
        {
            eventEntry  = await _db.Event.FindAsync(id);
        }
        public async Task<IActionResult> OnPostDelete(int id)
        {
            var eventToDelete = await _db.Event.FindAsync(id);
            if(eventToDelete == null){
                return NotFound();
            }
            _db.Event.Remove(eventToDelete);
            await _db.SaveChangesAsync();
            return RedirectToPage("CreateEvent");
        }
    }
}