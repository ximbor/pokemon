using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonCore.Models.Shakespeare
{
    public class ShakespeareRequest
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
