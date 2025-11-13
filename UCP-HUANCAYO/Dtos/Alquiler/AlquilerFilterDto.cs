namespace UCP_HUANCAYO.Dtos.Alquiler
{
    public class AlquilerFilterDto
    {
        public string? DocIdentTipo { get; set; }
        public string? DocIdentNro { get; set; }
        public string? NombrePredio { get; set; }
        public DateTime? PeriodoDesde { get; set; }
        public DateTime? PeriodoHasta { get; set; }
        public decimal? Costo { get; set; }
        public int? OrdenPago { get; set; }
        public DateTime? FechaOrden { get; set; }
        public string? Ci { get; set; }
        public DateTime? FechaCi { get; set; }
        public string? Observacion { get; set; }

        public string? SearchTerm { get; set; }
    }
}
