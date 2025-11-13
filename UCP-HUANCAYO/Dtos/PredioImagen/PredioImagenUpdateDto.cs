using System.ComponentModel.DataAnnotations;

namespace UCP_HUANCAYO.Dtos.PredioImagen
{
    public class PredioImagenUpdateDto
    {
        [Required]
        [StringLength(500)]
        public string Imagen { get; set; } = string.Empty;
    }
}
