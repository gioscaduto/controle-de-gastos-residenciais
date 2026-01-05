using Controle.Gastos.Residenciais.Business.Entities;

namespace Controle.Gastos.Residenciais.Business.Interfaces.Services
{
    public interface ITransacaoService : IDisposable
    {
        Task Adicionar(Transacao transacao);
    }
}
