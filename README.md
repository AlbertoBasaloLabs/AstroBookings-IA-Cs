# AstroBookings-IA-Cs

API minimal en C# (.NET 10) para gestionar cohetes, construida con desarrollo asistido por IA.

Basado en [AstroBookings](https://github.com/AlbertoBasaloLabs/astro-bookings/blob/main/README.es.md).

---

## API de Cohetes

Endpoints disponibles:

- `POST /cohetes`
- `GET /cohetes`
- `GET /cohetes/{id}`
- `PUT /cohetes/{id}`
- `DELETE /cohetes/{id}`
- `GET /salud`

### Contrato JSON

Solicitud (`POST`/`PUT`):

- `nombre` (obligatorio)
- `alcance` (uno de: `suborbital`, `orbital`, `luna`, `marte`)
- `capacidad` (entre `1` y `10`)

Respuesta de cohete:

- `id`
- `nombre`
- `alcance` (`valor`)
- `capacidad`

Respuesta de salud:

- `estado`
- `fechaHora`

## Ejecución

```powershell
dotnet restore AstroBookingsAPI/AstroBookingsAPI.csproj
dotnet build AstroBookingsAPI/AstroBookingsAPI.csproj
dotnet run --project AstroBookingsAPI/AstroBookingsAPI.csproj
```

## Pruebas

```powershell
dotnet test AstroBookingsAPI.E2ETests/AstroBookingsAPI.E2ETests.csproj
```

- **Autor**: [Alberto Basalo](https://albertobasalo.dev)
- **AI Code Academy en Español**: [AI Code Academy](https://aicode.academy)
- **Redes**:
  - [X](https://x.com/albertobasalo)
  - [LinkedIn](https://www.linkedin.com/in/albertobasalo/)
  - [GitHub](https://github.com/albertobasalo)
