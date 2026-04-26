using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace EasyInvoicePro.Core
{
    /// <summary>
    /// Uygulama ayarlarını yönetir. App.config dosyasını oku ve yaz.
    /// </summary>
    public static class AppSettings
    {
        // ==================== STATIC FIELD ====================
        private static Dictionary<string, string> _settings;

        // ==================== CONSTRUCTOR ====================
        /// <summary>Static constructor - Ayarları yükler</summary>
        static AppSettings()
        {
            LoadSettings();
        }

        // ==================== METOD: AYARLARI YÜKLE ====================
        /// <summary>
        /// App.config dosyasından tüm ayarları belleğe yükler
        /// </summary>
        private static void LoadSettings()
        {
            _settings = new Dictionary<string, string>();

            try
            {
                // App.config appSettings bölümünü oku
                foreach (string key in ConfigurationManager.AppSettings)
                {
                    string value = ConfigurationManager.AppSettings[key];
                    _settings[key] = value;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ayarlar yüklenirken hata: {ex.Message}");
            }
        }

        // ==================== METOD: AYAR DEĞERİ AL ====================
        /// <summary>
        /// Belirtilen anahtar için ayar değerini döner
        /// </summary>
        /// <param name="key">Ayar anahtarı</param>
        /// <param name="defaultValue">Bulunamazsa döndürülecek varsayılan değer</param>
        /// <returns>Ayar değeri veya varsayılan değer</returns>
        public static string Get(string key, string defaultValue = "")
        {
            if (_settings.ContainsKey(key))
                return _settings[key];
            return defaultValue;
        }

        // ==================== METOD: AYAR DEĞERINI AL (INT) ====================
        public static int GetInt(string key, int defaultValue = 0)
        {
            string value = Get(key);
            if (int.TryParse(value, out int result))
                return result;
            return defaultValue;
        }

        // ==================== METOD: AYAR DEĞERINI AL (BOOL) ====================
        public static bool GetBool(string key, bool defaultValue = false)
        {
            string value = Get(key);
            if (bool.TryParse(value, out bool result))
                return result;
            return defaultValue;
        }

        // ==================== METOD: AYAR DEĞERINI AL (DECIMAL) ====================
        public static decimal GetDecimal(string key, decimal defaultValue = 0m)
        {
            string value = Get(key);
            if (decimal.TryParse(value, out decimal result))
                return result;
            return defaultValue;
        }

        // ==================== METOD: AYAR DEĞERINI AYARLA ====================
        /// <summary>
        /// Ayarı belleğe ve config dosyasına yazır
        /// </summary>
        /// <param name="key">Ayar anahtarı</param>
        /// <param name="value">Yeni değer</param>
        public static void Set(string key, string value)
        {
            _settings[key] = value;
            SaveSetting(key, value);
        }

        // ==================== METOD: AYARLARI KAYDET ====================
        private static void SaveSetting(string key, string value)
        {
            try
            {
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings[key].Value = value;
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ayar kaydedilirken hata: {ex.Message}");
            }
        }

        // ==================== PROPERTIES: VERİTABANI ====================
        /// <summary>Veritabanı dosya adı</summary>
        public static string DatabasePath
        {
            get => Get("DatabasePath", Constants.DATABASE_FILENAME);
            set => Set("DatabasePath", value);
        }

        /// <summary>Veritabanı şifresi</summary>
        public static string DatabasePassword
        {
            get => Get("DatabasePassword", Constants.DATABASE_PASSWORD);
        }

        /// <summary>Veritabanı şifreleme etkin mi</summary>
        public static bool DatabaseEncryption
        {
            get => GetBool("DatabaseEncryption", true);
            set => Set("DatabaseEncryption", value.ToString());
        }

        // ==================== PROPERTIES: UYGULAMA ====================
        /// <summary>Uygulama başlığı</summary>
        public static string AppTitle
        {
            get => Get("AppTitle", Constants.APP_NAME);
        }

        /// <summary>Uygulama versiyonu</summary>
        public static string AppVersion
        {
            get => Get("AppVersion", Constants.APP_VERSION);
        }

        /// <summary>Dil (tr, en)</summary>
        public static string Language
        {
            get => Get("Language", "tr");
            set => Set("Language", value);
        }

        /// <summary>Tema (Light, Dark, Auto)</summary>
        public static string Theme
        {
            get => Get("Theme", "Light");
            set => Set("Theme", value);
        }

        // ==================== PROPERTIES: E-FATURA ====================
        /// <summary>E-Fatura Test Sunucusu URL'si</summary>
        public static string eFaturaTestUrl
        {
            get => Get("eFaturaTestUrl", Constants.EFATURA_TEST_URL);
        }

        /// <summary>E-Fatura Üretim Sunucusu URL'si</summary>
        public static string eFaturaProdUrl
        {
            get => Get("eFaturaProdUrl", Constants.EFATURA_PROD_URL);
        }

        /// <summary>E-Fatura Ortamı (Test/Production)</summary>
        public static string eFaturaEnvironment
        {
            get => Get("eFaturaEnvironment", "Test");
            set => Set("eFaturaEnvironment", value);
        }

        /// <summary>E-Fatura Timeout (ms)</summary>
        public static int eFaturaTimeout
        {
            get => GetInt("eFaturaTimeout", Constants.EFATURA_TIMEOUT_MS);
        }

        // ==================== PROPERTIES: LİSANS ====================
        /// <summary>Lisans Sunucusu URL'si</summary>
        public static string LicenseServer
        {
            get => Get("LicenseServer", "https://api.easyinvoicepro.com/license");
        }

        /// <summary>Demo Süresi (Gün)</summary>
        public static int DemoExpireDays
        {
            get => GetInt("DemoExpireDays", Constants.DEMO_EXPIRE_DAYS);
        }

        /// <summary>Demo Max Fatura</summary>
        public static int MaxInvoicesDemo
        {
            get => GetInt("MaxInvoicesDemo", Constants.MAX_INVOICES_DEMO);
        }

        /// <summary>Pro Max Fatura</summary>
        public static int MaxInvoicesPro
        {
            get => GetInt("MaxInvoicesPro", Constants.MAX_INVOICES_PRO);
        }

        // ==================== PROPERTIES: YEDEKLEME ====================
        /// <summary>Otomatik Yedekleme Etkin mi</summary>
        public static bool AutoBackupEnabled
        {
            get => GetBool("AutoBackupEnabled", true);
            set => Set("AutoBackupEnabled", value.ToString());
        }

        /// <summary>Otomatik Yedekleme Aralığı (Saat)</summary>
        public static int AutoBackupInterval
        {
            get => GetInt("AutoBackupInterval", 24);
            set => Set("AutoBackupInterval", value.ToString());
        }

        /// <summary>Yedekleme Klasörü Yolu</summary>
        public static string BackupPath
        {
            get => Get("BackupPath", Constants.BACKUPS_FOLDER);
        }

        // ==================== PROPERTIES: RAPOR ====================
        /// <summary>Rapor Klasörü Yolu</summary>
        public static string ReportPath
        {
            get => Get("ReportPath", Constants.REPORTS_FOLDER);
        }

        /// <summary>PDF Sayfa Boyutu</summary>
        public static string PDFPageSize
        {
            get => Get("PDFPageSize", "A4");
        }

        /// <summary>Excel Tarih Formatı</summary>
        public static string ExcelDateFormat
        {
            get => Get("ExcelDateFormat", Constants.EXCEL_DATE_FORMAT);
        }

        // ==================== METOD: TÜM AYARLARI YAZDIR (DEBUG) ====================
        /// <summary>Debug için tüm ayarları konsola yazdırır</summary>
        public static void PrintAll()
        {
            System.Diagnostics.Debug.WriteLine("\n=== AYARLAR ===");
            foreach (var kvp in _settings.OrderBy(x => x.Key))
            {
                System.Diagnostics.Debug.WriteLine($"{kvp.Key}: {kvp.Value}");
            }
            System.Diagnostics.Debug.WriteLine("===============\n");
        }
    }
}
