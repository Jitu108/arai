# =============================
# 1️⃣ Build stage
# =============================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy only .csproj files first (for restore cache)
COPY src/BuildingBlocks/BuildingBlocks.Domain/BuildingBlocks.Domain.csproj src/BuildingBlocks/BuildingBlocks.Domain/
COPY src/BuildingBlocks/BuildingBlocks.Application/BuildingBlocks.Application.csproj src/BuildingBlocks/BuildingBlocks.Application/
COPY src/BuildingBlocks/BuildingBlocks.Infrastructure/BuildingBlocks.Infrastructure.csproj src/BuildingBlocks/BuildingBlocks.Infrastructure/

COPY src/Services/PatientSvc/PatientSvc.Domain/PatientSvc.Domain.csproj src/Services/PatientSvc/PatientSvc.Domain/
COPY src/Services/PatientSvc/PatientSvc.Application/PatientSvc.Application.csproj src/Services/PatientSvc/PatientSvc.Application/
COPY src/Services/PatientSvc/PatientSvc.Infrastructure/PatientSvc.Infrastructure.csproj src/Services/PatientSvc/PatientSvc.Infrastructure/
COPY src/Services/PatientSvc/PatientSvc.Api/PatientSvc.Api.csproj src/Services/PatientSvc/PatientSvc.Api/

# Restore dependencies (from API project, will also restore references)
RUN dotnet restore src/Services/PatientSvc/PatientSvc.Api/PatientSvc.Api.csproj

# Copy the rest of the source code
COPY src/ ./

# Build and publish
RUN dotnet publish src/Services/PatientSvc/PatientSvc.Api/PatientSvc.Api.csproj -c Release -o /app/publish

# =============================
# 2️⃣ Runtime stage
# =============================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy published output
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "PatientSvc.Api.dll"]
