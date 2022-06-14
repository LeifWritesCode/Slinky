using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Slinky.Data.Model
{
    public class Audit
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        [Required]
        public AuditEvent AuditEvent { get; set; }

        [ForeignKey(nameof(User))]
        [Required]
        public int UserId { get; set; }

        public User User { get; set; }

        [ForeignKey(nameof(Shortlink))]
        [Required]
        public string ShortLinkId { get; set; }

        public Shortlink Shortlink { get; set; }
    }
}
