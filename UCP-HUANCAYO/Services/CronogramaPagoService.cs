using Microsoft.EntityFrameworkCore;
using UCP_HUANCAYO.Data;
using UCP_HUANCAYO.Dtos.CronogramaPago;
using UCP_HUANCAYO.Models;

namespace UCP_HUANCAYO.Services
{
    public class CronogramaPagoService
    {
        private readonly ApplicationDbContext _context;

        public CronogramaPagoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CronogramaPagoViewDto>> GetAllAsync()
        {
            return await _context.CronogramaPagos
                .Where(c => c.Activo)
                .AsNoTracking()
                .Select(c => new CronogramaPagoViewDto
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
                })
                .ToListAsync();
        }

        public async Task<CronogramaPagoViewDto?> GetByIdAsync(Guid id)
        {
            var cronograma = await _context.CronogramaPagos
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.IdCronograma == id && c.Activo);

            if (cronograma == null) return null;

            return new CronogramaPagoViewDto
            {
                IdCronograma = cronograma.IdCronograma,
                IdContrato = cronograma.IdContrato,
                PeriodoDesde = cronograma.PeriodoDesde,
                PeriodoHasta = cronograma.PeriodoHasta,
                OrdenPago = cronograma.OrdenPago,
                FechaOrden = cronograma.FechaOrden,
                Ci = cronograma.Ci,
                FechaCi = cronograma.FechaCi,
                Observacion = cronograma.Observacion
            };
        }

        public async Task<CronogramaPagoViewDto?> CreateAsync(CronogramaPagoCreateDto dto)
        {
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
                IdResponsable = dto.IdResponsable
            };

            _context.CronogramaPagos.Add(nuevoCronograma);
            contrato.Tiempo = siguienteOrden;

            await _context.SaveChangesAsync();

            return new CronogramaPagoViewDto
            {
                IdCronograma = nuevoCronograma.IdCronograma,
                IdContrato = nuevoCronograma.IdContrato,
                PeriodoDesde = nuevoCronograma.PeriodoDesde,
                PeriodoHasta = nuevoCronograma.PeriodoHasta,
                OrdenPago = nuevoCronograma.OrdenPago,
                FechaOrden = nuevoCronograma.FechaOrden,
                Ci = nuevoCronograma.Ci,
                FechaCi = nuevoCronograma.FechaCi,
                Observacion = nuevoCronograma.Observacion
            };
        }

        public async Task<CronogramaPagoViewDto?> PatchAsync(Guid id, CronogramaPagoPatchDto dto)
        {
            var cronograma = await _context.CronogramaPagos.FindAsync(id);
            if (cronograma == null || !cronograma.Activo) return null;

            bool cronogramaPagado = !string.IsNullOrWhiteSpace(cronograma.Ci) && cronograma.FechaCi != null;
            if (cronogramaPagado) return null;

            if (dto.Ci != null) cronograma.Ci = dto.Ci;
            if (dto.FechaCi.HasValue) cronograma.FechaCi = dto.FechaCi.Value;
            if (dto.Observacion != null) cronograma.Observacion = dto.Observacion;

            await _context.SaveChangesAsync();

            return new CronogramaPagoViewDto
            {
                IdCronograma = cronograma.IdCronograma,
                IdContrato = cronograma.IdContrato,
                PeriodoDesde = cronograma.PeriodoDesde,
                PeriodoHasta = cronograma.PeriodoHasta,
                OrdenPago = cronograma.OrdenPago,
                FechaOrden = cronograma.FechaOrden,
                Ci = cronograma.Ci,
                FechaCi = cronograma.FechaCi,
                Observacion = cronograma.Observacion
            };
        }

        public async Task<CronogramaPagoViewDto?> UpdateAsync(Guid id, CronogramaPagoUpdateDto dto)
        {
            var cronograma = await _context.CronogramaPagos.FindAsync(id);
            if (cronograma == null || !cronograma.Activo) return null;

            bool cronogramaPagado = !string.IsNullOrWhiteSpace(cronograma.Ci) && cronograma.FechaCi != null;
            if (cronogramaPagado) return null;

            cronograma.Ci = string.IsNullOrWhiteSpace(dto.Ci) ? "" : dto.Ci;
            cronograma.FechaCi = dto.FechaCi;
            cronograma.Observacion = dto.Observacion;

            await _context.SaveChangesAsync();

            return new CronogramaPagoViewDto
            {
                IdCronograma = cronograma.IdCronograma,
                IdContrato = cronograma.IdContrato,
                PeriodoDesde = cronograma.PeriodoDesde,
                PeriodoHasta = cronograma.PeriodoHasta,
                OrdenPago = cronograma.OrdenPago,
                FechaOrden = cronograma.FechaOrden,
                Ci = cronograma.Ci,
                FechaCi = cronograma.FechaCi,
                Observacion = cronograma.Observacion
            };
        }

        public async Task<bool> DesactivarAsync(Guid id)
        {
            var cronograma = await _context.CronogramaPagos.FindAsync(id);
            if (cronograma == null || !cronograma.Activo) return false;

            cronograma.Activo = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

