using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConnectUs.Model.Entities
{
    public class Chat
    {
        public Chat()
        {
            Messages = new List<Message>();
            Users = new List<ChatUser>();
        }

        [Key]
        public int ChatId { get; set; }

        [ForeignKey("Event")]
        public int EventId { get; set;}
        public ICollection<Message> Messages { get; set; }
        public ICollection<ChatUser> Users { get; set; }
    }
}