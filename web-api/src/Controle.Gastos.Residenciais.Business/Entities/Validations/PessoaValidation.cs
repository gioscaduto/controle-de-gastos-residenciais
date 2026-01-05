using FluentValidation;

namespace Controle.Gastos.Residenciais.Business.Entities.Validations;

public class PessoaValidation : AbstractValidator<Pessoa>
{
    public PessoaValidation()
    {
        RuleFor(c => c.Nome)
            .NotEmpty()
                .WithMessage("O campo {PropertyName} precisa ser fornecido")
            .Length(2, 255)
                .WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

        RuleFor(c => c.Idade)
            .NotEmpty()
                .WithMessage("O campo {PropertyName} precisa ser fornecido")
            .InclusiveBetween(0, 999)
                .WithMessage("O campo {PropertyName} precisa estar entre {MinValue} e {MaxValue}");
    }
}