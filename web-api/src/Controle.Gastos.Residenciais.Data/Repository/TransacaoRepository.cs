using Controle.Gastos.Residenciais.Business.Entities;
using Controle.Gastos.Residenciais.Business.Interfaces.Repositories;
using Controle.Gastos.Residenciais.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Controle.Gastos.Residenciais.Data.Repository
{
    public class TransacaoRepository : Repository<Transacao>, ITransacaoRepository
    {
        public TransacaoRepository(MeuDbContext context) : base(context)
        {
        }

        public async Task Remover(Expression<Func<Transacao, bool>> predicate)
        {
            var transacoes = await DbSet.Where(predicate).ToListAsync();

            if (transacoes.Any())
            {
                foreach (var transacao in transacoes)
                {
                    transacao.Remover();
                }

                DbSet.UpdateRange(transacoes);
                await SaveChanges();
            }
        }
    }
}
