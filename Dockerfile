FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ./*.sln ./
COPY ./Pokemon ./Pokemon/
COPY ./PokemonCore ./PokemonCore/
COPY ./PokemonTests ./PokemonTests
RUN dotnet restore

# Copy everything else and build
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app/Pokemon
COPY --from=build-env /app/out ./
ENTRYPOINT ["dotnet", "Pokemon.dll"]