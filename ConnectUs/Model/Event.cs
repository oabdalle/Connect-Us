using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;
namespace ConnectUs.Model
{
  public class Event
  {

    [Key]
    public int EventId { get; set; }

    [Required]
    public string Name { get; set; }
   
    [DataType(DataType.Date)]
    [Display(Name = "Event Date")]
    public DateTime EventDate { get; set; }

    
    [Required]
    public string Category { get; set; }
   
    [Required]
    public string Description { get; set; }

    [Required]
    public int HostID { get; set; }

    [Required]
    public List<EventImage> ImagePath { get;set; }

  }

}