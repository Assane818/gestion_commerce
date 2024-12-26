using WebGesCommande.Core;
using WebGesCommande.Models;

namespace WebService.Services
{
    public interface IUserService : IService<User>
    {
        Task<User?> Authenticate(string login, string password);
    }
}