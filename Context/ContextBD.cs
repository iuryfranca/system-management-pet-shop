using SystemManagementPetshop.Models;
using Microsoft.EntityFrameworkCore;

namespace SystemManagementPetshop.Context;

public class ContextDB : DbContext
{
    public ContextDB(DbContextOptions<ContextDB> options)
        : base(options) { }

    public DbSet<Animal> Animais { get; set; }
    public DbSet<Funcionario> Funcionarios { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Servico> Servicos { get; set; }
    public DbSet<ServicoProduto> ServicosProdutos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurar chave composta para ServicoProduto
        modelBuilder.Entity<ServicoProduto>()
            .HasKey(sp => new { sp.ServicoId, sp.ProdutoId });

        // Configurar relacionamentos
        modelBuilder.Entity<ServicoProduto>()
            .HasOne(sp => sp.Servico)
            .WithMany()
            .HasForeignKey(sp => sp.ServicoId);

        modelBuilder.Entity<ServicoProduto>()
            .HasOne(sp => sp.Produto)
            .WithMany()
            .HasForeignKey(sp => sp.ProdutoId);

        modelBuilder.Entity<Servico>()
            .HasOne(s => s.Funcionario)
            .WithMany()
            .HasForeignKey(s => s.FuncionarioId);

        modelBuilder.Entity<Servico>()
            .HasOne(s => s.Animal)
            .WithMany()
            .HasForeignKey(s => s.AnimalId);

        // Configurar enums como string
        modelBuilder.Entity<Animal>()
            .Property(a => a.TipoAni)
            .HasConversion<string>();

        modelBuilder.Entity<Produto>()
            .Property(p => p.Categoria)
            .HasConversion<string>();

        modelBuilder.Entity<Servico>()
            .Property(s => s.Status)
            .HasConversion<string>();
    }
}
