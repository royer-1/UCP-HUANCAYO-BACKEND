using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UCP_HUANCAYO.Models
{
    [Table("usuario")]
    public class Usuario
    {
        [Key]
        [Column("id_usuario")]
        public Guid IdUsuario { get; set; }

        [Column("id_dominio")]
        public Guid IdDominio { get; set; }
        public Dominio? Dominio { get; set; }

        [Column("alias")]
        public string? Alias { get; set; }

        [Column("doc_ident_tipo")]
        public string? DocIdentTipo { get; set; }

        [Column("doc_ident_nro")]
        public string? DocIdentNro { get; set; }

        [Column("nombres")]
        public string? Nombres { get; set; }

        [Column("correo")]
        public string? Correo { get; set; }

        [Column("telefono")]
        public string? Telefono { get; set; }

        [Column("clave")]
        public byte[]? Clave { get; set; }

        [Column("activo")]
        public bool Activo { get; set; }

        [Column("id_responsable")]
        public Guid IdResponsable { get; set; }
    }
}
