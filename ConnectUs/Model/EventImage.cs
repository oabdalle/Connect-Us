using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;
namespace ConnectUs.Model
{
  public class EventImage
  {

    [Key]
    public int ImageId {get;set;}

    [MaxLength]
    [Display(Name = "Event Image")]
    public string Content{ get; set; }

    [ForeignKey("Event")]    
    public int EventId { get; set; }
    public Event Event { get; set; }
  }

}