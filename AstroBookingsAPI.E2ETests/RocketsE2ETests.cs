using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace AstroBookingsAPI.E2ETests;

public sealed class RocketsE2ETests
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    [Fact]
    public async Task Create_WithValidPayload_ReturnsCreatedRocket()
    {
        await using WebApplicationFactory<Program> factory = new();
        using HttpClient client = CreateClient(factory);

        UpsertRocketRequest request = new("Starliner", "orbital", 4);

        HttpResponseMessage response = await client.PostAsJsonAsync("/rockets", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        RocketResponse? rocket = await response.Content.ReadFromJsonAsync<RocketResponse>(JsonOptions);
        Assert.NotNull(rocket);
        Assert.Equal("Starliner", rocket.Name);
        Assert.Equal("orbital", ExtractRange(rocket.Range));
        Assert.Equal(4, rocket.Capacity);
    }

    [Fact]
    public async Task Create_WithInvalidRange_ReturnsValidationError()
    {
        await using WebApplicationFactory<Program> factory = new();
        using HttpClient client = CreateClient(factory);

        HttpResponseMessage response = await client.PostAsJsonAsync("/rockets", new UpsertRocketRequest("Ranger", "deep-space", 3));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        await AssertValidationError(response, "Range");
    }

    [Fact]
    public async Task Create_WithInvalidCapacity_ReturnsValidationError()
    {
        await using WebApplicationFactory<Program> factory = new();
        using HttpClient client = CreateClient(factory);

        HttpResponseMessage response = await client.PostAsJsonAsync("/rockets", new UpsertRocketRequest("Ranger", "moon", 11));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        await AssertValidationError(response, "Capacity");
    }

    [Fact]
    public async Task Create_WithoutName_ReturnsValidationError()
    {
        await using WebApplicationFactory<Program> factory = new();
        using HttpClient client = CreateClient(factory);

        HttpResponseMessage response = await client.PostAsJsonAsync("/rockets", new UpsertRocketRequest(" ", "moon", 2));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        await AssertValidationError(response, "Name");
    }

    [Fact]
    public async Task GetAll_ReturnsStoredRockets()
    {
        await using WebApplicationFactory<Program> factory = new();
        using HttpClient client = CreateClient(factory);

        await CreateRocket(client, new UpsertRocketRequest("Apollo", "moon", 3));
        await CreateRocket(client, new UpsertRocketRequest("Pioneer", "mars", 5));

        HttpResponseMessage response = await client.GetAsync("/rockets");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        IReadOnlyCollection<RocketResponse>? rockets = await response.Content.ReadFromJsonAsync<IReadOnlyCollection<RocketResponse>>(JsonOptions);
        Assert.NotNull(rockets);
        Assert.Equal(2, rockets.Count);
        Assert.Contains(rockets, rocket => rocket.Name == "Apollo" && ExtractRange(rocket.Range) == "moon" && rocket.Capacity == 3);
        Assert.Contains(rockets, rocket => rocket.Name == "Pioneer" && ExtractRange(rocket.Range) == "mars" && rocket.Capacity == 5);
    }

    [Fact]
    public async Task GetById_WhenRocketExists_ReturnsRocket()
    {
        await using WebApplicationFactory<Program> factory = new();
        using HttpClient client = CreateClient(factory);

        RocketResponse created = await CreateRocket(client, new UpsertRocketRequest("Falcon", "orbital", 6));

        HttpResponseMessage response = await client.GetAsync($"/rockets/{created.Id}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        RocketResponse? rocket = await response.Content.ReadFromJsonAsync<RocketResponse>(JsonOptions);
        Assert.NotNull(rocket);
        Assert.Equal(created.Id, rocket.Id);
        Assert.Equal("Falcon", rocket.Name);
    }

    [Fact]
    public async Task GetById_WhenRocketMissing_ReturnsNotFound()
    {
        await using WebApplicationFactory<Program> factory = new();
        using HttpClient client = CreateClient(factory);

        HttpResponseMessage response = await client.GetAsync($"/rockets/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Update_WithValidPayload_ReturnsUpdatedRocket()
    {
        await using WebApplicationFactory<Program> factory = new();
        using HttpClient client = CreateClient(factory);

        RocketResponse created = await CreateRocket(client, new UpsertRocketRequest("Pathfinder", "suborbital", 2));

        HttpResponseMessage response = await client.PutAsJsonAsync($"/rockets/{created.Id}", new UpsertRocketRequest("Pathfinder X", "mars", 7));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        RocketResponse? updated = await response.Content.ReadFromJsonAsync<RocketResponse>(JsonOptions);
        Assert.NotNull(updated);
        Assert.Equal(created.Id, updated.Id);
        Assert.Equal("Pathfinder X", updated.Name);
        Assert.Equal("mars", ExtractRange(updated.Range));
        Assert.Equal(7, updated.Capacity);
    }

    [Fact]
    public async Task Update_WithInvalidRange_ReturnsValidationError()
    {
        await using WebApplicationFactory<Program> factory = new();
        using HttpClient client = CreateClient(factory);

        RocketResponse created = await CreateRocket(client, new UpsertRocketRequest("Nova", "orbital", 3));

        HttpResponseMessage response = await client.PutAsJsonAsync($"/rockets/{created.Id}", new UpsertRocketRequest("Nova", "jupiter", 3));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        await AssertValidationError(response, "Range");
    }

    [Fact]
    public async Task Update_WithInvalidCapacity_ReturnsValidationError()
    {
        await using WebApplicationFactory<Program> factory = new();
        using HttpClient client = CreateClient(factory);

        RocketResponse created = await CreateRocket(client, new UpsertRocketRequest("Nova", "orbital", 3));

        HttpResponseMessage response = await client.PutAsJsonAsync($"/rockets/{created.Id}", new UpsertRocketRequest("Nova", "moon", 0));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        await AssertValidationError(response, "Capacity");
    }

    [Fact]
    public async Task Update_WithoutName_ReturnsValidationError()
    {
        await using WebApplicationFactory<Program> factory = new();
        using HttpClient client = CreateClient(factory);

        RocketResponse created = await CreateRocket(client, new UpsertRocketRequest("Nova", "orbital", 3));

        HttpResponseMessage response = await client.PutAsJsonAsync($"/rockets/{created.Id}", new UpsertRocketRequest("", "moon", 2));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        await AssertValidationError(response, "Name");
    }

    [Fact]
    public async Task Delete_RemovesRocket_AndFutureGetReturnsNotFound()
    {
        await using WebApplicationFactory<Program> factory = new();
        using HttpClient client = CreateClient(factory);

        RocketResponse created = await CreateRocket(client, new UpsertRocketRequest("Voyager", "mars", 8));

        HttpResponseMessage deleteResponse = await client.DeleteAsync($"/rockets/{created.Id}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        HttpResponseMessage getResponse = await client.GetAsync($"/rockets/{created.Id}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    private static HttpClient CreateClient(WebApplicationFactory<Program> factory)
    {
        return factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost")
        });
    }

    private static async Task<RocketResponse> CreateRocket(HttpClient client, UpsertRocketRequest request)
    {
        HttpResponseMessage response = await client.PostAsJsonAsync("/rockets", request);
        response.EnsureSuccessStatusCode();

        RocketResponse? rocket = await response.Content.ReadFromJsonAsync<RocketResponse>(JsonOptions);
        return rocket ?? throw new InvalidOperationException("Rocket payload was empty.");
    }

    private static async Task AssertValidationError(HttpResponseMessage response, string expectedField)
    {
        using JsonDocument payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        JsonElement errors = payload.RootElement.GetProperty("errors");

        bool found = errors.EnumerateObject().Any(error => string.Equals(error.Name, expectedField, StringComparison.OrdinalIgnoreCase));
        Assert.True(found, $"Expected validation error for field '{expectedField}'.");
    }

    private static string ExtractRange(JsonElement range)
    {
        return range.ValueKind switch
        {
            JsonValueKind.String => range.GetString()!,
            JsonValueKind.Object => range.GetProperty("value").GetString()!,
            _ => throw new InvalidOperationException("Unexpected range format.")
        };
    }

    private sealed record UpsertRocketRequest(string Name, string Range, int Capacity);

    private sealed record RocketResponse(Guid Id, string Name, JsonElement Range, int Capacity);
}
