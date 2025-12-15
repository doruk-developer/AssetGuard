ASSETGUARD PROJE NAVIGASYON HARİTASI 
====================================

Bu dosya, Sidebar menüsündeki butonların hangi Controller ve Action'ı tetiklediğini gösterir.
Metinler, ekrandaki Sidebar menüsü ile NOKTASI VİRGÜLÜNE eşleşmektedir.

VARLIK (ENTITY)      CONTROLLER               ACTION (METOT)    SIDEBAR METNİ (GÖRÜNEN)
-------------------  -----------------------  ----------------  ---------------------------
Home                 HomeController           Index()           Genel Bakış

Asset                AssetController          Index()           Tüm Demirbaşlar
Asset                AssetController          Create()          Yeni Demirbaş Ekle
Category             CategoryController       Index()           Kategoriler

Assignment           AssignmentController     Create()          Zimmet Ata
Assignment           AssignmentController     Index()           Geçmiş Hareketler

Employee             EmployeeController       Index()           Personel Listesi
Department           DepartmentController     Index()           Departmanlar
Report               ReportController         Index()           Raporlar

Settings             SettingsController       Index()           Ayarlar