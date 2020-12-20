FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

# Copy csproj and restore as distinct layers

COPY Example/ApiFirst/*.csproj ./Example/ApiFirst/
COPY Jboss.AspNetCore.Authentication.Keycloak/*.csproj ./Jboss.AspNetCore.Authentication.Keycloak
RUN dotnet restore Example/ApiFirst/ApiFirst.csproj

# Copy everything else and build
COPY . ./
WORKDIR Example/ApiFirst
RUN dotnet publish -c Release -r linux-x64 -o app

# final stage/image
FROM mcr.microsoft.com/dotnet/runtime:5.0
WORKDIR /app
COPY --from=build ./Example/ApiFirst/app .
ENTRYPOINT ["dotnet", "ApiFirst.dll"]