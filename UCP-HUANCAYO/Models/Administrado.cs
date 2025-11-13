using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UCP_HUANCAYO.Models
{
    [Table("administrado")]
    public class Administrado
    {
        [Key]
        [Column("id_administrado")]
        public Guid IdAdministrado { get; set; }

        [Column("doc_ident_tipo")]
        public string? DocIdentTipo { get; set; }

        [Column("doc_ident_nro")]
        public string? DocIdentNro { get; set; }

        [Column("razon_social")]
        public string? RazonSocial { get; set; }

        [Column("telefono")]
        public string? Telefono { get; set; }

        [Column("correo")]
        public byte[]? Correo { get; set; }

        [Column("direccion")]
        public string? Direccion { get; set; }

        [Column("referencia")]
        public string? Referencia { get; set; }

        [Column("ubigeo")]
        public string? Ubigeo { get; set; }

        [Column("id_responsable")]
        public Guid IdResponsable { get; set; }

        [Column("activo")]
        public bool Activo { get; set; }

    }
}
