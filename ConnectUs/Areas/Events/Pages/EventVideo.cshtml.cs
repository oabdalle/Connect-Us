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
namespace ConnectUs.Areas.Events.Pages
{
    public class EventVideoModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private readonly ApplicationDbContext _db;
        

        private readonly UserManager<IdentityUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public List<VideoFiles> videolist = new List<VideoFiles>();
        
        public EventVideoModel(ILogger<IndexModel> logger, ApplicationDbContext db, UserManager<IdentityUser> userManager, IWebHostEnvironment env)
        {
            _logger = logger;
            _db = db;
            _userManager = userManager;
            _env = env;
        }
        public string _dir;
        public async Task OnGet()
        {   
            videolist = await _db.VideoFiles.Where(x=>x.HostId == _userManager.GetUserId(User)).ToListAsync();
            byte[] bytes;
            string fileName, contentType;
            bytes = System.Text.Encoding.UTF8.GetBytes(videolist[0].FilePath);
            contentType = videolist[0].FileType;
            fileName = videolist[0].Name;
            //return File(bytes, contentType, fileName);
            ViewData["vidName"] = "file.mp4";
        }

        
        public async Task<IActionResult> OnPostUploadVideo([FromForm]IFormFile fileupload)    
        {   
            _dir =  _env.WebRootPath;
            if (fileupload != null)    
            {   
              /*VideoFiles videoFile = new VideoFiles();
              using (var ms = new MemoryStream())
                    {
                        fileupload.OpenReadStream().CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        string s = Convert.ToBase64String(fileBytes);
                        videoFile.FilePath = s;
                        videoFile.Name = fileupload.FileName;
                        videoFile.FileSize = fileupload.Length;
                        videoFile.FileType = fileupload.ContentType;
                        videoFile.HostId = _userManager.GetUserId(User);
                        videoFile.EventId = "2";
                        await _db.VideoFiles.AddAsync(videoFile);
                        await _db.SaveChangesAsync();
                         return RedirectToPage("/EventVideo", new { are a = "Events" });
                    } */
                var uploads = Path.Combine(_dir, "uploads");
                var uniqueFileName = GetUniqueFileName(fileupload.FileName);
                ViewData["vidName"] = uniqueFileName;
                using(var fileStream =
                    new FileStream(Path.Combine(_dir, uniqueFileName), FileMode.Create, FileAccess.Write)){
                        fileupload.CopyTo(fileStream);
                    }
            }  
           return RedirectToPage("/EventVideo", new { area = "Events" });    
        }    
        /*public FileResult DownloadFile(int? fileId){
            videolist = _db.VideoFiles.Where(x=>x.HostId == _userManager.GetUserId(User)).ToListAsync();
            byte[] bytes;
            string fileName, contentType;
            //bytes = (byte[])videolist[0].FilePath;
            //contentType = videolist[0].FileSize;
            fileName = videolist[0].Name;
            return File(fileName);
        }*/
        private string GetUniqueFileName(string fileName)
            {
                fileName = Path.GetFileName(fileName);
                return  Path.GetFileNameWithoutExtension(fileName)
                        + "_" 
                        + Guid.NewGuid().ToString().Substring(0, 4) 
                        + Path.GetExtension(fileName);
            }
    }
}
