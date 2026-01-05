using Controle.Gastos.Residenciais.Business.Entities;

namespace Controle.Gastos.Residenciais.Business.Interfaces.Services
{
    public interface IPessoaService : IDisposable
    {
        Task Adicionar(Pessoa pessoa);
        Task Atualizar(Pessoa pessoa);
        Task Remover(Guid id);
    }
}
