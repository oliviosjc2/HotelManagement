FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Ajuste para copiar os arquivos .csproj corretamente com caminhos relativos ao diretório HM.API
COPY ../HM.Application/HM.Application.csproj HM.Application/
COPY ../HM.Domain/HM.Domain.csproj HM.Domain/
COPY ../HM.Infra/HM.Infra.csproj HM.Infra/
COPY HM.API/HM.API.csproj HM.API/

RUN dotnet restore "HM.API/HM.API.csproj"

# Copiar todos os arquivos restantes
COPY . .

WORKDIR "/src/HM.API"
RUN dotnet build "HM.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "HM.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HM.API.dll"]
