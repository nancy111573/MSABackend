using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using myMSABackend.Data;
using myMSABackend.Model;
using Microsoft.AspNetCore.Http.Extensions;

namespace myMSABackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly HttpClient _client;
        private readonly IDBRepo _repository;

        /// <summary />
        public PokemonController(IHttpClientFactory clientFactory, IDBRepo repository)
        {
            if (clientFactory is null)
            {
                throw new ArgumentNullException(nameof(clientFactory));
            }
            _client = clientFactory.CreateClient("pokemon");
            _repository = repository;
        }

        /// <summary>
        /// Gets the raw JSON of all items in pokemon
        /// </summary>
        /// <returns>A JSON object representing all items in pokemon</returns>
        [HttpGet]
        [Route("getRawItems")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetRawPokemonItems()
        {
            var res = await _client.GetAsync("item");
            var content = await res.Content.ReadAsStringAsync();
            return Ok(content);
        }

        /// <summary>
        /// Read all pokemons added to the team
        /// </summary>
        /// <returns>A list of Pokemon names</returns>
        [HttpGet]
        [Route("getPokemonTeam")]
        [ProducesResponseType(200)]
        public ActionResult<IEnumerable<Pokemon>> getPokemonTeam()
        {
            IEnumerable<Pokemon> pokemons = _repository.GetPokemons();
            IEnumerable<string> c = pokemons.Select(e => e.Name);
            return Ok(pokemons);
        }

        /// <summary>
        /// Read a pokemon's name, power, and experience if its in the team
        /// </summary>
        /// <param name="name">name of pokemon</param>
        /// <returns>The pokemon's details</returns>
        [HttpGet]
        [Route("getPokemon")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult getPokemon(string name)
        {
            Pokemon p = _repository.GetPokemonByName(name);
            if (p == null)
                return NotFound("Pokemon " + name + " is not in your team.");
            else
            {
                if (p.Nickname != null)
                    return Ok(p);
                else
                    return Ok(p);
            }
        }

        /// <summary>
        /// Get pokemon data from the official api
        /// </summary>
        /// <param name="name">name of a pokemon</param>
        /// <returns>A string of pokemon details</returns>
        [HttpGet]
        [Route("getPokemonFromAPI")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<string> GetRawPokemon(string name)
        {
            var res = await _client.GetAsync("pokemon/" + name);
            var content = await res.Content.ReadAsStringAsync();
            return content;
        }

        /// <summary>
        /// Add a pokemon to the team if its an official pokemon
        /// </summary>
        /// <param name="name">name of a pokemon</param>
        /// <returns>A 201 Created response</returns>
        [HttpPost]
        [Route("addPokemon")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> addPokemon(string name)
        {
            Pokemon p = _repository.GetPokemonByName(name);
            if (p == null)
            {
                var content = GetRawPokemon(name.ToLower()).Result;
                if (content == "Not Found")
                {
                    return NotFound(name + " is not an official Pokemon.");
                }
                // triming to get base experience and use assign to pokemon's "Power"
                int start = content.IndexOf("base_experience") + "base_experience".Length + 2;
                int end = content.Substring(start).IndexOf(",");
                var base_ep = Int32.Parse(content.Substring(start, end));
                Pokemon pokemon = new Pokemon { Name = name, Power = base_ep };
                Pokemon newP = _repository.AddPokemon(pokemon);
                return Created(new Uri(Request.GetEncodedUrl() + "/" + name), newP);
            }
            return BadRequest("Pokemon " + name + " is already in your team.");
        }


        /// <summary>
        /// Set a pokemon's nickname
        /// </summary>
        /// <param name="name">name of a pokemon</param>
        /// <param name="nickname">new value for nickname</param>
        /// <returns>A 201 Created Response></returns>
        [HttpPut]
        [Route("setNickname")]
        [ProducesResponseType(201)]
        [ProducesResponseType(404)]
        public IActionResult setNickname(string name, string nickname)
        {
            Pokemon p = _repository.GetPokemonByName(name);
            if (p == null)
                return NotFound("Pokemon " + name + " is not in your team.");
            else
            {
                _repository.SetPokemon(name, nickname);
                return Ok(p);
            }
        }


        /// <summary>
        /// Delete a pokemon form the team
        /// </summary>
        /// <param name="name">name of a pokemon</param>
        /// <returns>A 204 No Content Response</returns>
        [HttpDelete]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult deletePokemon(string name)
        {
            Pokemon p = _repository.GetPokemonByName(name);
            if (p != null)
            {
                _repository.RemovePokemon(p);
                return NoContent();
            }
            else
            {
                return BadRequest("You don't have a Pokemon " + name + " in your team.");
            }
        }
    }
}
