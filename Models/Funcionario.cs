using System.ComponentModel.DataAnnotations.Schema;

namespace SystemManagementPetshop.Models;

[Table("funcionario", Schema = "public")]
public class Funcionario
{
    [Column("id_func")]
    public int Id { get; set; }

    [Column("nome_func")]
    public string NomeFunc { get; set; } = "";

    [Column("cargo_func")]
    public string CargoFunc { get; set; } = "";

    [Column("telefone_func")]
    public string TelefoneFunc { get; set; } = "";

    [Column("email_func")]
    public string EmailFunc { get; set; } = "";

    [Column("senha_func")]
    public string SenhaFunc { get; set; } = "";
} 