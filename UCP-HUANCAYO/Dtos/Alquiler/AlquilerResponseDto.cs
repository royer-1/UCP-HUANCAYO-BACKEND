namespace UCP_HUANCAYO.Dtos.Alquiler
{
    public class AlquilerResponseDto
    {
        public Guid IdAlquiler { get; set; }
        public Guid IdPredio { get; set; }
        public Guid IdAdministrado { get; set; }
        public DateTime PeriodoDesde { get; set; }
        public DateTime PeriodoHasta { get; set; }
        public decimal Costo { get; set; }
        public int OrdenPago { get; set; }
        public DateTime FechaOrden { get; set; }
        public string? Ci { get; set; }
        public DateTime? FechaCi { get; set; }
        public string? Observacion { get; set; }
    }
}
