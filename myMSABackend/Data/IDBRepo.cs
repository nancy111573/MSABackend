using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using myMSABackend.Model;

namespace myMSABackend.Data
{
    // used for NSubstitute testing
    public interface IDBRepo
    {
        IEnumerable<Pokemon> GetPokemons();
        Pokemon GetPokemonByName(string name);
        Pokemon AddPokemon(Pokemon name);
        Pokemon SetPokemon(string oldName, string nickname);
        void RemovePokemon(Pokemon pokemon);
        void SaveChanges();
    }
}
