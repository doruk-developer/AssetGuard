using AssetGuard.Entity;
using FluentValidation;

namespace AssetGuard.Business.ValidationRules
{
    public class AssetValidator : AbstractValidator<Asset>
    {
        public AssetValidator()
        {
            // 1. Ürün Adı Kuralları
            RuleFor(x => x.AssetName)
                .NotEmpty().WithMessage("Ürün adı boş bırakılamaz.")
                .MinimumLength(3).WithMessage("Ürün adı en az 3 karakter olmalıdır.")
                .MaximumLength(100).WithMessage("Ürün adı 100 karakteri geçemez.");

            // 2. Seri Numarası Kuralları
            RuleFor(x => x.SerialNo)
                .NotEmpty().WithMessage("Seri numarası zorunludur.");

            // 3. Fiyat Kuralları
            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Fiyat 0'dan büyük olmalıdır.");

            // 4. Tarih Mantık Kontrolü
            RuleFor(x => x.WarrantyEndDate)
                .GreaterThanOrEqualTo(x => x.PurchaseDate)
                .When(x => x.WarrantyEndDate != null)
                .WithMessage("Garanti bitiş tarihi, satın alma tarihinden daha eski olamaz.");
        }
    }
}