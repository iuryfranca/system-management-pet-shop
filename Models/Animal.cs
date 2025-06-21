using System.ComponentModel.DataAnnotations.Schema;

namespace SystemManagementPetshop.Models;

public enum TipoAnimal
{
    Cachorro,
    Gato,
    Passaro,
    Peixe,
    Roedor,
    Reptil,
    Outro
}

[Table("animal", Schema = "public")]
public class Animal
{
    [Column("id_animal")]
    public int Id { get; set; }

    [Column("nome_ani")]
    public string NomeAni { get; set; } = "";

    [Column("tipo_ani")]
    public TipoAnimal TipoAni { get; set; }

    [Column("raca_ani")]
    public string RacaAni { get; set; } = "";

    [Column("idade_ani")]
    public int? IdadeAni { get; set; }

    [Column("peso_ani")]
    public double? PesoAni { get; set; }

    [Column("dono_nome")]
    public string? DonoNome { get; set; }

    [Column("dono_telefone")]
    public string? DonoTelefone { get; set; }
}