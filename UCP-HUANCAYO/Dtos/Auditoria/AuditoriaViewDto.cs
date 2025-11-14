namespace UCP_HUANCAYO.Dtos.Auditoria
{
    public class AuditoriaViewDto
    {
        public Guid IdAuditoria { get; set; }
        public DateTime Fecha { get; set; }
        public string? Tabla { get; set; }
        public Guid IdRegistro { get; set; }
        public string? Accion { get; set; }
        public string? Detalle { get; set; }
        public Guid IdUsuario { get; set; }
        public string? Conexion { get; set; }
        public string? ClienteNetAddress { get; set; }
        public string? SessionId { get; set; }
        public string? LoginName { get; set; }
    }
}
