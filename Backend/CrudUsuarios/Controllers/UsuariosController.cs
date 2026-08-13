using CrudUsuarios.Models;
using CrudUsuarios.Services;
using Microsoft.AspNetCore.Mvc;

namespace CrudUsuarios.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly UsuarioService _service;

    public UsuariosController(UsuarioService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
    {
        var usuarios = await _service.GetTodosAsync();

        return Ok(usuarios);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Usuario>> GetUsuario(int id)
    {
        var usuario = await _service.GetPorIdAsync(id);

        if (usuario == null)
        {
            return NotFound();
        }

        return Ok(usuario);
    }

    [HttpPost]
    public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
    {
        var resultado = await _service.CriarAsync(usuario);

        if (!resultado.Sucesso)
        {
            return BadRequest(resultado.Erro);
        }

        return CreatedAtAction(
            nameof(GetUsuario),
            new { id = resultado.Usuario!.Id },
            resultado.Usuario
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
    {
        var resultado = await _service.AtualizarAsync(id, usuario);

        if (!resultado.Sucesso)
        {
            if (resultado.Erro == "Usuário não encontrado.")
            {
                return NotFound();
            }

            return BadRequest(resultado.Erro);
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUsuario(int id)
    {
        var sucesso = await _service.ExcluirAsync(id);

        if (!sucesso)
        {
            return NotFound();
        }

        return NoContent();
    }
}