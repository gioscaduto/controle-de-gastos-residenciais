using Controle.Gastos.Residenciais.Business.Entities;
using Controle.Gastos.Residenciais.Business.Enums;
using System.ComponentModel.DataAnnotations;

namespace Controle.Gastos.Residenciais.Api.ViewModels
{
    public class CategoriaVm
    {
        public CategoriaVm()
        {
        }

        public CategoriaVm(Categoria categoria)
        {
            Id = categoria.Id;
            Descricao = categoria.Descricao;
            Finalidade = categoria.Finalidade;
        }

        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(255, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string Descricao { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public CategoriaFinalidade Finalidade { get; set; }

        public Categoria Map() => new(Descricao, Finalidade);

        public void Atualizar(Categoria categoria) =>
            categoria.Atualizar(Descricao, Finalidade);
    }
}
