using AssetGuard.Entity;
using FluentValidation;

namespace AssetGuard.Business.ValidationRules
{
    public class EmployeeValidator : AbstractValidator<Employee>
    {
        public EmployeeValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Personel adı boş geçilemez.")
                .MinimumLength(2).WithMessage("Ad en az 2 karakter olmalıdır.");

            // Soyad için Minimum 2 karakter kuralı
            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Lütfen personel soyadını giriniz.")
                .MinimumLength(2).WithMessage("Soyad çok kısa (En az 2 harf olmalı).");

            //  Mail formatı ve Şirket Domain kontrolü
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-posta adresi zorunludur.")
                .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.")
                // KURAL: Sadece şirket maili kabul edilsin
                .Must(email => email != null && email.EndsWith("@assetguard.com"))
                .WithMessage("Sadece kurumsal şirket maili (@assetguard.com) kullanılabilir.");


        }
    }
}