using System;
using System.Data.SQLite;
using System.IO;
using EasyInvoicePro.Core;

namespace EasyInvoicePro.Database
{
    /// <summary>
    /// SQLite veritabanını yönetir. İnitiyalizasyon, miğrasyonlar, bağlantı vb.
    /// </summary>
    public static class DatabaseManager
    {
        // ==================== STATIC FIELDS ====================
        private static SQLiteConnection _connection;
        private static string _databasePath;
        private static string _connectionString;

        // ==================== PROPERTY: BAĞLANTI ====================
        /// <summary>Veritabanı bağlantısını döner (Bağlantı havuzundan)</summary>
        public static SQLiteConnection Connection
        {
            get
            {
                if (_connection == null)
                    Initialize();

                if (_connection.State == System.Data.ConnectionState.Closed)
                    _connection.Open();

                return _connection;
            }
        }

        // ==================== METOD: İNİTİYALİZASYON ====================
        /// <summary>
        /// Veritabanını ilk kez kurar ve bağlantı hazırlar
        /// </summary>
        public static void Initialize()
        {
            try
            {
                // Veritabanı dosya yolunu al
                _databasePath = AppSettings.DatabasePath;

                // Veritabanı yolu mutlak yola dönüştür
                if (!Path.IsPathRooted(_databasePath))
                {
                    _databasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _databasePath);
                }

                // Bağlantı stringı oluştur
                _connectionString = $"Data Source={_databasePath};Version=3;";

                // Veritabanı dosyası yoksa oluştur
                if (!File.Exists(_databasePath))
                {
                    CreateDatabase();
                }

                // Bağlantı aç
                _connection = new SQLiteConnection(_connectionString);
                _connection.Open();

                // Şifreyi ayarla (varsa)
                if (AppSettings.DatabaseEncryption)
                {
                    string password = AppSettings.DatabasePassword;
                    using (var cmd = _connection.CreateCommand())
                    {
                        cmd.CommandText = $"PRAGMA key = '{password}';";
                        cmd.ExecuteNonQuery();
                    }
                }

                // Tabloları oluştur (varsa)
                CreateTables();

                System.Diagnostics.Debug.WriteLine("[DB] Başarıyla bağlantı kuruldu: " + _databasePath);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[DB ERROR] İnitiyalizasyon hatası: {ex.Message}");
                throw;
            }
        }

        // ==================== METOD: VERITABANI OLUŞTUR ====================
        /// <summary>
        /// Yeni SQLite veritabanı dosyasını oluşturur
        /// </summary>
        private static void CreateDatabase()
        {
            try
            {
                SQLiteConnection.CreateFile(_databasePath);
                System.Diagnostics.Debug.WriteLine("[DB] Yeni veritabanı dosyası oluşturuldu");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[DB ERROR] Veritabanı oluşturulurken hata: {ex.Message}");
                throw;
            }
        }

