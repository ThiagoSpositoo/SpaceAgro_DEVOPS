# =========================================================
# Dockerfile - SpaceAgro .NET API
# Global Solution 2026/1 - DevOps Tools & Cloud Computing
# =========================================================

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY SpaceAgro.DotNetApi.csproj ./
RUN dotnet restore SpaceAgro.DotNetApi.csproj

COPY . ./
RUN dotnet publish SpaceAgro.DotNetApi.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

RUN addgroup --system appgroup && adduser --system --ingroup appgroup appuser

COPY --from=build /app/publish ./

ENV ASPNETCORE_ENVIRONMENT=Docker
ENV ASPNETCORE_URLS=http://+:8080
ENV DB_PROVIDER=PostgreSQL
ENV AUTO_CREATE_DATABASE=true

EXPOSE 8080

USER appuser

ENTRYPOINT ["dotnet", "SpaceAgro.DotNetApi.dll"]
