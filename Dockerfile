FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env

WORKDIR /app

COPY NuGet.Config /


COPY . ./


# Copy csproj and restore as distinct layers
#COPY /Common/*.csproj ./
COPY *.csproj ./
#RUN dotnet restore
RUN dotnet restore WPH/WPH.csproj

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "WPH.dll"]