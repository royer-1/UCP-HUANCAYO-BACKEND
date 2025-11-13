using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UCP_HUANCAYO.Dtos.CronogramaPago
{
    public class CronogramaPagoCreateDto
    {
        [Required]
        public Guid IdContrato { get; set; }

        [Required]
        public DateTime PeriodoDesde { get; set; }

        [Required]
        public DateTime PeriodoHasta { get; set; }

        [StringLength(25)]
        public string? Ci { get; set; }

        public DateTime? FechaCi { get; set; }

        [StringLength(500)]
        public string? Observacion { get; set; }

        [Required]
        public Guid IdResponsable { get; set; }
    }
}
