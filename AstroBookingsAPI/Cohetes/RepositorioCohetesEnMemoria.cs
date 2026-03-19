namespace AstroBookingsAPI.Cohetes;

public sealed class RepositorioCohetesEnMemoria : IRepositorioCohetes
{
    private readonly Dictionary<Guid, Cohete> _cohetes = new();

    public IReadOnlyCollection<Cohete> ObtenerTodos()
    {
        return _cohetes.Values
            .OrderBy(static cohete => cohete.Nombre, StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }

    public Cohete? ObtenerPorId(Guid id)
    {
        return _cohetes.TryGetValue(id, out Cohete? cohete) ? cohete : null;
    }

    public Cohete Crear(string nombre, AlcanceCohete alcance, int capacidad)
    {
        Cohete cohete = new(Guid.NewGuid(), nombre, alcance, capacidad);
        _cohetes[cohete.Id] = cohete;

        return cohete;
    }

    public Cohete? Actualizar(Guid id, string nombre, AlcanceCohete alcance, int capacidad)
    {
        if (!_cohetes.ContainsKey(id))
        {
            return null;
        }

        Cohete cohete = new(id, nombre, alcance, capacidad);
        _cohetes[id] = cohete;

        return cohete;
    }

    public bool Eliminar(Guid id)
    {
        return _cohetes.Remove(id);
    }
}
