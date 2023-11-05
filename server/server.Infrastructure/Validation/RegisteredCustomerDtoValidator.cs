using FluentValidation;
using server.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Infrastructure.Validation;
public class RegisteredCustomerDtoValidator : AbstractValidator<RegisteredCustomerDto>
{
  public RegisteredCustomerDtoValidator()
  {
    RuleFor(c => c.FirstName)
      .NotNull()
      .NotEmpty()
      .WithMessage("Вы не указали имя");

    RuleFor(c => c.LastName)
      .NotNull()
      .NotEmpty()
      .WithMessage("Вы не указали фамилию");

    RuleFor(c => c.BirthDate)
      .Must(d => !d.Equals(default(DateTime))).WithMessage("Вы не указали дату своего рождения")
      .Must(d => d <= DateTime.Now).WithMessage("Некорректная дата");

    RuleFor(c => c.Phone)
      .NotNull()
      .NotEmpty()
      .WithMessage("Вы не указали номер телефона")
      .Matches(@"^((\+7)\(\d{3}\)-\d{3}(-\d{2}){2})$")
      .WithMessage("Некорректный номер телефона");

    RuleFor(c => c.Address)
      .NotNull()
      .NotEmpty()
      .WithMessage("Вы не указали свой адрес");

    RuleFor(c => c.City)
      .NotNull()
      .NotEmpty()
      .WithMessage("Вы не указали город своего проживания");

    RuleFor(c => c.Email)
      .NotNull()
      .NotEmpty()
      .WithMessage("Вы не указали свой email")
      .EmailAddress()
      .WithMessage("Некорректный email");

    RuleFor(c => c.Password)
      .NotNull()
      .NotEmpty()
      .WithMessage("Вы не указали свой пароль")
      .MinimumLength(8)
      .WithMessage("Пароль должен состоять не менее чем из 8 символов");
  }
}