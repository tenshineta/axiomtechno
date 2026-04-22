using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using axiomtechno.Models;

namespace axiomtechno.Data
{
    public class axiomtechnoContext : DbContext
    {
        public axiomtechnoContext (DbContextOptions<axiomtechnoContext> options)
            : base(options)
        {
        }

        public DbSet<axiomtechno.Models.Clientes> Clientes { get; set; } = default!;
        public DbSet<axiomtechno.Models.Pagos> Pagos { get; set; } = default!;
        public DbSet<axiomtechno.Models.Roles> Roles { get; set; } = default!;
        public DbSet<axiomtechno.Models.Usuarios> Usuarios { get; set; } = default!;
    }
}
