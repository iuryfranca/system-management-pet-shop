using SystemManagementPetshop.Context;
using SystemManagementPetshop.Models;
using Microsoft.EntityFrameworkCore;

namespace SystemManagementPetshop.Services;

public class ServicoService
{
    private readonly ContextDB _context;

    public ServicoService(ContextDB context)
    {
        _context = context;
    }

    public async Task<List<Servico>>? GetServicos()
    {
        return await _context.Servicos
            .Include(s => s.Funcionario)
            .Include(s => s.Animal)
            .ToListAsync();
    }

    public async Task<Servico>? GetServicoById(int id)
    {
        return await _context.Servicos
            .Include(s => s.Funcionario)
            .Include(s => s.Animal)
            .FirstOrDefaultAsync(p => p.Id == id)
            ?? throw new Exception("Serviço não encontrado");
    }

    public async Task<List<Servico>> GetServicosByStatus(StatusServico status)
    {
        return await _context.Servicos
            .Include(s => s.Funcionario)
            .Include(s => s.Animal)
            .Where(p => p.Status == status)
            .ToListAsync();
    }

    public async Task<List<Servico>> GetServicosByFuncionario(int funcionarioId)
    {
        return await _context.Servicos
            .Include(s => s.Funcionario)
            .Include(s => s.Animal)
            .Where(p => p.FuncionarioId == funcionarioId)
            .ToListAsync();
    }

    public async Task<List<Servico>> GetServicosByAnimal(int animalId)
    {
        return await _context.Servicos
            .Include(s => s.Funcionario)
            .Include(s => s.Animal)
            .Where(p => p.AnimalId == animalId)
            .ToListAsync();
    }

    public async Task<Servico>? CreateServico(Servico servico)
    {
        await _context.Servicos.AddAsync(servico);
        await _context.SaveChangesAsync();
        return servico;
    }

    public async Task DeleteServico(int id)
    {
        var servico = await GetServicoById(id);
        _context.Servicos.Remove(servico);
        await _context.SaveChangesAsync();
    }

    public async Task<Servico> UpdateServico(Servico servico)
    {
        if (servico == null)
        {
            throw new ArgumentNullException(nameof(servico));
        }

        bool servicoExiste = await _context.Servicos.AnyAsync(c => c.Id == servico.Id);
        if (!servicoExiste)
        {
            throw new Exception($"Serviço com ID {servico.Id} não encontrado");
        }

        _context.ChangeTracker.Clear();
        _context.Servicos.Update(servico);
        await _context.SaveChangesAsync();
        
        return servico;
    }
} 