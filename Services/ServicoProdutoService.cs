using SystemManagementPetshop.Context;
using SystemManagementPetshop.Models;
using Microsoft.EntityFrameworkCore;

namespace SystemManagementPetshop.Services;

public class ServicoProdutoService
{
    private readonly ContextDB _context;

    public ServicoProdutoService(ContextDB context)
    {
        _context = context;
    }

    public async Task<List<ServicoProduto>>? GetServicosProdutos()
    {
        return await _context.ServicosProdutos
            .Include(sp => sp.Servico)
            .Include(sp => sp.Produto)
            .ToListAsync();
    }

    public async Task<List<ServicoProduto>> GetServicosProdutosByServicoId(int servicoId)
    {
        return await _context.ServicosProdutos
            .Include(sp => sp.Servico)
            .Include(sp => sp.Produto)
            .Where(sp => sp.ServicoId == servicoId)
            .ToListAsync();
    }

    public async Task<List<ServicoProduto>> GetServicosProdutosByProdutoId(int produtoId)
    {
        return await _context.ServicosProdutos
            .Include(sp => sp.Servico)
            .Include(sp => sp.Produto)
            .Where(sp => sp.ProdutoId == produtoId)
            .ToListAsync();
    }

    public async Task<ServicoProduto>? GetServicoProdutoById(int servicoId, int produtoId)
    {
        return await _context.ServicosProdutos
            .Include(sp => sp.Servico)
            .Include(sp => sp.Produto)
            .FirstOrDefaultAsync(sp => sp.ServicoId == servicoId && sp.ProdutoId == produtoId)
            ?? throw new Exception("Relacionamento Serviço-Produto não encontrado");
    }

    public async Task<ServicoProduto>? CreateServicoProduto(ServicoProduto servicoProduto)
    {
        await _context.ServicosProdutos.AddAsync(servicoProduto);
        await _context.SaveChangesAsync();
        return servicoProduto;
    }

    public async Task DeleteServicoProduto(int servicoId, int produtoId)
    {
        var servicoProduto = await GetServicoProdutoById(servicoId, produtoId);
        _context.ServicosProdutos.Remove(servicoProduto);
        await _context.SaveChangesAsync();
    }

    public async Task<ServicoProduto> UpdateServicoProduto(ServicoProduto servicoProduto)
    {
        if (servicoProduto == null)
        {
            throw new ArgumentNullException(nameof(servicoProduto));
        }

        bool servicoProdutoExiste = await _context.ServicosProdutos
            .AnyAsync(sp => sp.ServicoId == servicoProduto.ServicoId && sp.ProdutoId == servicoProduto.ProdutoId);
        
        if (!servicoProdutoExiste)
        {
            throw new Exception($"Relacionamento Serviço-Produto não encontrado");
        }

        _context.ChangeTracker.Clear();
        _context.ServicosProdutos.Update(servicoProduto);
        await _context.SaveChangesAsync();
        
        return servicoProduto;
    }
} 