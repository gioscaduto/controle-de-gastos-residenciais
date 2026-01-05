using Controle.Gastos.Residenciais.Business.Enums;

namespace Controle.Gastos.Residenciais.Business.Entities;

public class Transacao : Entidade
{
    public Transacao(string descricao, decimal valor, Guid categoriaId, Guid pessoaId, TransacaoTipo tipo)
    {
        Descricao = descricao;
        Valor = valor;
        CategoriaId = categoriaId;
        PessoaId = pessoaId;
        Tipo = tipo;
        Validar();
    }

    // Usado pelo EF
    protected Transacao()
    {
    }

    public string Descricao { get; private set; } = null!;
    public decimal Valor { get; private set; }
    public Guid CategoriaId { get; private set; }
    public Categoria? Categoria { get; private set; }
    public Guid PessoaId { get; private set; }
    public Pessoa? Pessoa { get; private set; }
    public TransacaoTipo Tipo { get; private set; }

    public void Atualizar(string descricao, decimal valor, Guid categoriaId, Guid pessoaId, TransacaoTipo tipo)
    {
        Descricao = descricao;
        Valor = valor;
        CategoriaId = categoriaId;
        PessoaId = pessoaId;
        Tipo = tipo;
        Validar();
    }

    public void Validar()
    {
        if (string.IsNullOrWhiteSpace(Descricao))
            throw new ArgumentNullException("A descrição da transação é obrigatória.", nameof(Descricao));

        if (Valor <= 0)
            throw new ArgumentOutOfRangeException(nameof(Valor), "O valor da transação deve ser maior que zero.");

        if (CategoriaId == Guid.Empty)
            throw new ArgumentException("A categoria da transação é obrigatória.", nameof(CategoriaId));

        if (PessoaId == Guid.Empty)
            throw new ArgumentException("A pessoa da transação é obrigatória.", nameof(PessoaId));

        if (!Enum.IsDefined(typeof(TransacaoTipo), Tipo))
            throw new ArgumentOutOfRangeException(nameof(Tipo), "O tipo da transação é inválido.");
    }
}