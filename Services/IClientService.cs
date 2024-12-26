using WebGesCommande.Core;
using WebGesCommande.Models;

namespace WebGesCommande.Services;

public interface IClientService : IService<Client>
{
    Task<Client?> SelectByLogin(string login);
}