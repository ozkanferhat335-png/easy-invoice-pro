using System;
using System.Collections.Generic;
using System.Data.SQLite;
using EasyInvoicePro.Database;
using EasyInvoicePro.Models;
using EasyInvoicePro.Core;

namespace EasyInvoicePro.Services
{
    /// <summary>
    /// KDV beyannamesi işlemlerini yönetir
    /// </summary>
    public class KDVService
    {
        // ==================== METOD: KDV BEYANNAMESI HESAPLA ====================
        /// <summary>
        /// Belirtilen dönem için KDV beyannamesi hesaplar
        /// </summary>
        /// <param name="companyId">Şirket ID'si</param>
        /// <param name="year">Yıl</param>
        /// <param name="month">Ay</param>
        /// <returns>Hesaplanan KDV Beyannamesi nesnesi</returns>
        public static KDVDeclaration CalculateKDVDeclaration(int companyId, int year, int month)
        {
            try
            {
                var declaration = new KDVDeclaration
                {
                    CompanyId = companyId,
                    Year = year,
                    Month = month,
                    PeriodStartDate = new DateTime(year, month, 1),
                    PeriodEndDate = new DateTime(year, month, DateTime.DaysInMonth(year, month)),
                    CalculationDate = DateTime.Now
                };

                // Faturaları al
                string startDate = $"{year:0000}-{month:00}-01";
                string endDate = $"{year:0000}-{month:00}-{DateTime.DaysInMonth(year, month)}";

                string query = $@"
                    SELECT 
                        ii.TaxRate,
                        ii.LineTotal,
                        ii.LineTaxAmount
                    FROM InvoiceItems ii
                    INNER JOIN Invoices i ON ii.InvoiceId = i.Id
                    WHERE i.CompanyId = {companyId}
                    AND i.InvoiceDate BETWEEN '{startDate}' AND '{endDate}'
                    AND i.InvoiceType = {(int)InvoiceTypeEnum.Sales}
                ";

                using (var reader = DatabaseManager.ExecuteReader(query))
                {
                    while (reader.Read())
                    {
                        decimal taxRate = Convert.ToDecimal(reader["TaxRate"]);
                        decimal lineTotal = Convert.ToDecimal(reader["LineTotal"]);
                        decimal taxAmount = Convert.ToDecimal(reader["LineTaxAmount"]);

                        // KDV oranına göre sınıflandır
                        if (taxRate == Constants.KDV_RATE_20)
                        {
                            declaration.KDV1_20_Basis += lineTotal;
                            declaration.KDV1_20_Tax += taxAmount;
                        }
                        else if (taxRate == Constants.KDV_RATE_10)
                        {
                            declaration.KDV1_10_Basis += lineTotal;
                            declaration.KDV1_10_Tax += taxAmount;
                        }
                        else if (taxRate == Constants.KDV_RATE_5)
                        {
                            declaration.KDV1_5_Basis += lineTotal;
                            declaration.KDV1_5_Tax += taxAmount;
                        }
                        else if (taxRate == Constants.KDV_RATE_1)
                        {
                            declaration.KDV1_1_Basis += lineTotal;
                            declaration.KDV1_1_Tax += taxAmount;
                        }
                    }
                }

                // Toplam KDV hesapla
                declaration.TotalKDV = declaration.KDV1_20_Tax + declaration.KDV1_10_Tax + 
                                       declaration.KDV1_5_Tax + declaration.KDV1_1_Tax;

                return declaration;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR] KDV Beyannamesi hesaplanırken hata: {ex.Message}");
                throw;
            }
        }

        // ==================== METOD: KDV BEYANNAMESI KAYDET ====================
        /// <summary>
        /// KDV Beyannamesi nı veritabanına kaydeder
        /// </summary>
        /// <param name="declaration">KDV Beyannamesi nesnesi</param>
        /// <returns>Kaydedilen beyanname ID'si</returns>
        public static int SaveKDVDeclaration(KDVDeclaration declaration)
        {
            try
            {
                // Önce var mı kontrol et
                object existingId = DatabaseManager.ExecuteScalar(
                    $@"SELECT Id FROM KDVDeclarations 
                       WHERE CompanyId = {declaration.CompanyId} AND Year = {declaration.Year} AND Month = {declaration.Month}"
                );

                if (existingId != null && existingId != DBNull.Value)
                {
                    // Güncelle
                    declaration.Id = Convert.ToInt32(existingId);
                    UpdateKDVDeclaration(declaration);
                    return declaration.Id;
                }
                else
                {
                    // Yeni ekle
                    string query = $@"
                        INSERT INTO KDVDeclarations (CompanyId, Year, Month, KDV1_20_Basis, KDV1_20_Tax, 
                        KDV1_10_Basis, KDV1_10_Tax, KDV1_5_Basis, KDV1_5_Tax, KDV1_1_Basis, KDV1_1_Tax,
                        TotalKDV, PeriodStartDate, PeriodEndDate, IsSubmitted, Notes)
                        VALUES ({declaration.CompanyId}, {declaration.Year}, {declaration.Month},
                        {declaration.KDV1_20_Basis}, {declaration.KDV1_20_Tax},
                        {declaration.KDV1_10_Basis}, {declaration.KDV1_10_Tax},
                        {declaration.KDV1_5_Basis}, {declaration.KDV1_5_Tax},
                        {declaration.KDV1_1_Basis}, {declaration.KDV1_1_Tax},
                        {declaration.TotalKDV}, '{declaration.PeriodStartDate:yyyy-MM-dd}',
                        '{declaration.PeriodEndDate:yyyy-MM-dd}', 0, '{declaration.Notes}')
                    ";

                    DatabaseManager.ExecuteQuery(query);
                    object result = DatabaseManager.ExecuteScalar("SELECT last_insert_rowid()");
                    return Convert.ToInt32(result);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR] KDV Beyannamesi kaydedilirken hata: {ex.Message}");
                throw;
            }
        }

