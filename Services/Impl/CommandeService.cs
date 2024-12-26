using Microsoft.EntityFrameworkCore;
using WebGesCommande.Core.Impl;
using WebGesCommande.Data;
using WebGesCommande.Enum;
using WebGesCommande.Models;

namespace WebGesCommande.Services.Impl;

public class CommandeService : Service<Commande>, ICommandeService
{
    private readonly WebGesCommandeContext _context;
    public CommandeService(WebGesCommandeContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Commande>> SelectByClient(int idClient)
    {
        return await _context.Commandes.Include(c => c.Client).Where(c => c.Client.Id == idClient).ToListAsync();
    }

    public async Task<IEnumerable<Commande>> SelectByClientAndEtat(int idClient, EtatCommande etat)
    {
        return await _context.Commandes.Include(c => c.Client)
                                    .Where(c => c.Client.Id == idClient)
                                    .Where(c => c.EtatCommande == etat)
                                    .ToListAsync();
    }

    public async Task<IEnumerable<Commande>> SelectByClientAndEtatPaginate(int idClient, EtatCommande etat, int pageNumber, int limit)
    {
        return await _context.Commandes.Include(c => c.Client)
                                    .Where(c => c.Client.Id == idClient)
                                    .Where(c => c.EtatCommande == etat)
                                    .Skip((pageNumber - 1) * limit)
                                    .Take(limit)
                                    .ToListAsync();
    }

    public async Task<IEnumerable<Commande>> SelectByClientAndFitreAll(int idClient, EtatCommande etat, bool paye)
    {
        return await _context.Commandes.Include(c => c.Client)
                                    .Where(c => c.Client.Id == idClient)
                                    .Where(c => c.EtatCommande == etat)
                                    .Where(c => c.IsPaye == paye)
                                    .ToListAsync();
    }

    public async Task<IEnumerable<Commande>> SelectByClientAndFitreAllPaginate(int idClient, EtatCommande etat, bool paye, int pageNumber, int limit)
    {
        return await _context.Commandes.Include(c => c.Client)
                                    .Where(c => c.Client.Id == idClient)
                                    .Where(c => c.EtatCommande == etat)
                                    .Where(c => c.IsPaye == paye)
                                    .Skip((pageNumber - 1) * limit)
                                    .Take(limit)
                                    .ToListAsync();
    }

    public async Task<IEnumerable<Commande>> SelectByClientAndMonth(int idClient)
    {
        return await _context.Commandes.Include(c => c.Client)
                                    .Where(c => c.Client.Id == idClient)
                                    .Where(c => c.CreateAt.Month == DateTime.Now.Month && c.CreateAt.Year == DateTime.Now.Year)
                                    .ToListAsync();
    }

    public async Task<IEnumerable<Commande>> SelectByClientAndPayement(int idClient, bool paye)
    {
        return await _context.Commandes.Include(c => c.Client)
                                    .Where(c => c.Client.Id == idClient)
                                    .Where(c => c.IsPaye == paye)
                                    .ToListAsync();
    }

    public async Task<IEnumerable<Commande>> SelectByClientAndPayementPaginate(int idClient, bool paye, int pageNumber, int limit)
    {
        return await _context.Commandes.Include(c => c.Client)
                                    .Where(c => c.Client.Id == idClient)
                                    .Where(c => c.IsPaye == paye)
                                    .Skip((pageNumber - 1) * limit)
                                    .Take(limit)
                                    .ToListAsync();
    }

    public async Task<IEnumerable<Commande>> SelectByClientPaginate(int idClient, int pageNumber, int limit)
    {
        return await _context.Commandes.Include(c => c.Client)
                                    .Where(c => c.Client.Id == idClient)
                                    .Skip((pageNumber - 1) * limit)
                                    .Take(limit)
                                    .ToListAsync();
    }

    public async Task<IEnumerable<Commande>> SelectByEtat(EtatCommande etatCommande)
    {
        return await _context.Commandes.Include(c => c.Client).Where(c => c.EtatCommande == etatCommande).ToListAsync();
    }

    public async Task<IEnumerable<Commande>> SelectByEtatPaginate(EtatCommande etatCommande, int pageNumber, int limit)
    {
        return await _context.Commandes.Include(c => c.Client)
                                 .Where(c => c.EtatCommande == etatCommande)
                                 .Skip((pageNumber - 1) * limit)
                                 .Take(limit)
                                 .ToListAsync();
    }

    public new async Task<Commande?> SelectById(int id)
{
    return await _context.Commandes.Include(c => c.Client)
                                   .ThenInclude(client => client.User)
                                   .Include(c => c.Details)
                                   .ThenInclude(detail => detail.Produit)
                                   .FirstOrDefaultAsync(c => c.Id == id);
}
}