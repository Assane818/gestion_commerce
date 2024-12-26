using WebGesCommande.Enum;
using WebGesCommande.Models;

namespace WebGesCommande.Data.Fixtures
{
    public class DataFixtures
    {
        private readonly WebGesCommandeContext _context;

        public DataFixtures(WebGesCommandeContext context)
        {
            _context = context;
        }

        public void LoadData()
        {
            if (!_context.Users.Any())
            {
                var users = new List<User>();
                var roles = new Role[] { Role.RS, Role.COMPTABLE };
                var clients = new List<Client>();

                // Création d'une instance de Random
                Random random = new Random();

                // Ajout des utilisateurs
                for (int i = 0; i < 5; i++)
                {
                    users.Add(new User
                    {
                        Nom = $"Nom{i}",
                        Prenom = $"Prenom{i}",
                        Telephone = $"{77100101}+{i}",
                        login = $"email{i}@gmail.com",
                        Password = "Password" + i,
                        Role = roles[i % roles.Length],
                    });
                }

                // Ajout des clients avec leurs commandes
                for (int i = 0; i < 5; i++)
                {
                    clients.Add(new Client
                    {
                        Address = $"Address{i}",
                        User = new User
                        {
                            Nom = $"Nom{i}",
                            Prenom = $"Prenom{i}",
                            Telephone = $"{77100101}+{i}",
                            login = $"client{i}@gmail.com",
                            Password = "Client" + i,
                            Role = Role.CLIENT,
                        },
                        Commandes = new List<Commande>()
                    });
                }

                // Ajout des commandes et détails
                for (int i = 0; i < 5; i++)
                {
                    var client = new Client
                    {
                        Address = $"Address{i}",
                        User = new User
                        {
                            Nom = $"Nom{i}",
                            Prenom = $"Prenom{i}",
                            Telephone = $"{77100101}+{i}",
                            login = $"client{i}@gmail.com",
                            Password = "Client" + i,
                            Role = Role.CLIENT,
                        },
                        Commandes = new List<Commande>()
                    };

                    for (int j = 0; j < 2; j++)
                    {
                        var commande = new Commande
                        {
                            Total = random.Next(1000, 10000),  // Utilisation de la même instance de Random
                            Details = new List<Detail>()
                        };

                        for (int k = 0; k < 1; k++)
                        {
                            var detail = new Detail
                            {
                                Produit = new Produit
                                {
                                    Libelle = $"Produit{random.Next(1, 10)}",  // Utilisation de la même instance de Random
                                    Quantite = random.Next(1, 100),
                                    Prix = random.Next(100, 1000),
                                    Image = "https://cdn.pixabay.com/photo/2020/03/06/16/16/cornflakes-4907490_1280.jpg",
                                },
                                QteCommande = random.Next(1, 30)  // Utilisation de la même instance de Random
                            };

                            commande.Details.Add(detail);
                        }

                        client.Commandes.Add(commande);
                    }

                    clients.Add(client);
                }

                // Ajout des utilisateurs et des clients dans le contexte
                _context.Users.AddRange(users);
                _context.Clients.AddRange(clients);
                _context.SaveChanges();
            }
        }
    }
}



