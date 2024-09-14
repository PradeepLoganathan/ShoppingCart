# Use the official .NET 8 SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory inside the container
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ShoppingCartApi/ShoppingCartApi.csproj ./ShoppingCartApi/
RUN dotnet restore ./ShoppingCartApi/ShoppingCartApi.csproj

# Copy everything else and build
COPY ShoppingCartApi/. ./ShoppingCartApi/
WORKDIR /src/ShoppingCartApi
RUN dotnet publish -c Release -o /app/out

# Use the official ASP.NET Core runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

# Set the working directory inside the runtime container
WORKDIR /app

# Copy the published output from the build stage
COPY --from=build /app/out .

# Expose port 80
EXPOSE 80

# Start the application
ENTRYPOINT ["dotnet", "ShoppingCartApi.dll"]
