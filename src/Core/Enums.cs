using System;

namespace EasyInvoicePro.Core
{
    /// <summary>
    /// Uygulamanın enum tanımlamaları
    /// </summary>
    
    /// <summary>
    /// Fatura türü enumu (UBL-TR standartlarına uygun)
    /// </summary>
    public enum InvoiceTypeEnum
    {
        /// <summary>Satış Faturası (Code: 380)</summary>
        Sales = 380,
        
        /// <summary>İade Faturası (Code: 381)</summary>
        Return = 381,
        
        /// <summary>Proforma Faturası (Code: 380)</summary>
        Proforma = 380,
        
        /// <summary>Depo Faturası</summary>
        Warehouse = 380
    }

    /// <summary>
    /// Ödeme durumu enumu
    /// </summary>
    public enum PaymentStatusEnum
    {
        /// <summary>Beklemede (Ödenmeyen)</summary>
        Pending = 0,
        
        /// <summary>Kısmi Ödendi</summary>
        Partial = 1,
        
        /// <summary>Tam Ödendi</summary>
        Paid = 2,
        
        /// <summary>Vadesi Geçmiş</summary>
        Overdue = 3,
        
        /// <summary>İptal Edildi</summary>
        Cancelled = 4
    }

    /// <summary>
    /// E-Fatura durum enumu
    /// </summary>
    public enum eFaturaStatusEnum
    {
        /// <summary>Taslak (Henüz gönderilmemiş)</summary>
        Draft = 0,
        
        /// <summary>Gönderilmeyi Bekliyor</summary>
        PendingSend = 1,
        
        /// <summary>Gönderildi (Başarılı)</summary>
        Sent = 2,
        
        /// <summary>Reddedildi (Hata)</summary>
        Rejected = 3,
        
        /// <summary>Gönderme Hatası</summary>
        SendError = 4,
        
        /// <summary>Cevap Bekleniyor</summary>
        PendingResponse = 5
    }

    /// <summary>
    /// KDV hızı enumu (Türkiye standard oranları)
    /// </summary>
    public enum KDVRateEnum
    {
        /// <summary>Muafiyetli (%0)</summary>
        Exempt = 0,
        
        /// <summary>İndirimli - %1</summary>
        Rate1 = 1,
        
        /// <summary>İndirimli - %5</summary>
        Rate5 = 5,
        
        /// <summary>İndirimli - %10</summary>
        Rate10 = 10,
        
        /// <summary>Standart - %20</summary>
        Rate20 = 20
    }

    /// <summary>
    /// KDV beyanname türü enumu (GİB standartlarına uygun)
    /// </summary>
    public enum KDVDeclarationTypeEnum
    {
        /// <summary>KDV-1 Cetveli (Standart KDV)</summary>
        KDV1 = 1,
        
        /// <summary>KDV-2 Cetveli (Gümrük)</summary>
        KDV2 = 2,
        
        /// <summary>KDV-3 Cetveli (Muafiyetli)</summary>
        KDV3 = 3,
        
        /// <summary>KDV-4 Cetveli (Özel Muhasebeleştirme)</summary>
        KDV4 = 4
    }

    /// <summary>
    /// KDV dönem enumu (Aylar)
    /// </summary>
    public enum KDVPeriodEnum
    {
        January = 1,
        February = 2,
        March = 3,
        April = 4,
        May = 5,
        June = 6,
        July = 7,
        August = 8,
        September = 9,
        October = 10,
        November = 11,
        December = 12
    }

    /// <summary>
    /// Lisans seviyesi enumu (Ücretsiz, Ücretli vb.)
    /// </summary>
    public enum LicenseLevelEnum
    {
        /// <summary>Demo (30 gün, Max 50 fatura/ay)</summary>
        Demo = 0,
        
        /// <summary>Profesyonel (Max 500 fatura/ay, 499₺)</summary>
        Professional = 1,
        
        /// <summary>Kurumsal (Limitsiz, 999₺)</summary>
        Enterprise = 2,
        
        /// <summary>Geliştirici (Test Lisansı)</summary>
        Developer = 3
    }

    /// <summary>
    /// Uygulamanın çalıştığı ortam enumu
    /// </summary>
    public enum EnvironmentEnum
    {
        /// <summary>Test/Geliştirme Ortamı</summary>
        Development = 0,
        
        /// <summary>Test Ortamı (E-Fatura Test Server)</summary>
        Test = 1,
        
