using WebGesCommande.Core.Impl;
using WebGesCommande.Data;
using WebGesCommande.Models;
using Microsoft.EntityFrameworkCore;

namespace WebGesCommande.Services.Impl;

public class ClientService : Service<Client>, IClientService
{
    private readonly WebGesCommandeContext _context;
    public ClientService(WebGesCommandeContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Client?> SelectByLogin(string login)
    {
        return await _context.Clients.FirstOrDefaultAsync(c => c.User.login == login);
    }
}