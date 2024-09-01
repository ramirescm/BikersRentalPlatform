FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Motorcycle.Api/Motorcycle.Api.csproj", "src/Motorcycle.Api/"]
COPY ["src/Motorcycle.Data/Motorcycle.Data.csproj", "src/Motorcycle.Data/"]
COPY ["src/Motorcycle.Core/Motorcycle.Core.csproj", "src/Motorcycle.Core/"]
COPY ["src/Motorcycle.Shared/Motorcycle.Shared.csproj", "src/Motorcycle.Shared/"]
RUN dotnet restore "src/Motorcycle.Api/Motorcycle.Api.csproj"
COPY . .
WORKDIR "/src/src/Motorcycle.Api"
RUN dotnet build "Motorcycle.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Motorcycle.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Motorcycle.Api.dll"]
