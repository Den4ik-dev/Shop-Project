using FluentValidation;
using server.Domain.Dto;

namespace server.Infrastructure.Validation;
public class ChangedProductDtoValidator : AbstractValidator<ChangedProductDto>
{
  public ChangedProductDtoValidator()
  {
    RuleFor(p => p.ProductName)
      .NotNull()
      .NotEmpty()
      .WithMessage("Вы не указали название продукта");

    RuleFor(p => p.ProductDescription)
      .NotNull()
      .NotEmpty()
      .WithMessage("Вы не указали описание продукта");

    RuleFor(p => p.UnitPrice)
      .Must(up => up > 0)
      .WithMessage("Вы некорректно указали цену продукта");

    RuleFor(p => p.QuantityInStoke)
      .Must(qis => qis > 0)
      .WithMessage("Вы некорректно указали количество продукта на складе");

    RuleFor(p => p.CategoryId)
      .Must(ci => ci > 0)
      .WithMessage("Вы некорректно указали идентификатор категории");

    RuleFor(p => p.ManufacturerId)
      .Must(mi => mi > 0)
      .WithMessage("Вы некорректно указали идентификатор производителя");
  }
}
