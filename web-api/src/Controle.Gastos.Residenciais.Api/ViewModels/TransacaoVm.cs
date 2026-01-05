using Controle.Gastos.Residenciais.Business.Entities;
using Controle.Gastos.Residenciais.Business.Enums;
using System.ComponentModel.DataAnnotations;

namespace Controle.Gastos.Residenciais.Api.ViewModels
{
    public class TransacaoVm
    {
        public TransacaoVm()
        {
        }

        public TransacaoVm(Transacao transacao)
        {
            Id = transacao.Id;
            Descricao = transacao.Descricao;
            Valor = transacao.Valor;
            CategoriaId = transacao.CategoriaId;
            PessoaId = transacao.PessoaId;
            Tipo = transacao.Tipo;
        }

        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(1000, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string Descricao { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue, ErrorMessage = "O campo {0} precisa ser maior que zero")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public Guid CategoriaId { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public Guid PessoaId { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public TransacaoTipo Tipo { get; set; }

        public Transacao Map() => new(Descricao, Valor, CategoriaId, PessoaId, Tipo);
    }
}
