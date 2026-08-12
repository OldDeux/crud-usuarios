using CrudUsuarios.Data;
using CrudUsuarios.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrudUsuarios.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsuariosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
    {
        var usuarios = await _context.Usuarios.ToListAsync();

        return Ok(usuarios);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Usuario>> GetUsuario(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);

        if (usuario == null)
        {
            return NotFound();
        }

        return Ok(usuario);
    }

    [HttpPost]
    public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
    {
        var emailExiste = await _context.Usuarios
            .AnyAsync(u => u.Email == usuario.Email);

        if (emailExiste)
        {
            return BadRequest("O e-mail informado já está cadastrado.");
        }

        var cpfExiste = await _context.Usuarios
            .AnyAsync(u => u.CPF == usuario.CPF);

        if (cpfExiste)
        {
            return BadRequest("O CPF informado já está cadastrado.");
        }

        _context.Usuarios.Add(usuario);

        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetUsuario),
            new { id = usuario.Id },
            usuario
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
    {
        var usuarioExistente = await _context.Usuarios.FindAsync(id);

        if (usuarioExistente == null)
        {
            return NotFound();
        }

        var emailExiste = await _context.Usuarios
            .AnyAsync(u => u.Email == usuario.Email && u.Id != id);

        if (emailExiste)
        {
            return BadRequest("O e-mail informado já está cadastrado.");
        }

        var cpfExiste = await _context.Usuarios
            .AnyAsync(u => u.CPF == usuario.CPF && u.Id != id);

        if (cpfExiste)
        {
            return BadRequest("O CPF informado já está cadastrado.");
        }

        usuarioExistente.Nome = usuario.Nome;
        usuarioExistente.Email = usuario.Email;
        usuarioExistente.CPF = usuario.CPF;
        usuarioExistente.Telefone = usuario.Telefone;
        usuarioExistente.DataNascimento = usuario.DataNascimento;

        await _context.SaveChangesAsync();

        return NoContent();
    }
}
