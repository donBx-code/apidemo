using Microsoft.EntityFrameworkCore;

namespace apidemo.Models
{
    public class ClientContext : DbContext
    {
        public ClientContext(DbContextOptions<ClientContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
    

            modelBuilder.Seed();
        }

        public DbSet<Client> Clients { get; set; }
    
    }
}
