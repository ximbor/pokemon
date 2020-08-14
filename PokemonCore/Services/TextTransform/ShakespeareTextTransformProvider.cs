using PokemonCore.Models;
using PokemonCore.Models.Shakespeare;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace Pokemon.Services
{
    public class ShakespeareTextTransformProvider : ITextTransformProvider
    {
        private TextTransformProviderConfiguration _configuration = new TextTransformProviderConfiguration();

        private static readonly HttpClient _client = new HttpClient()
        {
            Timeout = TimeSpan.FromMilliseconds(3000)
        };

        public string SourceLanguage { get; set; } = "en";

        public async Task<string> Transform(string description)
        {
            try
            {
                var request = new ShakespeareRequest() { Text = description };
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(request));
                ByteArrayContent byteContent = new ByteArrayContent(buffer);
                HttpResponseMessage result = await _client.PostAsync(_configuration.EndpointBaseUrl, byteContent);

                if(result.StatusCode != HttpStatusCode.OK)
                {
                    throw new PokemonHttpException(result.ReasonPhrase, result.StatusCode);
                }

                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                string strResult = await result.Content.ReadAsStringAsync();
                ShakespeareResponse decodedResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<ShakespeareResponse>(strResult);
                return decodedResponse.contents.translated;
            }
            catch (WebException exc)
            {
                throw new PokemonHttpException(exc.Message, HttpStatusCode.RequestTimeout);
            }
            catch (TaskCanceledException)
            {
                throw new PokemonHttpException("Shakespeare endpoint not available.", HttpStatusCode.RequestTimeout);
            }
            catch (Exception exc)
            {
                if (exc.Message.Contains("404 (Not Found)"))
                {
                    throw new PokemonHttpException(exc.Message, System.Net.HttpStatusCode.NotFound);
                }

                if(exc.HResult== -2147467259)
                {
                    throw new PokemonHttpException(exc.Message, HttpStatusCode.RequestTimeout);
                }

                throw new PokemonHttpException(exc.Message, System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public void UpdateConfiguration(TextTransformProviderConfiguration config)
        {
            _configuration = config;
        }
    }

}
