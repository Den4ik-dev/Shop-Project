using FluentValidation;
using server.Domain.Dto;

namespace server.Infrastructure.Validation;
public class AddedDeliveryDtoValidator : AbstractValidator<AddedDeliveryDto>
{
  public AddedDeliveryDtoValidator()
  {
    RuleFor(d => d.ProductCount)
      .Must(pc => pc >= 1)
      .WithMessage("Такого количества товара не может быть");

    RuleFor(d => d.ProductId)
      .Must(pi => pi >= 1)
      .WithMessage("Такого идентификатора продукта не может быть");
  }
}