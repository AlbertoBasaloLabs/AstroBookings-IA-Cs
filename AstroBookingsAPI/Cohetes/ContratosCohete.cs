using System.Text.Json.Serialization;

namespace AstroBookingsAPI.Cohetes;

public sealed record SolicitudGuardarCohete(
    [property: JsonPropertyName("nombre")] string Nombre,
    [property: JsonPropertyName("alcance")] string Alcance,
    [property: JsonPropertyName("capacidad")] int Capacidad);

public sealed record ErrorValidacion(string Campo, string Mensaje);
