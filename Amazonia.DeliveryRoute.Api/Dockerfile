#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

# --------------------------------------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Install clang/zlib1g-dev dependencies for publishing to native
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
    clang zlib1g-dev

ARG BUILD_CONFIGURATION=Release

WORKDIR /src
COPY ["Amazonia.DeliveryRoute.Api/Amazonia.DeliveryRoute.Api.csproj", "Amazonia.DeliveryRoute.Api/"]
RUN dotnet restore "./Amazonia.DeliveryRoute.Api/./Amazonia.DeliveryRoute.Api.csproj"

COPY . .
WORKDIR "/src/Amazonia.DeliveryRoute.Api"
RUN dotnet build "./Amazonia.DeliveryRoute.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# --------------------------------------------------------
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Amazonia.DeliveryRoute.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=true

# --------------------------------------------------------
FROM mcr.microsoft.com/dotnet/runtime-deps:8.0 AS final
WORKDIR /app
EXPOSE 8080

COPY --from=publish /app/publish .
ENTRYPOINT ["./Amazonia.DeliveryRoute.Api"]