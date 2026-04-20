using System.ComponentModel.DataAnnotations;

namespace axiomtechno.Models
{
    public class Pagos
    {
        [Key]
        public int PagId { get; set; }
        public decimal PagMonto { get; set; }
        public DateOnly PagFecha { get; set; }
        public int ClId { get; set; }
        public Clientes? Cliente { get; set; }
    }
}
