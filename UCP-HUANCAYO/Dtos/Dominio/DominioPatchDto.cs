using System.ComponentModel.DataAnnotations;

namespace UCP_HUANCAYO.Dtos.Dominio
{
    public class DominioPatchDto
    {
        [StringLength(50)]
        public string? Nombre { get; set; }

        public bool? Ldap { get; set; }

        [StringLength(50)]
        public string? Servidor { get; set; }

        [StringLength(50)]
        public string? Conexion { get; set; }

        public bool? Default { get; set; }
    }
}
