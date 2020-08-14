using Pokemon.Models;
using PokemonCore.Models;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Pokemon.Services
{
    public class PokemonProvider: IPokemonProvider
    {
        private static readonly HttpClient _client = new HttpClient() {
            Timeout = TimeSpan.FromMilliseconds(3000)
        };
        private PokemonProviderConfiguration _configuration = new PokemonProviderConfiguration();

        public PokemonProvider()
        {
        }

        private string GetRoute(PokemonRoutes route)
        {
            return $"{GetBaseUrl()}/{PokemonRouting.Routes[(int)route]}";
        }

        private string GetBaseUrl()
        {
            return _configuration.EndpointBaseUrl;
        }

        private int GetPokemonVersionByUrl(string url)
        {
            string strNum = url[0..^1]
                                   .Substring(0)
                                   .Substring(url[0..^1].LastIndexOf('/') + 1);
            int.TryParse(strNum, out int version);
            return version;
        }


        public async Task<string> GetPokemonDescription(string name, string language = "en")
        {

            FlavorTextEntry description = (await GetPokemonByName(name)).Entries
                .Where(entry => entry.Language.Name == language)
                .OrderByDescending(entry => GetPokemonVersionByUrl(entry.Version.Url)).FirstOrDefault();

            if(description == null)
            {
                throw new PokemonHttpException($"No Pokemon description found for language '{language}'.", HttpStatusCode.NotFound);
            }

            return description.FlavorText;

        }

        private async Task<string> RetrievePokemonByName(string name)
        {
            try
            {
                var result = (await _client.GetStringAsync($"{GetRoute(PokemonRoutes.SPECIES)}/{name}"));
                return result;
            }
            catch (WebException exc)
            {
                throw new PokemonHttpException(exc.Message, HttpStatusCode.RequestTimeout);
            }
            catch (TaskCanceledException)
            {
                throw new PokemonHttpException("Pokemon endpoint not available.", HttpStatusCode.RequestTimeout);
            }
            catch (Exception exc)
            {
                if(exc.Message.Contains("404 (Not Found)"))
                {
                    throw new PokemonHttpException(exc.Message, System.Net.HttpStatusCode.NotFound);
                }

                throw new PokemonHttpException(exc.Message, System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public async Task<PokemonSpecie> GetPokemonByName(string name)
        {
            string result = await RetrievePokemonByName(name);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<PokemonSpecie>(result);
        }

        public void UpdateConfiguration(PokemonProviderConfiguration config)
        {
            _configuration = config;
        }

    }

}
