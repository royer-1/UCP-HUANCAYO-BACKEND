using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using UCP_HUANCAYO.Data;
using UCP_HUANCAYO.Dtos.PredioTipo;
using UCP_HUANCAYO.Helpers;
using UCP_HUANCAYO.Models;

namespace UCP_HUANCAYO.Services
{
    public class PredioTipoService
    {
        private readonly ApplicationDbContext _context;
        private readonly AuditoriaHelper _auditoriaHelper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PredioTipoService(ApplicationDbContext context, AuditoriaHelper auditoriaHelper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _auditoriaHelper = auditoriaHelper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<PredioTipoViewDto>> GetAllAsync()
        {
            return await _context.PredioTipos
                .Where(t => t.Activo)
                .AsNoTracking()
                .Select(t => new PredioTipoViewDto
                {
                    IdPredioTipo = t.IdPredioTipo,
                    NombreTipo = t.NombreTipo,
                    Contrato = t.Contrato
                })
                .ToListAsync();
        }

        public async Task<PredioTipoViewDto?> GetByIdAsync(Guid id)
        {
            return await _context.PredioTipos
                .Where(t => t.IdPredioTipo == id)
                .AsNoTracking()
                .Select(t => new PredioTipoViewDto
                {
                    IdPredioTipo = t.IdPredioTipo,
                    NombreTipo = t.NombreTipo,
                    Contrato = t.Contrato
                })
                .FirstOrDefaultAsync();
        }

        public async Task<PredioTipoViewDto> CreateAsync(PredioTipoCreateDto dto, Guid usuarioActual)
        {
            var tipo = new PredioTipo
            {
                IdPredioTipo = Guid.NewGuid(),
                NombreTipo = dto.NombreTipo,
                Contrato = dto.Contrato,
                IdResponsable = dto.IdResponsable,
                Activo = true
            };

            _context.PredioTipos.Add(tipo);
            await _context.SaveChangesAsync();

            var context = _httpContextAccessor.HttpContext!;
            var detalle = AuditoriaDetalleHelper.GenerarDetalle(tipo, "PredioTipo creado");

            await _auditoriaHelper.RegistrarDesdeContextoAsync(
                tabla: "predio_tipo",
                idRegistro: tipo.IdPredioTipo,
                accion: "INSERT",
                detalle: detalle,
                idUsuario: usuarioActual,
                context: context
            );

            return new PredioTipoViewDto
            {
                IdPredioTipo = tipo.IdPredioTipo,
                NombreTipo = tipo.NombreTipo,
                Contrato = tipo.Contrato
            };
        }

        public async Task<PredioTipoViewDto?> UpdateAsync(Guid id, PredioTipoUpdateDto dto, Guid usuarioActual)
        {
            var tipo = await _context.PredioTipos.FindAsync(id);
            if (tipo == null) return null;

            var tipoAntes = new PredioTipo
            {
                IdPredioTipo = tipo.IdPredioTipo,
                NombreTipo = tipo.NombreTipo,
                Contrato = tipo.Contrato,
                Activo = tipo.Activo,
                IdResponsable = tipo.IdResponsable
            };

            tipo.NombreTipo = dto.NombreTipo;
            tipo.Contrato = dto.Contrato;

            await _context.SaveChangesAsync();

            var context = _httpContextAccessor.HttpContext!;
            var detalle = AuditoriaDetalleHelper.GenerarCambios(tipoAntes, tipo, "PredioTipo actualizado");

            await _auditoriaHelper.RegistrarDesdeContextoAsync(
                tabla: "predio_tipo",
                idRegistro: tipo.IdPredioTipo,
                accion: "UPDATE",
                detalle: detalle,
                idUsuario: usuarioActual,
                context: context
            );

            return new PredioTipoViewDto
            {
                IdPredioTipo = tipo.IdPredioTipo,
                NombreTipo = tipo.NombreTipo,
                Contrato = tipo.Contrato
            };
        }

        public async Task<bool> DesactivarAsync(Guid id, Guid usuarioActual)
        {
            var tipo = await _context.PredioTipos.FindAsync(id);
            if (tipo == null) return false;

            tipo.Activo = false;
            await _context.SaveChangesAsync();

            var context = _httpContextAccessor.HttpContext!;
            await _auditoriaHelper.RegistrarDesdeContextoAsync(
                tabla: "predio_tipo",
                idRegistro: tipo.IdPredioTipo,
                accion: "DELETE",
                detalle: "PredioTipo desactivado (Activo=false)",
                idUsuario: usuarioActual,
                context: context
            );

            return true;
        }
    }
}
