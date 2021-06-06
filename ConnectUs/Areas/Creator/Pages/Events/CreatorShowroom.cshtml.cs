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
using System.Collections.Generic;

namespace ConnectUs.Areas.Creator.Pages.Events
{
       public class CreatorShowroom : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private IEventsService _eventsService;
        private IChatService _chatService;  
        public CreatorShowroom(ApplicationDbContext db, UserManager<IdentityUser> userManager, IChatService chatService, IEventsService eventsService){

            _userManager=userManager;
            _db = db;
            _chatService = chatService;
            _eventsService = eventsService;
        } 

        [BindProperty]
        public EventDescriptionViewModel EventEntry {get; set;}

        [BindProperty]
        public Chat Chat {get; set;}
  
        [BindProperty]
        public bool eventBooked {get; set;}
        public IEnumerable<Product> ProductList {get; set;}
        public Product ProductToDisplay {get; set;}
    
        public int likeCount {get; set;}
        public DateTime today {get; set;}
        public async Task OnGet(int id)
        {
            
            await InitPage(id);      
        }

        public async Task InitPage(int id){

            today = DateTime.Today;
            EventEntry = _eventsService.InitEvent(id).Result;

            if(EventEntry is not null){

              ProductList = await _db.Product.Where(x => x.EventId == EventEntry.Event.EventId).ToListAsync();
              ProductToDisplay = ProductList.FirstOrDefault(x => x.Selected == true);
              likeCount = EventEntry.Event.TotalLikes;

              if(!(await _chatService.CheckRoomExists(id)) && User.Identity.IsAuthenticated){
                    await _chatService.CreateRoom(id, _userManager.GetUserId(User));
                  
                }else if(User.Identity.IsAuthenticated){
                      await _chatService.JoinRoom(id, _userManager.GetUserId(User));
                }
            }



            Chat = _chatService.GetChat(id);

        }
  

        public async Task<IActionResult> OnPostDelete(int id)
        {
            var eventToDelete = await _db.Event.FindAsync(id);
            if(eventToDelete == null){
                return NotFound();
            }
            _db.EventImage.RemoveRange(_db.EventImage.Where(x => x.EventId == eventToDelete.EventId));
            _db.Event.Remove(eventToDelete);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Index",  new { area = "Events" });
        }

        public async Task<IActionResult> OnPostDisplayProduct(int productId){

               if(productId>0){

                  var product = await _db.Product.FindAsync(productId);
                  var productsToReset = await _db.Product.Where(x=>x.EventId==product.EventId).ToListAsync();
                  foreach (var item in productsToReset){
                    item.Selected=false;
                  }
                  product.Selected = true;
                  
                  await _db.SaveChangesAsync();
            
                  return new JsonResult(product);

               }else{

                  return new JsonResult("Failed");
               }        
        
        }
      
    }
}