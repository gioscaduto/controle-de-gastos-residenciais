FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["Controle.Gastos.Residenciais.sln", "./"]
COPY ["src/Controle.Gastos.Residenciais.Api/Controle.Gastos.Residenciais.Api.csproj", "src/Controle.Gastos.Residenciais.Api/"]
COPY ["src/Controle.Gastos.Residenciais.Business/Controle.Gastos.Residenciais.Business.csproj", "src/Controle.Gastos.Residenciais.Business/"]
COPY ["src/Controle.Gastos.Residenciais.Data/Controle.Gastos.Residenciais.Data.csproj", "src/Controle.Gastos.Residenciais.Data/"]

RUN dotnet restore "Controle.Gastos.Residenciais.sln"

COPY . .

WORKDIR "/src/src/Controle.Gastos.Residenciais.Api"
RUN dotnet build "Controle.Gastos.Residenciais.Api.csproj" -c release -o /app/build

FROM build AS publish
RUN dotnet publish "Controle.Gastos.Residenciais.Api.csproj" -c release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

RUN mkdir -p /app/data

COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Development

EXPOSE 8080
EXPOSE 443

ENTRYPOINT ["dotnet", "Controle.Gastos.Residenciais.Api.dll"]