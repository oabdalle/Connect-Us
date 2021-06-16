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
namespace ConnectUs.Services
{
  public class ImageService : IImageService
  {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ImageService(ApplicationDbContext db, IWebHostEnvironment hostEnvironment)
        {
            _db = db;
            this._hostEnvironment = hostEnvironment;
        }


        public async Task<string> Create(IFormFile ImageFile)
        {
            if (ImageFile != null)
            {
                //save image to wwwroot folder
                string wwwrootPath = _hostEnvironment.WebRootPath;
                string FileName = Path.GetFileNameWithoutExtension(ImageFile.FileName);
                string extension = Path.GetExtension(ImageFile.FileName);
                var FullFileName = FileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwrootPath + "/uploads/" + FullFileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(fileStream);
                }
                return FullFileName;
            }
            return null;
        }

        public bool Delete(string path)
        {

          var _dir =  _hostEnvironment.WebRootPath;
          var FullPath = Path.Combine(_dir, "uploads", path);
           try    
            {     
              if (FullPath != null)
              {
                if(System.IO.File.Exists(FullPath)){
                  System.IO.File.Delete(FullPath); 
                  return true;
                }  
              }
            }catch (IOException ioExp)  {
               Console.WriteLine("IOException source: {0}", ioExp.Source);
            }
            return false;
        }
    }
}