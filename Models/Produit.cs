using System.ComponentModel.DataAnnotations.Schema;

namespace WebGesCommande.Models;

public class Produit : AbstractEntity
{
    public string Libelle { get; set; }
    public double Prix { get; set; }
    public double Quantite { get; set; }
    public double QuantiteSeuil { get; set; }
    public string? Image { get; set; }
    [NotMapped]
    public IFormFile? ImageFile { get; set; }
    public ICollection<Detail>? Details { get; set; }
}