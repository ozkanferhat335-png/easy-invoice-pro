using System;
using System.Collections.Generic;

namespace EasyInvoicePro.Models
{
    /// <summary>
    /// Şirket bilgilerini temsil eden model
    /// </summary>
    public class Company
    {
        /// <summary>Şirket ID (Veritabanı Primary Key)</summary>
        public int Id { get; set; }

        /// <summary>Şirket Adı</summary>
        public string Name { get; set; }

        /// <summary>Vergi Kimlik Numarası (VKN)</summary>
        public string TaxNumber { get; set; }

        /// <summary>Vergi Mükellefi Adı (Yasal Adı)</summary>
        public string LegalName { get; set; }

        /// <summary>Merkez Adresi</summary>
        public string Address { get; set; }

        /// <summary>İl</summary>
        public string City { get; set; }

        /// <summary>İlçe</summary>
        public string District { get; set; }

        /// <summary>Posta Kodu</summary>
        public string ZipCode { get; set; }

        /// <summary>Telefon Numarası</summary>
        public string Phone { get; set; }

        /// <summary>Faks Numarası</summary>
        public string Fax { get; set; }

        /// <summary>E-Posta Adresi</summary>
        public string Email { get; set; }

        /// <summary>Web Sitesi</summary>
        public string Website { get; set; }

        /// <summary>E-Fatura Gönderici Hesabı</summary>
        public string eFaturaAlias { get; set; }

        /// <summary>E-Fatura Şifresi (Şifreli)</summary>
        public string eFaturaPassword { get; set; }

        /// <summary>Sertifika Dosya Yolu</summary>
        public string CertificatePath { get; set; }

        /// <summary>Sertifika Şifresi</summary>
        public string CertificatePassword { get; set; }

        /// <summary>Sertifika Başlangıç Tarihi</summary>
        public DateTime? CertificateStartDate { get; set; }

        /// <summary>Sertifika Bitiş Tarihi</summary>
        public DateTime? CertificateEndDate { get; set; }

        /// <summary>Muhasebeci Adı</summary>
        public string AccountantName { get; set; }

        /// <summary>Muhasebeci Telefonu</summary>
        public string AccountantPhone { get; set; }

        /// <summary>Muhasebeci E-Postası</summary>
        public string AccountantEmail { get; set; }

        /// <summary>Varsayılan KDV Oranı (%)</summary>
        public decimal DefaultKDVRate { get; set; } = 20m;

        /// <summary>Varsayılan Para Birimi (TRY, USD, EUR)</summary>
        public string DefaultCurrency { get; set; } = "TRY";

        /// <summary>Varsayılan Ödeme Vadesi (Gün)</summary>
        public int DefaultPaymentTermDays { get; set; } = 30;

        /// <summary>Logo Dosya Yolu</summary>
        public string LogoPath { get; set; }

        /// <summary>Düşük Stok Uyarı Sınırı</summary>
        public int LowStockWarningLevel { get; set; } = 10;

        /// <summary>Etkin mi</summary>
        public bool IsActive { get; set; } = true;

        /// <summary>Oluşturulma Tarihi</summary>
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        /// <summary>Güncellenme Tarihi</summary>
        public DateTime? UpdatedDate { get; set; }

        /// <summary>Notlar</summary>
        public string Notes { get; set; }
    }

    /// <summary>
    /// Müşteri/Cari bilgilerini temsil eden model
    /// </summary>
    public class Customer
    {
        /// <summary>Müşteri ID</summary>
        public int Id { get; set; }

        /// <summary>Şirket ID (Foreign Key)</summary>
        public int CompanyId { get; set; }

        /// <summary>Müşteri Adı</summary>
        public string Name { get; set; }

        /// <summary>Müşteri Türü (Bireysel/Kurumsal)</summary>
        public CustomerTypeEnum CustomerType { get; set; }

        /// <summary>Vergi Numarası (TCKN veya VKN)</summary>
        public string TaxNumber { get; set; }

        /// <summary>Bağlı Olduğu Vergi Dairesi</summary>
        public string TaxOffice { get; set; }

        /// <summary>Adresi</summary>
        public string Address { get; set; }

        /// <summary>İl</summary>
        public string City { get; set; }

        /// <summary>İlçe</summary>
        public string District { get; set; }

        /// <summary>Posta Kodu</summary>
        public string ZipCode { get; set; }

        /// <summary>Telefon Numarası</summary>
        public string Phone { get; set; }

        /// <summary>Cep Telefonu</summary>
        public string MobilePhone { get; set; }

        /// <summary>Faks</summary>
        public string Fax { get; set; }

        /// <summary>E-Posta</summary>
        public string Email { get; set; }

        /// <summary>E-Fatura E-Postası (İsteğe Bağlı)</summary>
        public string eFaturaEmail { get; set; }

