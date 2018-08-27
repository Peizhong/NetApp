FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 49903
EXPOSE 44335

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["NetApp.Services.Identity/NetApp.Services.Identity.csproj", "NetApp.Services.Identity/"]
RUN dotnet restore "NetApp.Services.Identity/NetApp.Services.Identity.csproj"
COPY . .
WORKDIR "/src/NetApp.Services.Identity"
RUN dotnet build "NetApp.Services.Identity.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "NetApp.Services.Identity.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "NetApp.Services.Identity.dll"]