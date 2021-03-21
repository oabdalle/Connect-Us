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
  public class EventListingViewModel
  {
  
        public IEnumerable<Event> Events { get; set; }
        public IEnumerable<Category> Categories { get; set; }

        public IEnumerable<EventImage> EventImages { get; set; }

        public EventListingViewModel()
        {
  
        }

  }

}