using Microsoft.EntityFrameworkCore;
using UCP_HUANCAYO.Data;
using UCP_HUANCAYO.Dtos.Auditoria;
using UCP_HUANCAYO.Models;

namespace UCP_HUANCAYO.Services
{
    public class AuditoriaService
    {
        private readonly ApplicationDbContext _context;

        public AuditoriaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AuditoriaViewDto>> GetAllAsync()
        {
            return await _context.Auditorias
                .AsNoTracking()
                .OrderByDescending(a => a.Fecha)
                .Select(a => new AuditoriaViewDto
                {
                    IdAuditoria = a.IdAuditoria,
                    Fecha = a.Fecha,
                    Tabla = a.Tabla,
                    IdRegistro = a.IdRegistro,
                    Accion = a.Accion,
                    Detalle = a.Detalle,
                    IdUsuario = a.IdUsuario,
                    Conexion = a.Conexion,
                    ClienteNetAddress = a.ClienteNetAddress,
                    SessionId = a.SessionId,
                    LoginName = a.LoginName
                })
                .ToListAsync();
        }

        public async Task<AuditoriaViewDto?> GetByIdAsync(Guid id)
        {
            return await _context.Auditorias
                .AsNoTracking()
                .Where(a => a.IdAuditoria == id)
                .Select(a => new AuditoriaViewDto
                {
                    IdAuditoria = a.IdAuditoria,
                    Fecha = a.Fecha,
                    Tabla = a.Tabla,
                    IdRegistro = a.IdRegistro,
                    Accion = a.Accion,
                    Detalle = a.Detalle,
                    IdUsuario = a.IdUsuario,
                    Conexion = a.Conexion,
                    ClienteNetAddress = a.ClienteNetAddress,
                    SessionId = a.SessionId,
                    LoginName = a.LoginName
                })
                .FirstOrDefaultAsync();
        }

        public async Task<AuditoriaViewDto> CreateAsync(AuditoriaCreateDto dto)
        {
            var auditoria = new Auditoria
            {
                IdAuditoria = Guid.NewGuid(),
                Fecha = DateTime.UtcNow,
                Tabla = dto.Tabla,
                IdRegistro = dto.IdRegistro,
                Accion = dto.Accion,
                Detalle = dto.Detalle,
                IdUsuario = dto.IdUsuario,
                Conexion = dto.Conexion,
                ClienteNetAddress = dto.ClienteNetAddress,
                SessionId = dto.SessionId,
                LoginName = dto.LoginName
            };

            _context.Auditorias.Add(auditoria);
            await _context.SaveChangesAsync();

            return new AuditoriaViewDto
            {
                IdAuditoria = auditoria.IdAuditoria,
                Fecha = auditoria.Fecha,
                Tabla = auditoria.Tabla,
                IdRegistro = auditoria.IdRegistro,
                Accion = auditoria.Accion,
                Detalle = auditoria.Detalle,
                IdUsuario = auditoria.IdUsuario,
                Conexion = auditoria.Conexion,
                ClienteNetAddress = auditoria.ClienteNetAddress,
                SessionId = auditoria.SessionId,
                LoginName = auditoria.LoginName
            };
        }
    }
}
