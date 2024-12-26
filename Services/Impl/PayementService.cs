using WebGesCommande.Core.Impl;
using WebGesCommande.Data;
using WebGesCommande.Models;

namespace WebGesCommande.Services.Impl;

public class PayementService : Service<Payement>, IPayementService
{
    WebGesCommandeContext _context;
    public PayementService(WebGesCommandeContext context) : base(context)
    {
        _context = context;
    }
}