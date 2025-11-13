using System.ComponentModel.DataAnnotations;

namespace UCP_HUANCAYO.Dtos.Administrado
{
    public class AdministradoPatchDto
    {
        [StringLength(100)]
        public string? RazonSocial { get; set; }

        [StringLength(20)]
        public string? Telefono { get; set; }

        [EmailAddress]
        public string? Correo { get; set; }

        [StringLength(500)]
        public string? Direccion { get; set; }

        [StringLength(500)]
        public string? Referencia { get; set; }

        [StringLength(6)]
        public string? Ubigeo { get; set; }

    }
}
