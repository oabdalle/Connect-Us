
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConnectUs.Model;
using ConnectUs.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConnectUs.Services
{
    public class ChatService : IChatService
    {
        private readonly ApplicationDbContext _db;

        public ChatService(ApplicationDbContext db){
          _db = db;
        } 

        public async Task CreateRoom(int eventId, string userId)
        {
            var chat = new Chat
            {
                EventId = eventId
            };

            chat.Users.Add(new ChatUser
            {
                UserId = userId,
            });

            await _db.Chat.AddAsync(chat);

            await _db.SaveChangesAsync();
        }
        
        public async Task<bool> CheckRoomExists(int eventId)
        {
            var room =  _db.Chat.FirstOrDefault(x => x.EventId == eventId);

            if(room == null)
              return false;
            else
              return true;
        }

        public async Task JoinRoom(int eventId, string userId)
        {   
            var room = _db.Chat.FirstOrDefault(x => x.EventId == eventId);

            var checkUser = _db.ChatUser.FirstOrDefault(x => x.UserId == userId);
            if(checkUser == null){
              var chatUser = new ChatUser
              {
                  ChatId = room.ChatId,
                  UserId = userId,
              };

              await _db.ChatUser.AddAsync(chatUser);

              await _db.SaveChangesAsync();
            }

        }
      
      public Chat GetChat(int eventId)
      {
            return _db.Chat
                .Include(x => x.Messages)
                .FirstOrDefault(x => x.EventId == eventId);
      }

      public async Task<Message> CreateMessage(int chatId, string message, string username)
        {
            var Message = new Message
            {
                ChatId = chatId,
                Text = message,
                Name = username,
                Timestamp = DateTime.Now
            };

            await _db.Message.AddAsync(Message);
            await _db.SaveChangesAsync();

            return Message;
        }



    }

}