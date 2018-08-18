FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["NetApp/NetApp.csproj", "NetApp/"]
RUN dotnet restore "NetApp/NetApp.csproj"
COPY . .
WORKDIR "/src/NetApp"
RUN dotnet build "NetApp.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "NetApp.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "NetApp.dll"]