using System.ComponentModel.DataAnnotations.Schema;

namespace SystemManagementPetshop.Models;

public enum CategoriaProduto
{
    Alimento,
    Medicamento,
    Higiene,
    Acessório,
    Outro
}

[Table("produto", Schema = "public")]
public class Produto
{
    [Column("id_produto")]
    public int Id { get; set; }

    [Column("nome_prod")]
    public string NomeProd { get; set; } = "";

    [Column("marca_prod")]
    public string? MarcaProd { get; set; }

    [Column("valor_prod")]
    public decimal ValorProd { get; set; }

    [Column("estoque")]
    public int Estoque { get; set; } = 0;

    [Column("categoria")]
    public CategoriaProduto Categoria { get; set; }
} 