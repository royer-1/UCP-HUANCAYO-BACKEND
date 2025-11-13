using System.ComponentModel.DataAnnotations;

namespace UCP_HUANCAYO.Dtos.PredioImagen
{
    public class PredioImagenCreateDto
    {
        [Required]
        public Guid IdPredio { get; set; }

        [Required]
        [StringLength(500)]
        public string Imagen { get; set; } = string.Empty;
    }
}
