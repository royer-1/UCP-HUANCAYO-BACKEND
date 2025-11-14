using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UCP_HUANCAYO.Models
{
    [Table("dominio")]
    public class Dominio
    {
        [Key]
        [Column("id_dominio")]
        public Guid IdDominio { get; set; }

        [Column("nombre")]
        public string? Nombre { get; set; }

        [Column("ldap")]
        public bool Ldap { get; set; }

        [Column("servidor")]
        public string? Servidor { get; set; }

        [Column("conexion")]
        public string? Conexion { get; set; }

        [Column("default")]
        public bool Default { get; set; }

        [Column("activo")]
        public bool Activo { get; set; }

        [Column("id_responsable")]
        public Guid IdResponsable { get; set; }
    }
}

