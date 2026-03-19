using AstroBookingsAPI.Rockets;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddSingleton<IRocketRepository, InMemoryRocketRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/health", () => Results.Ok(new HealthResponse("ok", DateTimeOffset.UtcNow)));

RouteGroupBuilder rockets = app.MapGroup("/rockets");

rockets.MapPost("/", (UpsertRocketRequest request, IRocketRepository repository) =>
{
    Dictionary<string, string[]> errors = RocketValidation.Validate(request);
    if (errors.Count > 0)
    {
        return Results.ValidationProblem(errors);
    }

    RocketRange.TryCreate(request.Range, out RocketRange range);
    Rocket rocket = repository.Create(request.Name.Trim(), range, request.Capacity);

    return Results.Created($"/rockets/{rocket.Id}", rocket);
});

rockets.MapGet("/", (IRocketRepository repository) => Results.Ok(repository.GetAll()));

rockets.MapGet("/{id:guid}", (Guid id, IRocketRepository repository) =>
{
    Rocket? rocket = repository.GetById(id);
    return rocket is null ? Results.NotFound() : Results.Ok(rocket);
});

rockets.MapPut("/{id:guid}", (Guid id, UpsertRocketRequest request, IRocketRepository repository) =>
{
    Dictionary<string, string[]> errors = RocketValidation.Validate(request);
    if (errors.Count > 0)
    {
        return Results.ValidationProblem(errors);
    }

    RocketRange.TryCreate(request.Range, out RocketRange range);
    Rocket? rocket = repository.Update(id, request.Name.Trim(), range, request.Capacity);

    return rocket is null ? Results.NotFound() : Results.Ok(rocket);
});

rockets.MapDelete("/{id:guid}", (Guid id, IRocketRepository repository) =>
{
    bool deleted = repository.Delete(id);
    return deleted ? Results.NoContent() : Results.NotFound();
});

app.Run();

public partial class Program;

public sealed record HealthResponse(string Status, DateTimeOffset DateTime);
