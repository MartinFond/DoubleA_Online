# Use the official .NET Core SDK image as the build image
FROM mcr.microsoft.com/dotnet/sdk:8.0.101 AS build

# Set the working directory in the container
WORKDIR /app

# Copy the project file and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the remaining source code
COPY . .

# Build the application
RUN dotnet publish -c Release -o out

#Build PostgreSQL
FROM postgres:latest AS build-postgres
COPY SQL_init/init.sql /docker-entrypoint-initdb.d/

# Use the official .NET Core runtime image as the base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0.1 AS runtime

# Set the working directory in the container
WORKDIR /app

# Copy the published app from the build image
COPY --from=build /app/out .

# Expose port 8080 (assuming your API listens on port 8080)
EXPOSE 8080
EXPOSE 5432

# Command to run the application
CMD ["dotnet", "API.dll"]

# Copy initialization script into the PostgreSQL image
COPY --from=build-postgres /docker-entrypoint-initdb.d/init.sql /docker-entrypoint-initdb.d/

