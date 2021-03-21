using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ConnectUs.Model.Entities
{
  public class Category
  {

    [Key]
    public int CategoryId {get;set;}

    [Required]
    [Display(Name = "Category Name")]
    public string CategoryName {get;set;}

  }

}