using System.ComponentModel.DataAnnotations;

namespace UCP_HUANCAYO.Dtos.Contrato
{
    public class ContratoPatchDto
    {
        public DateTime? FechaInicia { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? Importe { get; set; }

        public decimal? Agua { get; set; }

        public decimal? Electricidad { get; set; }
    }
}
