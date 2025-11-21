namespace UCP_HUANCAYO.Dtos.Auth
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expiracion { get; set; }
        public Guid IdUsuario { get; set; }
        public string Rol { get; set; } = string.Empty;
    }
}
