using Microsoft.EntityFrameworkCore;
using UCP_HUANCAYO.Data;
using UCP_HUANCAYO.Dtos.Alquiler;
using UCP_HUANCAYO.Models;

namespace UCP_HUANCAYO.Services
{
    public class AlquilerService
    {
        private readonly ApplicationDbContext _context;

        public AlquilerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AlquilerViewDto>> GetAllAsync()
        {
            return await _context.Alquileres
                .Include(a => a.Predio)
                .Where(a => a.Activo)
                .AsNoTracking()
                .Select(a => new AlquilerViewDto
                {
                    IdAlquiler = a.IdAlquiler,
                    IdPredio = a.IdPredio,
                    IdAdministrado = a.IdAdministrado,
                    NombrePredio = a.Predio.NombrePredio,
                    PeriodoDesde = a.PeriodoDesde,
                    PeriodoHasta = a.PeriodoHasta,
                    Costo = a.Costo,
                    OrdenPago = a.OrdenPago,
                    FechaOrden = a.FechaOrden,
                    Ci = a.Ci,
                    FechaCi = a.FechaCi,
                    Observacion = a.Observacion
                })
                .ToListAsync();
        }

        public async Task<AlquilerViewDto?> GetByIdAsync(Guid id)
        {
            return await _context.Alquileres
                .Include(a => a.Predio)
                .Where(a => a.IdAlquiler == id)
                .AsNoTracking()
                .Select(a => new AlquilerViewDto
                {
                    IdAlquiler = a.IdAlquiler,
                    IdPredio = a.IdPredio,
                    IdAdministrado = a.IdAdministrado,
                    NombrePredio = a.Predio.NombrePredio,
                    PeriodoDesde = a.PeriodoDesde,
                    PeriodoHasta = a.PeriodoHasta,
                    Costo = a.Costo,
                    OrdenPago = a.OrdenPago,
                    FechaOrden = a.FechaOrden,
                    Ci = a.Ci,
                    FechaCi = a.FechaCi,
                    Observacion = a.Observacion
                })
                .FirstOrDefaultAsync();
        }

        public async Task<AlquilerViewDto> CreateAsync(AlquilerCreateDto dto)
        {
            int siguienteOrden = await _context.Alquileres.CountAsync() + 1;

            var alquiler = new Alquiler
            {
                IdAlquiler = Guid.NewGuid(),
                IdPredio = dto.IdPredio,
                IdAdministrado = dto.IdAdministrado,
                PeriodoDesde = dto.PeriodoDesde,
                PeriodoHasta = dto.PeriodoHasta,
                Costo = dto.Costo,
                OrdenPago = siguienteOrden,
                FechaOrden = DateTime.UtcNow,
                Ci = dto.Ci,
                FechaCi = dto.FechaCi,
                Observacion = dto.Observacion,
                Activo = true,
                IdResponsable = dto.IdResponsable
            };

            _context.Alquileres.Add(alquiler);
            await _context.SaveChangesAsync();

            return new AlquilerViewDto
            {
                IdAlquiler = alquiler.IdAlquiler,
                IdPredio = alquiler.IdPredio,
                IdAdministrado = alquiler.IdAdministrado,
                PeriodoDesde = alquiler.PeriodoDesde,
                PeriodoHasta = alquiler.PeriodoHasta,
                Costo = alquiler.Costo,
                OrdenPago = alquiler.OrdenPago,
                FechaOrden = alquiler.FechaOrden,
                Ci = alquiler.Ci,
                FechaCi = alquiler.FechaCi,
                Observacion = alquiler.Observacion
            };
        }

        public async Task<AlquilerViewDto?> PatchAsync(Guid id, AlquilerPatchDto dto)
        {
            var alquiler = await _context.Alquileres.FindAsync(id);
            if (alquiler == null) return null;

            if (dto.PeriodoDesde.HasValue) alquiler.PeriodoDesde = dto.PeriodoDesde.Value;
            if (dto.PeriodoHasta.HasValue) alquiler.PeriodoHasta = dto.PeriodoHasta.Value;
            if (dto.Costo.HasValue) alquiler.Costo = dto.Costo.Value;
            if (dto.Ci != null) alquiler.Ci = dto.Ci;
            if (dto.FechaCi.HasValue) alquiler.FechaCi = dto.FechaCi.Value;
            if (dto.Observacion != null) alquiler.Observacion = dto.Observacion;

            await _context.SaveChangesAsync();

            return new AlquilerViewDto
            {
                IdAlquiler = alquiler.IdAlquiler,
                IdPredio = alquiler.IdPredio,
                IdAdministrado = alquiler.IdAdministrado,
                PeriodoDesde = alquiler.PeriodoDesde,
                PeriodoHasta = alquiler.PeriodoHasta,
                Costo = alquiler.Costo,
                OrdenPago = alquiler.OrdenPago,
                FechaOrden = alquiler.FechaOrden,
                Ci = alquiler.Ci,
                FechaCi = alquiler.FechaCi,
                Observacion = alquiler.Observacion
            };
        }

