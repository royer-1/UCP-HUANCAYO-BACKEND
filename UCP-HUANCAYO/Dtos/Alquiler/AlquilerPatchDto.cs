using System.ComponentModel.DataAnnotations;

namespace UCP_HUANCAYO.Dtos.Alquiler
{
    public class AlquilerPatchDto
    {
        public DateTime? PeriodoDesde { get; set; }

        public DateTime? PeriodoHasta { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? Costo { get; set; }

        [StringLength(25)]
        public string? Ci { get; set; }

        public DateTime? FechaCi { get; set; }

        [StringLength(500)]
        public string? Observacion { get; set; }
    }
}
