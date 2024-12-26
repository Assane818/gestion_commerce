using Humanizer;
using WebGesCommande.Core.Impl;
using WebGesCommande.Data;
using WebGesCommande.Models;

namespace WebGesCommande.Services.Impl;

public class LivraisonService : Service<Livraison>, ILivraisonService
{
    WebGesCommandeContext _context;
    public LivraisonService(WebGesCommandeContext context) : base(context)
    {
        _context = context;
    }
}