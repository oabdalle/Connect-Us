using System.Collections.Generic;
using System.Threading.Tasks;
using ConnectUs.Model;
using ConnectUs.Model.Entities;

namespace ConnectUs.Services
{
    public interface IChatService
    {
      
        Task CreateRoom(int eventId, string userId);
        Task<bool> CheckRoomExists(int eventId);
        Task JoinRoom(int eventId, string userId);
        Chat GetChat(int eventId);
        Task<Message> CreateMessage(int chatId, string message, string username);
        Task<int> AddLike(int chatId, int message, string username);

    }
}