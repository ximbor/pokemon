using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Pokemon.Services;
using PokemonCore.Models;
using PokemonCore.Models.Pokemon;

namespace Pokemon.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        private readonly ILogger<PokemonController> _logger;
        private readonly IPokemonProvider _pokemonProvider;
        private readonly ITextTransformProvider _textTransformProvider;

        public PokemonController(
            ILogger<PokemonController> logger,
            IOptions<BaseConfiguration> config,
            IPokemonProvider pokemonProvider,
            ITextTransformProvider textTransformProvider
        ) {
            // Assign injected servicers:
            _logger = logger;            
            _pokemonProvider = pokemonProvider;
            _textTransformProvider = textTransformProvider;

            // Update services' configuration:
            _pokemonProvider.UpdateConfiguration(config.Value.PokemonConfiguration);
            _textTransformProvider.UpdateConfiguration(config.Value.TextTransformConfiguration);

            _logger.LogInformation(config.Value.ToString());
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> Get([FromRoute] string name = null)
        {
            // If the name is empty:
            if (string.IsNullOrEmpty(name))
            {
                return new ContentResult() {
                    Content = "No Pokemon name has been specified", 
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }

            try
            {
                // Check if the user specified the "Accept-Language" header,
                // in this case the client's preferred language will be used:

                StringValues language;

                if (Request != null)
                {
                    Request.Headers.TryGetValue("Accept-Language", out language);
                }

                if (language == default(StringValues))
                {
                    language = new StringValues(_textTransformProvider.SourceLanguage);
                } else
                {
                    language = GetLanguage(language);
                }

                // Get Pokemon's description:
                string description = await _pokemonProvider.GetPokemonDescription(name.ToLowerInvariant(), language);

                // Translate Pokemon's description:
                string transformedDescription = (
                    language == _textTransformProvider.SourceLanguage ? (await _textTransformProvider.Transform(description)) : description
                );

                // Return Pokemon's translated description:
                return Ok(new PokemonResponseDTO()
                {
                    Name = name,
                    Description = transformedDescription
                });

            }
            catch (PokemonHttpException exc)
            {
                return new ContentResult() { Content = exc.Message, StatusCode = (int)exc.StatusCode };
            }
            catch (Exception exc)
            {
                return new ContentResult() { Content = exc.Message, StatusCode = (int)HttpStatusCode.InternalServerError };
            }
        }

        private string GetLanguage(string language)
        {
            // Parse languages string:
            var selectedLanguage = language.Split(',')
                .Select(StringWithQualityHeaderValue.Parse)
                .OrderByDescending(s => s.Quality.GetValueOrDefault(1)).FirstOrDefault().Value;

            // Removed region specific strings:
            if (selectedLanguage.Contains('-'))
            {
                selectedLanguage = selectedLanguage.Substring(0, selectedLanguage.IndexOf('-'));
            }
            
            return selectedLanguage;
        }

    }
}