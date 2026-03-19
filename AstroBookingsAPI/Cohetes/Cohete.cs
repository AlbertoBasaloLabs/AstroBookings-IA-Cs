using System.Text.Json.Serialization;

namespace AstroBookingsAPI.Cohetes;

public sealed record Cohete(
    [property: JsonPropertyName("id")] Guid Id,
    [property: JsonPropertyName("nombre")] string Nombre,
    [property: JsonPropertyName("alcance")] AlcanceCohete Alcance,
    [property: JsonPropertyName("capacidad")] int Capacidad);

public readonly record struct AlcanceCohete([property: JsonPropertyName("valor")] string Valor)
{
    private static readonly HashSet<string> _valoresCompatibles = new(StringComparer.OrdinalIgnoreCase)
    {
        "suborbital",
        "orbital",
        "luna",
        "marte"
    };

    public static bool IntentarCrear(string valor, out AlcanceCohete alcance)
    {
        if (_valoresCompatibles.Contains(valor))
        {
            alcance = new AlcanceCohete(valor.ToLowerInvariant());
            return true;
        }

        alcance = default;
        return false;
    }

    public static string[] ObtenerValoresCompatibles() => _valoresCompatibles.Select(static valor => valor).ToArray();
}
