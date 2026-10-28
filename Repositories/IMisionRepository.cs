using QuestLog.Models;

namespace QuestLog.Repositories;

public interface IMisionRepository
{
    IEnumerable<Mision> ObtenerTodas();
    Mision? ObtenerPorId(int id);
    Mision Crear(Mision mision);
    bool Actualizar(Mision mision);
    bool Eliminar(int id);
}