        /// <summary>Web Sitesi</summary>
        public string Website { get; set; }

        /// <summary>Kontakt Kişi Adı</summary>
        public string ContactPerson { get; set; }

        /// <summary>Kontakt Kişi Telefonu</summary>
        public string ContactPersonPhone { get; set; }

        /// <summary>Toplam Borç (Otomatik Hesaplanan)</summary>
        public decimal TotalDebt { get; set; } = 0m;

        /// <summary>Vadesi Geçmiş Borç</summary>
        public decimal OverdueDebt { get; set; } = 0m;

        /// <summary>Kredi Limiti</summary>
        public decimal CreditLimit { get; set; } = 0m;

        /// <summary>Risk Seviyesi (Düşük/Orta/Yüksek)</summary>
        public CustomerRiskEnum RiskLevel { get; set; } = CustomerRiskEnum.Low;

        /// <summary>Varsayılan Ödeme Şekli</summary>
        public PaymentMethodEnum DefaultPaymentMethod { get; set; } = PaymentMethodEnum.Cash;

        /// <summary>Varsayılan Ödeme Vadesi (Gün)</summary>
        public int DefaultPaymentTermDays { get; set; } = 0;

        /// <summary>İndirim Oranı (%)</summary>
        public decimal DiscountRate { get; set; } = 0m;

        /// <summary>Etkin mi</summary>
        public bool IsActive { get; set; } = true;

        /// <summary>Bloke mi (Borç Nedeniyle)</summary>
        public bool IsBlocked { get; set; } = false;

        /// <summary>Oluşturulma Tarihi</summary>
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        /// <summary>Güncellenme Tarihi</summary>
        public DateTime? UpdatedDate { get; set; }

        /// <summary>Notlar</summary>
        public string Notes { get; set; }
    }

    /// <summary>
    /// Ürün/Hizmet bilgilerini temsil eden model
    /// </summary>
    public class Product
    {
        /// <summary>Ürün ID</summary>
        public int Id { get; set; }

        /// <summary>Şirket ID (Foreign Key)</summary>
        public int CompanyId { get; set; }

        /// <summary>Ürün Kodu (SKU)</summary>
        public string Code { get; set; }

        /// <summary>Barkod</summary>
        public string Barcode { get; set; }

        /// <summary>Ürün Adı</summary>
        public string Name { get; set; }

        /// <summary>Ürün Açıklaması</summary>
        public string Description { get; set; }

        /// <summary>Kategori</summary>
        public string Category { get; set; }

        /// <summary>Birim (Adet, Kg, Saat, vb.)</summary>
        public string Unit { get; set; } = "Adet";

        /// <summary>Satış Fiyatı</summary>
        public decimal SalesPrice { get; set; }

        /// <summary>Maliyet Fiyatı</summary>
        public decimal CostPrice { get; set; }

        /// <summary>KDV Oranı (%)</summary>
        public decimal KDVRate { get; set; } = 20m;

        /// <summary>Mevcut Stok Miktarı</summary>
        public decimal StockQuantity { get; set; } = 0m;

        /// <summary>Minimum Stok Miktarı</summary>
        public decimal MinimumStock { get; set; } = 0m;

        /// <summary>Maksimum Stok Miktarı</summary>
        public decimal MaximumStock { get; set; } = 0m;

        /// <summary>Ürün Resmi Yolu (Optional)</summary>
        public string ImagePath { get; set; }

        /// <summary>Tedarikçi Bilgisi</summary>
        public string Supplier { get; set; }

        /// <summary>Etkin mi</summary>
        public bool IsActive { get; set; } = true;

        /// <summary>Oluşturulma Tarihi</summary>
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        /// <summary>Güncellenme Tarihi</summary>
        public DateTime? UpdatedDate { get; set; }

        /// <summary>Notlar</summary>
        public string Notes { get; set; }
    }

    /// <summary>
    /// Fatura başlığı bilgilerini temsil eden model
    /// </summary>
    public class Invoice
    {
        /// <summary>Fatura ID</summary>
        public int Id { get; set; }

        /// <summary>Şirket ID (Foreign Key)</summary>
        public int CompanyId { get; set; }

        /// <summary>Müşteri ID (Foreign Key)</summary>
        public int CustomerId { get; set; }

        /// <summary>Fatura Numarası (Sıra No)</summary>
        public string InvoiceNumber { get; set; }

        /// <summary>Fatura Tarihi</summary>
        public DateTime InvoiceDate { get; set; } = DateTime.Now;

        /// <summary>Fatura Türü (Satış, İade, vb.)</summary>
        public InvoiceTypeEnum InvoiceType { get; set; } = InvoiceTypeEnum.Sales;

        /// <summary>Ödeme Tarihi (Vade)</summary>
        public DateTime DueDate { get; set; }

