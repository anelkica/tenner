FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 3333

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["Tenner.API/Tenner.API.csproj", "Tenner.API/"]
RUN dotnet restore "Tenner.API/Tenner.API.csproj"
COPY . .
WORKDIR "/src/Tenner.API"
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Tenner.API.dll"]