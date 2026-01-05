using Controle.Gastos.Residenciais.Business.Enums;
using Controle.Gastos.Residenciais.Data.Context;
using Controle.Gastos.Residenciais.Data.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Controle.Gastos.Residenciais.Data.Queries;

public class TransacaoQuery : ITransacaoQuery
{
    private readonly MeuDbContext _context;

    public TransacaoQuery(MeuDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TransacaoCategoriaDto>> ListarPorCategoria() =>
        await _context.Transacoes
            .GroupBy(t => t.Categoria)
            .Select(g => new TransacaoCategoriaDto
            {
                Categoria = g.Key.Descricao,
                TotalReceita = g.Sum(t => t.Tipo == TransacaoTipo.Receita ? t.Valor : 0),
                TotalDespesa = g.Sum(t => t.Tipo == TransacaoTipo.Despesa ? t.Valor : 0)
            })
            .OrderBy(tc => tc.Categoria)
            .ToListAsync();

    public async Task<IEnumerable<TransacaoPessoaDto>> ListarPorPessoa() =>
        await _context.Transacoes
            .GroupBy(t => t.Pessoa)
            .Select(g => new TransacaoPessoaDto
            {
                Pessoa = g.Key.Nome,
                TotalReceita = g.Sum(t => t.Tipo == TransacaoTipo.Receita ? t.Valor : 0),
                TotalDespesa = g.Sum(t => t.Tipo == TransacaoTipo.Despesa ? t.Valor : 0)
            })
            .OrderBy(tc => tc.Pessoa)
            .ToListAsync();
}