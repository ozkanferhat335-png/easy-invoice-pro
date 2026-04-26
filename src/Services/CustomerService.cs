using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using EasyInvoicePro.Database;
using EasyInvoicePro.Models;

namespace EasyInvoicePro.Services
{
    /// <summary>
    /// Müşteri işlemlerini yönetir (CRUD operasyonları)
    /// </summary>
    public class CustomerService
    {
        // ==================== METOD: MÜŞTERI EKLE ====================
        /// <summary>
        /// Yeni bir müşteri ekler
        /// </summary>
        /// <param name="customer">Müşteri nesnesi</param>
        /// <returns>Eklenen müşterinin ID'si</returns>
        public static int AddCustomer(Customer customer)
        {
            try
            {
                string query = $@"
                    INSERT INTO Customers (CompanyId, Name, CustomerType, TaxNumber, TaxOffice, Address, City, District, 
                    ZipCode, Phone, MobilePhone, Fax, Email, eFaturaEmail, Website, ContactPerson, ContactPersonPhone,
                    TotalDebt, OverdueDebt, CreditLimit, RiskLevel, DefaultPaymentMethod, DefaultPaymentTermDays, 
                    DiscountRate, IsActive, IsBlocked, Notes)
                    VALUES ({customer.CompanyId}, '{customer.Name}', {(int)customer.CustomerType}, '{customer.TaxNumber}', 
                    '{customer.TaxOffice}', '{customer.Address}', '{customer.City}', '{customer.District}', 
                    '{customer.ZipCode}', '{customer.Phone}', '{customer.MobilePhone}', '{customer.Fax}', 
                    '{customer.Email}', '{customer.eFaturaEmail}', '{customer.Website}', '{customer.ContactPerson}', 
                    '{customer.ContactPersonPhone}', {customer.TotalDebt}, {customer.OverdueDebt}, {customer.CreditLimit}, 
                    {(int)customer.RiskLevel}, {(int)customer.DefaultPaymentMethod}, {customer.DefaultPaymentTermDays}, 
                    {customer.DiscountRate}, {(customer.IsActive ? 1 : 0)}, {(customer.IsBlocked ? 1 : 0)}, '{customer.Notes}')
                ";

                DatabaseManager.ExecuteQuery(query);

                // Son eklenen ID'yi al
                object result = DatabaseManager.ExecuteScalar("SELECT last_insert_rowid()");
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR] Müşteri eklenirken hata: {ex.Message}");
                throw;
            }
        }

