using Controle.Gastos.Residenciais.Data.Dtos;

namespace Controle.Gastos.Residenciais.Data.Queries;

public interface ITransacaoQuery
{
    Task<IEnumerable<TransacaoPessoaDto>> ListarPorPessoa();
    Task<IEnumerable<TransacaoCategoriaDto>> ListarPorCategoria();
}