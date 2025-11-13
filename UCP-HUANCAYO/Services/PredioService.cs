using Microsoft.EntityFrameworkCore;
using UCP_HUANCAYO.Data;
using UCP_HUANCAYO.Dtos.Predio;
using UCP_HUANCAYO.Models;

namespace UCP_HUANCAYO.Services
{
    public class PredioService
    {
        private readonly ApplicationDbContext _context;

        public PredioService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PredioViewDto>> GetAllAsync()
        {
            return await _context.Predios
                .Include(p => p.PredioTipo)
                .Where(p => p.Activo)
                .AsNoTracking()
                .Select(p => new PredioViewDto
                {
                    IdPredio = p.IdPredio,
                    IdPredioTipo = p.IdPredioTipo,
                    NombrePredio = p.NombrePredio,
                    NombreTipo = p.PredioTipo.NombreTipo,
                    Descripcion = p.Descripcion,
                    AreaPredio = p.AreaPredio,
                    Capacidad = p.Capacidad,
                    RegistroAgua = p.RegistroAgua,
                    RegistroLuz = p.RegistroLuz,
                    Direccion = p.Direccion,
                    Ubigeo = p.Ubigeo,
                    Latitud = p.Latitud,
                    Longitud = p.Longitud,

                    Imagenes = _context.PredioImagenes
                        .Where(i => i.IdPredio == p.IdPredio)
                        .Select(i => i.Imagen!)
                        .ToList()
                })
                .ToListAsync();
        }

