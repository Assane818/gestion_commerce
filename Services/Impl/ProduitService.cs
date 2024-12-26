using Microsoft.EntityFrameworkCore;
using WebGesCommande.Core.Impl;
using WebGesCommande.Data;
using WebGesCommande.Models;

namespace WebGesCommande.Services.Impl;

public class ProduitService : Service<Produit>, IProduitService
{
    private readonly WebGesCommandeContext _context;
    public ProduitService(WebGesCommandeContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Produit>> SelectArticlePaginate(int pageNumber, int limit)
    {
        return await _context.Produits
            .Skip((pageNumber - 1) * limit)
            .Take(limit).ToListAsync();
    }
}