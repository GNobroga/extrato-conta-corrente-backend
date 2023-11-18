using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace backend.Models;

public class Lancamento 
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int LancamentoId { get; set; }

    [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "A propriedade deve conter apenas caracteres alfanuméricos.")]
    public string? Descricao { get; set; }

    [Column(TypeName = "timestamp with time zone")]
    public DateTime Data { get; set; } = DateTime.Now;

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Valor { get; set; }

    public bool Avulso { get; set; }

    //O status pode ser “Válido” ou “Cancelado”
    [RegularExpression("^(Válido|Cancelado)$", ErrorMessage = "O status só pode ser Válido ou Cancelado")]
    public string Status { get; set; } = "Válido";
}