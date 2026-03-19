using System.Text.Json.Serialization;
using AstroBookingsAPI.Cohetes;

var constructorAplicacion = WebApplication.CreateBuilder(args);

// Add services to the container.
constructorAplicacion.Services.AddOpenApi();
constructorAplicacion.Services.AddSingleton<IRepositorioCohetes, RepositorioCohetesEnMemoria>();

var aplicacion = constructorAplicacion.Build();

// Configure the HTTP request pipeline.
if (aplicacion.Environment.IsDevelopment())
{
    aplicacion.MapOpenApi();
}

aplicacion.UseHttpsRedirection();

aplicacion.MapGet("/salud", () => Results.Ok(new RespuestaSalud("ok", DateTimeOffset.UtcNow)));

RouteGroupBuilder cohetes = aplicacion.MapGroup("/cohetes");

cohetes.MapPost("/", (SolicitudGuardarCohete solicitud, IRepositorioCohetes repositorio) =>
{
    Dictionary<string, string[]> errores = ValidadorCohete.Validar(solicitud);
    if (errores.Count > 0)
    {
        return Results.ValidationProblem(errores);
    }

    AlcanceCohete.IntentarCrear(solicitud.Alcance, out AlcanceCohete alcance);
    Cohete cohete = repositorio.Crear(solicitud.Nombre.Trim(), alcance, solicitud.Capacidad);

    return Results.Created($"/cohetes/{cohete.Id}", cohete);
});

cohetes.MapGet("/", (IRepositorioCohetes repositorio) => Results.Ok(repositorio.ObtenerTodos()));

cohetes.MapGet("/{id:guid}", (Guid id, IRepositorioCohetes repositorio) =>
{
    Cohete? cohete = repositorio.ObtenerPorId(id);
    return cohete is null ? Results.NotFound() : Results.Ok(cohete);
});

cohetes.MapPut("/{id:guid}", (Guid id, SolicitudGuardarCohete solicitud, IRepositorioCohetes repositorio) =>
{
    Dictionary<string, string[]> errores = ValidadorCohete.Validar(solicitud);
    if (errores.Count > 0)
    {
        return Results.ValidationProblem(errores);
    }

    AlcanceCohete.IntentarCrear(solicitud.Alcance, out AlcanceCohete alcance);
    Cohete? cohete = repositorio.Actualizar(id, solicitud.Nombre.Trim(), alcance, solicitud.Capacidad);

    return cohete is null ? Results.NotFound() : Results.Ok(cohete);
});

cohetes.MapDelete("/{id:guid}", (Guid id, IRepositorioCohetes repositorio) =>
{
    bool eliminado = repositorio.Eliminar(id);
    return eliminado ? Results.NoContent() : Results.NotFound();
});

aplicacion.Run();

public partial class Program;

public sealed record RespuestaSalud(
    [property: JsonPropertyName("estado")] string Estado,
    [property: JsonPropertyName("fechaHora")] DateTimeOffset FechaHora);
