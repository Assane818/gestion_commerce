using System.ComponentModel.DataAnnotations.Schema;

namespace WebGesCommande.Models
{
    public abstract class AbstractEntity
    {
        public int Id { get; set; }
        [Column("create_at")]
        public DateTime CreateAt { get; set; } = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
        [Column("update_at")]
        public DateTime UpdateAt { get; set; } = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
    }
}