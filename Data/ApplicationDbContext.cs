using BeSkSalon.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BeSkSalon.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Frizer> Frizeri { get; set; }
        public DbSet<Usluga> Usluge { get; set; }
        public DbSet<Termin> Termini { get; set; }
    }
}
