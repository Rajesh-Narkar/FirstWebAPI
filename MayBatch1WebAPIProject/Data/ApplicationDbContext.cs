using Microsoft.EntityFrameworkCore;
using MayBatch1WebAPIProject.Models;
namespace MayBatch1WebAPIProject.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options) 
        { 
        }
        public DbSet<Product> products { get; set; }
        public DbSet<Login> users { get; set; }
    }
}
