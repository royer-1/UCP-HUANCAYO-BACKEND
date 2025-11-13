using System.ComponentModel.DataAnnotations;

namespace UCP_HUANCAYO.Dtos.CronogramaPago
{
    public class CronogramaPagoViewDto
    {
        public Guid IdCronograma { get; set; }
        public Guid IdContrato { get; set; }
        public DateTime PeriodoDesde { get; set; }
        public DateTime PeriodoHasta { get; set; }
        public int OrdenPago { get; set; }
        public DateTime FechaOrden { get; set; }
        public string? Ci { get; set; }
        public DateTime? FechaCi { get; set; }
        public string? Observacion { get; set; }
        //public bool Activo { get; set; }
        //public Guid IdResponsable { get; set; }
    }
}
