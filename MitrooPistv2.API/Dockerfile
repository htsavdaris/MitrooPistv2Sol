#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MitrooPistv2.API/MitrooPistv2.API.csproj", "MitrooPistv2.API/"]
COPY ["MitrooPistV2.Data/MitrooPistV2.Data.csproj", "MitrooPistV2.Data/"]
RUN dotnet restore "MitrooPistv2.API/MitrooPistv2.API.csproj"
COPY . .
WORKDIR "/src/MitrooPistv2.API"
RUN dotnet build "MitrooPistv2.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MitrooPistv2.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MitrooPistv2.API.dll"]