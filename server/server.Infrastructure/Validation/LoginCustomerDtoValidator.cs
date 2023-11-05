using FluentValidation;
using server.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Infrastructure.Validation;
public class LoginCustomerDtoValidator : AbstractValidator<LoginCustomerDto>
{
  public LoginCustomerDtoValidator()
  {
    RuleFor(c => c.Email)
      .NotNull()
      .NotEmpty()
      .WithMessage("Вы не указали email");

    RuleFor(c => c.Password)
      .NotNull()
      .NotEmpty()
      .WithMessage("Вы не указали пароль");
  }
}