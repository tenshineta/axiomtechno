using System.ComponentModel.DataAnnotations;

namespace axiomtechno.DTO
{
    public class UsuarioDTO
    {
        [Required]
        [RegularExpression(@"^[A-Za-zÁÉÍÓÚáéíóúÑñ\s]+$", ErrorMessage = "El nombre solo debe contener letras.")]
        [Display(Name = "Nombre de usuario")]
        public string UsNombre { get; set; }

        [Required]
        [RegularExpression(@"^[A-Za-zÁÉÍÓÚáéíóúÑñ\s]+$", ErrorMessage = "El apellido solo debe contener letras.")]
        [Display(Name = "Apellido de usuario")]
        public string UsApellido { get; set; }

        [Required]
        [Range(6000000, 999999999, ErrorMessage = "El DNI debe tener 8 números exactos.")]
        [Display(Name = "DNI de usuario")]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "El DNI debe tener 8 números exactos.")]
        public long UsDni { get; set; }

        [Required]
        [Display(Name = "Email de usuario")]
        [EmailAddress(ErrorMessage = "El formato del email es inválido.")]
        public string UsCorreo { get; set; }

        [Display(Name = "Número de celular")]
        public long UsTelefono { get; set; }
        [Required]
        [Display(Name = "Ingrese su contraseña")]
        public string UsPasswordHash { get; set; }

        [Display(Name = "Ingrese su fecha de nacimiento.")]
        public DateTime UsFechaNacimiento { get; set; } = new DateTime(2000, 1, 1);

        public string? Rol { get; set; }
        public bool UsAutenticado { get; set; }
        public bool UsActivo { get; set; }
        public DateTime UsFechaCreacion { get; set; } = DateTime.Now;
    }
}
