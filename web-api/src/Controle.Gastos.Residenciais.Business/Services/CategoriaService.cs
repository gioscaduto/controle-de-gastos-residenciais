using Controle.Gastos.Residenciais.Business.Entities;
using Controle.Gastos.Residenciais.Business.Entities.Validations;
using Controle.Gastos.Residenciais.Business.Interfaces;
using Controle.Gastos.Residenciais.Business.Interfaces.Repositories;
using Controle.Gastos.Residenciais.Business.Interfaces.Services;

namespace Controle.Gastos.Residenciais.Business.Services
{
    public class CategoriaService : BaseService, ICategoriaService
    {
        private const string MSG_CATEGORIA_MESMA_DESCRICAO = "Já existe uma categoria com esta descrição";
        private readonly ICategoriaRepository _categoriaRepository;

        public CategoriaService(ICategoriaRepository categoriaRepository,
                                INotificador notificador) : base(notificador)
        {
            _categoriaRepository = categoriaRepository;
        }

        public async Task Adicionar(Categoria categoria)
        {
            if (!ExecutarValidacao(new CategoriaValidation(), categoria)) return;

            if (await _categoriaRepository.Existe(c => c.Descricao.Trim() == categoria.Descricao.Trim()))
            {
                Notificar(MSG_CATEGORIA_MESMA_DESCRICAO);
                return;
            }

            await _categoriaRepository.Adicionar(categoria);
        }

        public async Task Atualizar(Categoria categoria)
        {
            if (!ExecutarValidacao(new CategoriaValidation(), categoria)) return;

            if (!await ExisteCategoria(categoria.Id)) return;

            if (await _categoriaRepository.Existe(c =>
                    c.Id != categoria.Id && c.Descricao.Trim() == categoria.Descricao.Trim()))
            {
                Notificar(MSG_CATEGORIA_MESMA_DESCRICAO);
                return;
            }

            await _categoriaRepository.Atualizar(categoria);
        }

        public async Task Remover(Guid id)
        {
            if (!await ExisteCategoria(id)) return;

            await _categoriaRepository.Remover(id);
        }

        private async Task<bool> ExisteCategoria(Guid id)
        {
            var existe = await _categoriaRepository.Existe(c => c.Id == id);

            if (!existe)
            {
                Notificar("Categoria não encontrada");
                return false;
            }

            return true;
        }

        public void Dispose()
        {
            _categoriaRepository?.Dispose();
        }
    }
}
