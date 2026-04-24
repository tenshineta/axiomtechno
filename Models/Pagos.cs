using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace axiomtechno.Models
{
    public class Pagos
    {
        [Key]
        public int PagId { get; set; }
        [Display(Name = "Monto del pago")]
        [Required(ErrorMessage = "Debe ingresar un monto.")]
        [Range(0.1, 9999999999.99, ErrorMessage = "El monto debe ser mayor a 0.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PagMonto { get; set; }
        [Display(Name = "Fecha del pago")]
        [Required(ErrorMessage = "Debe ingresar una fecha del pago.")]
        [DataType(DataType.Date)]
        public DateOnly PagFecha { get; set; }
        [Display(Name = "Cliente")]
        [Required(ErrorMessage = "Debe seleccionar un cliente.")]
        public int ClId { get; set; }
        [ForeignKey("ClId")]
        public Clientes? Cliente { get; set; }
    }
}
