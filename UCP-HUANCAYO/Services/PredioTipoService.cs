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
        private readonly UsuarioContextHelper _usuarioContextHelper;

        public PredioTipoService(ApplicationDbContext context, AuditoriaHelper auditoriaHelper, UsuarioContextHelper usuarioContextHelper)
        {
            _context = context;
            _auditoriaHelper = auditoriaHelper;
            _usuarioContextHelper = usuarioContextHelper;
        }

        private PredioTipoViewDto MapToViewDto(PredioTipo t)
        {
            return new PredioTipoViewDto
            {
                IdPredioTipo = t.IdPredioTipo,
                NombreTipo = t.NombreTipo,
                Contrato = t.Contrato
            };
        }

        public async Task<List<PredioTipoViewDto>> GetAllAsync()
        {
            return await _context.PredioTipos
                .Where(t => t.Activo)
                .AsNoTracking()
                .Select(t => MapToViewDto(t))
                .ToListAsync();
        }

        public async Task<PredioTipoViewDto?> GetByIdAsync(Guid id)
        {
            return await _context.PredioTipos
                .Where(t => t.IdPredioTipo == id)
                .AsNoTracking()
                .Select(t => MapToViewDto(t))
                .FirstOrDefaultAsync();
        }

        public async Task<PredioTipoViewDto> CreateAsync(PredioTipoCreateDto dto)
        {
            var usuarioActual = _usuarioContextHelper.GetUsuarioActual();

            var tipo = new PredioTipo
            {
                IdPredioTipo = Guid.NewGuid(),
                NombreTipo = dto.NombreTipo,
                Contrato = dto.Contrato,
                IdResponsable = usuarioActual,
                Activo = true
            };

            _context.PredioTipos.Add(tipo);
            await _context.SaveChangesAsync();

            var detalle = AuditoriaDetalleHelper.GenerarDetalle(tipo, "PredioTipo creado");
            await _auditoriaHelper.RegistrarAsync("predio_tipo", tipo.IdPredioTipo, "INSERT", detalle);

            return MapToViewDto(tipo);
        }

        public async Task<PredioTipoViewDto?> UpdateAsync(Guid id, PredioTipoUpdateDto dto)
        {
            var usuarioActual = _usuarioContextHelper.GetUsuarioActual();

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
            tipo.IdResponsable = usuarioActual;

            await _context.SaveChangesAsync();

            var detalle = AuditoriaDetalleHelper.GenerarCambios(tipoAntes, tipo, "PredioTipo actualizado");
            await _auditoriaHelper.RegistrarAsync("predio_tipo", tipo.IdPredioTipo, "UPDATE", detalle);

            return MapToViewDto(tipo);
        }

        public async Task<bool> DesactivarAsync(Guid id)
        {
            var usuarioActual = _usuarioContextHelper.GetUsuarioActual();

            var tipo = await _context.PredioTipos.FindAsync(id);
            if (tipo == null) return false;

            tipo.Activo = false;
            tipo.IdResponsable = usuarioActual;

            await _context.SaveChangesAsync();

            await _auditoriaHelper.RegistrarAsync("predio_tipo", tipo.IdPredioTipo, "DELETE", "PredioTipo desactivado (Activo=false)");
            
            return true;
        }
    }
}
