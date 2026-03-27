# Etapa 1: Construcción
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar el archivo de proyecto para restaurar dependencias
# Se asume que el contexto de construcción es el directorio 'WorkTraceWebApp'
COPY ["WorkTrace.WebApp/WorkTrace.WebApp.csproj", "WorkTrace.WebApp/"]

# Restaurar dependencias (NuGet)
RUN dotnet restore "WorkTrace.WebApp/WorkTrace.WebApp.csproj"

# Copiar el resto del código fuente del proyecto
COPY . .

# Construir y publicar la aplicación en modo Release
WORKDIR "/src/WorkTrace.WebApp"
RUN dotnet publish "WorkTrace.WebApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa 2: Ejecución (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copiar los artefactos publicados desde la etapa de construcción
COPY --from=build /app/publish .

# Configurar el puerto para Render (8080)
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Definir el punto de entrada de la aplicación
ENTRYPOINT ["dotnet", "WorkTrace.WebApp.dll"]