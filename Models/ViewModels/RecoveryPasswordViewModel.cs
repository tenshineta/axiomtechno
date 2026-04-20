using System.ComponentModel.DataAnnotations;

namespace axiomtechno.Models.ViewModels
{
    public class RecoveryPasswordViewModel
    {
        public string? token { get; set; }

        [Required]
        [Display(Name = "DNI")]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "El DNI debe tener 8 dígitos.")]
        public string? UsDNI { get; set; }

        [Required]
        [Display(Name = "Nueva contraseña")]
        public string? UsPassword { get; set; }

        [Required]
        [Display(Name = "Confirmar contraseña")]
        [Compare("UsPassword", ErrorMessage = "Las contraseñas no coinciden.")]
        public string? UsPasswordConfirm { get; set; }
    }
}
