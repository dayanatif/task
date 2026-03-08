# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore
COPY ["TSManager.csproj", "./"]
RUN dotnet restore "TSManager.csproj"

# Copy source and build
COPY . .
RUN dotnet build "TSManager.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "TSManager.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 80

# Copy published app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "TSManager.dll"]
