namespace AstroBookingsAPI.Rockets;

public interface IRocketRepository
{
    IReadOnlyCollection<Rocket> GetAll();

    Rocket? GetById(Guid id);

    Rocket Create(string name, RocketRange range, int capacity);

    Rocket? Update(Guid id, string name, RocketRange range, int capacity);

    bool Delete(Guid id);
}
