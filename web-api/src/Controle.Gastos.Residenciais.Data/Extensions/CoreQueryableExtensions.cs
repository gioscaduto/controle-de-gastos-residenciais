using Microsoft.EntityFrameworkCore;

namespace Controle.Gastos.Residenciais.Data.Extensions;

public static class CoreQueryableExtensions
{
    public static async Task<IReadOnlyList<T>> Paginar<T>(this IQueryable<T> queryable, 
        int pagina, int qtdItensPorPagina)
    {
        pagina = Math.Max(1, pagina);
        qtdItensPorPagina = Math.Max(1, qtdItensPorPagina);

        return await
            queryable
           .Skip((pagina - 1) * qtdItensPorPagina)
           .Take(qtdItensPorPagina)
           .ToListAsync();
    }
}
