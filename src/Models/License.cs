using System;

namespace EasyInvoicePro.Models
{
    /// <summary>
    /// Lisans bilgilerini temsil eden model
    /// </summary>
    public class License
    {
        /// <summary>Lisans ID</summary>
        public int Id { get; set; }

        /// <summary>Lisans Anahtarı (Benzersiz)</summary>
        public string LicenseKey { get; set; }

        /// <summary>Makine ID (Hardware ID)</summary>
        public string MachineId { get; set; }

        /// <summary>Lisans Seviyesi</summary>
        public LicenseLevelEnum LicenseLevel { get; set; } = LicenseLevelEnum.Demo;

        /// <summary>Etkinleştirme Tarihi</summary>
        public DateTime? ActivationDate { get; set; }

        /// <summary>Son Kullanma Tarihi</summary>
        public DateTime? ExpirationDate { get; set; }

        /// <summary>Etkin mi</summary>
        public bool IsActive { get; set; } = true;

        /// <summary>Sahip Adı</summary>
        public string OwnerName { get; set; }

        /// <summary>Sahip E-Postası</summary>
        public string OwnerEmail { get; set; }

        /// <summary>Şirket Adı</summary>
        public string CompanyName { get; set; }

        /// <summary>Oluşturulma Tarihi</summary>
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        /// <summary>Son Doğrulama Tarihi</summary>
        public DateTime? LastValidationDate { get; set; }

        /// <summary>Notlar</summary>
        public string Notes { get; set; }
    }

    /// <summary>
    /// KDV Beyannamesi bilgilerini temsil eden model
    /// </summary>
    public class KDVDeclaration
    {
        /// <summary>Beyanname ID</summary>
        public int Id { get; set; }

        /// <summary>Şirket ID (Foreign Key)</summary>
        public int CompanyId { get; set; }

        /// <summary>Beyanname Yılı</summary>
        public int Year { get; set; }

        /// <summary>Beyanname Ayı</summary>
        public int Month { get; set; }

        /// <summary>KDV-1 Çetveli Matrahı (%20)</summary>
        public decimal KDV1_20_Basis { get; set; } = 0m;

        /// <summary>KDV-1 Çetveli KDV (%20)</summary>
        public decimal KDV1_20_Tax { get; set; } = 0m;

        /// <summary>KDV-1 Çetveli Matrahı (%10)</summary>
        public decimal KDV1_10_Basis { get; set; } = 0m;

        /// <summary>KDV-1 Çetveli KDV (%10)</summary>
        public decimal KDV1_10_Tax { get; set; } = 0m;

        /// <summary>KDV-1 Çetveli Matrahı (%5)</summary>
        public decimal KDV1_5_Basis { get; set; } = 0m;

        /// <summary>KDV-1 Çetveli KDV (%5)</summary>
        public decimal KDV1_5_Tax { get; set; } = 0m;

        /// <summary>KDV-1 Çetveli Matrahı (%1)</summary>
        public decimal KDV1_1_Basis { get; set; } = 0m;

        /// <summary>KDV-1 Çetveli KDV (%1)</summary>
        public decimal KDV1_1_Tax { get; set; } = 0m;

        /// <summary>İndirimli Satışlar</summary>
        public decimal DiscountedSales { get; set; } = 0m;

        /// <summary>Toplam KDV</summary>
        public decimal TotalKDV { get; set; } = 0m;

        /// <summary>Dönem Başlangıç Tarihi</summary>
        public DateTime PeriodStartDate { get; set; }

        /// <summary>Dönem Bitiş Tarihi</summary>
        public DateTime PeriodEndDate { get; set; }

        /// <summary>Hesaplama Tarihi</summary>
        public DateTime CalculationDate { get; set; } = DateTime.Now;

        /// <summary>Beyanname Yapıldı mı</summary>
        public bool IsSubmitted { get; set; } = false;

        /// <summary>Beyanname Gönderme Tarihi</summary>
        public DateTime? SubmissionDate { get; set; }

        /// <summary>Notlar</summary>
        public string Notes { get; set; }
    }

    /// <summary>
    /// Sertifika bilgilerini temsil eden model
    /// </summary>
    public class Certificate
    {
        /// <summary>Sertifika ID</summary>
        public int Id { get; set; }

        /// <summary>Şirket ID (Foreign Key)</summary>
        public int CompanyId { get; set; }

        /// <summary>Sertifika Dosya Adı</summary>
        public string FileName { get; set; }

        /// <summary>Sertifika Dosya Yolu</summary>
        public string FilePath { get; set; }

