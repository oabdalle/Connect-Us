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
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using ConnectUs.Services;
using ConnectUs.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace ConnectUs.Areas.Events.Pages
{
       public class EventDescription : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private IChatService _chatService;
        public EventDescription(ApplicationDbContext db, UserManager<IdentityUser> userManager, IChatService chatService){

            _userManager=userManager;
            _db = db;
            _chatService = chatService;
        } 

        [BindProperty]
        public Event eventEntry {get; set;}
        [BindProperty]
        public Category categoryEntry {get; set;}

        [BindProperty]
        public Chat Chat {get; set;}
  
        [BindProperty]
        public bool eventBooked {get; set;}

        public async Task OnGet(int id)
        {
            
            await InitEvent(id);      
        }

        public async Task InitEvent(int id){
            eventEntry  = await _db.Event.FindAsync(id);
            if(eventEntry is not null)
              categoryEntry = await _db.Category.FindAsync(eventEntry.CategoryId);
            
            if(!(await _chatService.CheckRoomExists(id))){
              if((eventEntry.EventDate > DateTime.Now) && User.Identity.IsAuthenticated){
                  await _chatService.CreateRoom(id, _userManager.GetUserId(User));
              }
            }else{
                if(User.Identity.IsAuthenticated)
                  await _chatService.JoinRoom(id, _userManager.GetUserId(User));
            }

            Chat = _chatService.GetChat(id);

            if(User.Identity.IsAuthenticated && User.IsInRole("Attendee")){
                var checkBooked = _db.Booking
                                  .FirstOrDefault(x=>x.EventId == id && (x.UserId == _userManager.GetUserId(User)));
                                
                if(checkBooked != null){
                  eventBooked=true;     
                }else{
                  eventBooked=false;
                }
            }
           
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
            if(userId !=null){
                var checkBooked = _db.Booking
                                  .FirstOrDefault(x=>x.EventId == eventId && (x.UserId == userId));
                if(checkBooked != null){
             
                  return RedirectToPage("/EventDescription", new {id = eventId});
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