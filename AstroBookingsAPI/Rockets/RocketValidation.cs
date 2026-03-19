namespace AstroBookingsAPI.Rockets;

public static class RocketValidation
{
    public static Dictionary<string, string[]> Validate(UpsertRocketRequest request)
    {
        Dictionary<string, string[]> errors = new(StringComparer.OrdinalIgnoreCase);

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            errors[nameof(request.Name)] = ["Name is required."];
        }

        if (!RocketRange.TryCreate(request.Range, out _))
        {
            errors[nameof(request.Range)] = [$"Range must be one of: {string.Join(", ", RocketRange.GetSupportedValues())}."];
        }

        if (request.Capacity < 1 || request.Capacity > 10)
        {
            errors[nameof(request.Capacity)] = ["Capacity must be between 1 and 10."];
        }

        return errors;
    }
}
