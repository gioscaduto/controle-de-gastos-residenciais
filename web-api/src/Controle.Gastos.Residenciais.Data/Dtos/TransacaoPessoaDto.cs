namespace Controle.Gastos.Residenciais.Data.Dtos;

public class TransacaoPessoaDto
{
    public string Pessoa { get; set; } = null!;
    public decimal TotalReceita { get; set; }
    public decimal TotalDespesa { get; set; }
    public decimal Saldo => TotalReceita - TotalDespesa;
}
