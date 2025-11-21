using Microsoft.EntityFrameworkCore;
using UCP_HUANCAYO.Data;
using UCP_HUANCAYO.Dtos.Dominio;
using UCP_HUANCAYO.Helpers;
using UCP_HUANCAYO.Models;

namespace UCP_HUANCAYO.Services
{
    public class DominioService
    {
        private readonly ApplicationDbContext _context;
        private readonly AuditoriaHelper _auditoriaHelper;
        private readonly UsuarioContextHelper _usuarioContextHelper;

        public DominioService(ApplicationDbContext context, AuditoriaHelper auditoriaHelper, UsuarioContextHelper usuarioContextHelper)
        {
            _context = context;
            _auditoriaHelper = auditoriaHelper;
            _usuarioContextHelper = usuarioContextHelper;
        }

        private DominioViewDto MapToViewDto(Dominio d)
        {
            return new DominioViewDto
            {
                IdDominio = d.IdDominio,
                Nombre = d.Nombre,
                Ldap = d.Ldap,
                Servidor = d.Servidor,
                Conexion = d.Conexion,
                Default = d.Default
            };
        }

        public async Task<IEnumerable<DominioViewDto>> GetAllAsync()
        {
            return await _context.Dominios
                .Where(d => d.Activo)
                .AsNoTracking()
                .Select(d => MapToViewDto(d))
                .ToListAsync();
        }

        public async Task<DominioViewDto?> GetByIdAsync(Guid id)
        {
            return await _context.Dominios
                .Where(d => d.Activo)
                .AsNoTracking()
                .Select(d => MapToViewDto(d))
                .FirstOrDefaultAsync();
        }

        public async Task<DominioViewDto> CreateAsync(DominioCreateDto dto)
        {
            var usuarioActual = _usuarioContextHelper.GetUsuarioActual();

            var dominio = new Dominio
            {
                IdDominio = Guid.NewGuid(),
                Nombre = dto.Nombre,
                Ldap = dto.Ldap,
                Servidor = dto.Servidor,
                Conexion = dto.Conexion,
                Default = dto.Default,
                Activo = true,
                IdResponsable = usuarioActual
            };

            _context.Dominios.Add(dominio);
            await _context.SaveChangesAsync();

            var detalle = AuditoriaDetalleHelper.GenerarDetalle(dominio, "Dominio creado");
            await _auditoriaHelper.RegistrarAsync("dominio", dominio.IdDominio, "INSERT", detalle);

            return MapToViewDto(dominio);
        }

        public async Task<DominioViewDto?> PatchAsync(Guid id, DominioPatchDto dto)
        {
            var usuarioActual = _usuarioContextHelper.GetUsuarioActual();

            var dominio = await _context.Dominios.FindAsync(id);
            if (dominio == null || !dominio.Activo) return null;

            var dominioAntes = new Dominio
            {
                IdDominio = dominio.IdDominio,
                Nombre = dominio.Nombre,
                Ldap = dominio.Ldap,
                Servidor = dominio.Servidor,
                Conexion = dominio.Conexion,
                Default = dominio.Default,
                Activo = dominio.Activo,
                IdResponsable = dominio.IdResponsable
            };

            if (dto.Nombre != null) dominio.Nombre = dto.Nombre;
            if (dto.Ldap.HasValue) dominio.Ldap = dto.Ldap.Value;
            if (dto.Servidor != null) dominio.Servidor = dto.Servidor;
            if (dto.Conexion != null) dominio.Conexion = dto.Conexion;
            if (dto.Default.HasValue) dominio.Default = dto.Default.Value;

            dominio.IdResponsable = usuarioActual;
            await _context.SaveChangesAsync();

            var detalle = AuditoriaDetalleHelper.GenerarCambios(dominioAntes, dominio, "Dominio actualizado parcialmente");
            await _auditoriaHelper.RegistrarAsync("dominio", dominio.IdDominio, "PATCH", detalle);

            return MapToViewDto(dominio); ;
        }

        public async Task<DominioViewDto?> UpdateAsync(Guid id, DominioUpdateDto dto)
        {
            var usuarioActual = _usuarioContextHelper.GetUsuarioActual();

            var dominio = await _context.Dominios.FindAsync(id);
            if (dominio == null || !dominio.Activo) return null;

            var dominioAntes = new Dominio
            {
                IdDominio = dominio.IdDominio,
                Nombre = dominio.Nombre,
                Ldap = dominio.Ldap,
                Servidor = dominio.Servidor,
                Conexion = dominio.Conexion,
                Default = dominio.Default,
                Activo = dominio.Activo,
                IdResponsable = dominio.IdResponsable
            };

            dominio.Nombre = dto.Nombre;
            dominio.Ldap = dto.Ldap;
            dominio.Servidor = dto.Servidor;
            dominio.Conexion = dto.Conexion;
            dominio.Default = dto.Default;
            dominio.IdResponsable = usuarioActual;

            await _context.SaveChangesAsync();

            var detalle = AuditoriaDetalleHelper.GenerarCambios(dominioAntes, dominio, "Dominio actualizado");
            await _auditoriaHelper.RegistrarAsync("dominio", dominio.IdDominio, "UPDATE", detalle);

            return MapToViewDto(dominio);
        }

        public async Task<bool> DesactivarAsync(Guid id)
        {
            var usuarioActual = _usuarioContextHelper.GetUsuarioActual();

            var dominio = await _context.Dominios.FindAsync(id);
            if (dominio == null || !dominio.Activo) return false;

            dominio.Activo = false;
            dominio.IdResponsable = usuarioActual;

            await _context.SaveChangesAsync();

            await _auditoriaHelper.RegistrarAsync("dominio", dominio.IdDominio, "DELETE", "Dominio desactivado (Activo=false)");
            
            return true;
        }
    }
}
