using System.ComponentModel.DataAnnotations;

namespace UCP_HUANCAYO.Dtos.PredioTipo
{
    public class PredioTipoViewDto
    {
        public Guid IdPredioTipo { get; set; }
        public string? NombreTipo { get; set; }
        public bool Contrato { get; set; }
        //public Guid IdResponsable { get; set; }
        //public bool Activo { get; set; }
    }
}
