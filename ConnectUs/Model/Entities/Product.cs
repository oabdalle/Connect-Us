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
  public class Product
  {

    [Key]
    public int ProductId {get;set;}
    [Required]    
    public string Name { get; set; }

    [Required]
    public string Price { get; set; }

    [MaxLength]
    [Display(Name = "Product Image")]
    public string ImageBase64{ get; set; }

    [ForeignKey("Event")]    
    public int EventId { get; set; }
    public Event Event { get; set; }
  }

}