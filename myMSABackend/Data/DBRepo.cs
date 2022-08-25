using System.Collections.Generic;
using System.Linq;
using myMSABackend.Model;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace myMSABackend.Data
{
    public class DBRepo : IDBRepo
    {
        private readonly IDBContext _dbContext;

        public DBRepo(IDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Pokemon> GetPokemons()
        {
            IEnumerable<Pokemon> pokemons = _dbContext.Pokemons.ToList<Pokemon>();
            return pokemons;
        }

        public Pokemon GetPokemonByName(string name)
        {
            if (name != null)
            {
                Pokemon poke = _dbContext.Pokemons.FirstOrDefault(e => e.Name == name.ToLower());
                return poke;
            }
            return null;
        }

        public Pokemon AddPokemon(Pokemon poke)
        {
            EntityEntry<Pokemon> e = _dbContext.Pokemons.Add(poke);
            Pokemon c = e.Entity;
            SaveChanges();
            return c;
        }

        public Pokemon SetPokemon(string name, string nickname)
        {
            Pokemon p = GetPokemonByName(name);
            if (p != null)
            {
                p.Nickname = nickname;
                _dbContext.SaveChanges();
            }
            return p;
        }

        public void RemovePokemon(Pokemon pokemon)
        {
            _dbContext.Pokemons.Remove(pokemon);
            SaveChanges();
        }


        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}
