using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UCP_HUANCAYO.Models
{
    [Table("token")]
    public class Token
    {
        [Key]
        [Column("id_token")]
        public Guid IdToken { get; set; }

        [Column("id_usuario")]
        public Guid IdUsuario { get; set; }
        public Usuario? Usuario { get; set; }

        [Column("expiracion")]
        public DateTime Expiracion { get; set; }

        [Column("ip")]
        public string? Ip { get; set; }

        [Column("revocado")]
        public bool Revocado { get; set; }
    }
}

