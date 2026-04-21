using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace axiomtechno.Models
{
    public class Pagos
    {
        [Key]
        public int PagId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal PagMonto { get; set; }
        public DateOnly PagFecha { get; set; }
        public int ClId { get; set; }
        public Clientes? Cliente { get; set; }
    }
}
