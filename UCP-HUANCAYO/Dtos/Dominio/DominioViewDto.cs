namespace UCP_HUANCAYO.Dtos.Dominio
{
    public class DominioViewDto
    {
        public Guid IdDominio { get; set; }
        public string? Nombre { get; set; }
        public bool Ldap { get; set; }
        public string? Servidor { get; set; }
        public string? Conexion { get; set; }
        public bool Default { get; set; }
    }
}
