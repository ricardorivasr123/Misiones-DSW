using QuestLog.Models;

namespace QuestLog.Services;

public interface IMisionService
{
    IEnumerable<Mision> ObtenerTodas();
    Mision? ObtenerPorId(int id);
    Mision Crear(Mision mision);
    bool Actualizar(int id, Mision mision);
    bool Eliminar(int id);
}
