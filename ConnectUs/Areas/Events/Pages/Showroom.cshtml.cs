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

namespace ConnectUs.Areas.Events.Pages
{
       public class Showroom : PageModel
       {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private IEventsService _eventsService;
        private IChatService _chatService;  
        public Showroom(ApplicationDbContext db, UserManager<IdentityUser> userManager,  IChatService chatService, IEventsService eventsService){

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
        public Profile profile {get; set;}
        
        public async Task<IActionResult> OnGet(int id)
        {

            var user = await _userManager.GetUserAsync(HttpContext.User);
            if(user !=null){
              var roles = await _userManager.GetRolesAsync(user);

              var role = roles[0];
              if(role == "Admin" || role == "Creator"){
                return RedirectToPage("/Events/CreatorShowroom",  new { area = "Creator", id = id  }); //on get, redirect creators to the creator showroom
              }
            }
              await InitPage(id);      
              return Page();

        }

        public async Task InitPage(int id){

            today = DateTime.Today;
            EventEntry = _eventsService.InitEvent(id).Result; //use events service to init event description, category, images

            if(EventEntry is not null){

              profile =  await _db.Profile.FirstOrDefaultAsync(x => x.HostId == EventEntry.Event.HostId); //display profile of host
              ProductList = await _db.Product.Where(x => x.EventId == EventEntry.Event.EventId).ToListAsync(); //show all products of event
              ProductToDisplay = ProductList.FirstOrDefault(x => x.Selected == true); //selected product on display
              likeCount = EventEntry.Event.TotalLikes;

              if(!(await _chatService.CheckRoomExists(id))){ //if the chat room doesn't exist and the event has started, create chat
                  if((EventEntry.Event.EventDate <= DateTime.Today) && User.Identity.IsAuthenticated){
                      await _chatService.CreateRoom(id, _userManager.GetUserId(User));
                  }
                }else{
                    if(User.Identity.IsAuthenticated)
                      await _chatService.JoinRoom(id, _userManager.GetUserId(User));
                }
            }

         if(today >= EventEntry.Event.EventDate.Date){ //if the event has started, get the chat needed to join

          Chat = _chatService.GetChat(id);
         }else{
           Chat = null;
         }
          if(User.Identity.IsAuthenticated && User.IsInRole("Attendee")){ //check if attendee has booked this event
              EventEntry.EventBooked= _eventsService.CheckBooking(id, _userManager.GetUserId(User)).Result;
            }else{
              EventEntry.EventBooked = false;
            }

        }
  
        public async Task<IActionResult> OnPostBookEvent(int eventId, string userId) //Allow attendees to book events they would like to attend
        {   
                            
            Booking booking = new Booking();
            if(userId !=null){
               Booking checkExistingBooking = _db.Booking.Where(x=>x.EventId == eventId).Where(x=>x.UserId == userId).FirstOrDefault(); //check if the booking exists
                if(checkExistingBooking != null){
                  return RedirectToPage("/Showroom", new {id = eventId});
                }
              DateTime newDateTime = DateTime.Now;
              booking.EventId = eventId;
              booking.UserId= userId;
              booking.DateBooked=newDateTime; //set booking date 
              await _db.Booking.AddAsync(booking);
              await _db.SaveChangesAsync();

              if(User.Identity.IsAuthenticated && User.IsInRole("Attendee")){ //redirect to correct page
                return RedirectToPage("/Index",  new { area = "Attendee" });
              }
              return RedirectToPage("/Index",  new { area = "Events" });

            }else{
              if(User.Identity.IsAuthenticated && User.IsInRole("Attendee")){
                return RedirectToPage("/Index",  new { area = "Attendee" });
              }
              return RedirectToPage("/Index",  new { area = "Events" });
            }
        }
        public async Task<IActionResult> OnPostDeleteBooking(int eventId, string userId) //Allow attendee to delete booking 
        {   
                            
            Booking booking = new Booking();
            if(userId !=null){
              booking = _db.Booking.Where(x=>x.EventId == eventId).Where(x=>x.UserId == userId).FirstOrDefault(); //check if the booking exists
                if(booking  == null){
                  return RedirectToPage("/Showroom", new {id = eventId});
                }
              _db.Booking.Remove(booking); //if the booking exists, remove it from the booking table
              await _db.SaveChangesAsync();
              return RedirectToPage("/Index",  new { area = "Attendee" });

            }else{
              return RedirectToPage("/Index",  new { area = "Attendee" });
            }
        }

        public async Task<IActionResult> OnPostDisplayProduct(int productId){ //allow attendees after an event has passed to display a product to buy

            if(productId>0){

              var product = await _db.Product.FindAsync(productId);
              var productsToReset = await _db.Product.Where(x=>x.EventId==product.EventId).ToListAsync(); //find all products, reset the Selected field so product display is updated
              foreach (var item in productsToReset){
                item.Selected=false;
              }
              product.Selected = true; //current product to display
              
              await _db.SaveChangesAsync();
        
              return new JsonResult(product); //return the product to display as json

            }else{

              return new JsonResult("Failed");
            }        
    
        }
      
    }
}