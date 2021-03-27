using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
namespace ConnectUs.Model.Entities
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
    public string Description { get; set; }
    
    [Required]
    [ForeignKey("Category")]    
    public int CategoryId { get; set; }
    
    public Category Category { get; set; }

   [Display(Name = "Gallery Images")]
   [NotMapped]
   [Required]
    public IFormFileCollection GalleryFiles { get; set; }

    [NotMapped]
    public List<EventImage> EventGallery { get; set; }

    [Required]
    public string HostId { get; set; }
  }

}