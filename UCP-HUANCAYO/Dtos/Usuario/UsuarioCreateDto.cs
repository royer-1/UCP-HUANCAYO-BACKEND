using System.ComponentModel.DataAnnotations;

namespace UCP_HUANCAYO.Dtos.Usuario
{
    public class UsuarioCreateDto
    {
        [Required]
        public Guid IdDominio { get; set; }

        [Required]
        [StringLength(50)]
        public string Alias { get; set; } = string.Empty;

        [Required]
        [StringLength(2)]
        public string DocIdentTipo { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string DocIdentNro { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Nombres { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Correo { get; set; }

        [StringLength(20)]
        public string? Telefono { get; set; }

        [Required]
        public string Clave { get; set; } = string.Empty;

        [Required]
        public Guid IdResponsable { get; set; }
    }
}
