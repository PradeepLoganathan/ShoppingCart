# Use the official .NET 7 SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory inside the container
WORKDIR /app

# Copy the .csproj file and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the application source code
COPY . ./

# Build the app in Release mode
RUN dotnet publish -c Release -o out

# Use the official .NET 7 ASP.NET Core runtime image for the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Set the working directory inside the runtime container
WORKDIR /app

# Copy the output of the build step into the runtime container
COPY --from=build /app/out .

# Expose the port on which the API will run
EXPOSE 80

# Set the entry point for the application
ENTRYPOINT ["dotnet", "ShoppingCartApi.dll"]