        // ==================== METOD: KDV BEYANNAMESI GÜNCELLE ====================
        public static bool UpdateKDVDeclaration(KDVDeclaration declaration)
        {
            try
            {
                string query = $@"
                    UPDATE KDVDeclarations SET
                    KDV1_20_Basis = {declaration.KDV1_20_Basis},
                    KDV1_20_Tax = {declaration.KDV1_20_Tax},
                    KDV1_10_Basis = {declaration.KDV1_10_Basis},
                    KDV1_10_Tax = {declaration.KDV1_10_Tax},
                    KDV1_5_Basis = {declaration.KDV1_5_Basis},
                    KDV1_5_Tax = {declaration.KDV1_5_Tax},
                    KDV1_1_Basis = {declaration.KDV1_1_Basis},
                    KDV1_1_Tax = {declaration.KDV1_1_Tax},
                    TotalKDV = {declaration.TotalKDV},
                    IsSubmitted = {(declaration.IsSubmitted ? 1 : 0)},
                    SubmissionDate = {(declaration.IsSubmitted ? $"'{declaration.SubmissionDate:yyyy-MM-dd}" : "NULL")},
                    Notes = '{declaration.Notes}'
                    WHERE Id = {declaration.Id}
                ";

                DatabaseManager.ExecuteQuery(query);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR] KDV Beyannamesi güncellenirken hata: {ex.Message}");
                throw;
            }
        }

        // ==================== METOD: KDV BEYANNAMESI AL ====================
        public static KDVDeclaration GetKDVDeclaration(int companyId, int year, int month)
        {
            try
            {
                string query = $@"
                    SELECT * FROM KDVDeclarations
                    WHERE CompanyId = {companyId} AND Year = {year} AND Month = {month}
                ";

                using (var reader = DatabaseManager.ExecuteReader(query))
                {
                    if (reader.Read())
                        return MapKDVDeclaration(reader);
                }
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR] KDV Beyannamesi alınırken hata: {ex.Message}");
                throw;
            }
        }

        // ==================== METOD: KDV BEYANNAMELERI AL ====================
        public static List<KDVDeclaration> GetKDVDeclarations(int companyId, int year)
        {
            var declarations = new List<KDVDeclaration>();
            try
            {
                string query = $@"
                    SELECT * FROM KDVDeclarations
                    WHERE CompanyId = {companyId} AND Year = {year}
                    ORDER BY Month
                ";

                using (var reader = DatabaseManager.ExecuteReader(query))
                {
                    while (reader.Read())
                        declarations.Add(MapKDVDeclaration(reader));
                }
                return declarations;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR] KDV Beyannameleri alınırken hata: {ex.Message}");
                throw;
            }
        }

        // ==================== HELPER: KDV BEYANNAMESI HARITA ====================
        private static KDVDeclaration MapKDVDeclaration(SQLiteDataReader reader)
        {
            return new KDVDeclaration
            {
                Id = Convert.ToInt32(reader["Id"]),
                CompanyId = Convert.ToInt32(reader["CompanyId"]),
                Year = Convert.ToInt32(reader["Year"]),
                Month = Convert.ToInt32(reader["Month"]),
                KDV1_20_Basis = Convert.ToDecimal(reader["KDV1_20_Basis"]),
                KDV1_20_Tax = Convert.ToDecimal(reader["KDV1_20_Tax"]),
                KDV1_10_Basis = Convert.ToDecimal(reader["KDV1_10_Basis"]),
                KDV1_10_Tax = Convert.ToDecimal(reader["KDV1_10_Tax"]),
                KDV1_5_Basis = Convert.ToDecimal(reader["KDV1_5_Basis"]),
                KDV1_5_Tax = Convert.ToDecimal(reader["KDV1_5_Tax"]),
                KDV1_1_Basis = Convert.ToDecimal(reader["KDV1_1_Basis"]),
                KDV1_1_Tax = Convert.ToDecimal(reader["KDV1_1_Tax"]),
                DiscountedSales = Convert.ToDecimal(reader["DiscountedSales"]),
                TotalKDV = Convert.ToDecimal(reader["TotalKDV"]),
                PeriodStartDate = DateTime.Parse(reader["PeriodStartDate"].ToString()),
                PeriodEndDate = DateTime.Parse(reader["PeriodEndDate"].ToString()),
                CalculationDate = DateTime.Parse(reader["CalculationDate"].ToString()),
                IsSubmitted = Convert.ToInt32(reader["IsSubmitted"]) == 1,
                SubmissionDate = reader["SubmissionDate"] != DBNull.Value ? DateTime.Parse(reader["SubmissionDate"].ToString()) : (DateTime?)null,
                Notes = reader["Notes"]?.ToString() ?? ""
            };
        }
    }
}
