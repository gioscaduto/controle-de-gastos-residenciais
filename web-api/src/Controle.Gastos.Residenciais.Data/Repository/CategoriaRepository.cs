using Controle.Gastos.Residenciais.Business.Entities;
using Controle.Gastos.Residenciais.Business.Interfaces.Repositories;
using Controle.Gastos.Residenciais.Data.Context;
using Controle.Gastos.Residenciais.Data.Extensions;

namespace Controle.Gastos.Residenciais.Data.Repository
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        private readonly ITransacaoRepository _transacaoRepository;

        public CategoriaRepository(MeuDbContext context, ITransacaoRepository transacaoRepository) : base(context)
        {
            _transacaoRepository = transacaoRepository;
        }

        public override async Task<IEnumerable<Categoria>> ListarPaginado(int pagina, int qtdItensPorPagina)
        {
            return await DbSet
              .OrderBy(c => c.Descricao)
              .Paginar(pagina, qtdItensPorPagina);
        }

        public override async Task Remover(Guid id)
        {
            await base.Remover(id);
            await _transacaoRepository.Remover(t => t.CategoriaId == id);
        }
    }
}