FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
#This image sets the ASPNETCORE_URLS environment variable to http://+:80
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["NetApp.Services.Browse/NetApp.Services.Browse.csproj", "NetApp.Services.Browse/"]
COPY ["NetApp.Repository/NetApp.Repository.csproj", "NetApp.Repository/"]
COPY ["NetApp.Models/NetApp.Models.csproj", "NetApp.Models/"]
COPY ["NetApp.Repository.Interfaces/NetApp.Repository.Interfaces.csproj", "NetApp.Repository.Interfaces/"]
COPY ["NetApp.Services.Lib/NetApp.Services.Lib.csproj", "NetApp.Services.Lib/"]
RUN dotnet restore "NetApp.Services.Browse/NetApp.Services.Browse.csproj"
COPY . .
WORKDIR "/src/NetApp.Services.Browse"
RUN dotnet build "NetApp.Services.Browse.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "NetApp.Services.Browse.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "NetApp.Services.Browse.dll"]