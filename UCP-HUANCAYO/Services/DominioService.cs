using Microsoft.EntityFrameworkCore;
using UCP_HUANCAYO.Data;
using UCP_HUANCAYO.Dtos.Dominio;
using UCP_HUANCAYO.Models;

namespace UCP_HUANCAYO.Services
{
    public class DominioService
    {
        private readonly ApplicationDbContext _context;

        public DominioService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DominioViewDto>> GetAllAsync()
        {
            return await _context.Dominios
                .Where(d => d.Activo)
                .AsNoTracking()
                .Select(d => new DominioViewDto
                {
                    IdDominio = d.IdDominio,
                    Nombre = d.Nombre,
                    Ldap = d.Ldap,
                    Servidor = d.Servidor,
                    Conexion = d.Conexion,
                    Default = d.Default
                })
                .ToListAsync();
        }

        public async Task<DominioViewDto?> GetByIdAsync(Guid id)
        {
            return await _context.Dominios
                .Where(d => d.Activo)
                .AsNoTracking()
                .Select(d => new DominioViewDto
                {
                    IdDominio = d.IdDominio,
                    Nombre = d.Nombre,
                    Ldap = d.Ldap,
                    Servidor = d.Servidor,
                    Conexion = d.Conexion,
                    Default = d.Default
                })
                .FirstOrDefaultAsync();
        }

        public async Task<DominioViewDto> CreateAsync(DominioCreateDto dto)
        {
            var dominio = new Dominio
            {
                IdDominio = Guid.NewGuid(),
                Nombre = dto.Nombre,
                Ldap = dto.Ldap,
                Servidor = dto.Servidor,
                Conexion = dto.Conexion,
                Default = dto.Default,
                Activo = true,
                IdResponsable = dto.IdResponsable
            };

            _context.Dominios.Add(dominio);
            await _context.SaveChangesAsync();

            return new DominioViewDto
            {
                IdDominio = dominio.IdDominio,
                Nombre = dominio.Nombre,
                Ldap = dominio.Ldap,
                Servidor = dominio.Servidor,
                Conexion = dominio.Conexion,
                Default = dominio.Default
            };
        }

        public async Task<DominioViewDto?> PatchAsync(Guid id, DominioPatchDto dto)
        {
            var dominio = await _context.Dominios.FindAsync(id);
            if (dominio == null || !dominio.Activo) return null;

            if (dto.Nombre != null) dominio.Nombre = dto.Nombre;
            if (dto.Ldap.HasValue) dominio.Ldap = dto.Ldap.Value;
            if (dto.Servidor != null) dominio.Servidor = dto.Servidor;
            if (dto.Conexion != null) dominio.Conexion = dto.Conexion;
            if (dto.Default.HasValue) dominio.Default = dto.Default.Value;

            await _context.SaveChangesAsync();

            return new DominioViewDto
            {
                IdDominio = dominio.IdDominio,
                Nombre = dominio.Nombre,
                Ldap = dominio.Ldap,
                Servidor = dominio.Servidor,
                Conexion = dominio.Conexion,
                Default = dominio.Default
            };
        }

        public async Task<DominioViewDto?> UpdateAsync(Guid id, DominioUpdateDto dto)
        {
            var dominio = await _context.Dominios.FindAsync(id);
            if (dominio == null || !dominio.Activo) return null;

            dominio.Nombre = dto.Nombre;
            dominio.Ldap = dto.Ldap;
            dominio.Servidor = dto.Servidor;
            dominio.Conexion = dto.Conexion;
            dominio.Default = dto.Default;

            await _context.SaveChangesAsync();

            return new DominioViewDto
            {
                IdDominio = dominio.IdDominio,
                Nombre = dominio.Nombre,
                Ldap = dominio.Ldap,
                Servidor = dominio.Servidor,
                Conexion = dominio.Conexion,
                Default = dominio.Default
            };
        }

        public async Task<bool> DesactivarAsync(Guid id)
        {
            var dominio = await _context.Dominios.FindAsync(id);
            if (dominio == null || !dominio.Activo) return false;

            dominio.Activo = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
