using System;
using System.Collections.Generic;
using System.Data.SQLite;
using EasyInvoicePro.Database;
using EasyInvoicePro.Models;
using System.Linq;

namespace EasyInvoicePro.Services
{
    /// <summary>
    /// Fatura işlemlerini yönetir (CRUD operasyonları, hesaplamalar)
    /// </summary>
    public class InvoiceService
    {
        // ==================== METOD: FATURA EKLE ====================
        /// <summary>
        /// Yeni bir fatura ekler
        /// </summary>
        /// <param name="invoice">Fatura nesnesi</param>
        /// <returns>Eklenen faturanın ID'si</returns>
        public static int AddInvoice(Invoice invoice)
        {
            try
            {
                string query = $@"
                    INSERT INTO Invoices (CompanyId, CustomerId, InvoiceNumber, InvoiceDate, InvoiceType, DueDate,
                    SubTotal, DiscountAmount, TotalTax, GrandTotal, PaidAmount, PaymentStatus, Currency, PaymentMethod,
                    IsSentToEFatura, eFaturaStatus, eFaturaUUID, eFaturaXML, QRCode, Notes)
                    VALUES ({invoice.CompanyId}, {invoice.CustomerId}, '{invoice.InvoiceNumber}', '{invoice.InvoiceDate:yyyy-MM-dd HH:mm:ss}',
                    {(int)invoice.InvoiceType}, '{invoice.DueDate:yyyy-MM-dd HH:mm:ss}',
                    {invoice.SubTotal}, {invoice.DiscountAmount}, {invoice.TotalTax}, {invoice.GrandTotal},
                    {invoice.PaidAmount}, {(int)invoice.PaymentStatus}, '{invoice.Currency}', {(int)invoice.PaymentMethod},
                    {(invoice.IsSentToEFatura ? 1 : 0)}, {(int)invoice.eFaturaStatus}, '{invoice.eFaturaUUID}',
                    '{invoice.eFaturaXML}', '{invoice.QRCode}', '{invoice.Notes}')
                ";

                DatabaseManager.ExecuteQuery(query);
                object result = DatabaseManager.ExecuteScalar("SELECT last_insert_rowid()");
                int invoiceId = Convert.ToInt32(result);

                // Fatura kalemleri ekle
                if (invoice.Items != null && invoice.Items.Count > 0)
                {
                    int lineNumber = 1;
                    foreach (var item in invoice.Items)
                    {
                        item.InvoiceId = invoiceId;
                        item.LineNumber = lineNumber++;
                        AddInvoiceItem(item);
                    }
                }

                return invoiceId;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR] Fatura eklenirken hata: {ex.Message}");
                throw;
            }
        }

