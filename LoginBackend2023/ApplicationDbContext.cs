using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LoginBackend2023
{
    public class ApplicationDbContext : IdentityDbContext {

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
    }
       
    
}


