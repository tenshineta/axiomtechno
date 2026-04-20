using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace axiomtechno.Models
{
    public class Usuarios
    {
        [Key]
        public int UsId { get; set; }
        public string UsNombre { get; set; }
        public string UsApellido { get; set; }
        public long UsDni { get; set; }
        public DateOnly UsFechaNacimiento { get; set; }
        public DateOnly UsFechaCreacion { get; set; } = new DateOnly();
        public string UsCorreo { get; set; }
        public string UsPasswordHash { get; set; }
        public string? token_recovery { get; set; }
        public bool UsActivo { get; set; }
        public long UsTelefono { get; set; }
        [NotMapped]
        public string NombreCompleto => $"{UsNombre} {UsApellido}";
        public int RolID { get; set; }
        public virtual Roles? Rol { get; set; }
    }
}
