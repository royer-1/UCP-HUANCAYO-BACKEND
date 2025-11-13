using System.ComponentModel.DataAnnotations;

namespace UCP_HUANCAYO.Dtos.PredioTipo
{
    public class PredioTipoUpdateDto
    {
        [Required]
        [StringLength(100)]
        public string NombreTipo { get; set; } = string.Empty;

        public bool Contrato { get; set; }
    }
}

