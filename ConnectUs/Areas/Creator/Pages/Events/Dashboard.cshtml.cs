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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using ConnectUs.Services;

namespace ConnectUs.Areas.Creator.Pages.Events
{

    public class DashboardModel : PageModel
    {
        private readonly ILogger<DashboardModel> _logger;
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<IdentityUser> _userManager;
        private IChatService _chatService;
        public Dictionary<Event, Chat> EventsChat {get; set;} 
        public List<Event> Events { get; set; }
        public List<Chat> Chats {get; set;}
        public DashboardModel(ILogger<DashboardModel> logger, ApplicationDbContext db, IWebHostEnvironment env, UserManager<IdentityUser> um, IChatService chatService)
        {
            _logger = logger;
            _db = db;
            _env=env;
            _userManager = um;
            _chatService = chatService;
        }

        public async Task OnGet()
        {
           await InitItems();

        }

        public async Task InitItems(){
            var hostId = _userManager.GetUserId(User);
            var events = await _db.Event.Where(x=>x.HostId == hostId).ToListAsync();
            
            EventsChat = new Dictionary<Event, Chat> (){};

            foreach (var item in events) { 
              var chat = _chatService.GetChat(item.EventId);
              if(chat != null){
                  EventsChat.Add(item, chat);
              }
          
            }
        
        }

    }

}