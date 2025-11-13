using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UCP_HUANCAYO.Models
{
    [Table("cronograma_pago")]
    public class CronogramaPago
    {
        [Key]
        [Column("id_cronograma")]
        public Guid IdCronograma { get; set; }

        [Column("id_contrato")]
        public Guid IdContrato { get; set; }

        [ForeignKey("IdContrato")]
        public Contrato? Contrato { get; set; }

        [Column("periodo_desde")]
        public DateTime PeriodoDesde { get; set; }

        [Column("periodo_hasta")]
        public DateTime PeriodoHasta { get; set; }

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
