#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["TW.SpotifyNugget/TW.SpotifyNugget.csproj", "TW.SpotifyNugget/"]
RUN dotnet restore "TW.SpotifyNugget/TW.SpotifyNugget.csproj"
COPY . .
WORKDIR "/src/TW.SpotifyNugget"
RUN dotnet build "TW.SpotifyNugget.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TW.SpotifyNugget.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TW.SpotifyNugget.dll"]