        // ==================== METOD: MÜŞTERI GÜNCELLE ====================
        /// <summary>
        /// Mevcut bir müşteriyi günceller
        /// </summary>
        /// <param name="customer">Güncellenecek müşteri nesnesi</param>
        /// <returns>Başarılı ise true</returns>
        public static bool UpdateCustomer(Customer customer)
        {
            try
            {
                string query = $@"
                    UPDATE Customers SET 
                    Name = '{customer.Name}',
                    CustomerType = {(int)customer.CustomerType},
                    TaxNumber = '{customer.TaxNumber}',
                    TaxOffice = '{customer.TaxOffice}',
                    Address = '{customer.Address}',
                    City = '{customer.City}',
                    District = '{customer.District}',
                    ZipCode = '{customer.ZipCode}',
                    Phone = '{customer.Phone}',
                    MobilePhone = '{customer.MobilePhone}',
                    Fax = '{customer.Fax}',
                    Email = '{customer.Email}',
                    eFaturaEmail = '{customer.eFaturaEmail}',
                    Website = '{customer.Website}',
                    ContactPerson = '{customer.ContactPerson}',
                    ContactPersonPhone = '{customer.ContactPersonPhone}',
                    TotalDebt = {customer.TotalDebt},
                    OverdueDebt = {customer.OverdueDebt},
                    CreditLimit = {customer.CreditLimit},
                    RiskLevel = {(int)customer.RiskLevel},
                    DefaultPaymentMethod = {(int)customer.DefaultPaymentMethod},
                    DefaultPaymentTermDays = {customer.DefaultPaymentTermDays},
                    DiscountRate = {customer.DiscountRate},
                    IsActive = {(customer.IsActive ? 1 : 0)},
                    IsBlocked = {(customer.IsBlocked ? 1 : 0)},
                    UpdatedDate = CURRENT_TIMESTAMP,
                    Notes = '{customer.Notes}'
                    WHERE Id = {customer.Id}
                ";

                DatabaseManager.ExecuteQuery(query);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR] Müşteri güncellenirken hata: {ex.Message}");
                throw;
            }
        }

        // ==================== METOD: MÜŞTERI SİL ====================
        /// <summary>
        /// Bir müşteriyi siler
        /// </summary>
        /// <param name="customerId">Müşteri ID'si</param>
        /// <returns>Başarılı ise true</returns>
        public static bool DeleteCustomer(int customerId)
        {
            try
            {
                string query = $"DELETE FROM Customers WHERE Id = {customerId}";
                DatabaseManager.ExecuteQuery(query);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR] Müşteri silinirken hata: {ex.Message}");
                throw;
            }
        }

        // ==================== METOD: MÜŞTERI AL ====================
        /// <summary>
        /// ID'ye göre bir müşteriyi alır
        /// </summary>
        /// <param name="customerId">Müşteri ID'si</param>
        /// <returns>Müşteri nesnesi veya null</returns>
        public static Customer GetCustomer(int customerId)
        {
            try
            {
                string query = $"SELECT * FROM Customers WHERE Id = {customerId}";
                using (var reader = DatabaseManager.ExecuteReader(query))
                {
                    if (reader.Read())
                        return MapCustomer(reader);
                }
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR] Müşteri alınırken hata: {ex.Message}");
                throw;
            }
        }

        // ==================== METOD: TÜM MÜŞTERİLER AL ====================
        /// <summary>
        /// Şirkete ait tüm müşterileri alır
        /// </summary>
        /// <param name="companyId">Şirket ID'si</param>
        /// <returns>Müşteri listesi</returns>
        public static List<Customer> GetCustomersByCompany(int companyId)
        {
            var customers = new List<Customer>();
            try
            {
                string query = $"SELECT * FROM Customers WHERE CompanyId = {companyId} ORDER BY Name";
                using (var reader = DatabaseManager.ExecuteReader(query))
                {
                    while (reader.Read())
                        customers.Add(MapCustomer(reader));
                }
                return customers;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR] Müşteriler alınırken hata: {ex.Message}");
                throw;
            }
        }

        // ==================== METOD: MÜŞTERI ARA ====================
        /// <summary>
        /// Müşteri adına veya vergi numarasına göre arar
        /// </summary>
        /// <param name="companyId">Şirket ID'si</param>
        /// <param name="searchTerm">Arama terimi</param>
        /// <returns>Müşteri listesi</returns>
        public static List<Customer> SearchCustomers(int companyId, string searchTerm)
        {
            var customers = new List<Customer>();
            try
            {
                string query = $@"
                    SELECT * FROM Customers 
                    WHERE CompanyId = {companyId} AND 
                    (Name LIKE '%{searchTerm}%' OR TaxNumber LIKE '%{searchTerm}%' OR Email LIKE '%{searchTerm}%')
                    ORDER BY Name
                ";
                using (var reader = DatabaseManager.ExecuteReader(query))
                {
                    while (reader.Read())
                        customers.Add(MapCustomer(reader));
                }
                return customers;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR] Müşteri araması sırasında hata: {ex.Message}");
                throw;
            }
        }

        // ==================== METOD: MÜŞTERİ BORÇLARINI GÜNCELLE ====================
        /// <summary>
        /// Müşterinin toplam ve vadesi geçmiş borçlarını hesaplar ve günceller
        /// </summary>
        /// <param name="customerId">Müşteri ID'si</param>
        public static void UpdateCustomerDebt(int customerId)
        {
            try
            {
                // Toplam borç hesapla
                object totalDebtObj = DatabaseManager.ExecuteScalar(
                    $@"SELECT SUM(GrandTotal - PaidAmount) FROM Invoices 
                      WHERE CustomerId = {customerId} AND PaymentStatus != {(int)PaymentStatusEnum.Paid}"
                );

                decimal totalDebt = totalDebtObj != null && totalDebtObj != DBNull.Value 
                    ? Convert.ToDecimal(totalDebtObj) 
                    : 0m;

                // Vadesi geçmiş borç hesapla
                object overdueDebtObj = DatabaseManager.ExecuteScalar(
                    $@"SELECT SUM(GrandTotal - PaidAmount) FROM Invoices 
                      WHERE CustomerId = {customerId} AND DueDate < CURRENT_TIMESTAMP AND PaymentStatus != {(int)PaymentStatusEnum.Paid}"
                );

                decimal overdueDebt = overdueDebtObj != null && overdueDebtObj != DBNull.Value 
                    ? Convert.ToDecimal(overdueDebtObj) 
                    : 0m;

                // Müşteri risk seviyesini belirle
                int riskLevel = overdueDebt > 0 ? (int)CustomerRiskEnum.High : (int)CustomerRiskEnum.Low;

                // Güncelle
                string query = $@"
                    UPDATE Customers SET 
                    TotalDebt = {totalDebt},
                    OverdueDebt = {overdueDebt},
                    RiskLevel = {riskLevel},
                    UpdatedDate = CURRENT_TIMESTAMP
                    WHERE Id = {customerId}
                ";

                DatabaseManager.ExecuteQuery(query);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR] Müşteri borçları güncellenirken hata: {ex.Message}");
            }
        }

        // ==================== METOD: YÜKSEK RİSK MÜŞTERİLER ====================
        /// <summary>
        /// Yüksek risk seviyesindeki müşterileri alır
        /// </summary>
        /// <param name="companyId">Şirket ID'si</param>
        /// <returns>Yüksek risk müşterileri listesi</returns>
        public static List<Customer> GetHighRiskCustomers(int companyId)
        {
            var customers = new List<Customer>();
            try
            {
                string query = $@"
                    SELECT * FROM Customers 
                    WHERE CompanyId = {companyId} AND RiskLevel = {(int)CustomerRiskEnum.High}
                    ORDER BY OverdueDebt DESC
                ";
                using (var reader = DatabaseManager.ExecuteReader(query))
                {
                    while (reader.Read())
                        customers.Add(MapCustomer(reader));
                }
                return customers;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR] Yüksek risk müşterileri alınırken hata: {ex.Message}");
                throw;
            }
        }

        // ==================== HELPER: MÜŞTERİ HARITA ====================
        private static Customer MapCustomer(SQLiteDataReader reader)
        {
            return new Customer
            {
                Id = Convert.ToInt32(reader["Id"]),
                CompanyId = Convert.ToInt32(reader["CompanyId"]),
                Name = reader["Name"]?.ToString() ?? "",
                CustomerType = (CustomerTypeEnum)Convert.ToInt32(reader["CustomerType"]),
                TaxNumber = reader["TaxNumber"]?.ToString() ?? "",
                TaxOffice = reader["TaxOffice"]?.ToString() ?? "",
                Address = reader["Address"]?.ToString() ?? "",
                City = reader["City"]?.ToString() ?? "",
                District = reader["District"]?.ToString() ?? "",
                ZipCode = reader["ZipCode"]?.ToString() ?? "",
                Phone = reader["Phone"]?.ToString() ?? "",
                MobilePhone = reader["MobilePhone"]?.ToString() ?? "",
                Fax = reader["Fax"]?.ToString() ?? "",
                Email = reader["Email"]?.ToString() ?? "",
                eFaturaEmail = reader["eFaturaEmail"]?.ToString() ?? "",
                Website = reader["Website"]?.ToString() ?? "",
                ContactPerson = reader["ContactPerson"]?.ToString() ?? "",
                ContactPersonPhone = reader["ContactPersonPhone"]?.ToString() ?? "",
                TotalDebt = Convert.ToDecimal(reader["TotalDebt"] ?? 0m),
                OverdueDebt = Convert.ToDecimal(reader["OverdueDebt"] ?? 0m),
                CreditLimit = Convert.ToDecimal(reader["CreditLimit"] ?? 0m),
                RiskLevel = (CustomerRiskEnum)Convert.ToInt32(reader["RiskLevel"]),
                DefaultPaymentMethod = (PaymentMethodEnum)Convert.ToInt32(reader["DefaultPaymentMethod"]),
                DefaultPaymentTermDays = Convert.ToInt32(reader["DefaultPaymentTermDays"]),
                DiscountRate = Convert.ToDecimal(reader["DiscountRate"] ?? 0m),
                IsActive = Convert.ToInt32(reader["IsActive"]) == 1,
                IsBlocked = Convert.ToInt32(reader["IsBlocked"]) == 1,
                CreatedDate = DateTime.Parse(reader["CreatedDate"]?.ToString() ?? DateTime.Now.ToString()),
                UpdatedDate = reader["UpdatedDate"] != DBNull.Value ? DateTime.Parse(reader["UpdatedDate"].ToString()) : (DateTime?)null,
                Notes = reader["Notes"]?.ToString() ?? ""
            };
        }
    }
}
