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
using System.Configuration;
using System.Data.SqlClient;
using System.Web;

namespace ConnectUs.Services
{
    public class EventsService : IEventsService
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public EventDescriptionViewModel EventEntry {get; set;}
        public Chat Chat {get; set;}
        public EventsService(ApplicationDbContext db, UserManager<IdentityUser> userManager){
          _db = db;
          _userManager = userManager;
        } 


        public async Task<EventDescriptionViewModel> InitEvent(int id){

            Event EventToFind = new Event();

            Category category = new Category();

            EventToFind = await _db.Event.FindAsync(id);
            
            category = await _db.Category.FindAsync(EventToFind.CategoryId);

            EventEntry = new EventDescriptionViewModel{
                Event = EventToFind,
                Category = category,
                EventImages= await _db.EventImage.Where(x=>x.EventId == EventToFind.EventId).ToListAsync()

              };
            return EventEntry;
        }

        public async Task<bool> CheckBooking(int id, string userId){

            List<Booking> checkBookings = new List<Booking>();
            var attendeeID = userId;
            checkBookings = await _db.Booking.Where(x=>x.EventId == id && x.UserId == attendeeID).ToListAsync();
            if(checkBookings.Count() > 0)
              return true;
            else
              return false;
      
        }


    }

}