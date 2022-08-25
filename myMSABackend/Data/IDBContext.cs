using Microsoft.EntityFrameworkCore;
using myMSABackend.Model;

namespace myMSABackend.Data
{
    // used for NSubstitute testing
    public interface IDBContext
    {
        DbSet<Pokemon> Pokemons { get; set; }
        int SaveChanges();
    }
}