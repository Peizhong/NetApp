
FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 5000

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY NetApp.csproj NetApp/
RUN dotnet restore NetApp/NetApp.csproj
WORKDIR /src/NetApp
COPY . .
RUN dotnet build NetApp.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish NetApp.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "NetApp.dll"]
