# STAGE 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj and restore as distinct layers for faster caching
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build the release
COPY . ./
RUN dotnet publish -c Release -o out

# STAGE 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy only the compiled output from the build stage
COPY --from=build /app/out .

# ASP.NET Core default port in .NET 8 containers is 8080
EXPOSE 8080

# Start the application
ENTRYPOINT ["dotnet", "SecureAuthApi.dll"]