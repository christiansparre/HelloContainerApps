#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 20001
EXPOSE 30001

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["HelloContainerApps.Silo/HelloContainerApps.Silo.csproj", "HelloContainerApps.Silo/"]
RUN dotnet restore "HelloContainerApps.Silo/HelloContainerApps.Silo.csproj"
COPY . .
WORKDIR "/src/HelloContainerApps.Silo"
RUN dotnet build "HelloContainerApps.Silo.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HelloContainerApps.Silo.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HelloContainerApps.Silo.dll"]