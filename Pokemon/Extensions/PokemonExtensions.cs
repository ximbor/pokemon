using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PokemonCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pokemon.Extensions
{
    public static class PokemonExtensions
    {
        public static void SetEnvironmentConfiguration(this BaseConfiguration config, IConfigurationRoot rootConfig)
        {
            // USED CONFIGURATION
            string pokemon_EnpointBaseUrl = rootConfig.GetSection("POKEMON_ENPOINT_URL").Value;
            string textTransform_EnpointBaseUrl = rootConfig.GetSection("TEXT_TRANSFORM_ENPOINT_URL").Value;

            // UNUSED CONFIGURATION:
            //string pokemon_ApiVersion = rootConfig.GetSection("POKEMON_API_VERSION").Value;
            //string textTransform_ApiVersion = rootConfig.GetSection("TEXT_TRANSFORM_API_VERSION").Value;
            //string textTransform_ApiSecretKey = rootConfig.GetSection("TEXT_TRANSFORM_API_SECRET_KEY").Value;

            if (pokemon_EnpointBaseUrl != null)
            {
                config.PokemonConfiguration.EndpointBaseUrl = pokemon_EnpointBaseUrl;
            }

            if (pokemon_EnpointBaseUrl != null)
            {
                config.TextTransformConfiguration.EndpointBaseUrl = textTransform_EnpointBaseUrl;
            }

        }
    }
}
