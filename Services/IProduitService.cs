using WebGesCommande.Core;
using WebGesCommande.Models;

namespace WebGesCommande.Services;

public interface IProduitService : IService<Produit>
{
    Task<IEnumerable<Produit>> SelectArticlePaginate(int pageNumber, int limit);
}