using Pokemon.Models;
using PokemonCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pokemon.Services
{
    public interface IPokemonProvider
    {
        Task<PokemonSpecie> GetPokemonByName(string name);

        Task<string> GetPokemonDescription(string name, string language = "en");

        void UpdateConfiguration(PokemonProviderConfiguration value);
    }
}
