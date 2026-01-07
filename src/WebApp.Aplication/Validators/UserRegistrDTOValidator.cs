using FluentValidation;
using WebApp.Aplication.Models.Users;

namespace WebApp.Aplication.Validators
{
    public class UserRegistrDTOValidator : AbstractValidator<UserRegistrDTO>
    {
        public UserRegistrDTOValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Ism kiritilishi shart")
                .MaximumLength(100).WithMessage("Ism 100 ta belgidan oshmasligi kerak");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Telefon raqam kiritilishi shart")
                .Matches(@"^\+?[0-9]{9,15}$").WithMessage("Telefon raqam formati noto'g'ri");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email kiritilishi shart")
                .EmailAddress().WithMessage("Email formati noto'g'ri");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Parol kiritilishi shart")
                .MinimumLength(6).WithMessage("Parol kamida 6 ta belgidan iborat bo'lishi kerak");
        }
    }
}