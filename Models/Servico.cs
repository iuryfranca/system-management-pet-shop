using System.ComponentModel.DataAnnotations.Schema;

namespace SystemManagementPetshop.Models;

public enum StatusServico
{
    Agendado,
    EmAndamento,
    Concluído,
    Cancelado
}

[Table("servico", Schema = "public")]
public class Servico
{
    [Column("id_servico")]
    public int Id { get; set; }

    [Column("nome_serv")]
    public string NomeServ { get; set; } = "";

    [Column("descricao_serv")]
    public string? DescricaoServ { get; set; }

    [Column("duracao_serv")]
    public TimeSpan? DuracaoServ { get; set; }

    [Column("valor_serv")]
    public decimal ValorServ { get; set; }

    [Column("id_func_fk")]
    public int? FuncionarioId { get; set; }

    [Column("id_animal_fk")]
    public int? AnimalId { get; set; }

    [Column("data_servico")]
    public DateTime DataServico { get; set; }

    [Column("status")]
    public StatusServico Status { get; set; } = StatusServico.Agendado;

    public Funcionario? Funcionario { get; set; }

    public Animal? Animal { get; set; }
} 