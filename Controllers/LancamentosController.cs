using backend.Data;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[Route("/api/[controller]")]
[ApiController]
public class LancamentosController : ControllerBase
{
    readonly AppDbContext _context;

    public LancamentosController(AppDbContext context) => _context = context;

    [HttpGet("{id:int}", Name = "FindOne")]
    public ActionResult<Lancamento> Get(int id)
    {
        var lancamento = _context.Lancamentos.Find(id);

        return lancamento != null ? Ok(lancamento) : NotFound();
    }

    [HttpPost]
    public ActionResult<Lancamento> Post([FromBody] Lancamento lancamento) 
    {   
        _context.Add(lancamento);
        _context.SaveChanges();

        return new CreatedAtRouteResult("FindOne", new {
            id = lancamento.LancamentoId
        }, lancamento);
    }


    [HttpDelete("{id:int}")]
    public ActionResult Cancelar(int id) 
    {


        var lancamento = _context.Lancamentos.FirstOrDefault(l => l.LancamentoId == id);

        if (lancamento is null)  
            throw new AppException("Não foi encontrado lançamento com este id para ser cancelado.");

        if (!lancamento.Avulso || lancamento.Status != "Válido")
            throw new AppException("Lançamento não qualificado para cancelamento.");

        lancamento.Status = "Cancelado";

        _context.SaveChanges();

        return Ok(lancamento);
    }

    

    [HttpPut("{id:int}")]
    public ActionResult<Lancamento> Put(int id, [FromBody] LancamentoDTO dto)
    {
        var lancamento = _context.Lancamentos.FirstOrDefault(l => l.LancamentoId == id);

        if (lancamento is null)
            return NotFound();

        lancamento.Descricao = string.IsNullOrEmpty(dto.Descricao) ? lancamento.Descricao : dto.Descricao;
        lancamento.Data = dto.Data.HasValue ? ((DateTime) dto.Data).ToUniversalTime() : lancamento.Data;
        lancamento.Valor = dto.Valor.HasValue ? (decimal) dto.Valor : lancamento.Valor;
        lancamento.Avulso = dto.Avulso.HasValue ? (bool) dto.Avulso : lancamento.Avulso;

        if (!string.IsNullOrEmpty(dto.Status) && (dto.Status.Contains("Válido") || dto.Status.Contains("Cancelado")))
            lancamento.Status = dto.Status;

        _context.SaveChanges();

        return Ok(lancamento);
    }


    [HttpGet]
    public ActionResult<IEnumerable<Lancamento>> GetLancamentos() 
    {

        IEnumerable<Lancamento> lancamentos = _context.Lancamentos;

        return Ok(lancamentos);
    }


    [HttpGet("alcance")]
    public ActionResult<IEnumerable<Lancamento>> FindByDate(DateTime abaixo = default, DateTime acima = default) 
    {   
        var currentData = DateTime.Now.AddDays(-2);

        IEnumerable<Lancamento> todos = _context.Lancamentos.OrderBy(l => l.LancamentoId).AsNoTracking();

        IEnumerable<Lancamento> lancamentos = todos.Where(l => l.Data.Date >= currentData.Date);

        if (abaixo != DateTime.MinValue) 
            lancamentos = todos.Where(l => l.Data.Date >= abaixo.Date);
        

        if (acima != DateTime.MinValue) 
        {
            if (acima < abaixo) throw new AppException("A data do limite superior é abaixo da data no limite inferior.");
            lancamentos = lancamentos.Where(l => l.Data.Date <= acima.Date);
        }

        Console.WriteLine(lancamentos.Count());
        

        return Ok(lancamentos);
    }

    [HttpPost("avulsos")]
    public ActionResult InserirNaoAvulso([FromBody] LancamentoDTO dto)
    {
        if (string.IsNullOrEmpty(dto.Descricao) || !dto.Valor.HasValue || !dto.Data.HasValue)
            throw new AppException("Descrição ou Valor ou Data não foram passados corretamente.");

        dto.Status = "Válido";
        dto.Avulso = false;

        var lancamento = new Lancamento {
            Avulso = (bool) dto.Avulso,
            Data = (DateTime) dto.Data,
            Descricao = dto.Descricao,
            Status = dto.Status
        };

        _context.Lancamentos.Add(lancamento);


        _context.SaveChanges();


        return CreatedAtRoute("FindOne", new { id = lancamento.LancamentoId }, lancamento);
    }
}