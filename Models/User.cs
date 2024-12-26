using WebGesCommande.Enum;

namespace WebGesCommande.Models
{
    public class User : Personne
    {
        public required string login  { get; set; }
        public required string Password { get; set; }
        public Role Role { get; set; }
    }
}