using WebGesCommande.Core;
using WebGesCommande.Enum;
using WebGesCommande.Models;

namespace WebGesCommande.Services;

public interface ICommandeService : IService<Commande>
{
    Task<IEnumerable<Commande>> SelectByClient(int idClient);
    Task<IEnumerable<Commande>> SelectByClientAndEtat(int idClient, EtatCommande etat);
    Task<IEnumerable<Commande>> SelectByClientAndPayement(int idClient, bool paye);
    Task<IEnumerable<Commande>> SelectByClientAndFitreAll(int idClient, EtatCommande etat ,bool paye);
    Task<IEnumerable<Commande>> SelectByClientAndMonth(int idClient);
    Task<IEnumerable<Commande>> SelectByClientPaginate(int idClient, int pageNumber, int limit);
    Task<IEnumerable<Commande>> SelectByClientAndEtatPaginate(int idClient, EtatCommande etat, int pageNumber, int limit);
    Task<IEnumerable<Commande>> SelectByClientAndPayementPaginate(int idClient, bool paye, int pageNumber, int limit);
    Task<IEnumerable<Commande>> SelectByClientAndFitreAllPaginate(int idClient, EtatCommande etat ,bool paye, int pageNumber, int limit);
    Task<IEnumerable<Commande>> SelectByEtat(EtatCommande etatCommande);
    Task<IEnumerable<Commande>> SelectByEtatPaginate(EtatCommande etatCommande, int pageNumber, int limit);
}