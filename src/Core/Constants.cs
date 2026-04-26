using System;
using System.Collections.Generic;

namespace EasyInvoicePro.Core
{
    /// <summary>
    /// Uygulamanın sabit değerleri (Türkçe ve İngilizce)
    /// </summary>
    public static class Constants
    {
        // ==================== UYGULAMA BİLGİSİ ====================
        public const string APP_NAME = "EasyInvoice Pro";
        public const string APP_VERSION = "1.0.0";
        public const string APP_COMPANY = "EasyInvoice Pro";
        public const string APP_COPYRIGHT = "© 2024 EasyInvoice Pro. All Rights Reserved.";

        // ==================== VERİTABANI ====================
        public const string DATABASE_FILENAME = "EasyInvoicePro.db";
        public const string DATABASE_PASSWORD = "EasyInvoicePro2024!";
        public const int DATABASE_KDF_ITERATIONS = 400000;

        // ==================== LİSANS ====================
        public const int DEMO_EXPIRE_DAYS = 30;
        public const int MAX_INVOICES_DEMO = 50;
        public const int MAX_INVOICES_PRO = 500;
        public const int MAX_INVOICES_ENTERPRISE = int.MaxValue;

        // ==================== E-FATURA ====================
        public const string EFATURA_TEST_URL = "https://efatura-test.uyumsoft.com.tr/Services/Integration/UEBIntegratorV3?wsdl";
        public const string EFATURA_PROD_URL = "https://efatura.gov.tr/Services/Integration/UEBIntegratorV3?wsdl";
        public const int EFATURA_TIMEOUT_MS = 30000;
        public const string UBL_VERSION = "2.1";
        public const string CUSTOMIZATION_ID = "tr-ubl";

        // ==================== KDV ORANLAR ====================
        public const decimal KDV_RATE_20 = 0.20m;
        public const decimal KDV_RATE_10 = 0.10m;
        public const decimal KDV_RATE_5 = 0.05m;
        public const decimal KDV_RATE_1 = 0.01m;
        public const decimal KDV_RATE_0 = 0.00m;

        // ==================== PARA BİRİMİ ====================
        public const string CURRENCY_CODE = "TRY";
        public const string CURRENCY_SYMBOL = "₺";

        // ==================== DOSYA YOLLARI ====================
        public const string REPORTS_FOLDER = "Reports";
        public const string BACKUPS_FOLDER = "Backups";
        public const string TEMP_FOLDER = "Temp";
        public const string LOGS_FOLDER = "Logs";

        // ==================== DOSYA UZANTILARI ====================
        public const string PDF_EXTENSION = ".pdf";
        public const string EXCEL_EXTENSION = ".xlsx";
        public const string XML_EXTENSION = ".xml";
        public const string JSON_EXTENSION = ".json";
        public const string CERTIFICATE_EXTENSION = ".pfx";

        // ==================== TARIH VE SAAT FORMATı ====================
        public const string DATE_FORMAT = "dd.MM.yyyy";
        public const string TIME_FORMAT = "HH:mm:ss";
        public const string DATETIME_FORMAT = "dd.MM.yyyy HH:mm:ss";
        public const string EXCEL_DATE_FORMAT = "dd.mm.yyyy";

        // ==================== SAYFA BOYUTLARI ====================
        public const int PAGE_SIZE_A4_WIDTH = 210; // mm
        public const int PAGE_SIZE_A4_HEIGHT = 297; // mm

        // ==================== RENKLER (HEX) ====================
        public static class Colors
        {
            public const string PRIMARY = "#2C3E50";      // Başlık, sidebar
            public const string SECONDARY = "#3498DB";    // Accent butonlar
            public const string SUCCESS = "#27AE60";      // Yeşil, başarı
            public const string WARNING = "#F39C12";      // Turuncu, uyarı
            public const string DANGER = "#E74C3C";       // Kırmızı, hata
            public const string INFO = "#16A085";         // Bilgi
            public const string LIGHT = "#ECF0F1";        // Açık arka plan
            public const string DARK = "#34495E";         // Koyu arka plan
            public const string GRAY = "#95A5A6";         // Gri
            public const string WHITE = "#FFFFFF";        // Beyaz
        }

