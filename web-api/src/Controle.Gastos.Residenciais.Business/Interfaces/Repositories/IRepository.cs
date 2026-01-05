using System.Linq.Expressions;
using Controle.Gastos.Residenciais.Business.Entities;

namespace Controle.Gastos.Residenciais.Business.Interfaces.Repositories
{
    public interface IRepository<TEntity> : IDisposable where TEntity : Entidade
    {
        
        Task<TEntity> ObterPorId(Guid id);
        Task<List<TEntity>> Listar();
        Task<IEnumerable<TEntity>> Listar(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> ListarPaginado(int pagina, int qtdItensPorPagina);
        Task<IEnumerable<TEntity>> ListarPaginado(Expression<Func<TEntity, bool>> predicate, int pagina, int qtdItensPorPagina);
        Task<int> QuantidadeRegistros();
        Task<int> QuantidadeRegistros(Expression<Func<TEntity, bool>> predicate);
        Task<bool> Existe(Expression<Func<TEntity, bool>> predicate);
        Task Adicionar(TEntity entity);
        Task Atualizar(TEntity entity);
        Task Remover(Guid id);
        Task<int> SaveChanges();
    }
}