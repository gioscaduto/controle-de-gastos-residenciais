using Controle.Gastos.Residenciais.Business.Entities;
using System.ComponentModel.DataAnnotations;

namespace Controle.Gastos.Residenciais.Api.ViewModels
{
    public class PessoaVm
    {
        public PessoaVm()
        {   
        }
        public PessoaVm(Pessoa pessoa)
        {
            Id = pessoa.Id;
            Nome = pessoa.Nome;
            Idade = pessoa.Idade;
        }

        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(255, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string Nome { get; set; }

        [Range(0, 999, ErrorMessage = "O campo {0} precisa estar entre {1} e {2}")]
        public int Idade { get; set; }

        public Pessoa Map() => new(Nome, Idade);

        public void Atualizar(Pessoa pessoa) =>
            pessoa.Atualizar(Nome, Idade);
    }
}
