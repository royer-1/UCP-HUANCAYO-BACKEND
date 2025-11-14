using UCP_HUANCAYO.Dtos.Auditoria;
using UCP_HUANCAYO.Services;

namespace UCP_HUANCAYO.Helpers
{
    public class AuditoriaHelper
    {
        private readonly AuditoriaService _auditoriaService;

        public AuditoriaHelper(AuditoriaService auditoriaService)
        {
            _auditoriaService = auditoriaService;
        }

        public async Task RegistrarAsync(
            string tabla,
            Guid idRegistro,
            string accion,
            string? detalle,
            Guid idUsuario,
            string conexion,
            string clienteNetAddress,
            string sessionId,
            string loginName)
        {
            var dto = new AuditoriaCreateDto
            {
                Tabla = tabla,
                IdRegistro = idRegistro,
                Accion = accion,
                Detalle = detalle,
                IdUsuario = idUsuario,
                Conexion = conexion,
                ClienteNetAddress = clienteNetAddress,
                SessionId = sessionId,
                LoginName = loginName
            };

            await _auditoriaService.CreateAsync(dto);
        }

        public async Task RegistrarDesdeContextoAsync(
            string tabla,
            Guid idRegistro,
            string accion,
            string? detalle,
            Guid idUsuario,
            HttpContext context)
        {
            var ip = context.Connection.RemoteIpAddress?.ToString() ?? "desconocido";
            var sessionId = context.TraceIdentifier;
            var loginName = context.User.Identity?.Name ?? "anónimo";

            await RegistrarAsync(tabla, idRegistro, accion, detalle, idUsuario, "SQLServer", ip, sessionId, loginName);
        }
    }
}

