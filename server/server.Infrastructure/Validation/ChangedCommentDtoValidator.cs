using FluentValidation;
using server.Domain.Dto;

namespace server.Infrastructure.Validation;
public class ChangedCommentDtoValidator : AbstractValidator<ChangedCommentDto>
{
  public ChangedCommentDtoValidator()
  {
    RuleFor(c => c.Content)
      .NotNull()
      .NotEmpty()
      .WithMessage("Вы не указали содежимое комментария");
  }
}