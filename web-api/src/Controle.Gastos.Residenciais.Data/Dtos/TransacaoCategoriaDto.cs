namespace Controle.Gastos.Residenciais.Data.Dtos;

public class TransacaoCategoriaDto
{
    public string Categoria { get; set; } = null!;
    public decimal TotalReceita { get; set; }
    public decimal TotalDespesa { get; set; }
    public decimal Saldo => TotalReceita - TotalDespesa;
}