using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pokemon.Models
{
    public class PokemonSpecie
    {

        [JsonProperty("flavor_text_entries")]
        public List<FlavorTextEntry> Entries { get; set; } = new List<FlavorTextEntry>();

    }


    public class FlavorTextEntry
    {
        [JsonProperty("flavor_text")]
        public string FlavorText { get; set; }

        [JsonProperty("language")]
        public ResourceEntry Language { get; set; }

        [JsonProperty("version")]
        public ResourceEntry Version { get; set; }

    }

    public class ResourceEntry
    {
        public string Name { get; set; }

        public string Url { get; set; }
    }

}
