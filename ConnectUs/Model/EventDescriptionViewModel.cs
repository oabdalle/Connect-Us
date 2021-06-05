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
  public class EventDescriptionViewModel
  {
        public Event Event { get; set; }
        
        public Category Category { get; set; }

        public List<EventImage> EventImages { get; set; }
        public bool EventBooked { get; set; }
   
        public EventDescriptionViewModel()
        {
  
        }

  }

}