using Controle.Gastos.Residenciais.Business.Enums;

namespace Controle.Gastos.Residenciais.Business.Entities;

public class Categoria : Entidade
{
    public Categoria(string descricao, CategoriaFinalidade finalidade)
    {
        Descricao = descricao;
        Finalidade = finalidade;
        Validar();
    }

    // Usado pelo EF
    protected Categoria()
    {
    }

    public string Descricao { get; private set; } = null!;
    public CategoriaFinalidade Finalidade { get; private set; }

    public void Atualizar(string descricao, CategoriaFinalidade finalidade)
    {
        Descricao = descricao;
        Finalidade = finalidade;
        Validar();
    }

    public void Validar()
    {
        if (string.IsNullOrWhiteSpace(Descricao))
            throw new ArgumentNullException("A descrição da categoria é obrigatória.", nameof(Descricao));

        if (!Enum.IsDefined(typeof(CategoriaFinalidade), Finalidade))
            throw new ArgumentOutOfRangeException(nameof(Finalidade), "A finalidade da categoria é inválida.");
    }
}
