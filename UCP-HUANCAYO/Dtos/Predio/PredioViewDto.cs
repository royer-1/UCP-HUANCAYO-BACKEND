namespace UCP_HUANCAYO.Dtos.Predio
{
    public class PredioViewDto
    {
        public Guid IdPredio { get; set; }
        public Guid IdPredioTipo { get; set; }
        public string? NombrePredio { get; set; }
        public string? NombreTipo { get; set; }
        public string? Descripcion { get; set; }
        public decimal AreaPredio { get; set; }
        public int? Capacidad { get; set; }
        public bool? RegistroAgua { get; set; }
        public bool? RegistroLuz { get; set; }
        public string? Direccion { get; set; }
        public string? Ubigeo { get; set; }
        public string? Latitud { get; set; }
        public string? Longitud { get; set; }
        public Guid IdResponsable { get; set; }
        public bool Activo { get; set; }
        public List<string>? Imagenes { get; set; }
    }
}
