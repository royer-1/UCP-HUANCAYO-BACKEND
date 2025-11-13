using System.ComponentModel.DataAnnotations;

namespace UCP_HUANCAYO.Dtos.CronogramaPago
{
    public class CronogramaPagoUpdateDto
    {
        public string? Ci { get; set; }
        public DateTime? FechaCi { get; set; }
        public string? Observacion { get; set; }
    }
}
