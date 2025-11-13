using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UCP_HUANCAYO.Models
{
    [Table("contrato")]
    public class Contrato
    {
        [Key]
        [Column("id_contrato")]
        public Guid IdContrato { get; set; }

        [Column("id_predio")]
        public Guid IdPredio { get; set; }

        [ForeignKey("IdPredio")]
        public Predio? Predio { get; set; }

        [Column("id_administrado")]
        public Guid IdAdministrado { get; set; }

        [ForeignKey("IdAdministrado")]
        public Administrado? Administrado { get; set; }

        [Column("periodo")]
        public string? Periodo { get; set; }

        [Column("numero")]
        public int Numero { get; set; }

        [Column("fecha_inicia")]
        public DateTime FechaInicia { get; set; }

        [Column("tiempo")]
        public int Tiempo { get; set; }

        [Column("importe")]
        public decimal Importe { get; set; }

        [Column("agua")]
        public decimal? Agua { get; set; }

        [Column("electricidad")]
        public decimal? Electricidad { get; set; }

        [Column("activo")]
        public bool Activo { get; set; }

        [Column("id_responsable")]
        public Guid IdResponsable { get; set; }

        public ICollection<CronogramaPago> CronogramasPago { get; set; } = new List<CronogramaPago>();

    }
}
