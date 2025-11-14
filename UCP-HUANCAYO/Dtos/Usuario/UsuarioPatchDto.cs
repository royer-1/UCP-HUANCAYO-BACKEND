using System.ComponentModel.DataAnnotations;

namespace UCP_HUANCAYO.Dtos.Usuario
{
    public class UsuarioPatchDto
    {
        [StringLength(50)]
        public string? Alias { get; set; }

        [StringLength(200)]
        public string? Nombres { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string? Correo { get; set; }

        [StringLength(20)]
        public string? Telefono { get; set; }

        public string? Clave { get; set; }
    }
}
