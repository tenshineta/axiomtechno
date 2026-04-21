using Microsoft.EntityFrameworkCore;

namespace axiomtechno.Data
{
    public class axiomtechnocontext : DbContext
    {
        public axiomtechnocontext(DbContextOptions<axiomtechnocontext> options) : base(options)
        {
        }
        public DbSet<axiomtechno.Models.Usuarios> Usuarios { get; set; }
        public DbSet<axiomtechno.Models.Clientes> Clientes { get; set; }
        public DbSet<axiomtechno.Models.Pagos> Pagos { get; set; }
        public DbSet<axiomtechno.Models.Roles> Roles { get; set; }
    }
}