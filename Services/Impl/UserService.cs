using Microsoft.EntityFrameworkCore;
using WebGesCommande.Core.Impl;
using WebGesCommande.Data;
using WebGesCommande.Models;
using WebService.Services;

namespace WebGesCommande.Services.Impl;

public class UserService : Service<User>, IUserService
{
    private readonly WebGesCommandeContext _context;
    public UserService(WebGesCommandeContext context) : base(context)
    {
        _context = context;
    }

    public async Task<User?> Authenticate(string login, string password)
    {
        return await _context.Users.Where(u => u.login == login && u.Password == password).FirstOrDefaultAsync();
    }
}