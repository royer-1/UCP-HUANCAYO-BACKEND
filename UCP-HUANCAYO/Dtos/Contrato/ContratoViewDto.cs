using System.ComponentModel.DataAnnotations;

namespace UCP_HUANCAYO.Dtos.Contrato
{
    public class ContratoViewDto
    {
        public Guid IdContrato { get; set; }
        public Guid IdPredio { get; set; }
        public Guid IdAdministrado { get; set; }
        public string? Periodo { get; set; }
        public int Numero { get; set; }
        public DateTime FechaInicia { get; set; }
        public int Tiempo { get; set; }
        public decimal? Importe { get; set; }
        public decimal? Agua { get; set; }
        public decimal? Electricidad { get; set; }
        //public bool Activo { get; set; }
        //public Guid IdResponsable { get; set; }
    }
}
