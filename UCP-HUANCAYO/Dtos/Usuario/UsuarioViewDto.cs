namespace UCP_HUANCAYO.Dtos.Usuario
{
    public class UsuarioViewDto
    {
        public Guid IdUsuario { get; set; }
        public Guid IdDominio { get; set; }
        public string? NombreDominio { get; set; }
        public string? Alias { get; set; }
        public string? DocIdentTipo { get; set; }
        public string? DocIdentNro { get; set; }
        public string? Nombres { get; set; }
        public string? Correo { get; set; }
        public string? Telefono { get; set; }
        public string? Clave { get; set; }
    }
}
