using backend.Models;

namespace backend.Services.Interfaces;

public interface ILancamentoService
{
    public LancamentoDTO Obter(int id);

    public LancamentoDTO Inserir(LancamentoDTO dto);

    public void Cancelar(int id);

    public LancamentoDTO? Modificar(int id, LancamentoDTO dto);

    public IEnumerable<LancamentoDTO> Listar();

    public IEnumerable<LancamentoDTO> EncontrarPorData(DateTime abaixo, DateTime acima);

    public LancamentoDTO InserirAvulso(LancamentoDTO dto);
}