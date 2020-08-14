using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Pokemon.Controllers;
using Pokemon.Services;
using PokemonCore.Models;
using Microsoft.Extensions.Options;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using PokemonCore.Models.Pokemon;

namespace PokemonTests
{
    public class PokemonControllerTests
    {

        private static ILogger<PokemonController> GetMockLogger()
        {
            return Mock.Of<ILogger<PokemonController>>();
        }

        [Theory]
        [InlineData("charizard")]
        [InlineData("ditto")]
        public async void Should_return_a_description_and_the_correct_name(string name)
        {
            var logger = PokemonControllerTests.GetMockLogger();
            var config = Options.Create(new BaseConfiguration());
            var transformProvider = new ShakespeareTextTransformProvider();
            var pokemonProvider = new PokemonProvider();
            var pokemonController = new PokemonController(logger, config, pokemonProvider, transformProvider);
            var response = (await pokemonController.Get(name)) as OkObjectResult;
            
            Assert.NotNull(response);
            Assert.Equal(response.StatusCode, (int)HttpStatusCode.OK);

            PokemonResponseDTO content = response.Value as PokemonResponseDTO;

            Assert.Equal(name, content.Name);    
        }

        [Theory]
        [InlineData(null)]
        public async void Should_return_BAD_REQUEST_for_null_name(string name)
        {
            var logger = PokemonControllerTests.GetMockLogger();
            var config = Options.Create(new BaseConfiguration());
            var transformProvider = new ShakespeareTextTransformProvider();
            var pokemonProvider = new PokemonProvider();
            var pokemonController = new PokemonController(logger, config, pokemonProvider, transformProvider);

            ContentResult response = (await pokemonController.Get(name)) as ContentResult;
            Assert.NotNull(response);
            Assert.Equal(response.StatusCode, (int)HttpStatusCode.BadRequest);

        }

        [Theory]
        [InlineData("")]
        public async void Should_return_BAD_RQUEST_for_empty_name(string name)
        {
            var logger = PokemonControllerTests.GetMockLogger();
            var config = Options.Create(new BaseConfiguration());
            var transformProvider = new ShakespeareTextTransformProvider();
            var pokemonProvider = new PokemonProvider();
            var pokemonController = new PokemonController(logger, config, pokemonProvider, transformProvider);

            ContentResult response = (await pokemonController.Get(name)) as ContentResult;
            Assert.NotNull(response);
            Assert.Equal(response.StatusCode, (int)HttpStatusCode.BadRequest);

        }

        [Theory]
        [InlineData("customInvalidName")]
        public async void Should_return_NOT_FOUND_for_invalid_name(string name)
        {
            var logger = PokemonControllerTests.GetMockLogger();
            var config = Options.Create(new BaseConfiguration());
            var transformProvider = new ShakespeareTextTransformProvider();
            var pokemonProvider = new PokemonProvider();
            var pokemonController = new PokemonController(logger, config, pokemonProvider, transformProvider);

            ContentResult response = (await pokemonController.Get(name)) as ContentResult;
            Assert.NotNull(response);
            Assert.Equal(response.StatusCode, (int)HttpStatusCode.NotFound);
        }

        [Theory]
        [InlineData("¡ ¢®")]
        [InlineData("اسم غير صالح")]
        [InlineData("неправильное имя")]
        [InlineData("无效的名字")]
        public async void Should_return_NOT_FOUND_for_non_ASCII_invalid_name(string name)
        {
            var logger = PokemonControllerTests.GetMockLogger();
            var config = Options.Create(new BaseConfiguration());
            var transformProvider = new ShakespeareTextTransformProvider();
            var pokemonProvider = new PokemonProvider();
            var pokemonController = new PokemonController(logger, config, pokemonProvider, transformProvider);

            ContentResult response = (await pokemonController.Get(name)) as ContentResult;
            Assert.NotNull(response);
            Assert.Equal(response.StatusCode, (int)HttpStatusCode.NotFound);

        }

        [Theory]
        [InlineData("charizard", "http://localhost/fake-pokemon-endpoint")]
        public async void Should_handle_Pokemon_endpoint_not_available(string name, string pokemonEndpoint)
        {
            var logger = PokemonControllerTests.GetMockLogger();
            var config = Options.Create(new BaseConfiguration());
            config.Value.PokemonConfiguration.EndpointBaseUrl = pokemonEndpoint;
            var transformProvider = new ShakespeareTextTransformProvider();
            var pokemonProvider = new PokemonProvider();
            var pokemonController = new PokemonController(logger, config, pokemonProvider, transformProvider);

            ContentResult response = (await pokemonController.Get(name)) as ContentResult;
            Assert.NotNull(response);
            Assert.Equal(response.StatusCode, (int)HttpStatusCode.RequestTimeout);
        }

        [Theory]
        [InlineData("charizard", "http://localhost/fake-shakespeare-endpoint")]
        public async void Should_handle_Shakespeare_endpoint_not_available(string name, string shakespeareEndpoint)
        {
            var logger = PokemonControllerTests.GetMockLogger();
            var config = Options.Create(new BaseConfiguration());
            config.Value.TextTransformConfiguration.EndpointBaseUrl = shakespeareEndpoint;
            var transformProvider = new ShakespeareTextTransformProvider();
            var pokemonProvider = new PokemonProvider();
            var pokemonController = new PokemonController(logger, config, pokemonProvider, transformProvider);

            ContentResult response = (await pokemonController.Get(name)) as ContentResult;
            Assert.NotNull(response);
            Assert.Equal(response.StatusCode, (int)HttpStatusCode.RequestTimeout);
        }
    
    }
}
