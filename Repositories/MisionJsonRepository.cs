using System.Text.Encodings.Web;
using System.Text.Json;
using QuestLog.Models;

namespace QuestLog.Repositories;

public class MisionJsonRepository : IMisionRepository
{
    private readonly string _filePath;
    private readonly JsonSerializerOptions _jsonOptions;

    public MisionJsonRepository(IWebHostEnvironment env)
    {
        _filePath = Path.Combine(env.ContentRootPath, "misiones.json");
        _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        if (!File.Exists(_filePath))
        {
            GuardarDatos(new List<Mision>());
        }
    }

    private List<Mision> LeerDatos()
    {
        if (!File.Exists(_filePath))
        {
            return new List<Mision>();
        }

        var json = File.ReadAllText(_filePath);
        if (string.IsNullOrWhiteSpace(json))
        {
            return new List<Mision>();
        }

        return JsonSerializer.Deserialize<List<Mision>>(json) ?? new List<Mision>();
    }

    private void GuardarDatos(List<Mision> misiones)
    {
        var json = JsonSerializer.Serialize(misiones, _jsonOptions);
        File.WriteAllText(_filePath, json);
    }

    public IEnumerable<Mision> ObtenerTodas()
    {
        return LeerDatos();
    }

    public Mision? ObtenerPorId(int id)
    {
        var misiones = LeerDatos();
        return misiones.FirstOrDefault(m => m.Id == id);
    }

    public Mision Crear(Mision mision)
    {
        var misiones = LeerDatos();
        mision.Id = misiones.Any() ? misiones.Max(m => m.Id) + 1 : 1;
        misiones.Add(mision);
        GuardarDatos(misiones);
        return mision;
    }

    public bool Actualizar(Mision mision)
    {
        var misiones = LeerDatos();
        var index = misiones.FindIndex(m => m.Id == mision.Id);
        if (index == -1)
        {
            return false;
        }

        misiones[index] = mision;
        GuardarDatos(misiones);
        return true;
    }

    public bool Eliminar(int id)
    {
        var misiones = LeerDatos();
        var mision = misiones.FirstOrDefault(m => m.Id == id);
        if (mision == null)
        {
            return false;
        }

        misiones.Remove(mision);
        GuardarDatos(misiones);
        return true;
    }
}
