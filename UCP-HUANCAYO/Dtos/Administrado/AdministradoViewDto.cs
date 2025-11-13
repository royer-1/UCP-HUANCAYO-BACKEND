namespace UCP_HUANCAYO.Dtos.Administrado
{
    public class AdministradoViewDto
    {
        public Guid IdAdministrado { get; set; }
        public string? DocIdentTipo { get; set; }
        public string? DocIdentNro { get; set; }
        public string? RazonSocial { get; set; }
        public string? Telefono { get; set; }
        public string? Correo { get; set; }
        public string? Direccion { get; set; }
        public string? Referencia { get; set; }
        public string? Ubigeo { get; set; }
        //public Guid IdResponsable { get; set; }
        //public bool Activo { get; set; }

    }
}

