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
  public class VideoFiles    
    {    
        public int ID { get; set; }    
        public string Name { get; set; }    
        public Nullable<long> FileSize { get; set; }
        public string FileType { get; set; }      
        public string FilePath { get; set; }
        [ForeignKey("Host")]
        public string HostId {get; set; }
        [ForeignKey("Event")]
        public string EventId {get; set; }    
    }    
}