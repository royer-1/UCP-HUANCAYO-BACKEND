using System.ComponentModel.DataAnnotations;

namespace UCP_HUANCAYO.Dtos.Auditoria
{
    public class AuditoriaCreateDto
    {
        [Required]
        [StringLength(50)]
        public string Tabla { get; set; } = string.Empty;

        [Required]
        public Guid IdRegistro { get; set; }

        [StringLength(80)]
        public string? Accion { get; set; }

        [MaxLength]
        public string? Detalle { get; set; }

        [Required]
        public Guid IdUsuario { get; set; }

        [Required]
        [StringLength(100)]
        public string Conexion { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string ClienteNetAddress { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string SessionId { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string LoginName { get; set; } = string.Empty;
    }
}
