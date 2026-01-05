using FluentValidation;

namespace Controle.Gastos.Residenciais.Business.Entities.Validations;

public class TransacaoValidation : AbstractValidator<Transacao>
{
    public TransacaoValidation()
    {
        RuleFor(c => c.Descricao)
            .NotEmpty()
                .WithMessage("O campo {PropertyName} precisa ser fornecido")
            .Length(2, 1000)
                .WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

        RuleFor(c => c.Valor)
            .NotNull()
                .WithMessage("O campo {PropertyName} precisa ser fornecido")
            .GreaterThan(0)
                .WithMessage("O campo {PropertyName} precisa ser maior que {ComparisonValue}");

        RuleFor(c => c.Tipo)
           .NotNull()
               .WithMessage("O campo {PropertyName} precisa ser fornecido")
           .NotEmpty()
               .WithMessage("O campo {PropertyName} precisa ser fornecido");

        RuleFor(c => c.CategoriaId)
           .NotNull()
               .WithMessage("O campo {PropertyName} precisa ser fornecido")
           .NotEmpty()
               .WithMessage("O campo {PropertyName} precisa ser fornecido");

        RuleFor(c => c.PessoaId)
           .NotNull()
               .WithMessage("O campo {PropertyName} precisa ser fornecido")
           .NotEmpty()
               .WithMessage("O campo {PropertyName} precisa ser fornecido");
    }
}