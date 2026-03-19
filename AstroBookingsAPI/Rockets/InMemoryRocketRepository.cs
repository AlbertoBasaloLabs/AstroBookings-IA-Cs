namespace AstroBookingsAPI.Rockets;

public sealed class InMemoryRocketRepository : IRocketRepository
{
    private readonly Dictionary<Guid, Rocket> _rockets = new();

    public IReadOnlyCollection<Rocket> GetAll()
    {
        return _rockets.Values
            .OrderBy(static rocket => rocket.Name, StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }

    public Rocket? GetById(Guid id)
    {
        return _rockets.TryGetValue(id, out Rocket? rocket) ? rocket : null;
    }

    public Rocket Create(string name, RocketRange range, int capacity)
    {
        Rocket rocket = new(Guid.NewGuid(), name, range, capacity);
        _rockets[rocket.Id] = rocket;

        return rocket;
    }

    public Rocket? Update(Guid id, string name, RocketRange range, int capacity)
    {
        if (!_rockets.ContainsKey(id))
        {
            return null;
        }

        Rocket rocket = new(id, name, range, capacity);
        _rockets[id] = rocket;

        return rocket;
    }

    public bool Delete(Guid id)
    {
        return _rockets.Remove(id);
    }
}
