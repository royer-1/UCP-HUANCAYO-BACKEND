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
        public async Task RegistrarAsync(string tabla, Guid idRegistro, string accion, string? detalle)
        {
            var dto = new AuditoriaCreateDto
            {
                Tabla = tabla,
                IdRegistro = idRegistro,
                Accion = accion,
                Detalle = detalle
            };

            await _auditoriaService.CreateAsync(dto);
        }
    }
}

