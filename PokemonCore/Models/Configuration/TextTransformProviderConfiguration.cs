
namespace PokemonCore.Models
{
    public class TextTransformProviderConfiguration
    {
        public string EndpointBaseUrl { get; set; } = "https://api.funtranslations.com/translate/shakespeare";
        public uint ApiVersion { get; set; } = 1;
        public string ApiSecretkey { get; set; } = "";

        public override string ToString()
        {
            return $"TEXT-TRANSFORM API ENDPOINT: {EndpointBaseUrl}\n" +
                   $"TEXT-TRANSFORM API VERSION: {ApiVersion}\n" +
                   $"TEXT-TRANSFORM API VERSION: {ApiSecretkey}";
        }
    }
}
