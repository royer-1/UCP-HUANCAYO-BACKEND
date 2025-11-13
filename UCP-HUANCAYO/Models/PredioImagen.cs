using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace UCP_HUANCAYO.Models
{
    [Table("predio_img")]
    public class PredioImagen
    {
        [Key]
        [Column("id_imagen")]
        public Guid IdImagen { get; set; }

        [Column("id_predio")]
        public Guid IdPredio { get; set; }

        [ForeignKey("IdPredio")]
        [JsonIgnore]
        public Predio? Predio { get; set; }

        [Column("imagen")]
        public string? Imagen { get; set; }

        [Column("activo")]
        public bool Activo { get; set; }
    }
}
