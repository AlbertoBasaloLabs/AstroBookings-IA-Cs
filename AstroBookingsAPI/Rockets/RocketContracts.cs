namespace AstroBookingsAPI.Rockets;

public sealed record UpsertRocketRequest(string Name, string Range, int Capacity);

public sealed record ValidationIssue(string Field, string Message);
