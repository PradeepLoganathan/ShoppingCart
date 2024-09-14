# Use the official .NET SDK image to build and publish the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory inside the container
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ProductApi/ProductApi.csproj ./ProductApi/
RUN dotnet restore ./ProductApi/ProductApi.csproj

# Copy everything else and build
COPY ProductApi/. ./ProductApi/
WORKDIR /src/ProductApi
RUN dotnet publish -c Release -o /app/publish

# Use the official ASP.NET Core runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

# Set the working directory inside the runtime container
WORKDIR /app

# Copy the published output from the build stage
COPY --from=build /app/publish .

# Expose port 80
EXPOSE 80

# Start the application
ENTRYPOINT ["dotnet", "ProductApi.dll"]