        public async Task<AlquilerViewDto?> UpdateAsync(Guid id, AlquilerUpdateDto dto)
        {
            var alquiler = await _context.Alquileres.FindAsync(id);
            if (alquiler == null) return null;

            alquiler.PeriodoDesde = dto.PeriodoDesde;
            alquiler.PeriodoHasta = dto.PeriodoHasta;
            alquiler.Costo = dto.Costo;
            alquiler.OrdenPago = dto.OrdenPago;
            alquiler.FechaOrden = dto.FechaOrden;
            alquiler.Ci = dto.Ci;
            alquiler.FechaCi = dto.FechaCi;
            alquiler.Observacion = dto.Observacion;

            await _context.SaveChangesAsync();

            return new AlquilerViewDto
            {
                IdAlquiler = alquiler.IdAlquiler,
                IdPredio = alquiler.IdPredio,
                IdAdministrado = alquiler.IdAdministrado,
                PeriodoDesde = alquiler.PeriodoDesde,
                PeriodoHasta = alquiler.PeriodoHasta,
                Costo = alquiler.Costo,
                OrdenPago = alquiler.OrdenPago,
                FechaOrden = alquiler.FechaOrden,
                Ci = alquiler.Ci,
                FechaCi = alquiler.FechaCi,
                Observacion = alquiler.Observacion
            };
        }

        public async Task<bool> DesactivarAsync(Guid id)
        {
            var alquiler = await _context.Alquileres.FindAsync(id);
            if (alquiler == null) return false;

            alquiler.Activo = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<(List<AlquilerFilterDto> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, AlquilerFilterDto filters)
        {
            var query = _context.Alquileres
                .Include(a => a.Predio)
                .Include(a => a.Administrado)
                .Where(a => a.Activo)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(filters.SearchTerm))
            {
                query = query.Where(a =>
                    a.Predio.NombrePredio.Contains(filters.SearchTerm) ||
                    a.Administrado.DocIdentNro.Contains(filters.SearchTerm) ||
                    a.Administrado.DocIdentTipo.Contains(filters.SearchTerm));
            }

            if (!string.IsNullOrWhiteSpace(filters.DocIdentTipo))
                query = query.Where(a => a.Administrado.DocIdentTipo.Contains(filters.DocIdentTipo));

            if (!string.IsNullOrWhiteSpace(filters.DocIdentNro))
                query = query.Where(a => a.Administrado.DocIdentNro.Contains(filters.DocIdentNro));

            if (!string.IsNullOrWhiteSpace(filters.NombrePredio))
                query = query.Where(a => a.Predio.NombrePredio.Contains(filters.NombrePredio));

            if (filters.PeriodoDesde.HasValue)
                query = query.Where(a => a.PeriodoDesde.Date == filters.PeriodoDesde.Value.Date);

            if (filters.PeriodoHasta.HasValue)
                query = query.Where(a => a.PeriodoHasta.Date == filters.PeriodoHasta.Value.Date);

            if (filters.Costo.HasValue)
                query = query.Where(a => a.Costo == filters.Costo.Value);

            if (filters.OrdenPago.HasValue)
                query = query.Where(a => a.OrdenPago == filters.OrdenPago.Value);

            if (filters.FechaOrden.HasValue)
                query = query.Where(a => a.FechaOrden.Date == filters.FechaOrden.Value.Date);

            if (!string.IsNullOrWhiteSpace(filters.Ci))
                query = query.Where(a => a.Ci.Contains(filters.Ci));

            if (filters.FechaCi.HasValue)
                query = query.Where(a => a.FechaCi.HasValue && a.FechaCi.Value.Date == filters.FechaCi.Value.Date);


            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(a => a.FechaOrden)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new AlquilerFilterDto
                {
                    NombrePredio = a.Predio.NombrePredio,
                    DocIdentTipo = a.Administrado.DocIdentTipo,
                    DocIdentNro = a.Administrado.DocIdentNro,
                    PeriodoDesde = a.PeriodoDesde,
                    PeriodoHasta = a.PeriodoHasta,
                    Costo = a.Costo,
                    OrdenPago = a.OrdenPago,
                    FechaOrden = a.FechaOrden,
                    Ci = a.Ci,
                    FechaCi = a.FechaCi,
                    Observacion = a.Observacion,
                })
                .ToListAsync();

            return (items, totalCount);
        }
    }
}
