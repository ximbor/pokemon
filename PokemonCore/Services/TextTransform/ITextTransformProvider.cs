using PokemonCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pokemon.Services
{
    public interface ITextTransformProvider
    {
        void UpdateConfiguration(TextTransformProviderConfiguration config);
        Task<string> Transform(string description);
        string SourceLanguage { get; set; }
    }
}
