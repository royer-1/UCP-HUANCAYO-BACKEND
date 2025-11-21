using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using UCP_HUANCAYO.Data;
using UCP_HUANCAYO.Dtos.CronogramaPago;
using UCP_HUANCAYO.Helpers;
using UCP_HUANCAYO.Models;

namespace UCP_HUANCAYO.Services
{
    public class CronogramaPagoService
    {
        private readonly ApplicationDbContext _context;
        private readonly AuditoriaHelper _auditoriaHelper;
        private readonly UsuarioContextHelper _usuarioContextHelper;

        public CronogramaPagoService(ApplicationDbContext context, AuditoriaHelper auditoriaHelper, UsuarioContextHelper usuarioContextHelper)
        {
            _context = context;
            _auditoriaHelper = auditoriaHelper;
            _usuarioContextHelper = usuarioContextHelper;
        }

        private static CronogramaPagoViewDto MapToViewDto(CronogramaPago c)
        {
            return new CronogramaPagoViewDto
            {
                IdCronograma = c.IdCronograma,
                IdContrato = c.IdContrato,
                PeriodoDesde = c.PeriodoDesde,
                PeriodoHasta = c.PeriodoHasta,
                OrdenPago = c.OrdenPago,
                FechaOrden = c.FechaOrden,
                Ci = c.Ci,
                FechaCi = c.FechaCi,
                Observacion = c.Observacion
            };
        }

        public async Task<IEnumerable<CronogramaPagoViewDto>> GetAllAsync()
        {
            return await _context.CronogramaPagos
                .Where(c => c.Activo)
                .AsNoTracking()
                .Select(c => MapToViewDto(c))
                .ToListAsync();
        }

        public async Task<CronogramaPagoViewDto?> GetByIdAsync(Guid id)
        {
            var cronograma = await _context.CronogramaPagos
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.IdCronograma == id && c.Activo);

            if (cronograma == null) return null;

            return MapToViewDto(cronograma);
        }

        public async Task<CronogramaPagoViewDto?> CreateAsync(CronogramaPagoCreateDto dto)
        {
            var usuarioActual = _usuarioContextHelper.GetUsuarioActual();

            var contrato = await _context.Contratos
                .Include(c => c.CronogramasPago)
                .FirstOrDefaultAsync(c => c.IdContrato == dto.IdContrato);

            if (contrato == null || !contrato.Activo) return null;

            int siguienteOrden = contrato.CronogramasPago.Count + 1;

            var nuevoCronograma = new CronogramaPago
            {
                IdCronograma = Guid.NewGuid(),
                IdContrato = contrato.IdContrato,
                PeriodoDesde = dto.PeriodoDesde.Date,
                PeriodoHasta = dto.PeriodoHasta.Date,
                OrdenPago = siguienteOrden,
                FechaOrden = DateTime.UtcNow,
                Ci = string.IsNullOrWhiteSpace(dto.Ci) ? "" : dto.Ci,
                FechaCi = dto.FechaCi,
                Observacion = dto.Observacion,
                Activo = true,
                IdResponsable = usuarioActual
            };

            _context.CronogramaPagos.Add(nuevoCronograma);
            contrato.Tiempo = siguienteOrden;

            await _context.SaveChangesAsync();

            var detalle = AuditoriaDetalleHelper.GenerarDetalle(nuevoCronograma, "CronogramaPago creado");
            await _auditoriaHelper.RegistrarAsync("cronograma_pago", nuevoCronograma.IdCronograma, "INSERT", detalle);

            return MapToViewDto(nuevoCronograma);
        }

