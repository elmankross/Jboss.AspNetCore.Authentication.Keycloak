FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build

# Copy csproj and restore as distinct layers

COPY Example/ApiSecond/*.csproj ./Example/ApiSecond/
COPY Jboss.AspNetCore.Authentication.Keycloak/*.csproj ./Jboss.AspNetCore.Authentication.Keycloak
RUN dotnet restore Example/ApiSecond/ApiSecond.csproj

# Copy everything else and build
COPY . ./
WORKDIR Example/ApiSecond
RUN dotnet publish -c Release -r linux-x64 -o app

# final stage/image
FROM mcr.microsoft.com/dotnet/runtime:3.1
WORKDIR /app
COPY --from=build ./Example/ApiSecond/app .
ENTRYPOINT ["dotnet", "ApiSecond.dll"]