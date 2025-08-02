using Microsoft.EntityFrameworkCore;

namespace backend_challenge;

// Could not use because of time constraints
// It was supposed to be a mock database for the application
public class AppDBContext : DbContext
{
    public AppDBContext(DbContextOptions<AppDBContext> options) : base(options){}
    public DbSet<Client> Clients { get; set; }
}