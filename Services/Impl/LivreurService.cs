using WebGesCommande.Core.Impl;
using WebGesCommande.Data;
using WebGesCommande.Models;

namespace WebGesCommande.Services.Impl;
public class LivreurService : Service<Livreur>, ILivreurService
{
    WebGesCommandeContext _context;
    public LivreurService(WebGesCommandeContext context) : base(context)
    {
        _context = context;
    }
}
