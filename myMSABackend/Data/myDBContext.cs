using Microsoft.EntityFrameworkCore;
using myMSABackend.Model;

namespace myMSABackend.Data
{
    public class myDBContext : DbContext, IDBContext
    {
        public myDBContext(DbContextOptions<myDBContext> options) : base(options)
        { 
            Database.EnsureCreated(); 
        }
        public DbSet<Pokemon> Pokemons { get; set; }
    }

}
