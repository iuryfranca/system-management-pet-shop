using SystemManagementPetshop.Context;
using SystemManagementPetshop.Models;
using Microsoft.EntityFrameworkCore;

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

        _context.ChangeTracker.Clear();
        _context.Funcionarios.Update(funcionario);
        await _context.SaveChangesAsync();
        
        return funcionario;
    }
} 