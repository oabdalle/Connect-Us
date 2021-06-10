using System.Collections.Generic;
using System.Threading.Tasks;
using ConnectUs.Model;
using ConnectUs.Model.Entities;
using Microsoft.AspNetCore.Http;
namespace ConnectUs.Services
{
    public interface IImageService
    {
        Task<string> Create(IFormFile ImageFile);
        Task<bool> Delete(string path);

    }
}