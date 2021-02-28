using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ConnectUs.Model
{
  public class EventImage
  {

    [Key]
    public int ImageId {get;set;}

    public string Path {get;set;}

  }

}