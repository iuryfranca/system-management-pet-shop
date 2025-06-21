using SystemManagementPetshop.Context;
using SystemManagementPetshop.Models;
using Microsoft.EntityFrameworkCore;

namespace SystemManagementPetshop.Services;

public class ProdutoService
{
    private readonly ContextDB _context;

    public ProdutoService(ContextDB context)
    {
        _context = context;
    }

    public async Task<List<Produto>>? GetProdutos()
    {
        return await _context.Produtos.ToListAsync();
    }

    public async Task<Produto>? GetProdutoById(int id)
    {
        return await _context.Produtos.FirstOrDefaultAsync(p => p.Id == id)
            ?? throw new Exception("Produto não encontrado");
    }

    public async Task<List<Produto>> GetProdutosByCategoria(CategoriaProduto categoria)
    {
        return await _context.Produtos.Where(p => p.Categoria == categoria).ToListAsync();
    }

    public async Task<List<Produto>> GetProdutosEmEstoque()
    {
        return await _context.Produtos.Where(p => p.Estoque > 0).ToListAsync();
    }

    public async Task<List<Produto>> GetProdutosByMarca(string marca)
    {
        return await _context.Produtos.Where(p => p.MarcaProd.Contains(marca)).ToListAsync();
    }

    public async Task<Produto>? CreateProduto(Produto produto)
    {
        await _context.Produtos.AddAsync(produto);
        await _context.SaveChangesAsync();
        return produto;
    }

    public async Task DeleteProduto(int id)
    {
        var produto = await GetProdutoById(id);
        _context.Produtos.Remove(produto);
        await _context.SaveChangesAsync();
    }

    public async Task<Produto> UpdateProduto(Produto produto)
    {
        if (produto == null)
        {
            throw new ArgumentNullException(nameof(produto));
        }

        bool produtoExiste = await _context.Produtos.AnyAsync(c => c.Id == produto.Id);
        if (!produtoExiste)
        {
            throw new Exception($"Produto com ID {produto.Id} não encontrado");
        }

        _context.ChangeTracker.Clear();
        _context.Produtos.Update(produto);
        await _context.SaveChangesAsync();
        
        return produto;
    }
} 