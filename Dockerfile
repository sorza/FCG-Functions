FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["FCG-Functions.csproj", "FCG-Functions/"]
RUN dotnet restore "FCG-Functions/FCG-Functions.csproj"

COPY . FCG-Functions/
WORKDIR /src/FCG-Functions
RUN dotnet publish "FCG-Functions.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/azure-functions/dotnet-isolated:4-dotnet-isolated8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
