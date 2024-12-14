# Stage 1: Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory inside the container
WORKDIR /src

# Copy the csproj file and restore dependencies
COPY ProductApi/ProductApi.csproj ProductApi/
RUN dotnet restore ProductApi/ProductApi.csproj

# Copy the remaining source code and build
COPY ProductApi/. ProductApi/
WORKDIR /src/ProductApi
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

# Set the working directory inside the runtime container
WORKDIR /app

# Copy the published output from the build stage to the runtime stage
COPY --from=build /app/publish .

# Optional: Set ASPNETCORE_URLS so the app listens on port 80
ENV ASPNETCORE_URLS=http://+:80

# Expose port 80 to outside the container
EXPOSE 80

# Set the entry point to run the application
ENTRYPOINT ["dotnet", "ProductApi.dll"]
