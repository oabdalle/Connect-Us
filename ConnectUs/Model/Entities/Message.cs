using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;
namespace ConnectUs.Model.Entities
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }

        public int ChatId { get; set; }
        public Chat Chat { get; set; }
    }
}