        public async Task<PredioViewDto?> GetByIdAsync(Guid id)
        {
            return await _context.Predios
                .Include(p => p.PredioTipo)
                .Where(p => p.IdPredio == id)
                .AsNoTracking()
                .Select(p => new PredioViewDto
                {
                    IdPredio = p.IdPredio,
                    IdPredioTipo = p.IdPredioTipo,
                    NombrePredio = p.NombrePredio,
                    NombreTipo = p.PredioTipo.NombreTipo,
                    Descripcion = p.Descripcion,
                    AreaPredio = p.AreaPredio,
                    Capacidad = p.Capacidad,
                    RegistroAgua = p.RegistroAgua,
                    RegistroLuz = p.RegistroLuz,
                    Direccion = p.Direccion,
                    Ubigeo = p.Ubigeo,
                    Latitud = p.Latitud,
                    Longitud = p.Longitud,

                    Imagenes = _context.PredioImagenes
                        .Where(i => i.IdPredio == p.IdPredio)
                        .Select(i => i.Imagen!)
                        .ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<PredioViewDto> CreateAsync(PredioCreateDto dto)
        {
            var predio = new Predio
            {
                IdPredio = Guid.NewGuid(),
                IdPredioTipo = dto.IdPredioTipo,
                NombrePredio = dto.NombrePredio,
                Descripcion = string.IsNullOrWhiteSpace(dto.Descripcion) ? null : dto.Descripcion,
                AreaPredio = dto.AreaPredio,
                Capacidad = dto.Capacidad,
                RegistroAgua = dto.RegistroAgua,
                RegistroLuz = dto.RegistroLuz,
                Direccion = dto.Direccion,
                Ubigeo = dto.Ubigeo,
                Latitud = string.IsNullOrWhiteSpace(dto.Latitud) ? null : dto.Latitud,
                Longitud = string.IsNullOrWhiteSpace(dto.Longitud) ? null : dto.Longitud,
                IdResponsable = dto.IdResponsable,
                Activo = true
            };

            _context.Predios.Add(predio);

            if (dto.ImagenesPredio != null && dto.ImagenesPredio.Any())
            {
                foreach (var img in dto.ImagenesPredio)
                {
                    var imagen = new PredioImagen
                    {
                        IdImagen = Guid.NewGuid(),
                        IdPredio = predio.IdPredio,
                        Imagen = img,
                        Activo = true
                    };
                    _context.PredioImagenes.Add(imagen);
                }
            }

            await _context.SaveChangesAsync();

            return new PredioViewDto
            {
                IdPredio = predio.IdPredio,
                IdPredioTipo = predio.IdPredioTipo,
                NombrePredio = predio.NombrePredio,
                Descripcion = predio.Descripcion,
                AreaPredio = predio.AreaPredio,
                Capacidad = predio.Capacidad,
                RegistroAgua = predio.RegistroAgua,
                RegistroLuz = predio.RegistroLuz,
                Direccion = predio.Direccion,
                Ubigeo = predio.Ubigeo,
                Latitud = predio.Latitud,
                Longitud = predio.Longitud,
                Imagenes = _context.PredioImagenes
                    .Where(i => i.IdPredio == predio.IdPredio)
                    .Select(i => i.Imagen!)
                    .ToList()
            };
        }

        public async Task<PredioViewDto?> PatchAsync(Guid id, PredioPatchDto dto)
        {
            var predio = await _context.Predios.FindAsync(id);
            if (predio == null) return null;

            if (dto.NombrePredio != null) predio.NombrePredio = dto.NombrePredio;
            if (dto.Descripcion != null) predio.Descripcion = dto.Descripcion;
            if (dto.AreaPredio.HasValue) predio.AreaPredio = dto.AreaPredio.Value;
            if (dto.Capacidad.HasValue) predio.Capacidad = dto.Capacidad.Value;
            if (dto.RegistroAgua.HasValue) predio.RegistroAgua = dto.RegistroAgua.Value;
            if (dto.RegistroLuz.HasValue) predio.RegistroLuz = dto.RegistroLuz.Value;
            if (dto.Direccion != null) predio.Direccion = dto.Direccion;
            if (dto.Ubigeo != null) predio.Ubigeo = dto.Ubigeo;
            if (dto.Latitud != null) predio.Latitud = dto.Latitud;
            if (dto.Longitud != null) predio.Longitud = dto.Longitud;

            if (dto.ImagenesPredio != null)
            {
                var imagenesAnteriores = _context.PredioImagenes.Where(i => i.IdPredio == id);
                _context.PredioImagenes.RemoveRange(imagenesAnteriores);

                foreach (var img in dto.ImagenesPredio)
                {
                    if (!string.IsNullOrWhiteSpace(img))
                    {
                        var imagen = new PredioImagen
                        {
                            IdImagen = Guid.NewGuid(),
                            IdPredio = id,
                            Imagen = img,
                            Activo = true
                        };
                        _context.PredioImagenes.Add(imagen);
                    }
                }
            }

            await _context.SaveChangesAsync();

            return new PredioViewDto
            {
                IdPredio = predio.IdPredio,
                IdPredioTipo = predio.IdPredioTipo,
                NombrePredio = predio.NombrePredio,
                Descripcion = predio.Descripcion,
                AreaPredio = predio.AreaPredio,
                Capacidad = predio.Capacidad,
                RegistroAgua = predio.RegistroAgua,
                RegistroLuz = predio.RegistroLuz,
                Direccion = predio.Direccion,
                Ubigeo = predio.Ubigeo,
                Latitud = predio.Latitud,
                Longitud = predio.Longitud,
                Imagenes = await _context.PredioImagenes
                    .Where(i => i.IdPredio == predio.IdPredio && i.Activo)
                    .Select(i => i.Imagen!)
                    .ToListAsync()
            };
        }

        public async Task<PredioViewDto?> UpdateAsync(Guid id, PredioUpdateDto dto)
        {
            var predio = await _context.Predios.FindAsync(id);
            if (predio == null) return null;

            predio.NombrePredio = dto.NombrePredio;
            predio.Descripcion = string.IsNullOrWhiteSpace(dto.Descripcion) ? null : dto.Descripcion;
            predio.AreaPredio = dto.AreaPredio;
            predio.Capacidad = dto.Capacidad;
            predio.RegistroAgua = dto.RegistroAgua;
            predio.RegistroLuz = dto.RegistroLuz;
            predio.Direccion = dto.Direccion;
            predio.Ubigeo = dto.Ubigeo;
            predio.Latitud = string.IsNullOrWhiteSpace(dto.Latitud) ? null : dto.Latitud;
            predio.Longitud = string.IsNullOrWhiteSpace(dto.Longitud) ? null : dto.Longitud;

            var imagenesAnteriores = _context.PredioImagenes.Where(i => i.IdPredio == id);
            _context.PredioImagenes.RemoveRange(imagenesAnteriores);

            if (dto.ImagenesPredio != null && dto.ImagenesPredio.Any())
            {
                foreach (var img in dto.ImagenesPredio)
                {
                    if (!string.IsNullOrWhiteSpace(img))
                    {
                        var imagen = new PredioImagen
                        {
                            IdImagen = Guid.NewGuid(),
                            IdPredio = id,
                            Imagen = img,
                            Activo = true
                        };
                        _context.PredioImagenes.Add(imagen);
                    }
                }
            }

            await _context.SaveChangesAsync();

            return new PredioViewDto
            {
                IdPredio = predio.IdPredio,
                IdPredioTipo = predio.IdPredioTipo,
                NombrePredio = predio.NombrePredio,
                Descripcion = predio.Descripcion,
                AreaPredio = predio.AreaPredio,
                Capacidad = predio.Capacidad,
                RegistroAgua = predio.RegistroAgua,
                RegistroLuz = predio.RegistroLuz,
                Direccion = predio.Direccion,
                Ubigeo = predio.Ubigeo,
                Latitud = predio.Latitud,
                Longitud = predio.Longitud,
                Imagenes = await _context.PredioImagenes
                    .Where(i => i.IdPredio == predio.IdPredio)
                    .Select(i => i.Imagen!)
                    .ToListAsync()
            };
        }

        public async Task<bool> DesactivarAsync(Guid id)
        {
            var predio = await _context.Predios
                .Include(p => p.Imagenes)
                .FirstOrDefaultAsync(p => p.IdPredio == id);

            if (predio == null) return false;

            predio.Activo = false;

            foreach (var imagen in predio.Imagenes)
            {
                imagen.Activo = false;
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}

