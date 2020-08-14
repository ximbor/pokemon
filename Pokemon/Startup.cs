using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Pokemon.Extensions;
using Pokemon.Services;
using PokemonCore.Models;

namespace Pokemon
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddOptions();
            var appConfiguration = new ConfigurationBuilder()
              .AddEnvironmentVariables()
              .Build();
            var pokemonConfig = new BaseConfiguration();
            var shakespeareTransformProvider = new ShakespeareTextTransformProvider();
            var pokemonProvider = new PokemonProvider();
            pokemonConfig.SetEnvironmentConfiguration(appConfiguration);

            services.AddSingleton(pokemonConfig);
            services.AddSingleton<IPokemonProvider>(pokemonProvider);
            services.AddSingleton<ITextTransformProvider>(shakespeareTransformProvider);

            services.AddLogging(
                builder =>
                {
                    builder
                        .AddFilter("Microsoft", LogLevel.Information)
                        .AddFilter("System", LogLevel.Information)
                        .AddConsole();
                });

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
