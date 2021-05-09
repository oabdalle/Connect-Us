using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;
namespace ConnectUs.Model.Entities
{
    public class ProductDisplay
    {
        [Key]
        public int DisplayId { get; set; }
        public DateTime Timestamp { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}