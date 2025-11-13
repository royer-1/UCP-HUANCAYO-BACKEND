using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UCP_HUANCAYO.Models
{
    [Table("alquiler")]
    public class Alquiler
    {
        [Key]
        [Column("id_alquiler")]
        public Guid IdAlquiler { get; set; }

        [Column("id_predio")]
        public Guid IdPredio { get; set; }

        public Predio? Predio { get; set; }

        [Column("id_administrado")]
        public Guid IdAdministrado { get; set; }
        public Administrado? Administrado { get; set; }

        [Column("periodo_desde")]
        public DateTime PeriodoDesde { get; set; }

        [Column("periodo_hasta")]
        public DateTime PeriodoHasta { get; set; }

        [Column("costo")]
        public decimal Costo { get; set; }

        [Column("orden_pago")]
        public int OrdenPago { get; set; }

        [Column("fecha_orden")]
        public DateTime FechaOrden { get; set; }

        [Column("ci")]
        public string? Ci { get; set; }

        [Column("fecha_ci")]
        public DateTime? FechaCi { get; set; }

        [Column("observacion")]
        public string? Observacion { get; set; }

        [Column("activo")]
        public bool Activo { get; set; }

        [Column("id_responsable")]
        public Guid IdResponsable { get; set; }
    }
}
