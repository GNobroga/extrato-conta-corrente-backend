using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace backend.Models;

public class LancamentoDTO
{
    [JsonPropertyName("id")]
    public int? LancamentoId { get; set; }

    [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "A propriedade deve conter apenas caracteres alfanuméricos.")]
    public string? Descricao { get; set; }

    public DateTime? Data { get; set; }

    public decimal Valor { get; set; }

    public bool Avulso { get; set; }

    [RegularExpression("^(Válido|Cancelado)$", ErrorMessage = "O status só pode ser Válido ou Cancelado")]
    public string Status { get; set; } = "Válido";
}