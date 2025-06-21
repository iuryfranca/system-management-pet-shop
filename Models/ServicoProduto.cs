using System.ComponentModel.DataAnnotations.Schema;

namespace SystemManagementPetshop.Models;

[Table("servico_produto", Schema = "public")]
public class ServicoProduto
{
    [Column("id_servico_fk")]
    public int ServicoId { get; set; }

    [Column("id_produto_fk")]
    public int ProdutoId { get; set; }

    [Column("quantidade")]
    public int Quantidade { get; set; } = 1;

    [Column("valor_unitario")]
    public decimal ValorUnitario { get; set; }

    public Servico? Servico { get; set; }

    public Produto? Produto { get; set; }
} 