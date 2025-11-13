using Microsoft.EntityFrameworkCore;
using UCP_HUANCAYO.Data;
using UCP_HUANCAYO.Dtos.Contrato;
using UCP_HUANCAYO.Models;

namespace UCP_HUANCAYO.Services
{
    public class ContratoService
    {
        private readonly ApplicationDbContext _context;

        public ContratoService(ApplicationDbContext context)
        {
            _context = context;
        }

        private List<CronogramaPago> GenerarCronogramaPagos(Guid idContrato, DateTime fechaInicio, int tiempo, Guid idResponsable)
        {
            var cronogramas = new List<CronogramaPago>();
            var fechaActual = fechaInicio;

            for (int i = 1; i <= tiempo; i++)
            {
                cronogramas.Add(new CronogramaPago
                {
                    IdCronograma = Guid.NewGuid(),
                    IdContrato = idContrato,
                    PeriodoDesde = fechaActual.Date,
                    PeriodoHasta = fechaActual.AddMonths(1).Date,
                    OrdenPago = i,
                    FechaOrden = DateTime.UtcNow,
                    Ci = "",
                    FechaCi = null,
                    Observacion = "",
                    Activo = true,
                    IdResponsable = idResponsable
                });

                fechaActual = fechaActual.AddMonths(1);
            }

            return cronogramas;
        }

        public async Task<IEnumerable<ContratoViewDto>> GetAllAsync()
        {
            return await _context.Contratos
                .Where(c => c.Activo)
                .AsNoTracking()
                .Select(c => new ContratoViewDto
                {
                    IdContrato = c.IdContrato,
                    IdPredio = c.IdPredio,
                    IdAdministrado = c.IdAdministrado,
                    Periodo = c.Periodo,
                    Numero = c.Numero,
                    FechaInicia = c.FechaInicia,
                    Tiempo = c.Tiempo,
                    Importe = c.Importe,
                    Agua = c.Agua,
                    Electricidad = c.Electricidad,
                    Activo = c.Activo,
                    IdResponsable = c.IdResponsable
                })
                .ToListAsync();
        }

        public async Task<ContratoDetalleDto?> GetByIdAsync(Guid id)
        {
            var contrato = await _context.Contratos
                .Include(c => c.CronogramasPago)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.IdContrato == id);

            if (contrato == null) return null;

            var cronogramasDto = contrato.CronogramasPago
                .Where(c => c.Activo)
                .Select(c => new CronogramaPagoDto
                {
                    IdCronograma = c.IdCronograma,
                    PeriodoDesde = c.PeriodoDesde,
                    PeriodoHasta = c.PeriodoHasta,
                    OrdenPago = c.OrdenPago,
                    FechaOrden = c.FechaOrden,
                    Ci = c.Ci,
                    FechaCi = c.FechaCi,
                    Observacion = c.Observacion,
                    Activo = c.Activo,
                    IdResponsable = c.IdResponsable
                })
                .ToList();

            return new ContratoDetalleDto
            {
                IdContrato = contrato.IdContrato,
                IdPredio = contrato.IdPredio,
                IdAdministrado = contrato.IdAdministrado,
                Periodo = contrato.Periodo,
                Numero = contrato.Numero,
                FechaInicia = contrato.FechaInicia,
                Tiempo = contrato.Tiempo,
                Importe = contrato.Importe,
                Agua = contrato.Agua,
                Electricidad = contrato.Electricidad,
                Activo = contrato.Activo,
                IdResponsable = contrato.IdResponsable,
                Cronogramas = cronogramasDto
            };
        }

        public async Task<ContratoResponseDto?> CreateAsync(ContratoCreateDto dto)
        {
            var predio = await _context.Predios
                .Include(p => p.PredioTipo)
                .FirstOrDefaultAsync(p => p.IdPredio == dto.IdPredio);

            if (predio == null || predio.PredioTipo.NombreTipo.ToLower().Contains("auditorio"))
                return null;

            int numeroContrato = await _context.Contratos.CountAsync() + 1;
            string periodoContrato = $"{DateTime.UtcNow.Year}-{numeroContrato}";

            var idContrato = Guid.NewGuid();

            var contrato = new Contrato
            {
                IdContrato = idContrato,
                IdPredio = dto.IdPredio,
                IdAdministrado = dto.IdAdministrado,
                Periodo = periodoContrato,
                Numero = numeroContrato,
                FechaInicia = dto.FechaInicia,
                Tiempo = dto.Tiempo,
                Importe = dto.Importe,
                Agua = dto.Agua,
                Electricidad = dto.Electricidad,
                Activo = true,
                IdResponsable = dto.IdResponsable,
                CronogramasPago = GenerarCronogramaPagos(idContrato, dto.FechaInicia, dto.Tiempo, dto.IdResponsable)
            };

            _context.Contratos.Add(contrato);
            await _context.SaveChangesAsync();

            return new ContratoResponseDto
            {
                IdContrato = contrato.IdContrato,
                IdPredio = contrato.IdPredio,
                IdAdministrado = contrato.IdAdministrado,
                Periodo = contrato.Periodo,
                Numero = contrato.Numero,
                FechaInicia = contrato.FechaInicia,
                Tiempo = contrato.Tiempo,
                Importe = contrato.Importe,
                Agua = contrato.Agua,
                Electricidad = contrato.Electricidad
            };
        }

