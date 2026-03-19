using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace AstroBookingsAPI.E2ETests;

public sealed class CohetesE2ETests
{
    private static readonly JsonSerializerOptions OpcionesJson = new(JsonSerializerDefaults.Web);

    [Fact]
    public async Task Crear_ConCargaValida_RetornaCoheteCreado()
    {
        await using WebApplicationFactory<Program> fabrica = new();
        using HttpClient cliente = CrearCliente(fabrica);

        SolicitudGuardarCohete solicitud = new("Starliner", "orbital", 4);

        HttpResponseMessage respuesta = await cliente.PostAsJsonAsync("/cohetes", solicitud);

        Assert.Equal(HttpStatusCode.Created, respuesta.StatusCode);

        RespuestaCohete? cohete = await respuesta.Content.ReadFromJsonAsync<RespuestaCohete>(OpcionesJson);
        Assert.NotNull(cohete);
        Assert.Equal("Starliner", cohete.Nombre);
        Assert.Equal("orbital", ExtraerAlcance(cohete.Alcance));
        Assert.Equal(4, cohete.Capacidad);
    }

    [Fact]
    public async Task Crear_ConAlcanceInvalido_RetornaErrorValidacion()
    {
        await using WebApplicationFactory<Program> fabrica = new();
        using HttpClient cliente = CrearCliente(fabrica);

        HttpResponseMessage respuesta = await cliente.PostAsJsonAsync("/cohetes", new SolicitudGuardarCohete("Ranger", "espacio-profundo", 3));

        Assert.Equal(HttpStatusCode.BadRequest, respuesta.StatusCode);
        await AfirmarErrorValidacion(respuesta, "Alcance");
    }

    [Fact]
    public async Task Crear_ConCapacidadInvalida_RetornaErrorValidacion()
    {
        await using WebApplicationFactory<Program> fabrica = new();
        using HttpClient cliente = CrearCliente(fabrica);

        HttpResponseMessage respuesta = await cliente.PostAsJsonAsync("/cohetes", new SolicitudGuardarCohete("Ranger", "luna", 11));

        Assert.Equal(HttpStatusCode.BadRequest, respuesta.StatusCode);
        await AfirmarErrorValidacion(respuesta, "Capacidad");
    }

    [Fact]
    public async Task Crear_SinNombre_RetornaErrorValidacion()
    {
        await using WebApplicationFactory<Program> fabrica = new();
        using HttpClient cliente = CrearCliente(fabrica);

        HttpResponseMessage respuesta = await cliente.PostAsJsonAsync("/cohetes", new SolicitudGuardarCohete(" ", "luna", 2));

        Assert.Equal(HttpStatusCode.BadRequest, respuesta.StatusCode);
        await AfirmarErrorValidacion(respuesta, "Nombre");
    }

    [Fact]
    public async Task ObtenerTodos_RetornaCohetesAlmacenados()
    {
        await using WebApplicationFactory<Program> fabrica = new();
        using HttpClient cliente = CrearCliente(fabrica);

        await CrearCohete(cliente, new SolicitudGuardarCohete("Apollo", "luna", 3));
        await CrearCohete(cliente, new SolicitudGuardarCohete("Pioneer", "marte", 5));

        HttpResponseMessage respuesta = await cliente.GetAsync("/cohetes");

        Assert.Equal(HttpStatusCode.OK, respuesta.StatusCode);

        IReadOnlyCollection<RespuestaCohete>? cohetes = await respuesta.Content.ReadFromJsonAsync<IReadOnlyCollection<RespuestaCohete>>(OpcionesJson);
        Assert.NotNull(cohetes);
        Assert.Equal(2, cohetes.Count);
        Assert.Contains(cohetes, cohete => cohete.Nombre == "Apollo" && ExtraerAlcance(cohete.Alcance) == "luna" && cohete.Capacidad == 3);
        Assert.Contains(cohetes, cohete => cohete.Nombre == "Pioneer" && ExtraerAlcance(cohete.Alcance) == "marte" && cohete.Capacidad == 5);
    }

    [Fact]
    public async Task ObtenerPorId_CuandoCoheteExiste_RetornaCohete()
    {
        await using WebApplicationFactory<Program> fabrica = new();
        using HttpClient cliente = CrearCliente(fabrica);

        RespuestaCohete creado = await CrearCohete(cliente, new SolicitudGuardarCohete("Falcon", "orbital", 6));

        HttpResponseMessage respuesta = await cliente.GetAsync($"/cohetes/{creado.Id}");

        Assert.Equal(HttpStatusCode.OK, respuesta.StatusCode);

        RespuestaCohete? cohete = await respuesta.Content.ReadFromJsonAsync<RespuestaCohete>(OpcionesJson);
        Assert.NotNull(cohete);
        Assert.Equal(creado.Id, cohete.Id);
        Assert.Equal("Falcon", cohete.Nombre);
    }

