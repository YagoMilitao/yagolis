namespace Pokedex.Models
{
    public class PokemonModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Numero { get; set; }
        public string[] Tipo { get; set; }
        public string Genero { get; set; }
    }
}
