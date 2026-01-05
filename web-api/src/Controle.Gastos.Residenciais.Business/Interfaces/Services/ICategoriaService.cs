using Controle.Gastos.Residenciais.Business.Entities;

namespace Controle.Gastos.Residenciais.Business.Interfaces.Services
{
    public interface ICategoriaService : IDisposable
    {
        Task Adicionar(Categoria categoria);
        Task Atualizar(Categoria categoria);
        Task Remover(Guid id);
    }
}
