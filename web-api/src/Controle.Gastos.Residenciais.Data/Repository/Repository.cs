using Controle.Gastos.Residenciais.Business.Entities;
using Controle.Gastos.Residenciais.Business.Interfaces.Repositories;
using Controle.Gastos.Residenciais.Data.Context;
using Controle.Gastos.Residenciais.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Controle.Gastos.Residenciais.Data.Repository
{
    public abstract class Repository<TEntidade> : IRepository<TEntidade> where TEntidade : Entidade
    {
        protected readonly MeuDbContext Db;
        protected readonly DbSet<TEntidade> DbSet;

        protected Repository(MeuDbContext db)
        {
            Db = db;
            DbSet = db.Set<TEntidade>();
        }

        public virtual async Task<TEntidade> ObterPorId(Guid id) =>
            await DbSet.FindAsync(id);

        public virtual async Task<List<TEntidade>> Listar() =>
            await DbSet.ToListAsync();

        public async virtual Task<IEnumerable<TEntidade>> Listar(Expression<Func<TEntidade, bool>> predicate) =>
            await DbSet.AsNoTracking().Where(predicate).ToListAsync();

        public async virtual Task<IEnumerable<TEntidade>> ListarPaginado(int pagina, int qtdItensPorPagina) =>
           await DbSet
               .Paginar(pagina, qtdItensPorPagina);

        public async virtual Task<IEnumerable<TEntidade>> ListarPaginado(Expression<Func<TEntidade, bool>> predicate,
            int pagina, int qtdItensPorPagina)
        {
            return await DbSet
                .Where(predicate)
                .Paginar(pagina, qtdItensPorPagina);
        }

        public virtual async Task<int> QuantidadeRegistros() =>
            await DbSet.CountAsync();

        public virtual async Task<int> QuantidadeRegistros(Expression<Func<TEntidade, bool>> predicate) =>
            await DbSet.CountAsync(predicate);

        public virtual async Task<bool> Existe(Expression<Func<TEntidade, bool>> predicate) =>
            await DbSet.AnyAsync(predicate);

        public virtual async Task Adicionar(TEntidade entidade)
        {
            DbSet.Add(entidade);
            await SaveChanges();
        }

        public virtual async Task Atualizar(TEntidade entidade)
        {
            DbSet.Update(entidade);
            await SaveChanges();
        }

        public virtual async Task Remover(Guid id)
        {
            var entidade = await ObterPorId(id);
            entidade.Remover();
            await Atualizar(entidade);
        }

        public async virtual Task<int> SaveChanges() =>
            await Db.SaveChangesAsync();

        public virtual void Dispose() =>
            Db?.Dispose();
    }
}