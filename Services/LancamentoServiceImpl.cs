using System.Xml;
using AutoMapper;
using backend.Data;
using backend.Models;
using backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace backend.Services;

public class LancamentoServiceImpl : ILancamentoService
{
    private readonly IMapper _mapper;
    private readonly AppDbContext _context;

    public LancamentoServiceImpl(IMapper mapper, AppDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public void Cancelar(int id)
    {
         var lancamento = _context.Lancamentos.FirstOrDefault(l => l.LancamentoId == id) ?? throw new AppException("Não foi encontrado lançamento com este id para ser cancelado.");

        if (!lancamento.Avulso || lancamento.Status != "Válido")
            throw new AppException("Lançamento não qualificado para cancelamento.");

        lancamento.Status = "Cancelado";

        _context.SaveChanges();
    }

    public IEnumerable<LancamentoDTO> EncontrarPorData(DateTime abaixo, DateTime acima)
    {
        var currentData = DateTime.Now.AddDays(-2);

        IEnumerable<Lancamento> todos = _context.Lancamentos.AsNoTracking();

        IEnumerable<Lancamento> lancamentos = todos.Where(l => l.Data.Date >= currentData.Date);

        if (abaixo != DateTime.MinValue) 
            lancamentos = todos.Where(l => l.Data.Date >= abaixo.Date);

        if (acima != DateTime.MinValue)
        {
            if (acima.Date < abaixo.Date) throw new AppException("A data do limite superior é abaixo da data no limite inferior.");

            lancamentos = lancamentos.Where(l => l.Data.Date <= acima.Date);
        }

        return lancamentos.OrderBy(l => l.LancamentoId).Select(l => _mapper.Map<Lancamento, LancamentoDTO>(l));
    }

    public LancamentoDTO Inserir(LancamentoDTO dto)
    {
        dto.LancamentoId = null;

        var novoLancamento = _mapper.Map<LancamentoDTO, Lancamento>(dto);

        _context.Add(novoLancamento);

        _context.SaveChanges();

        return _mapper.Map<Lancamento, LancamentoDTO>(novoLancamento);
    }

    public IEnumerable<LancamentoDTO> Listar()
    {
        return _context.Lancamentos.AsNoTracking().Select(l => _mapper.Map<Lancamento, LancamentoDTO>(l));
    }

    public LancamentoDTO? Modificar(int id, LancamentoDTO dto)
    {
        var lancamento = _context.Lancamentos.AsNoTracking().FirstOrDefault(l => l.LancamentoId == id) ?? throw new AppException($"Lançamento com id {id} não existe.", 404);

        dto.LancamentoId = id;

        _mapper.Map(dto, lancamento);

        _context.Entry(lancamento).State = EntityState.Modified;

        _context.SaveChanges();

        return _mapper.Map<Lancamento, LancamentoDTO>(lancamento);
    }

    public LancamentoDTO Obter(int id)
    {
        var lancamento = _context.Lancamentos.AsNoTracking().FirstOrDefault(l => l.LancamentoId == id) ?? throw new AppException($"Lançamento com id {id} não existe.", 404);

        return _mapper.Map<Lancamento, LancamentoDTO>(lancamento);
    }

    public LancamentoDTO InserirAvulso(LancamentoDTO dto) 
    {
        dto.LancamentoId = null;
        dto.Status = "Válido";
        dto.Avulso = false;

        var lancamento = _mapper.Map<LancamentoDTO, Lancamento>(dto);

        _context.Lancamentos.Add(lancamento);

        _context.SaveChanges();

        return _mapper.Map<Lancamento, LancamentoDTO>(lancamento);
    }
}