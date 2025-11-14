using System.ComponentModel.DataAnnotations;

namespace UCP_HUANCAYO.Dtos.Token
{
    public class TokenCreateDto
    {
        [Required]
        public Guid IdUsuario { get; set; }

        [Required]
        public DateTime Expiracion { get; set; }

        [StringLength(80)]
        public string? Ip { get; set; }
    }
}
