namespace UCP_HUANCAYO.Dtos.Token
{
    public class TokenViewDto
    {
        public Guid IdToken { get; set; }
        public Guid IdUsuario { get; set; }
        public DateTime Expiracion { get; set; }
        public string? Ip { get; set; }
        public bool Revocado { get; set; }
    }
}
