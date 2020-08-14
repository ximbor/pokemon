
namespace PokemonCore.Models
{
    public class BaseConfiguration
    {
        public PokemonProviderConfiguration PokemonConfiguration { get; set; }
            = new PokemonProviderConfiguration();
        public TextTransformProviderConfiguration TextTransformConfiguration { get; set; }
            = new TextTransformProviderConfiguration();

        public override string ToString()
        {
            return $"Configuration:\n" +
                   $"{PokemonConfiguration.ToString()}\n" +
                   $"{TextTransformConfiguration.ToString()}";
        }
    }
}
