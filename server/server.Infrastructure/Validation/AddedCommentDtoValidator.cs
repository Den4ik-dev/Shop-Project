using FluentValidation;
using server.Domain.Dto;

namespace server.Infrastructure.Validation;
public class AddedCommentDtoValidator : AbstractValidator<AddedCommentDto>
{
  public AddedCommentDtoValidator()
  {
    RuleFor(c => c.Content)
      .NotNull()
      .NotEmpty()
      .WithMessage("Вы не указали содежимое комментария");

    RuleFor(c => c.ProductId)
      .Must(pi => pi >= 1)
      .WithMessage("Такого идентификатора продукта не может быть");
  }
}