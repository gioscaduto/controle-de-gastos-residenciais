using Controle.Gastos.Residenciais.Business.Entities;
using System.Linq.Expressions;

namespace Controle.Gastos.Residenciais.Business.Interfaces.Repositories;

public interface ITransacaoRepository : IRepository<Transacao>
{
    public Task Remover(Expression<Func<Transacao, bool>> predicate);
}
