using FluentValidation;
using server.Domain.Dto;

namespace server.Infrastructure.Validation;
public class AddedBasketItemDtoValidator : AbstractValidator<AddedBasketItemDto>
{
  public AddedBasketItemDtoValidator()
  {
    RuleFor(bi => bi.ProductId)
      .Must(pi => pi > 0).WithMessage("Не может быть такого идентификатора продукта");

    RuleFor(bi => bi.ProductCount)
      .Must(pc => pc > 0).WithMessage("Не может быть такой стоимости продукта");
  }
}