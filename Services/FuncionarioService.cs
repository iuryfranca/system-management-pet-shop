using SystemManagementPetshop.Context;
using SystemManagementPetshop.Models;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace SystemManagementPetshop.Services;

public class FuncionarioService
{
    private readonly ContextDB _context;

    public FuncionarioService(ContextDB context)
    {
        _context = context;
    }

    public async Task<List<Funcionario>>? GetFuncionarios()
    {
        return await _context.Funcionarios.ToListAsync();
    }

    public async Task<Funcionario>? GetFuncionarioById(int id)
    {
        return await _context.Funcionarios.FirstOrDefaultAsync(p => p.Id == id)
            ?? throw new Exception("Funcionário não encontrado");
    }

    public async Task<Funcionario>? GetFuncionarioByEmail(string email)
    {
        return await _context.Funcionarios.FirstOrDefaultAsync(p => p.EmailFunc == email)
            ?? throw new Exception("Funcionário não encontrado");
    }

    public async Task<Funcionario>? CreateFuncionario(Funcionario funcionario)
    {
        _context.ChangeTracker.Clear();

        // Hash da senha antes de salvar
        if (!string.IsNullOrEmpty(funcionario.SenhaFunc))
        {
            funcionario.SenhaFunc = HashPassword(funcionario.SenhaFunc);
        }

        await _context.Funcionarios.AddAsync(funcionario);
        await _context.SaveChangesAsync();
        return funcionario;
    }

    public async Task DeleteFuncionario(int id)
    {
        var funcionario = await GetFuncionarioById(id);
        _context.Funcionarios.Remove(funcionario);
        await _context.SaveChangesAsync();
    }

    public async Task<Funcionario> UpdateFuncionario(Funcionario funcionario)
    {
        if (funcionario == null)
        {
            throw new ArgumentNullException(nameof(funcionario));
        }

        bool funcionarioExiste = await _context.Funcionarios.AnyAsync(c => c.Id == funcionario.Id);
        if (!funcionarioExiste)
        {
            throw new Exception($"Funcionário com ID {funcionario.Id} não encontrado");
        }

        // Verificar se a senha foi alterada (nova senha não deve vir hasheada)
        var funcionarioExistente = await _context.Funcionarios.AsNoTracking()
            .FirstOrDefaultAsync(f => f.Id == funcionario.Id);

        if (funcionarioExistente != null &&
            !string.IsNullOrEmpty(funcionario.SenhaFunc) &&
            funcionario.SenhaFunc != funcionarioExistente.SenhaFunc)
        {
            // Se a senha é diferente da atual e não parece estar hasheada (muito curta ou muito simples)
            // então fazemos o hash da nova senha
            if (funcionario.SenhaFunc.Length < 60) // Senhas BCrypt têm ~60 caracteres
            {
                funcionario.SenhaFunc = HashPassword(funcionario.SenhaFunc);
            }
        }

        _context.ChangeTracker.Clear();
        _context.Funcionarios.Update(funcionario);
        await _context.SaveChangesAsync();

        return funcionario;
    }

    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt());
    }

    private bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }

    public async Task<bool> ValidarCredenciais(string email, string senha)
    {
        try
        {
            var funcionario = await _context.Funcionarios
                .FirstOrDefaultAsync(f => f.EmailFunc == email);

            if (funcionario == null)
                return false;

            return VerifyPassword(senha, funcionario.SenhaFunc);
        }
        catch
        {
            return false;
        }
    }
}