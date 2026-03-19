namespace AstroBookingsAPI.Rockets;

public sealed record Rocket(Guid Id, string Name, RocketRange Range, int Capacity);

public readonly record struct RocketRange(string Value)
{
    private static readonly HashSet<string> SupportedValues = new(StringComparer.OrdinalIgnoreCase)
    {
        "suborbital",
        "orbital",
        "moon",
        "mars"
    };

    public static bool TryCreate(string value, out RocketRange range)
    {
        if (SupportedValues.Contains(value))
        {
            range = new RocketRange(value.ToLowerInvariant());
            return true;
        }

        range = default;
        return false;
    }

    public static string[] GetSupportedValues() => SupportedValues.Select(static value => value).ToArray();
}