        // ==================== MESAJLAR ====================
        public static class Messages
        {
            // Başarı
            public const string SUCCESS_SAVED = "Başarıyla kaydedildi.";
            public const string SUCCESS_DELETED = "Başarıyla silindi.";
            public const string SUCCESS_UPDATED = "Başarıyla güncellendi.";
            public const string SUCCESS_EXPORTED = "Başarıyla dışa aktarıldı.";
            public const string SUCCESS_SENT = "Başarıyla gönderildi.";

            // Hata
            public const string ERROR_GENERIC = "Bir hata oluştu. Lütfen tekrar deneyiniz.";
            public const string ERROR_DATABASE = "Veritabanı bağlantı hatası.";
            public const string ERROR_NETWORK = "İnternet bağlantısı hatası.";
            public const string ERROR_FILE_NOT_FOUND = "Dosya bulunamadı.";
            public const string ERROR_PERMISSION_DENIED = "Yetki hatası.";
            public const string ERROR_INVALID_INPUT = "Geçersiz giriş.";
            public const string ERROR_LICENSE_EXPIRED = "Lisans süresi dolmuş.";
            public const string ERROR_LICENSE_INVALID = "Geçersiz lisans anahtarı.";

            // Uyarı
            public const string WARNING_UNSAVED_CHANGES = "Kaydedilmemiş değişiklikler var. Devam etmek istediğinizden emin misiniz?";
            public const string WARNING_DELETE_CONFIRM = "Bu işlem geri alınamaz. Silmek istediğinizden emin misiniz?";
            public const string WARNING_DEMO_EXPIRED = "Demo süresi dolmuştur. Lütfen lisans satın alınız.";

            // Bilgi
            public const string INFO_PROCESSING = "İşleniyor, lütfen bekleyiniz...";
            public const string INFO_IMPORTING = "Aktarılıyor, lütfen bekleyiniz...";
            public const string INFO_EXPORTING = "Dışa aktarılıyor, lütfen bekleyiniz...";
        }

        // ==================== FATURA TÜRLERİ ====================
        public enum InvoiceType
        {
            SATIS = 380,      // Satış Faturası
            IADE = 381,        // İade Faturası
            PESIN = 380,       // Peşin Satış
            VADELI = 380       // Vadeli Satış
        }

        // ==================== ÖDEME DURUMLARI ====================
        public enum PaymentStatus
        {
            PENDING = 0,       // Beklemede
            PARTIAL = 1,       // Kısmi
            PAID = 2,           // Ödendi
            OVERDUE = 3        // Vadesi geçmiş
        }

        // ==================== E-FATURA DURUMLARI ====================
        public enum eFaturaStatus
        {
            DRAFT = 0,         // Taslak
            PENDING = 1,       // Gönderilmeyi bekliyor
            SENT = 2,           // Gönderildi
            REJECTED = 3,      // Reddedildi
            ERROR = 4           // Hata
        }

        // ==================== KDV BEYANNAME TÜRÜ ====================
        public enum KDVDeclarationType
        {
            KDV1 = 1,  // Standart KDV
            KDV2 = 2,  // İndirimli KDV
            KDV3 = 3,  // Muafiyetli KDV
            KDV4 = 4   // Özel Muhasebeleştirme
        }

        // ==================== LİSANS SEVİYESİ ====================
        public enum LicenseLevel
        {
            DEMO = 0,          // Demo (30 gün)
            PRO = 1,            // Profesyonel (500 fatura/ay)
            ENTERPRISE = 2      // Kurumsal (Limitsiz)
        }

