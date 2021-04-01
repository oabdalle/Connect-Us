using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ConnectUs.Model;
using ConnectUs.Model.Entities;
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
        [BindProperty]
        public Category categoryEntry {get; set;}

        public async Task OnGet(int id)
        {
            await InitEvent(id);
        }

        public async Task InitEvent(int id){
            eventEntry  = await _db.Event.FindAsync(id);
            if(eventEntry is not null)
              categoryEntry = await _db.Category.FindAsync(eventEntry.CategoryId);

        }
        public async Task<IActionResult> OnPostDelete(int id)
        {
            var eventToDelete = await _db.Event.FindAsync(id);
            //List<EventImage> eventImages = new List<EventImage>();
            if(eventToDelete == null){
                return NotFound();
            }
            _db.EventImage.RemoveRange(_db.EventImage.Where(x => x.EventId == eventToDelete.EventId));
            _db.Event.Remove(eventToDelete);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Index",  new { area = "Events" });
        }
        public async Task<IActionResult> OnPostBookEvent(int eventId, string userId)
        {   
                            
            Booking booking = new Booking();
            if(eventId !=null & userId !=null){
              var checkBooked = _db.Booking
                                  .Where(x=>x.EventId == eventId)
                                  .Where(x=>x.UserId == userId);
                if(checkBooked == null){
                  await InitEvent(eventId);
                  return RedirectToPage(eventId);
                }
              DateTime newDateTime = DateTime.Now;
              booking.EventId = eventId;
              booking.UserId= userId;
              booking.DateBooked=newDateTime;
              await _db.Booking.AddAsync(booking);
              await _db.SaveChangesAsync();
              return RedirectToPage("/Index",  new { area = "Events" });

            }else{
              return RedirectToPage("/Index",  new { area = "Events" });
            }
        }
    }
}