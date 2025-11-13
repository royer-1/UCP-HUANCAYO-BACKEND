using System.ComponentModel.DataAnnotations;

namespace UCP_HUANCAYO.Dtos.Predio
{
    public class PredioPatchDto
    {
        [StringLength(255)]
        public string? NombrePredio { get; set; }

        [StringLength(500)]
        public string? Descripcion { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? AreaPredio { get; set; }

        public int? Capacidad { get; set; }

        public bool? RegistroAgua { get; set; }

        public bool? RegistroLuz { get; set; }

        [StringLength(500)]
        public string? Direccion { get; set; }

        [StringLength(6)]
        public string? Ubigeo { get; set; }

        [StringLength(20)]
        public string? Latitud { get; set; }

        [StringLength(20)]
        public string? Longitud { get; set; }

        public List<string>? ImagenesPredio { get; set; }
    }
}