        /// <summary>Üretim Ortamı (E-Fatura Production)</summary>
        Production = 2
    }

    /// <summary>
    /// Müşteri türü enumu
    /// </summary>
    public enum CustomerTypeEnum
    {
        /// <summary>Bireysel (TCKN ile)</summary>
        Individual = 0,
        
        /// <summary>Kurumsal (VKN ile)</summary>
        Corporate = 1
    }

    /// <summary>
    /// Müşteri risk durumu enumu
    /// </summary>
    public enum CustomerRiskEnum
    {
        /// <summary>Düşük Risk (Yeşil)</summary>
        Low = 0,
        
        /// <summary>Orta Risk (Sarı)</summary>
        Medium = 1,
        
        /// <summary>Yüksek Risk (Kırmızı) - Vadesi geçmiş</summary>
        High = 2
    }

    /// <summary>
    /// Ödeme yöntemi enumu
    /// </summary>
    public enum PaymentMethodEnum
    {
        /// <summary>Nakit</summary>
        Cash = 0,
        
        /// <summary>Kredi Kartı</summary>
        CreditCard = 1,
        
        /// <summary>Banka Transferi</summary>
        BankTransfer = 2,
        
        /// <summary>Çek</summary>
        Check = 3,
        
        /// <summary>Senet</summary>
        Promissory = 4,
        
        /// <summary>Diğer</summary>
        Other = 5
    }

    /// <summary>
    /// Tema enumu
    /// </summary>
    public enum ThemeEnum
    {
        /// <summary>Açık Tema</summary>
        Light = 0,
        
        /// <summary>Koyu Tema</summary>
        Dark = 1,
        
        /// <summary>Otomatik (Sistem Ayarlarını Takip)</summary>
        Auto = 2
    }

    /// <summary>
    /// Dil enumu
    /// </summary>
    public enum LanguageEnum
    {
        /// <summary>Türkçe</summary>
        Turkish = 0,
        
        /// <summary>İngilizce</summary>
        English = 1
    }

    /// <summary>
    /// Rapor türü enumu
    /// </summary>
    public enum ReportTypeEnum
    {
        /// <summary>Satış Raporu</summary>
        Sales = 0,
        
        /// <summary>Müşteri Raporu</summary>
        Customer = 1,
        
        /// <summary>Ürün Raporu</summary>
        Product = 2,
        
        /// <summary>KDV Raporu</summary>
        KDV = 3,
        
        /// <summary>Cari Raporu</summary>
        Receivable = 4,
        
        /// <summary>E-Fatura Raporu</summary>
        eFatura = 5
    }

    /// <summary>
    /// Rapor formatı enumu
    /// </summary>
    public enum ReportFormatEnum
    {
        /// <summary>PDF Formatı</summary>
        PDF = 0,
        
        /// <summary>Excel Formatı</summary>
        Excel = 1,
        
        /// <summary>CSV Formatı</summary>
        CSV = 2,
        
        /// <summary>Yazdırma Önizlemesi</summary>
        Print = 3
    }

    /// <summary>
    /// İşlem türü enumu (Logging/Audit için)
    /// </summary>
    public enum AuditActionEnum
    {
        /// <summary>Oluştur</summary>
        Create = 0,
        
        /// <summary>Güncelle</summary>
        Update = 1,
        
        /// <summary>Sil</summary>
        Delete = 2,
        
        /// <summary>Giriş Yap</summary>
        Login = 3,
        
        /// <summary>Çıkış Yap</summary>
        Logout = 4,
        
        /// <summary>Fatura Gönder</summary>
        SendInvoice = 5,
        
        /// <summary>Rapor İndir</summary>
        ExportReport = 6
    }

    /// <summary>
    /// Sertifika durumu enumu
    /// </summary>
    public enum CertificateStatusEnum
    {
        /// <summary>Geçerli</summary>
        Valid = 0,
        
        /// <summary>Süresi Yaklaşıyor (30 gün içinde)</summary>
        Expiring = 1,
        
        /// <summary>Süresi Dolmuş</summary>
        Expired = 2,
        
        /// <summary>Yüklenememedi (Hata)</summary>
        LoadError = 3
    }

    /// <summary>
    /// Sıralama türü enumu
    /// </summary>
    public enum SortOrderEnum
    {
        /// <summary>Artan Sıra</summary>
        Ascending = 0,
        
        /// <summary>Azalan Sıra</summary>
        Descending = 1
    }
}
