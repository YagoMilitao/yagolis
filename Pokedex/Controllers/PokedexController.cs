using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Pokedex.Models;

namespace Pokedex.Controllers
{
    [ApiController]
    [Route("pokedex/controller")]
    public class PokedexController :ControllerBase
    {
        private static readonly HttpClient client = new HttpClient();
        private static readonly string pokeApiUrl = "https://pokeapi.co/api/v2/pokemon/";

        private static List<PokemonModel> pokemons = new List<PokemonModel>();

        [HttpGet]
        public ActionResult<IEnumerable<PokemonModel>> Get()
        {
            return pokemons;
        }

        [HttpGet("{id}")]
        public ActionResult<PokemonModel> Get(int id)
        {
            var pokemon = pokemons.FirstOrDefault(p => p.Id == id);
            if (pokemon == null)
            {
                return NotFound();
            }
            return pokemon;
        }
        [HttpPost]
        public async Task<ActionResult<PokemonModel>> Post(PokemonModel pokemon)
        {
            // buscar dados do pokemon na PokeAPI
            var response = await client.GetAsync(pokeApiUrl + pokemon.Numero);
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var pokemonData = JsonConvert.DeserializeObject<JObject>(responseString);

                // preencher dados do pokemon capturado com os dados da PokeAPI
                pokemon.Nome = pokemonData["name"].ToString();
                pokemon.Tipo = pokemonData["types"].Select(t => t["type"]["name"].ToString()).ToArray();
            }
            else
            {
                return BadRequest();
            }

            pokemon.Id = pokemons.Count + 1;
            pokemons.Add(pokemon);

            return CreatedAtAction(nameof(Get), new { id = pokemon.Id }, pokemon);
        }

        /*[HttpPut("{id}")]
        public ActionResult<PokemonModel> Put(int id, PokemonModel pokemon)
        {
            var pokemonIndex = pokemons.FindIndex(p => p.Id == id);
            if (pokemonIndex == -1)
            {
                {
                    throw new Exception($"Pokemon with ID {id} not found.");
                }

                pokemons[pokemonIndex] = pokemon;

                return NoContent();
            }
            //Adicionando um retorno padrão no final do método
            return BadRequest();
        }*/

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var pokemonIndex = pokemons.FindIndex(p => p.Id == id);
            if (pokemonIndex == -1)
            {
            return NotFound();
            }
            pokemons.RemoveAt(pokemonIndex);
            return NoContent();
            }
    }
}
