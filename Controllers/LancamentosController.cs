using backend.Models;
using backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[Route("/api/[controller]")]
[ApiController]
public class LancamentosController : ControllerBase
{
    private readonly ILancamentoService _service;

    public LancamentosController(ILancamentoService service) => _service = service;


    [HttpGet("{id:int}", Name = "Encontrar")]
    public ActionResult<LancamentoDTO> Obter(int id)
    {
        return _service.Obter(id);
    }

    [HttpPost]
    public ActionResult<Lancamento> Inserir([FromBody] LancamentoDTO dto)
    {
        var inserido = _service.Inserir(dto);
        return new CreatedAtRouteResult("Encontrar", new { id = inserido.LancamentoId }, inserido);
    }


    [HttpDelete("{id:int}")]
    public ActionResult<LancamentoDTO> Cancelar(int id)
    {
        _service.Cancelar(id);
        return Ok();
    }


    [HttpPut("{id:int}")]
    public ActionResult<LancamentoDTO> Modificar(int id, LancamentoDTO dto)
    {
        return Ok(_service.Modificar(id, dto));
    }

    [HttpGet]
    public ActionResult<IEnumerable<LancamentoDTO>> Listar() => Ok(_service.Listar());


    [HttpGet("alcance")]
    public ActionResult<IEnumerable<LancamentoDTO>> EncontrarPorData(DateTime abaixo = default, DateTime acima = default)
    {
        return Ok(_service.EncontrarPorData(abaixo, acima));
    }

    [HttpPost("avulsos")]
    public ActionResult<LancamentoDTO> InserirNaoAvulso([FromBody] LancamentoDTO dto)
    {
        var inserido = _service.InserirAvulso(dto);

        return CreatedAtRoute("Encontrar", new { id = inserido.LancamentoId }, inserido);
    }
}