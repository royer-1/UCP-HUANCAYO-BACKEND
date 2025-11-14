using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UCP_HUANCAYO.Models
{
    [Table("auditoria")]
    public class Auditoria
    {
        [Key]
        [Column("id_auditoria")]
        public Guid IdAuditoria { get; set; }

        [Column("fecha")]
        public DateTime Fecha { get; set; }

        [Column("tabla")]
        public string? Tabla { get; set; }

        [Column("id_registro")]
        public Guid IdRegistro { get; set; }

        [Column("accion")]
        public string? Accion { get; set; }

        [Column("detalle")]
        public string? Detalle { get; set; }

        [Column("id_usuario")]
        public Guid IdUsuario { get; set; }
        public Usuario? Usuario { get; set; }

        [Column("conexion")]
        public string? Conexion { get; set; }

        [Column("cliente_net_address")]
        public string? ClienteNetAddress { get; set; }

        [Column("session_id")]
        public string?   SessionId { get; set; }

        [Column("login_name")]
        public string? LoginName { get; set; }
    }
}

