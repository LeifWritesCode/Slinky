using System.ComponentModel.DataAnnotations;

namespace Slinky.Data.Model
{
    public class Shortlink
    {
        [Key]
        [MaxLength(12)]
        [Required]
        public string Id { get; set; }

        [Required]
        public Uri Uri { get; set; }

        [Required]
        public int UserId { get; set; }

        public User User { get; set; }

        public List<Audit> Audits { get; set; }
    }
}
