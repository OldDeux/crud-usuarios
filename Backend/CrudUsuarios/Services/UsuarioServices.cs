using CrudUsuarios.Data;
using CrudUsuarios.Models;
using Microsoft.EntityFrameworkCore;

namespace CrudUsuarios.Services;

public class UsuarioService
{
    private readonly AppDbContext _context;

    public UsuarioService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Usuario>> GetTodosAsync()
    {
        return await _context.Usuarios.ToListAsync();
    }

    public async Task<Usuario?> GetPorIdAsync(int id)
    {
        return await _context.Usuarios.FindAsync(id);
    }

    public async Task<(bool Sucesso, string? Erro, Usuario? Usuario)> CriarAsync(Usuario usuario)
    {
        var emailExiste = await _context.Usuarios
            .AnyAsync(u => u.Email == usuario.Email);

        if (emailExiste)
        {
            return (false, "O e-mail informado já está cadastrado.", null);
        }

        var cpfExiste = await _context.Usuarios
            .AnyAsync(u => u.CPF == usuario.CPF);

        if (cpfExiste)
        {
            return (false, "O CPF informado já está cadastrado.", null);
        }

        if (usuario.DataNascimento > DateTime.Now)
        {
            return (false, "A data de nascimento não pode ser futura.", null);
        }

        _context.Usuarios.Add(usuario);

        await _context.SaveChangesAsync();

        return (true, null, usuario);
    }

    public async Task<(bool Sucesso, string? Erro)> AtualizarAsync(int id, Usuario usuario)
    {
        var usuarioExistente = await _context.Usuarios.FindAsync(id);

        if (usuarioExistente == null)
        {
            return (false, "Usuário não encontrado.");
        }

        var emailExiste = await _context.Usuarios
            .AnyAsync(u => u.Email == usuario.Email && u.Id != id);

        if (emailExiste)
        {
            return (false, "O e-mail informado já está cadastrado.");
        }

        var cpfExiste = await _context.Usuarios
            .AnyAsync(u => u.CPF == usuario.CPF && u.Id != id);

        if (cpfExiste)
        {
            return (false, "O CPF informado já está cadastrado.");
        }

        if (usuario.DataNascimento > DateTime.Now)
        {
            return (false, "A data de nascimento não pode ser futura.");
        }

        usuarioExistente.Nome = usuario.Nome;
        usuarioExistente.Email = usuario.Email;
        usuarioExistente.CPF = usuario.CPF;
        usuarioExistente.Telefone = usuario.Telefone;
        usuarioExistente.DataNascimento = usuario.DataNascimento;

        await _context.SaveChangesAsync();

        return (true, null);
    }

    public async Task<bool> ExcluirAsync(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);

        if (usuario == null)
        {
            return false;
        }

        _context.Usuarios.Remove(usuario);

        await _context.SaveChangesAsync();

        return true;
    }
}