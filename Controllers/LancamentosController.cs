using AutoMapper;
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
    readonly IMapper _mapper;

    public LancamentosController(AppDbContext context, IMapper mapper) 
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet("{id:int}", Name = "Encontrar")]
    public ActionResult<Lancamento> Get(int id)
    {
        var lancamento = _context.Lancamentos.FirstOrDefault(l => l.LancamentoId == id);

        if (lancamento != null) 
        {
            return Ok(_mapper.Map<Lancamento, LancamentoDTO>(lancamento));
        }

        return NotFound();
    }

    [HttpPost]
    public ActionResult<Lancamento> Post([FromBody] LancamentoDTO dto) 
    {   
        dto.LancamentoId = null;

        var novoLancamento = _mapper.Map<LancamentoDTO, Lancamento>(dto);

        _context.Add(novoLancamento);

        _context.SaveChanges();

        return new CreatedAtRouteResult("Encontrar", new {
            id = novoLancamento.LancamentoId
        }, novoLancamento);
    }


    [HttpDelete("{id:int}")]
    public ActionResult Cancelar(int id) 
    {
        var lancamento = _context.Lancamentos.FirstOrDefault(l => l.LancamentoId == id) ?? throw new AppException("Não foi encontrado lançamento com este id para ser cancelado.");
        
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

        if (lancamento is null) return NotFound();

        _mapper.Map(dto, lancamento);

        _context.SaveChanges();

        return Ok(lancamento);
    }

    [HttpGet]
    public ActionResult<IEnumerable<Lancamento>> GetLancamentos() => Ok(_context.Lancamentos);


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
            if (acima.Date < abaixo.Date) throw new AppException("A data do limite superior é abaixo da data no limite inferior.");
            
            lancamentos = lancamentos.Where(l => l.Data.Date <= acima.Date);
        }

        return Ok(lancamentos.Select(l => _mapper.Map<Lancamento, LancamentoDTO>(l)));
    }

    [HttpPost("avulsos")]
    public ActionResult InserirNaoAvulso([FromBody] LancamentoDTO dto)
    {
        dto.LancamentoId = null;
        dto.Status = "Válido";
        dto.Avulso = false;

        var lancamento = _mapper.Map<LancamentoDTO, Lancamento>(dto);

        _context.Lancamentos.Add(lancamento);

        _context.SaveChanges();

        return CreatedAtRoute("FindOne", new { id = lancamento.LancamentoId }, lancamento);
    }
}