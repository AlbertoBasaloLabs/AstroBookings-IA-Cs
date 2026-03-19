# Instrucciones para Agentes

## Resumen del Producto
- AstroBookingsAPI expone una API HTTP para gestionar cohetes.
- Incluye CRUD de cohetes y un endpoint de salud.
- El almacenamiento actual es en memoria y está orientado a aprendizaje con IA.

## Implementación Técnica

### Stack Tecnológico
- Lenguaje: **C# sobre .NET 10 (net10.0)**
- Framework: **ASP.NET Core Minimal API 10**
- Base de datos: **Repositorio en memoria (`InMemoryRocketRepository`)**
- Seguridad: **HTTPS redirection y validación de payloads**
- Tests: **xUnit + Microsoft.AspNetCore.Mvc.Testing**
- Logging: **Logging nativo de ASP.NET Core (appsettings)**

### Flujo de desarrollo

```powershell
# Configurar el proyecto
 dotnet restore AstroBookingsAPI/AstroBookingsAPI.csproj

# Construir/Compilar el proyecto
 dotnet build AstroBookingsAPI/AstroBookingsAPI.csproj

# Ejecutar el proyecto
 dotnet run --project AstroBookingsAPI/AstroBookingsAPI.csproj

# Ejecutar tests
 dotnet test AstroBookingsAPI.E2ETests/AstroBookingsAPI.E2ETests.csproj

# Desplegar el proyecto
 dotnet publish AstroBookingsAPI/AstroBookingsAPI.csproj -c Release -o ./publish
```

### Estructura de carpetas

```text
.                                               # Raíz del proyecto
├── github/copilot-instructions.md              # Instrucciones para agentes
├── README.md                                   # Documentación principal
├── AstroBookingsAPI/                           # API minimal en ASP.NET Core
│   ├── Program.cs                              # Endpoints y configuración
│   ├── Rockets/                                # Dominio, contratos y repositorio
│   └── appsettings*.json                       # Logging y configuración
├── AstroBookingsAPI.E2ETests/                  # Tests E2E con xUnit
└── .github/prompts/                            # Prompts de soporte
```

## Entorno
- Las variables y procedimientos del código y la documentación deben estar en Español.
- Priorizar concisión sobre gramática en las respuestas.
- Entorno Windows usando terminal powershell.
- La rama por defecto es `main`.
