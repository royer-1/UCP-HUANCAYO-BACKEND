using System.Security.Claims;

namespace UCP_HUANCAYO.Helpers
{
    public class UsuarioContextHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsuarioContextHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid GetUsuarioActual()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var idUsuarioClaim = httpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return idUsuarioClaim != null ? Guid.Parse(idUsuarioClaim) : Guid.Empty;
        }

        public string GetLoginName()
        {
            return _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "anónimo";
        }

        public string GetIpAddress()
        {
            return _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "desconocido";
        }

        public string GetSessionId()
        {
            return _httpContextAccessor.HttpContext?.TraceIdentifier ?? Guid.NewGuid().ToString();
        }
    }
}
