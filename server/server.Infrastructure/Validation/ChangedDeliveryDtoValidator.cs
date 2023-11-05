using FluentValidation;
using server.Domain.Dto;

namespace server.Infrastructure.Validation;
public class ChangedDeliveryDtoValidator : AbstractValidator<ChangedDeliveryDto>
{
  public ChangedDeliveryDtoValidator()
  {
    RuleFor(d => d.ProductCount)
      .Must(pc => pc >= 1)
      .WithMessage("Такого количества товара не может быть");
  }
}