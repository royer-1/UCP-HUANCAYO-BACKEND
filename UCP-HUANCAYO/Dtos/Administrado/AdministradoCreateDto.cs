using System.ComponentModel.DataAnnotations;

namespace UCP_HUANCAYO.Dtos.Administrado
{
    public class AdministradoCreateDto
    {
        [Required]
        [StringLength(2)]
        public string DocIdentTipo { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string DocIdentNro { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        public string RazonSocial { get; set; } = string.Empty;

        [Required]
        [StringLength(15)]
        public string Telefono { get; set; } = string.Empty;

        public string? Correo { get; set; }

        [Required]
        [StringLength(500)]
        public string Direccion { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Referencia { get; set; }

        [Required]
        [StringLength(6)]
        public string Ubigeo { get; set; } = string.Empty;

        [Required]
        public Guid IdResponsable { get; set; }
    }
}

