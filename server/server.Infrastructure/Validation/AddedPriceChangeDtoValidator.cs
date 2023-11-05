using FluentValidation;
using server.Domain.Dto;

namespace server.Infrastructure.Validation;
public class AddedPriceChangeDtoValidator : AbstractValidator<AddedPriceChangeDto>
{
  public AddedPriceChangeDtoValidator()
  {
    RuleFor(pch => pch.NewPrice)
      .Must(np => np >= 1)
      .WithMessage("Такой цены не может быть");

    RuleFor(pch => pch.ProductId)
      .Must(pi => pi >= 1)
      .WithMessage("Такого идентификатора продукта не может быть");
  }
}