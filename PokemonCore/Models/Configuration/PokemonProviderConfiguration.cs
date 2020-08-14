using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace PokemonCore.Models
{
    public class PokemonProviderConfiguration
    {
        public string EndpointBaseUrl { get; set; } = "https://pokeapi.co/api/v2";
        public uint ApiVersion { get; set; } = 2;

        public override string ToString()
        {
            return $"POKEMON API ENDPOINT: {EndpointBaseUrl}\n" +
                   $"POKEMON API VERSION: {ApiVersion}";
        }
    }
}
