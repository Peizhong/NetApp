FROM microsoft/dotnet:2.1.1-aspnetcore-runtime AS base

# 作者信息 
MAINTAINER wangpeizhong

#RUN apt-get update
#RUN apt-get upgrade

WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1.302-sdk AS build
WORKDIR /src
COPY NetApp.Service/NetApp.Service.csproj NetApp.Service/
COPY NetApp.Entities/NetApp.Entities.csproj NetApp.Entities/
RUN dotnet restore NetApp.Service/NetApp.Service.csproj
COPY . .
WORKDIR /src/NetApp.Service
RUN dotnet build NetApp.Service.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish NetApp.Service.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "NetApp.Service.dll"]
