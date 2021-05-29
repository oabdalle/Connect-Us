using System.Collections.Generic;
using System.Threading.Tasks;
using ConnectUs.Model;
using ConnectUs.Model.Entities;

namespace ConnectUs.Services
{
    public interface IEventsService
    {
        Task<EventDescriptionViewModel> InitEvent(int id);
        Task<bool> CheckBooking(int id, string userId);

    }
}