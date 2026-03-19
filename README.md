# AstroBookings-IA-Cs

C# version of AstroBookings API developed with AI-Drive Development

Based on [AstroBookings brieffing](https://github.com/AlbertoBasaloLabs/astro-bookings)

---

## Rockets API

The `feat/rockets` release adds CRUD endpoints for rocket management:

- `POST /rockets`
- `GET /rockets`
- `GET /rockets/{id}`
- `PUT /rockets/{id}`
- `DELETE /rockets/{id}`

Payload validation rules:

- `name`: required
- `range`: one of `suborbital`, `orbital`, `moon`, `mars`
- `capacity`: from `1` to `10`

## Run tests

```bash
dotnet test AstroBookingsAPI.E2ETests/AstroBookingsAPI.E2ETests.csproj
```

- **Author**: [Alberto Basalo](https://albertobasalo.dev)
- **Ai Code Academy en Español**: [AI code Academy](https://aicode.academy)
- **Socials**:
  - [X](https://x.com/albertobasalo)
  - [LinkedIn](https://www.linkedin.com/in/albertobasalo/)
  - [GitHub](https://github.com/albertobasalo)
