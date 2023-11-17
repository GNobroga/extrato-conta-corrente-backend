using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace backend.Models;

public class LancamentoDTO
{

    public int? Id { get; set; }
    
    public string Descricao { get; set; }

    public DateTime? Data { get; set; }

    public decimal? Valor { get; set; }

    public bool? Avulso { get; set; }

    public string Status { get; set; } = "Válido";
}