    [Fact]
    public async Task ObtenerPorId_CuandoCoheteNoExiste_RetornaNoEncontrado()
    {
        await using WebApplicationFactory<Program> fabrica = new();
        using HttpClient cliente = CrearCliente(fabrica);

        HttpResponseMessage respuesta = await cliente.GetAsync($"/cohetes/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, respuesta.StatusCode);
    }

    [Fact]
    public async Task Actualizar_ConCargaValida_RetornaCoheteActualizado()
    {
        await using WebApplicationFactory<Program> fabrica = new();
        using HttpClient cliente = CrearCliente(fabrica);

        RespuestaCohete creado = await CrearCohete(cliente, new SolicitudGuardarCohete("Pathfinder", "suborbital", 2));

        HttpResponseMessage respuesta = await cliente.PutAsJsonAsync($"/cohetes/{creado.Id}", new SolicitudGuardarCohete("Pathfinder X", "marte", 7));

        Assert.Equal(HttpStatusCode.OK, respuesta.StatusCode);

        RespuestaCohete? actualizado = await respuesta.Content.ReadFromJsonAsync<RespuestaCohete>(OpcionesJson);
        Assert.NotNull(actualizado);
        Assert.Equal(creado.Id, actualizado.Id);
        Assert.Equal("Pathfinder X", actualizado.Nombre);
        Assert.Equal("marte", ExtraerAlcance(actualizado.Alcance));
        Assert.Equal(7, actualizado.Capacidad);
    }

    [Fact]
    public async Task Actualizar_ConAlcanceInvalido_RetornaErrorValidacion()
    {
        await using WebApplicationFactory<Program> fabrica = new();
        using HttpClient cliente = CrearCliente(fabrica);

        RespuestaCohete creado = await CrearCohete(cliente, new SolicitudGuardarCohete("Nova", "orbital", 3));

        HttpResponseMessage respuesta = await cliente.PutAsJsonAsync($"/cohetes/{creado.Id}", new SolicitudGuardarCohete("Nova", "jupiter", 3));

        Assert.Equal(HttpStatusCode.BadRequest, respuesta.StatusCode);
        await AfirmarErrorValidacion(respuesta, "Alcance");
    }

    [Fact]
    public async Task Actualizar_ConCapacidadInvalida_RetornaErrorValidacion()
    {
        await using WebApplicationFactory<Program> fabrica = new();
        using HttpClient cliente = CrearCliente(fabrica);

        RespuestaCohete creado = await CrearCohete(cliente, new SolicitudGuardarCohete("Nova", "orbital", 3));

        HttpResponseMessage respuesta = await cliente.PutAsJsonAsync($"/cohetes/{creado.Id}", new SolicitudGuardarCohete("Nova", "luna", 0));

        Assert.Equal(HttpStatusCode.BadRequest, respuesta.StatusCode);
        await AfirmarErrorValidacion(respuesta, "Capacidad");
    }

    [Fact]
    public async Task Actualizar_SinNombre_RetornaErrorValidacion()
    {
        await using WebApplicationFactory<Program> fabrica = new();
        using HttpClient cliente = CrearCliente(fabrica);

        RespuestaCohete creado = await CrearCohete(cliente, new SolicitudGuardarCohete("Nova", "orbital", 3));

        HttpResponseMessage respuesta = await cliente.PutAsJsonAsync($"/cohetes/{creado.Id}", new SolicitudGuardarCohete("", "luna", 2));

        Assert.Equal(HttpStatusCode.BadRequest, respuesta.StatusCode);
        await AfirmarErrorValidacion(respuesta, "Nombre");
    }

    [Fact]
    public async Task Eliminar_EliminaCohete_YConsultaPosteriorRetornaNoEncontrado()
    {
        await using WebApplicationFactory<Program> fabrica = new();
        using HttpClient cliente = CrearCliente(fabrica);

        RespuestaCohete creado = await CrearCohete(cliente, new SolicitudGuardarCohete("Voyager", "marte", 8));

        HttpResponseMessage respuestaEliminar = await cliente.DeleteAsync($"/cohetes/{creado.Id}");
        Assert.Equal(HttpStatusCode.NoContent, respuestaEliminar.StatusCode);

        HttpResponseMessage respuestaConsulta = await cliente.GetAsync($"/cohetes/{creado.Id}");
        Assert.Equal(HttpStatusCode.NotFound, respuestaConsulta.StatusCode);
    }

    private static HttpClient CrearCliente(WebApplicationFactory<Program> fabrica)
    {
        return fabrica.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost")
        });
    }

    private static async Task<RespuestaCohete> CrearCohete(HttpClient cliente, SolicitudGuardarCohete solicitud)
    {
        HttpResponseMessage respuesta = await cliente.PostAsJsonAsync("/cohetes", solicitud);
        respuesta.EnsureSuccessStatusCode();

        RespuestaCohete? cohete = await respuesta.Content.ReadFromJsonAsync<RespuestaCohete>(OpcionesJson);
        return cohete ?? throw new InvalidOperationException("La carga del cohete estaba vacía.");
    }

    private static async Task AfirmarErrorValidacion(HttpResponseMessage respuesta, string campoEsperado)
    {
        using JsonDocument carga = JsonDocument.Parse(await respuesta.Content.ReadAsStringAsync());
        JsonElement errores = carga.RootElement.GetProperty("errors");

        bool encontrado = errores.EnumerateObject().Any(error => string.Equals(error.Name, campoEsperado, StringComparison.OrdinalIgnoreCase));
        Assert.True(encontrado, $"Se esperaba error de validación para el campo '{campoEsperado}'.");
    }

    private static string ExtraerAlcance(JsonElement alcance)
    {
        return alcance.ValueKind switch
        {
            JsonValueKind.String => alcance.GetString()!,
            JsonValueKind.Object => alcance.GetProperty("valor").GetString()!,
            _ => throw new InvalidOperationException("Formato de alcance no esperado.")
        };
    }

    private sealed record SolicitudGuardarCohete(string Nombre, string Alcance, int Capacidad);

    private sealed record RespuestaCohete(Guid Id, string Nombre, JsonElement Alcance, int Capacidad);
}
