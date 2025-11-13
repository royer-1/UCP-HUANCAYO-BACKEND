using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UCP_HUANCAYO.Models
{
    [Table("predio_tipo")]
    public class PredioTipo
    {
        [Key]
        [Column("id_predio_tipo")]
        public Guid IdPredioTipo { get; set; }

        [Column("nombre_tipo")]
        public string? NombreTipo { get; set; }

        [Column("contrato")]
        public bool Contrato { get; set; }

        [Column("id_responsable")]
        public Guid IdResponsable { get; set; }

        [Column("activo")]
        public bool Activo { get; set; }

        public ICollection<Predio>? Predios { get; set; }
    }
}
