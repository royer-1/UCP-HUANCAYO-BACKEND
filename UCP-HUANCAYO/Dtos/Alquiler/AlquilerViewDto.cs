using System.ComponentModel.DataAnnotations;

namespace UCP_HUANCAYO.Dtos.Alquiler
{
    public class AlquilerViewDto
    {
        public Guid IdAlquiler { get; set; }
        public Guid IdPredio { get; set; }
        public string? NombrePredio { get; set; }
        public Guid IdAdministrado { get; set; }
        public DateTime PeriodoDesde { get; set; }
        public DateTime PeriodoHasta { get; set; }
        public decimal Costo { get; set; }
        public int OrdenPago { get; set; }
        public DateTime FechaOrden { get; set; }
        public string? Ci { get; set; }
        public DateTime? FechaCi { get; set; }
        public string? Observacion { get; set; }
        public bool Activo { get; set; }
        public Guid IdResponsable { get; set; }
    }
}
