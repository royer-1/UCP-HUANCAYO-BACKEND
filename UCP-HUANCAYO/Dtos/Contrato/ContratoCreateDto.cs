using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UCP_HUANCAYO.Dtos.Contrato
{
    public class ContratoCreateDto
    {
        [Required]
        public Guid IdPredio { get; set; }

        [Required]
        public Guid IdAdministrado { get; set; }

        [Required]
        public DateTime FechaInicia { get; set; }

        [Required]
        public int Tiempo { get; set; }

        [Required]
        public decimal Importe { get; set; }

        public decimal? Agua { get; set; }

        public decimal? Electricidad { get; set; }

        [Required]
        public Guid IdResponsable { get; set; }
    }
}
