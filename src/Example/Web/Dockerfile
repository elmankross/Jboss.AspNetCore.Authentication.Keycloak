FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

# Copy csproj and restore as distinct layers

COPY Example/Web/*.csproj ./Example/Web/
COPY Jboss.AspNetCore.Authentication.Keycloak/*.csproj ./Jboss.AspNetCore.Authentication.Keycloak
RUN dotnet restore Example/Web/Web.csproj

# Copy everything else and build
COPY . ./
WORKDIR Example/Web
RUN dotnet publish -c Release -r linux-x64 -o app

# final stage/image
FROM mcr.microsoft.com/dotnet/runtime:5.0
WORKDIR /app
COPY --from=build ./Example/Web/app .
ENTRYPOINT ["dotnet", "Web.dll"]