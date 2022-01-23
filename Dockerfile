#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Src/BitcoinMonitor/BitcoinMonitor.csproj", "Src/BitcoinMonitor/"]
RUN dotnet restore "Src/BitcoinMonitor/BitcoinMonitor.csproj"
COPY . .
WORKDIR "/src/Src/BitcoinMonitor"
RUN dotnet build "BitcoinMonitor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BitcoinMonitor.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BitcoinMonitor.dll"]