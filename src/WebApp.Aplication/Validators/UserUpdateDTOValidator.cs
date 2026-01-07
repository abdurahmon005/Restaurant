using FluentValidation;
using WebApp.Aplication.Models.Users;

namespace WebApp.Aplication.Validators
{
    public class UserUpdateDTOValidator : AbstractValidator<UserUpdateDTO>
    {
        public UserUpdateDTOValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(100).WithMessage("Ism 100 ta belgidan oshmasligi kerak")
                .When(x => !string.IsNullOrEmpty(x.Name));

            RuleFor(x => x.Phone)
                .Matches(@"^\+?[0-9]{9,15}$").WithMessage("Telefon raqam formati noto'g'ri")
                .When(x => !string.IsNullOrEmpty(x.Phone));

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Email formati noto'g'ri")
                .When(x => !string.IsNullOrEmpty(x.Email));
        }
    }
}