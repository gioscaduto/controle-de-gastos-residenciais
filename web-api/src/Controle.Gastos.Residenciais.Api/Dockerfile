FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY "*.sln" .

COPY ["Controle.Gastos.Residenciais.Api.csproj", "Controle.Gastos.Residenciais.Api/"]
COPY "lControle.Gastos.Residenciais.Business/Controle.Gastos.Residenciais.Business.csproj" "Controle.Gastos.Residenciais.Business/"
#COPY ["./src/Controle.Gastos.Residenciais.Data/Controle.Gastos.Residenciais.Data.csproj", "Controle.Gastos.Residenciais.Data/"]

#RUN dotnet restore "src/Controle.Gastos.Residenciais.Api/Controle.Gastos.Residenciais.Api.csproj"
#
#COPY . .
#WORKDIR "/src/src/Controle.Gastos.Residenciais.Api"
#RUN dotnet build "Controle.Gastos.Residenciais.Api.csproj" -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "Controle.Gastos.Residenciais.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "Controle.Gastos.Residenciais.Api.dll"]