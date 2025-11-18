FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Adocao.Web/Adocao.Web.csproj", "Adocao.Web/"]
COPY ["Adocao.Application/Adocao.Application.csproj", "Adocao.Application/"]
COPY ["Adocao.Core/Adocao.Core.csproj", "Adocao.Core/"]
COPY ["Adocao.Domain/Adocao.Domain.csproj", "Adocao.Domain/"]
COPY ["Adocao.Infra/Adocao.Infra.csproj", "Adocao.Infra/"]

RUN dotnet restore "Adocao.Web/Adocao.Web.csproj"

COPY . .
WORKDIR "/src/Adocao.Web"
RUN dotnet build "Adocao.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Adocao.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Adocao.Web.dll"]