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
  public class Profile
  {

    [Key]
    public int ImageId {get;set;}

    [MaxLength]

    [Display(Name = "Profile Image")]
    public string Content{ get; set; }
    
    [ForeignKey("Host")]
    public string HostId {get; set; }
    [NotMapped]
    [Required]
    public IFormFile GalleryFiles { get; set; }
  }

}