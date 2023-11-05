using FluentValidation;
using server.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Infrastructure.Validation;

public class TokenDtoValidator : AbstractValidator<TokenDto>
{
  public TokenDtoValidator()
  {
    RuleFor(t => t.AccessToken)
      .NotNull()
      .NotEmpty();

    RuleFor(t => t.RefreshToken)
      .NotNull()
      .NotEmpty();
  }
}
