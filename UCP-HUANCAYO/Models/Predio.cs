using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UCP_HUANCAYO.Models
{
    [Table("predio")]
    public class Predio
    {
        [Key]
        [Column("id_predio")]
        public Guid IdPredio { get; set; }

        [Column("id_predio_tipo")]
        public Guid IdPredioTipo { get; set; }

        public PredioTipo? PredioTipo { get; set; }

        [Column("nombre_predio")]
        public string? NombrePredio { get; set; }

        [Column("descripcion")]
        public string? Descripcion { get; set; }

        [Column("area_predio")]
        public decimal AreaPredio { get; set; }

        [Column("capacidad")]
        public int? Capacidad { get; set; }

        [Column("registro_agua")]
        public bool? RegistroAgua { get; set; }

        [Column("registro_luz")]
        public bool? RegistroLuz { get; set; }

        [Column("direccion")]
        public string? Direccion { get; set; }

        [Column("ubigeo")]
        public string? Ubigeo { get; set; }

        [Column("latitud")]
        public string? Latitud { get; set; }

        [Column("longitud")]
        public string? Longitud { get; set; }

        [Column("activo")]
        public bool Activo { get; set; }

        [Column("id_responsable")]
        public Guid IdResponsable { get; set; }

        public ICollection<PredioImagen>? Imagenes { get; set; }
    }
}
