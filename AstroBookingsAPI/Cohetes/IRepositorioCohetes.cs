namespace AstroBookingsAPI.Cohetes;

public interface IRepositorioCohetes
{
    IReadOnlyCollection<Cohete> ObtenerTodos();

    Cohete? ObtenerPorId(Guid id);

    Cohete Crear(string nombre, AlcanceCohete alcance, int capacidad);

    Cohete? Actualizar(Guid id, string nombre, AlcanceCohete alcance, int capacidad);

    bool Eliminar(Guid id);
}
