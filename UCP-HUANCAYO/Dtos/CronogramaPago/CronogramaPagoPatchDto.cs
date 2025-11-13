using System.ComponentModel.DataAnnotations;

namespace UCP_HUANCAYO.Dtos.CronogramaPago
{
    public class CronogramaPagoPatchDto
    {
        [StringLength(25)]
        public string? Ci { get; set; }

        public DateTime? FechaCi { get; set; }

        [StringLength(500)]
        public string? Observacion { get; set; }
    }
}
