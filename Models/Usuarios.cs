using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace axiomtechno.Models
{
    public class Usuarios
    {
        [Key]
        public int UsId { get; set; }

        [Required]
        [RegularExpression(@"^[\p{L}\s'-]+$", ErrorMessage = "El nombre solo debe contener letras y espacios.")]
        [Display(Name = "Nombre de usuario")]
        public string UsNombre { get; set; }

        [Required]
        [RegularExpression(@"^[\p{L}\s'-]+$", ErrorMessage = "El apellido solo debe contener letras y espacios.")]
        [Display(Name = "Apellido de usuario")]
        public string UsApellido { get; set; }

        [Required]
        [Range(6000000, 999999999, ErrorMessage = "El DNI debe tener 8 números exactos.")]
        [Display(Name = "DNI de usuario")]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "El DNI debe tener 8 números exactos.")]
        public long? UsDni { get; set; }

        public DateTime UsFechaNacimiento { get; set; }

        public DateTime UsFechaCreacion { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Correo de usuario")]
        [EmailAddress(ErrorMessage = "El formato del correo es inválido.")]
        public string UsCorreo { get; set; }

        [Required]
        [MinLength(8)]
        public string UsPasswordHash { get; set; }

        public string? token_recovery { get; set; }

        [Display(Name = "¿Usuario activo?")]
        public bool UsActivo { get; set; }

        [Display(Name = "Número de celular")]
        public long UsTelefono { get; set; }

        [NotMapped]
        public string NombreCompleto => $"{UsNombre} {UsApellido}";

        public int RolID { get; set; }

        public virtual Roles? Rol { get; set; }
    }
}
