using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ConnectUs.Model;
using ConnectUs.Model.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using ConnectUs.Services;

namespace ConnectUs.Areas.Creator.Pages.Events
{

    public class DashboardModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<IdentityUser> _userManager;
        private IChatService _chatService;
        public Dictionary<Event, Chat> EventsChat {get; set;} 
        public Dictionary<string, List<Event>> EventsDict { get; set; }
        public List<Chat> Chats {get; set;}
        public int LikeTotal {get; set;}
        public IEnumerable<EventImage> EventImages { get; set; }
        public DashboardModel(ApplicationDbContext db, IWebHostEnvironment env, UserManager<IdentityUser> um, IChatService chatService)
        {
            _db = db;
            _env=env;
            _userManager = um;
            _chatService = chatService;
        }

        public async Task OnGet(string id)
        {
            await InitItems(id);
        }

        public async Task InitItems(string hostId = null){ //init the events hosted by the creater, and the sentiment from the chat associated with each event
            EventsDict = new Dictionary<string, List<Event>>(){};
            if(hostId == null)
                hostId = _userManager.GetUserId(User);
                
            var events = await _db.Event.Where(x=>x.HostId == hostId).ToListAsync();
            EventImages = await _db.EventImage.ToListAsync();
            List<Event> pastEvents = new List<Event>();
            List<Event> upcomingEvents = new List<Event>();
            List<Event> currentEvents = new List<Event>();
            foreach (var item in events) { //sort events out by upcoming, current and past, and add to a list
                if(item.EventDate > DateTime.Today)
                    upcomingEvents.Add(item);
                else if(item.EventDate == DateTime.Today)
                    currentEvents.Add(item);
                else
                    pastEvents.Add(item);
            }
            EventsDict.Add("Past Events", pastEvents); //add lists to a dict
            EventsDict.Add("Future Events", upcomingEvents);
            EventsDict.Add("Ongoing Events", currentEvents);
            EventsChat = new Dictionary<Event, Chat> (){};

            foreach (var item in events) { 
              var chat = _chatService.GetChat(item.EventId);
              if(chat != null){
                  EventsChat.Add(item, chat); //init event chat dict with the event info as key and the chat as the value
              }
              LikeTotal = item.TotalLikes;
            }
        
        }

    }

}