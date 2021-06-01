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
  public class EditEventViewModel
  {
        public string Name { get; set; }
   
        [DataType(DataType.Date)]
        [Display(Name = "Event Date")]
        public DateTime EventDate { get; set; }
   
        public string Description { get; set; }

        [Display(Name = "Gallery Images")]
        public IFormFileCollection GalleryFiles { get; set; }

        public EditEventViewModel()
        {
  
        }

  }

}