        // ==================== METOD: FATURA KALEMİ EKLE ====================
        public static int AddInvoiceItem(InvoiceItem item)
        {
            try
            {
                string query = $@"
                    INSERT INTO InvoiceItems (InvoiceId, ProductId, Description, Quantity, UnitPrice, Unit,
                    DiscountRate, DiscountAmount, TaxRate, LineTotal, LineTotalWithTax, LineTaxAmount, LineNumber, Notes)
                    VALUES ({item.InvoiceId}, {(item.ProductId.HasValue ? item.ProductId.Value : 0)}, '{item.Description}',
                    {item.Quantity}, {item.UnitPrice}, '{item.Unit}',
                    {item.DiscountRate}, {item.DiscountAmount}, {item.TaxRate}, {item.LineTotal},
                    {item.LineTotalWithTax}, {item.LineTaxAmount}, {item.LineNumber}, '{item.Notes}')
                ";

                DatabaseManager.ExecuteQuery(query);
                object result = DatabaseManager.ExecuteScalar("SELECT last_insert_rowid()");
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR] Fatura kalemi eklenirken hata: {ex.Message}");
                throw;
            }
        }

        // ==================== METOD: FATURA GÜNCELLE ====================
        public static bool UpdateInvoice(Invoice invoice)
        {
            try
            {
                string query = $@"
                    UPDATE Invoices SET
                    InvoiceNumber = '{invoice.InvoiceNumber}',
                    InvoiceDate = '{invoice.InvoiceDate:yyyy-MM-dd HH:mm:ss}',
                    InvoiceType = {(int)invoice.InvoiceType},
                    DueDate = '{invoice.DueDate:yyyy-MM-dd HH:mm:ss}',
                    SubTotal = {invoice.SubTotal},
                    DiscountAmount = {invoice.DiscountAmount},
                    TotalTax = {invoice.TotalTax},
                    GrandTotal = {invoice.GrandTotal},
                    PaidAmount = {invoice.PaidAmount},
                    PaymentStatus = {(int)invoice.PaymentStatus},
                    Currency = '{invoice.Currency}',
                    PaymentMethod = {(int)invoice.PaymentMethod},
                    IsSentToEFatura = {(invoice.IsSentToEFatura ? 1 : 0)},
                    eFaturaStatus = {(int)invoice.eFaturaStatus},
                    eFaturaUUID = '{invoice.eFaturaUUID}',
                    QRCode = '{invoice.QRCode}',
                    Notes = '{invoice.Notes}',
                    UpdatedDate = CURRENT_TIMESTAMP
                    WHERE Id = {invoice.Id}
                ";

                DatabaseManager.ExecuteQuery(query);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR] Fatura güncellenirken hata: {ex.Message}");
                throw;
            }
        }

        // ==================== METOD: FATURA SİL ====================
        public static bool DeleteInvoice(int invoiceId)
        {
            try
            {
                string query = $"DELETE FROM Invoices WHERE Id = {invoiceId}";
                DatabaseManager.ExecuteQuery(query);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR] Fatura silinirken hata: {ex.Message}");
                throw;
            }
        }

        // ==================== METOD: FATURA AL ====================
        public static Invoice GetInvoice(int invoiceId)
        {
            try
            {
                string query = $"SELECT * FROM Invoices WHERE Id = {invoiceId}";
                using (var reader = DatabaseManager.ExecuteReader(query))
                {
                    if (reader.Read())
                    {
                        var invoice = MapInvoice(reader);
                        invoice.Items = GetInvoiceItems(invoiceId);
                        invoice.Payments = GetPayments(invoiceId);
                        return invoice;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR] Fatura alınırken hata: {ex.Message}");
                throw;
            }
        }

        // ==================== METOD: TÜM FATURALAR AL ====================
        public static List<Invoice> GetInvoicesByCompany(int companyId, DateTime? startDate = null, DateTime? endDate = null)
        {
            var invoices = new List<Invoice>();
            try
            {
                string dateFilter = "";
                if (startDate.HasValue && endDate.HasValue)
                {
                    dateFilter = $" AND InvoiceDate BETWEEN '{startDate:yyyy-MM-dd}' AND '{endDate:yyyy-MM-dd}'";
                }

                string query = $@"
                    SELECT * FROM Invoices 
                    WHERE CompanyId = {companyId}{dateFilter}
                    ORDER BY InvoiceDate DESC
                ";
                using (var reader = DatabaseManager.ExecuteReader(query))
                {
                    while (reader.Read())
                        invoices.Add(MapInvoice(reader));
                }
                return invoices;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR] Faturalar alınırken hata: {ex.Message}");
                throw;
            }
        }

        // ==================== METOD: FATURA KALEMLERI AL ====================
        public static List<InvoiceItem> GetInvoiceItems(int invoiceId)
        {
            var items = new List<InvoiceItem>();
            try
            {
                string query = $"SELECT * FROM InvoiceItems WHERE InvoiceId = {invoiceId} ORDER BY LineNumber";
                using (var reader = DatabaseManager.ExecuteReader(query))
                {
                    while (reader.Read())
                        items.Add(MapInvoiceItem(reader));
                }
                return items;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR] Fatura kalemleri alınırken hata: {ex.Message}");
                throw;
            }
        }

        // ==================== METOD: ÖDEME KAYITLARI AL ====================
        public static List<Payment> GetPayments(int invoiceId)
        {
            var payments = new List<Payment>();
            try
            {
                string query = $"SELECT * FROM Payments WHERE InvoiceId = {invoiceId} ORDER BY PaymentDate DESC";
                using (var reader = DatabaseManager.ExecuteReader(query))
                {
                    while (reader.Read())
                        payments.Add(MapPayment(reader));
                }
                return payments;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR] Ödeme kayitları alınırken hata: {ex.Message}");
                throw;
            }
        }

        // ==================== METOD: ÖDEME EKLE ====================
        public static int AddPayment(Payment payment)
        {
            try
            {
                string query = $@"
                    INSERT INTO Payments (InvoiceId, PaymentDate, Amount, PaymentMethod, ReferenceNumber, Notes)
                    VALUES ({payment.InvoiceId}, '{payment.PaymentDate:yyyy-MM-dd HH:mm:ss}', {payment.Amount},
                    {(int)payment.PaymentMethod}, '{payment.ReferenceNumber}', '{payment.Notes}')
                ";

                DatabaseManager.ExecuteQuery(query);

                // Faturanın ödeme durumunu güncelle
                UpdateInvoicePaymentStatus(payment.InvoiceId);

                object result = DatabaseManager.ExecuteScalar("SELECT last_insert_rowid()");
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR] Ödeme eklenirken hata: {ex.Message}");
                throw;
            }
        }

        // ==================== METOD: ÖDEME DURUMUNU GÜNCELLE ====================
        public static void UpdateInvoicePaymentStatus(int invoiceId)
        {
            try
            {
                // Faturanın detaylarını al
                object totalObj = DatabaseManager.ExecuteScalar($"SELECT GrandTotal FROM Invoices WHERE Id = {invoiceId}");
                object paidObj = DatabaseManager.ExecuteScalar($"SELECT SUM(Amount) FROM Payments WHERE InvoiceId = {invoiceId}");

                if (totalObj == null || totalObj == DBNull.Value) return;

                decimal total = Convert.ToDecimal(totalObj);
                decimal paid = paidObj != null && paidObj != DBNull.Value ? Convert.ToDecimal(paidObj) : 0m;

                int status = (int)PaymentStatusEnum.Pending;
                if (paid >= total)
                    status = (int)PaymentStatusEnum.Paid;
                else if (paid > 0)
                    status = (int)PaymentStatusEnum.Partial;

                // Ödeme durumunu güncelle
                string query = $@"
                    UPDATE Invoices SET
                    PaidAmount = {paid},
                    PaymentStatus = {status},
                    UpdatedDate = CURRENT_TIMESTAMP
                    WHERE Id = {invoiceId}
                ";

                DatabaseManager.ExecuteQuery(query);

                // Müşteri borçlarını güncelle
                object customerIdObj = DatabaseManager.ExecuteScalar($"SELECT CustomerId FROM Invoices WHERE Id = {invoiceId}");
                if (customerIdObj != null)
                {
                    int customerId = Convert.ToInt32(customerIdObj);
                    CustomerService.UpdateCustomerDebt(customerId);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR] Ödeme durumu güncellenirken hata: {ex.Message}");
            }
        }

        // ==================== METOD: FATURA NUMARASİ OLUSTUR ====================
        public static string GenerateInvoiceNumber(int companyId)
        {
            try
            {
                // Son fatura numarasını al
                object lastNo = DatabaseManager.ExecuteScalar(
                    $@"SELECT InvoiceNumber FROM Invoices WHERE CompanyId = {companyId} 
                       ORDER BY CreatedDate DESC LIMIT 1"
                );

                if (lastNo == null || lastNo == DBNull.Value)
                    return $"FTR-{DateTime.Now:yyyy}-001";

                string lastInvoice = lastNo.ToString();
                // Son no'dan 1 arttır
                string[] parts = lastInvoice.Split('-');
                if (parts.Length >= 3 && int.TryParse(parts[2], out int lastNum))
                {
                    return $"{parts[0]}-{parts[1]}-{(lastNum + 1):000}";
                }

                return $"FTR-{DateTime.Now:yyyy}-001";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR] Fatura numarası oluşturulurken hata: {ex.Message}");
                return $"FTR-{DateTime.Now:yyyy}-001";
            }
        }

        // ==================== HELPER: FATURA HARITA ====================
        private static Invoice MapInvoice(SQLiteDataReader reader)
        {
            return new Invoice
            {
                Id = Convert.ToInt32(reader["Id"]),
                CompanyId = Convert.ToInt32(reader["CompanyId"]),
                CustomerId = Convert.ToInt32(reader["CustomerId"]),
                InvoiceNumber = reader["InvoiceNumber"]?.ToString() ?? "",
                InvoiceDate = DateTime.Parse(reader["InvoiceDate"]?.ToString() ?? DateTime.Now.ToString()),
                InvoiceType = (InvoiceTypeEnum)Convert.ToInt32(reader["InvoiceType"]),
                DueDate = DateTime.Parse(reader["DueDate"]?.ToString() ?? DateTime.Now.ToString()),
                SubTotal = Convert.ToDecimal(reader["SubTotal"] ?? 0m),
                DiscountAmount = Convert.ToDecimal(reader["DiscountAmount"] ?? 0m),
                TotalTax = Convert.ToDecimal(reader["TotalTax"] ?? 0m),
                GrandTotal = Convert.ToDecimal(reader["GrandTotal"] ?? 0m),
                PaidAmount = Convert.ToDecimal(reader["PaidAmount"] ?? 0m),
                PaymentStatus = (PaymentStatusEnum)Convert.ToInt32(reader["PaymentStatus"]),
                Currency = reader["Currency"]?.ToString() ?? "TRY",
                PaymentMethod = (PaymentMethodEnum)Convert.ToInt32(reader["PaymentMethod"]),
                IsSentToEFatura = Convert.ToInt32(reader["IsSentToEFatura"]) == 1,
                eFaturaStatus = (eFaturaStatusEnum)Convert.ToInt32(reader["eFaturaStatus"]),
                eFaturaUUID = reader["eFaturaUUID"]?.ToString() ?? "",
                eFaturaXML = reader["eFaturaXML"]?.ToString() ?? "",
                QRCode = reader["QRCode"]?.ToString() ?? "",
                Notes = reader["Notes"]?.ToString() ?? "",
                CreatedDate = DateTime.Parse(reader["CreatedDate"]?.ToString() ?? DateTime.Now.ToString()),
                UpdatedDate = reader["UpdatedDate"] != DBNull.Value ? DateTime.Parse(reader["UpdatedDate"].ToString()) : (DateTime?)null
            };
        }

        // ==================== HELPER: FATURA KALEMİ HARITA ====================
        private static InvoiceItem MapInvoiceItem(SQLiteDataReader reader)
        {
            return new InvoiceItem
            {
                Id = Convert.ToInt32(reader["Id"]),
                InvoiceId = Convert.ToInt32(reader["InvoiceId"]),
                ProductId = reader["ProductId"] != DBNull.Value ? Convert.ToInt32(reader["ProductId"]) : (int?)null,
                Description = reader["Description"]?.ToString() ?? "",
                Quantity = Convert.ToDecimal(reader["Quantity"]),
                UnitPrice = Convert.ToDecimal(reader["UnitPrice"]),
                Unit = reader["Unit"]?.ToString() ?? "Adet",
                DiscountRate = Convert.ToDecimal(reader["DiscountRate"] ?? 0m),
                DiscountAmount = Convert.ToDecimal(reader["DiscountAmount"] ?? 0m),
                TaxRate = Convert.ToDecimal(reader["TaxRate"] ?? 20m),
                LineTotal = Convert.ToDecimal(reader["LineTotal"]),
                LineTotalWithTax = Convert.ToDecimal(reader["LineTotalWithTax"]),
                LineTaxAmount = Convert.ToDecimal(reader["LineTaxAmount"]),
                LineNumber = Convert.ToInt32(reader["LineNumber"]),
                Notes = reader["Notes"]?.ToString() ?? ""
            };
        }

        // ==================== HELPER: ÖDEME HARITA ====================
        private static Payment MapPayment(SQLiteDataReader reader)
        {
            return new Payment
            {
                Id = Convert.ToInt32(reader["Id"]),
                InvoiceId = Convert.ToInt32(reader["InvoiceId"]),
                PaymentDate = DateTime.Parse(reader["PaymentDate"]?.ToString() ?? DateTime.Now.ToString()),
                Amount = Convert.ToDecimal(reader["Amount"]),
                PaymentMethod = (PaymentMethodEnum)Convert.ToInt32(reader["PaymentMethod"]),
                ReferenceNumber = reader["ReferenceNumber"]?.ToString() ?? "",
                Notes = reader["Notes"]?.ToString() ?? "",
                CreatedDate = DateTime.Parse(reader["CreatedDate"]?.ToString() ?? DateTime.Now.ToString())
            };
        }
    }
}
