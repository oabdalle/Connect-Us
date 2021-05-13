using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConnectUs.Model.Entities
{
    public class Like
    {
        [Key]
        public int LikeId { get; set; }

        [ForeignKey("Event")]
        public int EventId { get; set;}
        [ForeignKey("User")]
        public string UserId { get; set; }
        public bool Liked {get; set;}
    }
}