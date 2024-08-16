# Base image for ASP.NET Core runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Set the environment to Production
ENV ASPNETCORE_ENVIRONMENT=Production

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy the project files and restore dependencies
COPY ["Jewelry-BE/Jewelry-BE.csproj", "Jewelry-BE/"]
COPY ["Repository/Repository.csproj", "Repository/"]
COPY ["DAOs/DAOs.csproj", "DAOs/"]
COPY ["BOs/BOs.csproj", "BOs/"]
RUN dotnet restore "./Jewelry-BE/Jewelry-BE.csproj"

# Copy all the source files and build the project
COPY . .
WORKDIR "/src/Jewelry-BE"
RUN dotnet build "./Jewelry-BE.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Jewelry-BE.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage: production image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Run the application
ENTRYPOINT ["dotnet", "Jewelry-BE.dll"]
