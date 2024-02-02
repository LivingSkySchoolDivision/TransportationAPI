#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["WebFrontend/WebFrontend.csproj", "WebFrontend/"]
COPY ["DB/DB.csproj", "DB/"]
COPY ["Model/Model.csproj", "Model/"]
RUN dotnet restore "WebFrontend/WebFrontend.csproj"
COPY . .
WORKDIR "/src/WebFrontend"
RUN dotnet build "WebFrontend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebFrontend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebFrontend.dll"]