FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/EtudeReussie.Api/EtudeReussie.Api.csproj", "src/EtudeReussie.Api/"]
RUN dotnet restore "src/EtudeReussie.Api/EtudeReussie.Api.csproj"

COPY . .
WORKDIR "/src/src/EtudeReussie.Api"
RUN dotnet publish "EtudeReussie.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "EtudeReussie.Api.dll"]