        /// <summary>Ara Toplam (KDV Hariç)</summary>
        public decimal SubTotal { get; set; } = 0m;

        /// <summary>İndirim Tutarı</summary>
        public decimal DiscountAmount { get; set; } = 0m;

        /// <summary>KDV Toplam</summary>
        public decimal TotalTax { get; set; } = 0m;

        /// <summary>Genel Toplam (KDV Dahil)</summary>
        public decimal GrandTotal { get; set; } = 0m;

        /// <summary>Ödenmiş Tutar</summary>
        public decimal PaidAmount { get; set; } = 0m;

        /// <summary>Ödeme Durumu</summary>
        public PaymentStatusEnum PaymentStatus { get; set; } = PaymentStatusEnum.Pending;

        /// <summary>Para Birimi</summary>
        public string Currency { get; set; } = "TRY";

        /// <summary>Ödeme Şekli</summary>
        public PaymentMethodEnum PaymentMethod { get; set; } = PaymentMethodEnum.Cash;

        /// <summary>E-Fatura Gönderildi mi</summary>
        public bool IsSentToEFatura { get; set; } = false;

        /// <summary>E-Fatura Durumu</summary>
        public eFaturaStatusEnum eFaturaStatus { get; set; } = eFaturaStatusEnum.Draft;

        /// <summary>E-Fatura UUID (İçerik Referans Numarası)</summary>
        public string eFaturaUUID { get; set; }

        /// <summary>E-Fatura XML (Depolama için)</summary>
        public string eFaturaXML { get; set; }

        /// <summary>Faturanın QR Kodu</summary>
        public string QRCode { get; set; }

        /// <summary>Notlar/Açıklamalar</summary>
        public string Notes { get; set; }

        /// <summary>Oluşturulma Tarihi</summary>
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        /// <summary>Güncellenme Tarihi</summary>
        public DateTime? UpdatedDate { get; set; }

        /// <summary>Fatura Kalemleri (Navigation)</summary>
        public List<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();

        /// <summary>Ödeme Kayıtları (Navigation)</summary>
        public List<Payment> Payments { get; set; } = new List<Payment>();
    }

    /// <summary>
    /// Fatura kalemlerini temsil eden model
    /// </summary>
    public class InvoiceItem
    {
        /// <summary>Kalem ID</summary>
        public int Id { get; set; }

        /// <summary>Fatura ID (Foreign Key)</summary>
        public int InvoiceId { get; set; }

        /// <summary>Ürün ID (Foreign Key - Optional)</summary>
        public int? ProductId { get; set; }

        /// <summary>Kalem Açıklaması</summary>
        public string Description { get; set; }

        /// <summary>Miktar</summary>
        public decimal Quantity { get; set; }

        /// <summary>Birim Fiyatı</summary>
        public decimal UnitPrice { get; set; }

        /// <summary>Birim (Adet, Kg, vb.)</summary>
        public string Unit { get; set; } = "Adet";

        /// <summary>İndirim Oranı (%)</summary>
        public decimal DiscountRate { get; set; } = 0m;

        /// <summary>İndirim Tutarı</summary>
        public decimal DiscountAmount { get; set; } = 0m;

        /// <summary>KDV Oranı (%)</summary>
        public decimal TaxRate { get; set; } = 20m;

        /// <summary>Satır Toplamı (KDV Hariç)</summary>
        public decimal LineTotal { get; set; }

        /// <summary>Satır Toplamı (KDV Dahil)</summary>
        public decimal LineTotalWithTax { get; set; }

        /// <summary>Satır KDV Tutarı</summary>
        public decimal LineTaxAmount { get; set; }

        /// <summary>Sıra Numarası</summary>
        public int LineNumber { get; set; }

        /// <summary>Notlar</summary>
        public string Notes { get; set; }
    }

    /// <summary>
    /// Ödeme kaydını temsil eden model
    /// </summary>
    public class Payment
    {
        /// <summary>Ödeme ID</summary>
        public int Id { get; set; }

        /// <summary>Fatura ID (Foreign Key)</summary>
        public int InvoiceId { get; set; }

        /// <summary>Ödeme Tarihi</summary>
        public DateTime PaymentDate { get; set; } = DateTime.Now;

        /// <summary>Ödenen Tutarı</summary>
        public decimal Amount { get; set; }

        /// <summary>Ödeme Şekli</summary>
        public PaymentMethodEnum PaymentMethod { get; set; } = PaymentMethodEnum.Cash;

        /// <summary>Referans Numarası (Çek No, Havale No, vb.)</summary>
        public string ReferenceNumber { get; set; }

        /// <summary>Notlar</summary>
        public string Notes { get; set; }

        /// <summary>Oluşturulma Tarihi</summary>
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
