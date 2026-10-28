using Microsoft.AspNetCore.Mvc;
using QuestLog.Models;
using QuestLog.Services;

namespace QuestLog.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MisionesController : ControllerBase
{
    private readonly IMisionService _service;

    public MisionesController(IMisionService service)
    {
        _service = service;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Mision>> ObtenerTodas()
    {
        var misiones = _service.ObtenerTodas();
        return Ok(misiones);
    }

    [HttpGet("{id}")]
    public ActionResult<Mision> ObtenerPorId(int id)
    {
        var mision = _service.ObtenerPorId(id);
        if (mision == null)
        {
            return NotFound(new { mensaje = $"No se encontró la misión con ID {id}." });
        }

        return Ok(mision);
    }

    [HttpPost]
    public ActionResult<Mision> Crear([FromBody] Mision mision)
    {
        try
        {
            var creada = _service.Crear(mision);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = creada.Id }, creada);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public IActionResult Actualizar(int id, [FromBody] Mision mision)
    {
        try
        {
            var actualizada = _service.Actualizar(id, mision);
            if (!actualizada)
            {
                return NotFound(new { mensaje = $"No se encontró la misión con ID {id} para actualizar." });
            }

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public IActionResult Eliminar(int id)
    {
        var eliminada = _service.Eliminar(id);
        if (!eliminada)
        {
            return NotFound(new { mensaje = $"No se encontró la misión con ID {id} para eliminar." });
        }

        return NoContent();
    }
}
