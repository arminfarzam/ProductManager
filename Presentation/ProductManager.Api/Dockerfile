#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Presentation/ProductManager.Api/ProductManager.Api.csproj", "Presentation/ProductManager.Api/"]
COPY ["IoC/ProductManager.IoC/ProductManager.IoC.csproj", "IoC/ProductManager.IoC/"]
COPY ["Application/ProductManager.Application/ProductManager.Application.csproj", "Application/ProductManager.Application/"]
COPY ["Domain/ProductManager.Domain/ProductManager.Domain.csproj", "Domain/ProductManager.Domain/"]
COPY ["Data/ProductManager.Data/ProductManager.Data.csproj", "Data/ProductManager.Data/"]
RUN dotnet restore "./Presentation/ProductManager.Api/./ProductManager.Api.csproj"
COPY . .
WORKDIR "/src/Presentation/ProductManager.Api"
RUN dotnet build "./ProductManager.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ProductManager.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProductManager.Api.dll"]