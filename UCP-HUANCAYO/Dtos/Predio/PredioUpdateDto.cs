using System.ComponentModel.DataAnnotations;

namespace UCP_HUANCAYO.Dtos.Predio
{
    public class PredioUpdateDto
    {
        //[Required]
        //public Guid IdPredioTipo { get; set; }

        [Required]
        [StringLength(255)]
        public string NombrePredio { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Descripcion { get; set; }

        [Required]
        [Range(typeof(decimal), "0", "99999999.99")]
        public decimal AreaPredio { get; set; }

        [Range(0, 10000)]
        public int? Capacidad { get; set; }

        public bool? RegistroAgua { get; set; }
        public bool? RegistroLuz { get; set; }

        [Required]
        [StringLength(500)]
        public string Direccion { get; set; } = string.Empty;

        [Required]
        [StringLength(6)]
        public string Ubigeo { get; set; } = string.Empty;

        [StringLength(20)]
        public string? Latitud { get; set; }

        [StringLength(20)]
        public string? Longitud { get; set; }

        public List<string>? ImagenesPredio { get; set; }
    }
}
