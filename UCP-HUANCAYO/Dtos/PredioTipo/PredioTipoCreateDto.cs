using System.ComponentModel.DataAnnotations;

namespace UCP_HUANCAYO.Dtos.PredioTipo
{
    public class PredioTipoCreateDto
    {
        [Required]
        [StringLength(100)]
        public string NombreTipo { get; set; } = string.Empty;

        //[Required]
        public bool Contrato { get; set; }

        [Required]
        public Guid IdResponsable { get; set; }
    }
}
