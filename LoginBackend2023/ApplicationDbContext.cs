using LoginBackend2023.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using static System.Reflection.Metadata.BlobBuilder;

namespace LoginBackend2023
{
    public class ApplicationDbContext : IdentityDbContext {

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Favoritos> Favoritos{ get; set; }
    }
       
    
}


