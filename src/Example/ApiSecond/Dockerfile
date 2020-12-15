FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

# Copy csproj and restore as distinct layers

COPY Example/ApiSecond/*.csproj ./Example/ApiSecond/
COPY Microsoft.AspNetCore.Authentication.Keycloak/*.csproj ./Microsoft.AspNetCore.Authentication.Keycloak
RUN dotnet restore Example/ApiSecond/ApiSecond.csproj

# Copy everything else and build
COPY . ./
WORKDIR Example/ApiSecond
RUN dotnet publish -c Release -r linux-x64 -o app

# final stage/image
FROM mcr.microsoft.com/dotnet/runtime:5.0
WORKDIR /app
COPY --from=build ./Example/ApiSecond/app .
ENTRYPOINT ["dotnet", "ApiSecond.dll"]