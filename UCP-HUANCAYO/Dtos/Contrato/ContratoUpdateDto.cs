using System.ComponentModel.DataAnnotations;

namespace UCP_HUANCAYO.Dtos.Contrato
{
    public class ContratoUpdateDto
    {
        public DateTime FechaInicia { get; set; }

        public decimal Importe { get; set; }

        public decimal? Agua { get; set; }

        public decimal? Electricidad { get; set; }
    }
}
