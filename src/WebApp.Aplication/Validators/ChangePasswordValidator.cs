using FluentValidation;
using WebApp.Aplication.Models;
using WebApp.Aplication.Models.Users; // или WebApp.Aplication.Models.Users - в зависимости от того, где находится ChangePassword

namespace WebApp.Aplication.Validators
{
    public class ChangePasswordValidator : AbstractValidator<ChangePassword>
    {
        public ChangePasswordValidator()
        {
            RuleFor(x => x.OldPassword)
                .NotEmpty().WithMessage("Eski parolni kiriting");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("Yangi parolni kiriting")
                .MinimumLength(6).WithMessage("Yangi parol kamida 6 ta belgidan iborat bo'lishi kerak")
                .NotEqual(x => x.OldPassword).WithMessage("Yangi parol eski paroldan farq qilishi kerak");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Parolni tasdiqlang")
                .Equal(x => x.NewPassword).WithMessage("Parollar mos kelmaydi");
        }
    }
}