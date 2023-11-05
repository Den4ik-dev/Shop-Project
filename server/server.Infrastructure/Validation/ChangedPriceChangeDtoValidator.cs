using FluentValidation;
using server.Domain.Dto;

namespace server.Infrastructure.Validation;
public class ChangedPriceChangeDtoValidator : AbstractValidator<ChangedPriceChangeDto>
{
  public ChangedPriceChangeDtoValidator()
  {
    RuleFor(pch => pch.NewPrice)
      .Must(np => np >= 1)
      .WithMessage("Такой цены не может быть");

    RuleFor(pch => pch.ProductId)
      .Must(np => np >= 1)
      .WithMessage("Такого идентификатора продукта не может быть");
  }
}