        /// <summary>Sertifika Konu Adı (CN - Common Name)</summary>
        public string SubjectName { get; set; }

        /// <summary>Sertifika Veren (Issuer)</summary>
        public string Issuer { get; set; }

        /// <summary>Seri Numarası</summary>
        public string SerialNumber { get; set; }

        /// <summary>Parmak İzi (Thumbprint)</summary>
        public string Thumbprint { get; set; }

        /// <summary>Geçerlilik Başlangıç Tarihi</summary>
        public DateTime ValidFrom { get; set; }

        /// <summary>Geçerlilik Bitiş Tarihi</summary>
        public DateTime ValidTo { get; set; }

        /// <summary>Sertifika Durumu</summary>
        public CertificateStatusEnum Status { get; set; } = CertificateStatusEnum.Valid;

        /// <summary>Yükleme Tarihi</summary>
        public DateTime UploadDate { get; set; } = DateTime.Now;

        /// <summary>Etkin mi (Kullanılıyor mu)</summary>
        public bool IsActive { get; set; } = true;

        /// <summary>Notlar</summary>
        public string Notes { get; set; }
    }

    /// <summary>
    /// E-Fatura gönderimi kaydını temsil eden model
    /// </summary>
    public class eFaturaLog
    {
        /// <summary>Log ID</summary>
        public int Id { get; set; }

        /// <summary>Fatura ID (Foreign Key)</summary>
        public int InvoiceId { get; set; }

        /// <summary>Gönderme Tarihi</summary>
        public DateTime SendDate { get; set; } = DateTime.Now;

        /// <summary>İçerik Referans Numarası (UUID)</summary>
        public string UUID { get; set; }

        /// <summary>Gönderme Durumu</summary>
        public eFaturaStatusEnum Status { get; set; } = eFaturaStatusEnum.Draft;

        /// <summary>Yanıt Kodu</summary>
        public string ResponseCode { get; set; }

        /// <summary>Yanıt Mesajı</summary>
        public string ResponseMessage { get; set; }

        /// <summary>XML İçeriği</summary>
        public string XMLContent { get; set; }

        /// <summary>Alıcı E-Postası</summary>
        public string ReceiverEmail { get; set; }

        /// <summary>Notlar</summary>
        public string Notes { get; set; }
    }

    /// <summary>
    /// Yedekleme kaydını temsil eden model
    /// </summary>
    public class BackupLog
    {
        /// <summary>Yedekleme Log ID</summary>
        public int Id { get; set; }

        /// <summary>Yedekleme Tarihi</summary>
        public DateTime BackupDate { get; set; } = DateTime.Now;

        /// <summary>Yedekleme Dosya Adı</summary>
        public string FileName { get; set; }

        /// <summary>Yedekleme Dosya Yolu</summary>
        public string FilePath { get; set; }

        /// <summary>Yedekleme Boyutu (Byte)</summary>
        public long FileSize { get; set; }

        /// <summary>Yedekleme Türü (Otomatik/Manuel)</summary>
        public string BackupType { get; set; }

        /// <summary>Başarılı mı</summary>
        public bool IsSuccessful { get; set; } = true;

        /// <summary>Hata Mesajı (Varsa)</summary>
        public string ErrorMessage { get; set; }

        /// <summary>Notlar</summary>
        public string Notes { get; set; }
    }

    /// <summary>
    /// Denetim Günlüğü (Audit Log) - Tüm işlemleri takip eder
    /// </summary>
    public class AuditLog
    {
        /// <summary>Log ID</summary>
        public int Id { get; set; }

        /// <summary>Şirket ID</summary>
        public int? CompanyId { get; set; }

        /// <summary>İşlem Tarihi</summary>
        public DateTime ActionDate { get; set; } = DateTime.Now;

        /// <summary>İşlem Türü</summary>
        public AuditActionEnum ActionType { get; set; }

        /// <summary>Etkilenen Tablo Adı</summary>
        public string TableName { get; set; }

        /// <summary>Etkilenen Kayıt ID</summary>
        public int? RecordId { get; set; }

        /// <summary>Açıklama</summary>
        public string Description { get; set; }

        /// <summary>Eski Değer (JSON)</summary>
        public string OldValue { get; set; }

        /// <summary>Yeni Değer (JSON)</summary>
        public string NewValue { get; set; }

        /// <summary>Yapan Kullanıcı Adı</summary>
        public string Username { get; set; }

        /// <summary>IP Adresi</summary>
        public string IPAddress { get; set; }

        /// <summary>Notlar</summary>
        public string Notes { get; set; }
    }
}
