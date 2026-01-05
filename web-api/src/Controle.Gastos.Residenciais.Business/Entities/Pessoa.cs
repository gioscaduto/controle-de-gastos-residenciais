namespace Controle.Gastos.Residenciais.Business.Entities;

public class Pessoa : Entidade
{
    public Pessoa(string nome, int idade)
    {
        Nome = nome;
        Idade = idade;
        Validar();
    }

    // Usado pelo EF
    protected Pessoa()
    {
    }

    public string Nome { get; private set; } = null!;
    public int Idade { get; private set; }

    public void Atualizar(string nome, int idade)
    {
        Nome = nome;
        Idade = idade;
        Validar();
    }

    public void Validar()
    {
        if (string.IsNullOrWhiteSpace(Nome))
            throw new ArgumentNullException("O nome da pessoa é obrigatório.", nameof(Nome));

        if (Idade < 0)
            throw new ArgumentOutOfRangeException("A idade da pessoa não pode ser negativa.", nameof(Idade));
    }
}
