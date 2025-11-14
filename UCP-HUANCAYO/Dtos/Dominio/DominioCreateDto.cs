using System.ComponentModel.DataAnnotations;

namespace UCP_HUANCAYO.Dtos.Dominio
{
    public class DominioCreateDto
    {
        [Required]
        [StringLength(50)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public bool Ldap { get; set; }

        [StringLength(50)]
        public string? Servidor { get; set; }

        [StringLength(50)]
        public string? Conexion { get; set; }

        [Required]
        public bool Default { get; set; }

        [Required]
        public Guid IdResponsable { get; set; }
    }
}