        public async Task<CronogramaPagoViewDto?> PatchAsync(Guid id, CronogramaPagoPatchDto dto)
        {
            var usuarioActual = _usuarioContextHelper.GetUsuarioActual();

            var cronograma = await _context.CronogramaPagos.FindAsync(id);
            if (cronograma == null || !cronograma.Activo) return null;

            bool cronogramaPagado = !string.IsNullOrWhiteSpace(cronograma.Ci) && cronograma.FechaCi != null;
            if (cronogramaPagado) return null;

            var cronogramaAntes = new CronogramaPago
            {
                IdCronograma = cronograma.IdCronograma,
                IdContrato = cronograma.IdContrato,
                PeriodoDesde = cronograma.PeriodoDesde,
                PeriodoHasta = cronograma.PeriodoHasta,
                OrdenPago = cronograma.OrdenPago,
                FechaOrden = cronograma.FechaOrden,
                Ci = cronograma.Ci,
                FechaCi = cronograma.FechaCi,
                Observacion = cronograma.Observacion,
                Activo = cronograma.Activo,
                IdResponsable = cronograma.IdResponsable
            };

            if (dto.Ci != null) cronograma.Ci = dto.Ci;
            if (dto.FechaCi.HasValue) cronograma.FechaCi = dto.FechaCi.Value;
            if (dto.Observacion != null) cronograma.Observacion = dto.Observacion;

            cronograma.IdResponsable = usuarioActual;
            await _context.SaveChangesAsync();

            var detalle = AuditoriaDetalleHelper.GenerarCambios(cronogramaAntes, cronograma, "CronogramaPago actualizado parcialmente");
            await _auditoriaHelper.RegistrarAsync("cronograma_pago", cronograma.IdCronograma, "PATCH", detalle);

            return MapToViewDto(cronograma);
        }

        public async Task<CronogramaPagoViewDto?> UpdateAsync(Guid id, CronogramaPagoUpdateDto dto)
        {
            var usuarioActual = _usuarioContextHelper.GetUsuarioActual();

            var cronograma = await _context.CronogramaPagos.FindAsync(id);
            if (cronograma == null || !cronograma.Activo) return null;

            bool cronogramaPagado = !string.IsNullOrWhiteSpace(cronograma.Ci) && cronograma.FechaCi != null;
            if (cronogramaPagado) return null;

            var cronogramaAntes = new CronogramaPago
            {
                IdCronograma = cronograma.IdCronograma,
                IdContrato = cronograma.IdContrato,
                PeriodoDesde = cronograma.PeriodoDesde,
                PeriodoHasta = cronograma.PeriodoHasta,
                OrdenPago = cronograma.OrdenPago,
                FechaOrden = cronograma.FechaOrden,
                Ci = cronograma.Ci,
                FechaCi = cronograma.FechaCi,
                Observacion = cronograma.Observacion,
                Activo = cronograma.Activo,
                IdResponsable = cronograma.IdResponsable
            };

            cronograma.Ci = string.IsNullOrWhiteSpace(dto.Ci) ? "" : dto.Ci;
            cronograma.FechaCi = dto.FechaCi;
            cronograma.Observacion = dto.Observacion;
            cronograma.IdResponsable = usuarioActual;

            await _context.SaveChangesAsync();

            var detalle = AuditoriaDetalleHelper.GenerarCambios(cronogramaAntes, cronograma, "CronogramaPago actualizado");
            await _auditoriaHelper.RegistrarAsync("cronograma_pago", cronograma.IdCronograma, "UPDATE", detalle);

            return MapToViewDto(cronograma);
        }

        public async Task<bool> DesactivarAsync(Guid id)
        {
            var usuarioActual = _usuarioContextHelper.GetUsuarioActual();

            var cronograma = await _context.CronogramaPagos.FindAsync(id);
            if (cronograma == null || !cronograma.Activo) return false;

            cronograma.Activo = false;
            cronograma.IdResponsable = usuarioActual;

            await _context.SaveChangesAsync();

            await _auditoriaHelper.RegistrarAsync("cronograma_pago", cronograma.IdCronograma, "DELETE", "Cronograma desactivado (Activo=false)");
            
            return true;
        }
    }
}

