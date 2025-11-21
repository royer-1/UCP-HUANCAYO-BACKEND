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
    }
}
