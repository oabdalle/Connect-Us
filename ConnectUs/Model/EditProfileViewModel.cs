using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ConnectUs.Model.Entities;

namespace ConnectUs.Model
{
  public class EditProfileViewModel
  {
    [Display(Name = "Profile Image")]
    public string Content{ get; set; }
    public IFormFile GalleryFiles { get; set; }
    public EditProfileViewModel()
    {
  
    }

  }

}