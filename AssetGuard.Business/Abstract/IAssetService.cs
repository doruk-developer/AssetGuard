using AssetGuard.Entity;

namespace AssetGuard.Business.Abstract
{
    public interface IAssetService
    {
        void TAdd(Asset entity);
        void TDelete(Asset entity);
        void TUpdate(Asset entity);
        List<Asset> TGetAll();
        Asset TGetById(int id);

        // Dashboard Metotları
        decimal TGetTotalInventoryValue();
        int TGetTotalAssetCount();

        // --- Raporlama için Eklenecek Servisler ---
        List<Asset> TGetAssetsExpiringSoon(int days); // Garanti Raporu
        List<Asset> TGetAssetsByStatus(string statusName); // Arıza Raporu
        List<Asset> TGetAllWithDetails(); // Mali Rapor (ve Arıza için de lazım)

        string GenerateQrCode(string detailUrl);
    }
}