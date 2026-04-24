using System.ComponentModel.DataAnnotations;

namespace axiomtechno.Models
{
    public class Clientes
    {
        [Key]
        public int ClId { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚÜúññÑ\s]+$", ErrorMessage = "Solo permite letras")]
        [MaxLength(50, ErrorMessage = "No se permiten más de 50 caracteres")]
        [Display(Name = "Nombre del cliente")]
        public string ClNombre { get; set; }

        [Required(ErrorMessage = "La descripcion es obligatorio")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚÜúññÑ\s]+$", ErrorMessage = "Solo permite letras")]
        [MaxLength(50, ErrorMessage = "No se permiten más de 50 caracteres")]
        [Display(Name = "Descripcion")]
        public string ClDesc { get; set; }

        [Display(Name = "URL del cliente")]
        public string ClUrl { get; set; }

        [Required(ErrorMessage = "El telefono es obligatorio")]
        [RegularExpression(@"^\d{10}", ErrorMessage = "Ingrese un número de teléfono válido de 10 dígitos.")]
        [Range(1000000000, 9999999999, ErrorMessage = "Ingrese un número de teléfono válido sin espacios ni guiones y sin 0 ni 15.")]
        [Display(Name = "Número de teléfono")]
        public long ClTelefono { get; set; }

        [Display(Name = "Imagen del cliente")]
        public string ClImagen { get; set; }

        [Display(Name = "El usuario está activo?")]
        public bool ClActivo { get; set; }

        [Display(Name = "Fecha de alta del cliente")]
        public DateOnly ClFechaAlta { get; set; }

        [Display(Name = "Fecha de baja del cliente")]
        public DateOnly ClFechaBaja { get; set; }

        public virtual ICollection<Pagos> Pagos { get; set; } = new List<Pagos>();
    }
}
