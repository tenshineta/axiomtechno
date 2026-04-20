using System.ComponentModel.DataAnnotations;

namespace axiomtechno.Models
{
    public class Roles
    {
        [Key]
        public int RolId { get; set; }
        public string RolNombre { get; set; }
        public virtual ICollection<Usuarios> Usuarios { get; set; } = new List<Usuarios>();
    }
}
