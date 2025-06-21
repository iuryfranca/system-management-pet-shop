using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemManagementPetshop.Models;

[Table("servico_produto", Schema = "public")]
public class ServicoProduto
{
    [Key, Column("id_servico_fk", Order = 0)]
    public int ServicoId { get; set; }

    [Key, Column("id_produto_fk", Order = 1)]
    public int ProdutoId { get; set; }

    [Column("quantidade")]
    public int Quantidade { get; set; } = 1;

    [Column("valor_unitario")]
    public decimal ValorUnitario { get; set; }

    public Servico? Servico { get; set; }

    public Produto? Produto { get; set; }
}