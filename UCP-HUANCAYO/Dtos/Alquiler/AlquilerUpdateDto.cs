using System.ComponentModel.DataAnnotations;

namespace UCP_HUANCAYO.Dtos.Alquiler
{
    public class AlquilerUpdateDto
    {
        //[Required]
        //public Guid IdPredio { get; set; }

        //[Required]
        //public Guid IdAdministrado { get; set; }

        public DateTime PeriodoDesde { get; set; }

        public DateTime PeriodoHasta { get; set; }

        public decimal Costo { get; set; }

        public int OrdenPago { get; set; }
        public DateTime FechaOrden { get; set; }

        [StringLength(25)]
        public string? Ci { get; set; }
        public DateTime FechaCi { get; set; }

        [StringLength(500)]
        public string? Observacion { get; set; }
    }
}
