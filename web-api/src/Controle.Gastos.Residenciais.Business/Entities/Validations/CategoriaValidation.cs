using FluentValidation;

namespace Controle.Gastos.Residenciais.Business.Entities.Validations;

public class CategoriaValidation : AbstractValidator<Categoria>
{
    public CategoriaValidation()
    {
        RuleFor(c => c.Descricao)
            .NotEmpty()
                .WithMessage("O campo {PropertyName} precisa ser fornecido")
            .Length(2, 255)
                .WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

        RuleFor(c => c.Finalidade)
            .NotNull()
                .WithMessage("O campo {PropertyName} precisa ser fornecido")
            .NotEmpty()
                .WithMessage("O campo {PropertyName} precisa ser fornecido");
    }
}