        public async Task<ContratoResponseDto?> PatchAsync(Guid id, ContratoPatchDto dto)
        {
            var contrato = await _context.Contratos
                .Include(c => c.CronogramasPago)
                .FirstOrDefaultAsync(c => c.IdContrato == id);

            if (contrato == null) return null;

            bool contratoPagado = contrato.CronogramasPago
                .Where(c => c.Activo)
                .All(c => !string.IsNullOrEmpty(c.Ci) && c.FechaCi != null);

            if (contratoPagado) return null;

            if (dto.FechaInicia.HasValue) contrato.FechaInicia = dto.FechaInicia.Value;
            if (dto.Importe.HasValue) contrato.Importe = dto.Importe.Value;
            if (dto.Agua.HasValue) contrato.Agua = dto.Agua.Value;
            if (dto.Electricidad.HasValue) contrato.Electricidad = dto.Electricidad.Value;

            await _context.SaveChangesAsync();

            return new ContratoResponseDto
            {
                IdContrato = contrato.IdContrato,
                IdPredio = contrato.IdPredio,
                IdAdministrado = contrato.IdAdministrado,
                Periodo = contrato.Periodo,
                Numero = contrato.Numero,
                FechaInicia = contrato.FechaInicia,
                Tiempo = contrato.Tiempo,
                Importe = contrato.Importe,
                Agua = contrato.Agua,
                Electricidad = contrato.Electricidad
            };
        }

        public async Task<ContratoResponseDto?> UpdateAsync(Guid id, ContratoUpdateDto dto)
        {
            var contrato = await _context.Contratos
                .Include(c => c.CronogramasPago)
                .FirstOrDefaultAsync(c => c.IdContrato == id);

            if (contrato == null) return null;

            bool contratoPagado = contrato.CronogramasPago
                .Where(c => c.Activo)
                .All(c => !string.IsNullOrEmpty(c.Ci) && c.FechaCi != null);

            if (contratoPagado) return null;

            contrato.FechaInicia = dto.FechaInicia;
            contrato.Importe = dto.Importe;
            contrato.Agua = dto.Agua;
            contrato.Electricidad = dto.Electricidad;

            await _context.SaveChangesAsync();

            return new ContratoResponseDto
            {
                IdContrato = contrato.IdContrato,
                IdPredio = contrato.IdPredio,
                IdAdministrado = contrato.IdAdministrado,
                Periodo = contrato.Periodo,
                Numero = contrato.Numero,
                FechaInicia = contrato.FechaInicia,
                Tiempo = contrato.Tiempo,
                Importe = contrato.Importe,
                Agua = contrato.Agua,
                Electricidad = contrato.Electricidad
            };
        }

        public async Task<bool> DesactivarAsync(Guid id)
        {
            var contrato = await _context.Contratos
                .Include(c => c.CronogramasPago)
                .FirstOrDefaultAsync(c => c.IdContrato == id);

            if (contrato == null) return false;

            contrato.Activo = false;
            foreach (var cronograma in contrato.CronogramasPago)
            {
                cronograma.Activo = false;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<(List<ContratoFilterDto> Items, int TotalCount)> GetPagedAsync(
    int page, int pageSize, ContratoFilterDto filters)
        {
            var query = _context.Contratos
                .Include(c => c.Administrado)
                .Include(c => c.Predio)
                .Where(c => c.Activo)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(filters.SearchTerm))
            {
                query = query.Where(c =>
                    c.Predio.NombrePredio.Contains(filters.SearchTerm) ||
                    c.Administrado.DocIdentNro.Contains(filters.SearchTerm) ||
                    c.Administrado.DocIdentTipo.Contains(filters.SearchTerm) ||
                    c.Periodo.Contains(filters.SearchTerm));
            }

            if (!string.IsNullOrWhiteSpace(filters.DocIdentTipo))
                query = query.Where(c => c.Administrado.DocIdentTipo.Contains(filters.DocIdentTipo));

            if (!string.IsNullOrWhiteSpace(filters.DocIdentNro))
                query = query.Where(c => c.Administrado.DocIdentNro.Contains(filters.DocIdentNro));

            if (!string.IsNullOrWhiteSpace(filters.NombrePredio))
                query = query.Where(c => c.Predio.NombrePredio.Contains(filters.NombrePredio));

            if (!string.IsNullOrWhiteSpace(filters.Periodo))
                query = query.Where(c => c.Periodo.Contains(filters.Periodo));

            if (filters.Numero.HasValue)
                query = query.Where(c => c.Numero == filters.Numero.Value);

            if (filters.FechaInicia.HasValue)
                query = query.Where(c => c.FechaInicia.Date == filters.FechaInicia.Value.Date);

            if (filters.Tiempo.HasValue)
                query = query.Where(c => c.Tiempo == filters.Tiempo.Value);

            if (filters.Importe.HasValue)
                query = query.Where(c => c.Importe == filters.Importe.Value);

            if (filters.Agua.HasValue)
                query = query.Where(c => c.Agua == filters.Agua.Value);

            if (filters.Electricidad.HasValue)
                query = query.Where(c => c.Electricidad == filters.Electricidad.Value);

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(c => c.FechaInicia)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new ContratoFilterDto
                {
                    NombrePredio = c.Predio.NombrePredio,
                    DocIdentTipo = c.Administrado.DocIdentTipo,
                    DocIdentNro = c.Administrado.DocIdentNro,
                    Periodo = c.Periodo,
                    Numero = c.Numero,
                    FechaInicia = c.FechaInicia,
                    Tiempo = c.Tiempo,
                    Importe = c.Importe,
                    Agua = c.Agua,
                    Electricidad = c.Electricidad
                })
                .ToListAsync();

            return (items, totalCount);
        }

    }
}
