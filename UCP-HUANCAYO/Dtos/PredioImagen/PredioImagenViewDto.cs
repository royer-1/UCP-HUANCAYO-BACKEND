using System.ComponentModel.DataAnnotations;

namespace UCP_HUANCAYO.Dtos.PredioImagen
{
    public class PredioImagenViewDto
    {
        public Guid IdImagen { get; set; }
        public Guid IdPredio { get; set; }
        public string Imagen { get; set; } = string.Empty;
        public bool Activo { get; set; }
    }
}
