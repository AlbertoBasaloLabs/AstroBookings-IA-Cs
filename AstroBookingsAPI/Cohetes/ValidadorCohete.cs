namespace AstroBookingsAPI.Cohetes;

public static class ValidadorCohete
{
    public static Dictionary<string, string[]> Validar(SolicitudGuardarCohete solicitud)
    {
        Dictionary<string, string[]> errores = new(StringComparer.OrdinalIgnoreCase);

        if (string.IsNullOrWhiteSpace(solicitud.Nombre))
        {
            errores[nameof(solicitud.Nombre)] = ["El nombre es obligatorio."];
        }

        if (!AlcanceCohete.IntentarCrear(solicitud.Alcance, out _))
        {
            errores[nameof(solicitud.Alcance)] = [$"El alcance debe ser uno de: {string.Join(", ", AlcanceCohete.ObtenerValoresCompatibles())}."];
        }

        if (solicitud.Capacidad < 1 || solicitud.Capacidad > 10)
        {
            errores[nameof(solicitud.Capacidad)] = ["La capacidad debe estar entre 1 y 10."];
        }

        return errores;
    }
}
