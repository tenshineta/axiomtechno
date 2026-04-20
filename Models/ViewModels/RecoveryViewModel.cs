using System.ComponentModel.DataAnnotations;

namespace axiomtechno.Models.ViewModels
{
    public class RecoveryViewModel
    {
        [EmailAddress]
        [Required(ErrorMessage = "El campo email es obligatorio.")]
        public string? UsEmail { get; set; }
    }
}