        // ==================== BAŞARILI SONUÇ KOD ====================
        public const int SUCCESS_CODE = 0;
        public const int ERROR_CODE = -1;
        public const int WARNING_CODE = 1;
    }

    /// <summary>
    /// Türkçe UI metin sabitler
    /// </summary>
    public static class UITexts
    {
        // ==================== BUTON METİNLERİ ====================
        public const string BTN_SAVE = "Kaydet";
        public const string BTN_DELETE = "Sil";
        public const string BTN_CANCEL = "İptal";
        public const string BTN_CLOSE = "Kapat";
        public const string BTN_NEW = "Yeni";
        public const string BTN_EDIT = "Düzenle";
        public const string BTN_SEARCH = "Ara";
        public const string BTN_REFRESH = "Yenile";
        public const string BTN_EXPORT = "Dışa Aktar";
        public const string BTN_IMPORT = "İçe Aktar";
        public const string BTN_PRINT = "Yazdır";
        public const string BTN_PREVIEW = "Önizle";
        public const string BTN_SEND = "Gönder";
        public const string BTN_CALCULATE = "Hesapla";
        public const string BTN_DOWNLOAD = "İndir";
        public const string BTN_UPLOAD = "Yükle";
        public const string BTN_OK = "Tamam";
        public const string BTN_YES = "Evet";
        public const string BTN_NO = "Hayır";

        // ==================== FORM BAŞLIKLARI ====================
        public const string FORM_TITLE_DASHBOARD = "Panoyum";
        public const string FORM_TITLE_LOGIN = "Giriş Yap";
        public const string FORM_TITLE_INVOICE = "Fatura Kes";
        public const string FORM_TITLE_CUSTOMER = "Müşteri Yönetimi";
        public const string FORM_TITLE_PRODUCT = "Ürün Yönetimi";
        public const string FORM_TITLE_EFATURA = "E-Fatura Yönetimi";
        public const string FORM_TITLE_KDV = "KDV Bildirimi";
        public const string FORM_TITLE_REPORT = "Raporlar";
        public const string FORM_TITLE_SETTINGS = "Ayarlar";
        public const string FORM_TITLE_ABOUT = "Hakkında";

        // ==================== ETIKET METİNLERİ ====================
        public const string LBL_USERNAME = "Kullanıcı Adı:";
        public const string LBL_PASSWORD = "Şifre:";
        public const string LBL_EMAIL = "E-Posta:";
        public const string LBL_PHONE = "Telefon:";
        public const string LBL_ADDRESS = "Adres:";
        public const string LBL_CITY = "İl:";
        public const string LBL_COUNTRY = "Ülke:";
        public const string LBL_TAX_ID = "Vergi Numarası:";
        public const string LBL_COMPANY_NAME = "Şirket Adı:";
        public const string LBL_INVOICE_NO = "Fatura No:";
        public const string LBL_INVOICE_DATE = "Fatura Tarihi:";
        public const string LBL_PAYMENT_STATUS = "Ödeme Durumu:";
        public const string LBL_TOTAL = "Toplam:";
        public const string LBL_SUBTOTAL = "Ara Toplam:";
        public const string LBL_TAX = "KDV:";
        public const string LBL_AMOUNT = "Tutar:";
        public const string LBL_QUANTITY = "Miktar:";
        public const string LBL_PRICE = "Fiyat:";
        public const string LBL_DISCOUNT = "İndirim:";

        // ==================== TABLO BAŞLIKLARI ====================
        public const string COL_ID = "ID";
        public const string COL_NAME = "Ad";
        public const string COL_EMAIL = "E-Posta";
        public const string COL_PHONE = "Telefon";
        public const string COL_INVOICE_NO = "Fatura No";
        public const string COL_DATE = "Tarih";
        public const string COL_AMOUNT = "Tutar";
        public const string COL_STATUS = "Durum";
        public const string COL_ACTIONS = "İşlemler";
    }
}
