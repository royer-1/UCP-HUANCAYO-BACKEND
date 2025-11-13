namespace UCP_HUANCAYO.Dtos.Contrato
{
    public class ContratoFilterDto
    {
        public string? DocIdentTipo { get; set; }
        public string? DocIdentNro { get; set; }
        public string? NombrePredio { get; set; }
        public string? Periodo { get; set; }
        public int? Numero { get; set; }
        public DateTime? FechaInicia { get; set; }
        public int? Tiempo { get; set; }
        public decimal? Importe { get; set; }
        public decimal? Agua { get; set; }
        public decimal? Electricidad { get; set; }

        public string? SearchTerm { get; set; }
    }
}
