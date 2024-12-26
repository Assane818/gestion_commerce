using Microsoft.EntityFrameworkCore;
using WebGesCommande.Models;

namespace WebGesCommande.Data;


    public class WebGesCommandeContext : DbContext
    {
        public WebGesCommandeContext(DbContextOptions<WebGesCommandeContext> options) : base(options) { }
        public DbSet<Client> Clients { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Commande> Commandes { get; set; }
        public DbSet<Detail> Details { get; set; }
        public DbSet<Livreur> Livreurs { get; set; }
        public DbSet<Payement> Payements { get; set; }
        public DbSet<Produit> Produits { get; set; }
        public DbSet<Livraison> Livraisons { get; set; }
    }