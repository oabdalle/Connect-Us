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
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using ConnectUs.Services;

namespace ConnectUs.Areas.Creator.Pages.Events
{
    public class PostData
    {
        public string Result {get; set;}
    }
    public class EditEvent : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly IImageService _is;
        public EditEvent(ApplicationDbContext db, IImageService iS){
            _db = db;
            _is = iS;
        } 
        
        public Event eventEntry {get; set;}
        [BindProperty]
        public Category categoryEntry {get; set;}
        [BindProperty]
        public EditEventViewModel EventViewModel {get; set;}
        public List<EventImage> eventsImages {get; set;}


        public async Task OnGet(int id)
        {
            eventEntry  = await _db.Event.FindAsync(id);
            EventViewModel = new EditEventViewModel{
              Name= eventEntry.Name,
              EventDate = eventEntry.EventDate,
              Description = eventEntry.Description,
              GalleryFiles = eventEntry.GalleryFiles
            };
            eventsImages = await _db.EventImage.Where(x => x.EventId == id).ToListAsync();
            if(eventEntry is not null){
            categoryEntry = await _db.Category.FindAsync(eventEntry.CategoryId);
            }
        } 

        public async Task<IActionResult> OnPostRemoveImage(int imageid){
            var imageToDelete = await _db.EventImage.FindAsync(imageid);
            _is.Delete(imageToDelete.ImagePath);
            if(imageToDelete.Cover==true){
               var setCoverImage = await _db.EventImage.FindAsync(imageid+1);
               setCoverImage.Cover=true;
            }
            _db.EventImage.Remove(imageToDelete);
            await _db.SaveChangesAsync();
            eventsImages = await _db.EventImage.Where(x => x.EventId == imageToDelete.EventId).ToListAsync();
            return RedirectToPage("/Events/EditEvent",  new { area = "Creator", id = imageToDelete.EventId});
        }

          public async Task<IActionResult> OnPostSetCover(int imageid){
            
            var imageToCover = await _db.EventImage.FindAsync(imageid);
            var imageToReset = await _db.EventImage.FirstOrDefaultAsync(x => x.EventId == imageToCover.EventId && x.Cover == true);
            imageToCover.Cover = true;
            imageToReset.Cover = false;
            await _db.SaveChangesAsync();

            eventsImages = await _db.EventImage.Where(x => x.EventId == imageToCover.EventId).ToListAsync();
           
            return RedirectToPage("/Events/EditEvent",  new { area = "Creator", id = imageToCover.EventId});
        }

        public async Task<IActionResult> OnPostAlterEvent(int id)
        {

            if(ModelState.IsValid)
            {
                eventEntry  = await _db.Event.FindAsync(id);
                var eventToUpdate = await _db.Event.FindAsync(eventEntry.EventId);
                var categoryToUpdate =  await _db.Category.FindAsync(eventEntry.CategoryId);
                eventToUpdate.Name = EventViewModel.Name;
                eventToUpdate.EventDate = EventViewModel.EventDate;
                eventToUpdate.Description = EventViewModel.Description;
                categoryToUpdate.CategoryName = categoryEntry.CategoryName;
                eventToUpdate.EventGallery = new List<EventImage>();

                if(EventViewModel.GalleryFiles != null){
                    foreach (var file in EventViewModel.GalleryFiles){
                    {

                        var filename = _is.Create(file).Result;
                        if(filename!=null){
                        
                            eventToUpdate.EventGallery.Add(new EventImage()
                            {
                                ImagePath = filename,
                                Event = eventToUpdate
                            });
                        }
                    
                    }
                    }
                }
                foreach (EventImage img in eventToUpdate.EventGallery){
                    await _db.EventImage.AddAsync(img);
                }
                await _db.SaveChangesAsync();
                return RedirectToPage("/Events/EditEvent",  new { area = "Creator", id = id});
            }
            return RedirectToPage("/Index",  new { area = "Events" });
        }
      
      
    }
}