        // ==================== METOD: TABLOLARI OLUŞTUR ====================
        /// <summary>
        /// Veritabanında gerekli tabloları oluşturur
        /// </summary>
        private static void CreateTables()
        {
            try
            {
                using (var cmd = Connection.CreateCommand())
                {
                    // Tabloların olup olmadığını kontrol et
                    cmd.CommandText = "SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='Companies';";
                    int tableCount = Convert.ToInt32(cmd.ExecuteScalar());

                    if (tableCount == 0)
                    {
                        // Tüm tabloları oluştur
                        ExecuteSQLScript(DatabaseInitializer.GetCreateTableScript());
                        System.Diagnostics.Debug.WriteLine("[DB] Tablolar başarıyla oluşturuldu");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[DB ERROR] Tablolar oluşturulurken hata: {ex.Message}");
                throw;
            }
        }

        // ==================== METOD: SQL SCRIPT ÇALIŞTIR ====================
        /// <summary>
        /// Bir SQL scriptıni çalıştırır (Birden çok sorgu)
        /// </summary>
        /// <param name="script">SQL scriptı</param>
        public static void ExecuteSQLScript(string script)
        {
            try
            {
                using (var cmd = Connection.CreateCommand())
                {
                    // Scriptı sorguya göre böl
                    string[] queries = script.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string query in queries)
                    {
                        if (!string.IsNullOrWhiteSpace(query))
                        {
                            cmd.CommandText = query.Trim();
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[DB ERROR] SQL Script çalıştırılırken hata: {ex.Message}");
                throw;
            }
        }

        // ==================== METOD: SORGU ÇALIŞTIR ====================
        /// <summary>
        /// Bir SQL sorgusunu çalıştırır ve etkilenen satır sayısını döner
        /// </summary>
        /// <param name="query">SQL sorgusu</param>
        /// <returns>Etkilenen satır sayısı</returns>
        public static int ExecuteQuery(string query)
        {
            try
            {
                using (var cmd = Connection.CreateCommand())
                {
                    cmd.CommandText = query;
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[DB ERROR] Sorgu çalıştırılırken hata: {ex.Message}");
                throw;
            }
        }

        // ==================== METOD: VERI AL ====================
        /// <summary>
        /// Bir SQL sorgusunu çalıştırır ve veri okuyucuyu döner
        /// </summary>
        /// <param name="query">SQL sorgusu</param>
        /// <returns>SQLiteDataReader nesnesi</returns>
        public static SQLiteDataReader ExecuteReader(string query)
        {
            try
            {
                var cmd = Connection.CreateCommand();
                cmd.CommandText = query;
                return cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[DB ERROR] Okuyucu çalıştırılırken hata: {ex.Message}");
                throw;
            }
        }

        // ==================== METOD: SKALER DEĞER AL ====================
        /// <summary>
        /// Bir SQL sorgusunun skaler sonucunu döner (COUNT, SUM vb.)
        /// </summary>
        /// <param name="query">SQL sorgusu</param>
        /// <returns>Skaler değer</returns>
        public static object ExecuteScalar(string query)
        {
            try
            {
                using (var cmd = Connection.CreateCommand())
                {
                    cmd.CommandText = query;
                    return cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[DB ERROR] Skaler sorgu çalıştırılırken hata: {ex.Message}");
                throw;
            }
        }

        // ==================== METOD: BAGLANTIVI KAPAT ====================
        /// <summary>
        /// Veritabanı bağlantısını kapatır
        /// </summary>
        public static void Close()
        {
            try
            {
                if (_connection != null && _connection.State == System.Data.ConnectionState.Open)
                {
                    _connection.Close();
                    _connection.Dispose();
                    _connection = null;
                    System.Diagnostics.Debug.WriteLine("[DB] Bağlantı kapatıldı");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[DB ERROR] Bağlantı kapatılırken hata: {ex.Message}");
            }
        }

        // ==================== METOD: VERITABANI SIFİRLA ====================
        /// <summary>
        /// Veritabanını yeni baştan oluşturur (Üzerindeki veriler silinir)
        /// </summary>
        public static void Reset()
        {
            try
            {
                Close();

                if (File.Exists(_databasePath))
                {
                    File.Delete(_databasePath);
                    System.Diagnostics.Debug.WriteLine("[DB] Veritabanı dosyası silindi");
                }

                Initialize();
                System.Diagnostics.Debug.WriteLine("[DB] Veritabanı yeniden oluşturuldu");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[DB ERROR] Reset sırasında hata: {ex.Message}");
                throw;
            }
        }

        // ==================== METOD: BAĞLANTI TEST ====================
        /// <summary>
        /// Veritabanı bağlantısını test eder
        /// </summary>
        /// <returns>Bağlantı başarılı ise true</returns>
        public static bool TestConnection()
        {
            try
            {
                using (var cmd = Connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT 1;";
                    var result = cmd.ExecuteScalar();
                    return result != null;
                }
            }
            catch
            {
                return false;
            }
        }

        // ==================== METOD: VERITABANI YEDEKLE ====================
        /// <summary>
        /// Veritabanını bir dosyaya yedekler
        /// </summary>
        /// <param name="backupPath">Yedekleme dosyası yolu</param>
        /// <returns>Başarılı ise true</returns>
        public static bool Backup(string backupPath)
        {
            try
            {
                Close();

                if (File.Exists(_databasePath))
                {
                    File.Copy(_databasePath, backupPath, true);
                    System.Diagnostics.Debug.WriteLine($"[DB] Yedekleme başarı: {backupPath}");
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[DB ERROR] Yedekleme hatası: {ex.Message}");
                return false;
            }
            finally
            {
                Initialize();
            }
        }

        // ==================== METOD: VERITABANI GERI YÜKLE ====================
        /// <summary>
        /// Yedekten veritabanını geri yükler
        /// </summary>
        /// <param name="backupPath">Yedekleme dosyası yolu</param>
        /// <returns>Başarılı ise true</returns>
        public static bool Restore(string backupPath)
        {
            try
            {
                if (!File.Exists(backupPath))
                    return false;

                Close();

                // Eski veritabanını sil
                if (File.Exists(_databasePath))
                    File.Delete(_databasePath);

                // Yedek dosyasını geri kopyala
                File.Copy(backupPath, _databasePath, true);

                System.Diagnostics.Debug.WriteLine($"[DB] Geri yükleme başarı: {backupPath}");
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[DB ERROR] Geri yükleme hatası: {ex.Message}");
                return false;
            }
            finally
            {
                Initialize();
            }
        }
    }
}
