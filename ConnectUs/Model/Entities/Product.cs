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
    [Display(Name = "Price ($)")]
    [RegularExpression(@"^\d+\.\d{0,2}$", ErrorMessage="Please enter a decimal amount (ex. 10.00)")]
    [Range(0, 9999999999999999.99)]
    public decimal Price { get; set; }

    [MaxLength]
    [Display(Name = "Product Image")]
    public string ImageBase64{ get; set; }

    [ForeignKey("Event")]    
    public int EventId { get; set; }
    public Event Event { get; set; }
    public string VideoBase64{get; set;}
  }

}