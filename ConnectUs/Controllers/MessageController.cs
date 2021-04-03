using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ConnectUs.Hubs;
using ConnectUs.Model;
using ConnectUs.Model.Entities;
using ConnectUs.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace ConnectUs.Controllers
{
   [ApiController]
    [Route("api/[controller]")]
    public class MessageController : Controller
    {
        private IChatService _chatService;
        private readonly UserManager<IdentityUser> _userManager;

        public MessageController(IChatService chatService, UserManager<IdentityUser> userManager){
          _chatService = chatService;
           _userManager=userManager;
        }

          [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] ChatMessageInputs chatMessage, [FromServices] IHubContext<ChatHub> chat)
        {
            var Message = await _chatService.CreateMessage(chatMessage.roomId, chatMessage.message, ((await _userManager.GetUserAsync(User)).UserName));
          
            await chat.Clients.Group(chatMessage.roomId.ToString())
                .SendAsync("RecieveMessage", new
                {
                    Text = Message.Text,
                    Name = Message.Name,
                    Timestamp = Message.Timestamp.ToString("dd/MM/yyyy hh:mm:ss")
                });
            return new JsonResult("Success");
        }




    }

    public class ChatMessageInputs
    {
       
        public int roomId { get; set; }

        public string message { get; set; }
    }

}
