using QuestLog.Models;
using QuestLog.Repositories;

namespace QuestLog.Services;

public class MisionService : IMisionService
{
    private readonly IMisionRepository _repository;
    private static readonly string[] DificultadesValidas = { "Fácil", "Media", "Difícil" };

    public MisionService(IMisionRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<Mision> ObtenerTodas()
    {
        return _repository.ObtenerTodas();
    }

    public Mision? ObtenerPorId(int id)
    {
        return _repository.ObtenerPorId(id);
    }

    public Mision Crear(Mision mision)
    {
        ValidarMision(mision);

        // Regla de negocio: Todas las misiones nuevas deberán iniciar como no completadas.
        mision.Completada = false;

        return _repository.Crear(mision);
    }

    public bool Actualizar(int id, Mision mision)
    {
        // Regla de negocio: No se deberán permitir modificaciones de misiones inexistentes.
        var existente = _repository.ObtenerPorId(id);
        if (existente == null)
        {
            return false;
        }

        ValidarMision(mision);

        mision.Id = id;
        return _repository.Actualizar(mision);
    }

    public bool Eliminar(int id)
    {
        // Regla de negocio: No se deberán permitir eliminaciones de misiones inexistentes.
        var existente = _repository.ObtenerPorId(id);
        if (existente == null)
        {
            return false;
        }

        return _repository.Eliminar(id);
    }

    private void ValidarMision(Mision mision)
    {
        if (string.IsNullOrWhiteSpace(mision.Nombre))
        {
            throw new ArgumentException("El nombre de la misión es obligatorio.");
        }

        if (!DificultadesValidas.Contains(mision.Dificultad))
        {
            throw new ArgumentException("La dificultad únicamente podrá ser Fácil, Media o Difícil.");
        }

        if (mision.RecompensaXP < 0)
        {
            throw new ArgumentException("La recompensa XP no puede ser negativa.");
        }
    }
}
