using System;
using System.Collections.Generic;
using System.Data.SQLite;
using EasyInvoicePro.Database;
using EasyInvoicePro.Models;

namespace EasyInvoicePro.Services
{
    /// <summary>
    /// Ürün işlemlerini yönetir (CRUD operasyonları)
    /// </summary>
    public class ProductService
    {
        // ==================== METOD: ÜRÜN EKLE ====================
        public static int AddProduct(Product product)
        {
            try
            {
                string query = $@"
                    INSERT INTO Products (CompanyId, Code, Barcode, Name, Description, Category, Unit, 
                    SalesPrice, CostPrice, KDVRate, StockQuantity, MinimumStock, MaximumStock, 
                    ImagePath, Supplier, IsActive, Notes)
                    VALUES ({product.CompanyId}, '{product.Code}', '{product.Barcode}', '{product.Name}', 
                    '{product.Description}', '{product.Category}', '{product.Unit}', {product.SalesPrice}, 
                    {product.CostPrice}, {product.KDVRate}, {product.StockQuantity}, {product.MinimumStock}, 
                    {product.MaximumStock}, '{product.ImagePath}', '{product.Supplier}', 
                    {(product.IsActive ? 1 : 0)}, '{product.Notes}')
                ";

                DatabaseManager.ExecuteQuery(query);
                object result = DatabaseManager.ExecuteScalar("SELECT last_insert_rowid()");
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR] Ürün eklenirken hata: {ex.Message}");
                throw;
            }
        }

        // ==================== METOD: ÜRÜN GÜNCELLE ====================
        public static bool UpdateProduct(Product product)
        {
            try
            {
                string query = $@"
                    UPDATE Products SET 
                    Code = '{product.Code}',
                    Barcode = '{product.Barcode}',
                    Name = '{product.Name}',
                    Description = '{product.Description}',
                    Category = '{product.Category}',
                    Unit = '{product.Unit}',
                    SalesPrice = {product.SalesPrice},
                    CostPrice = {product.CostPrice},
                    KDVRate = {product.KDVRate},
                    StockQuantity = {product.StockQuantity},
                    MinimumStock = {product.MinimumStock},
                    MaximumStock = {product.MaximumStock},
                    ImagePath = '{product.ImagePath}',
                    Supplier = '{product.Supplier}',
                    IsActive = {(product.IsActive ? 1 : 0)},
                    UpdatedDate = CURRENT_TIMESTAMP,
                    Notes = '{product.Notes}'
                    WHERE Id = {product.Id}
                ";

                DatabaseManager.ExecuteQuery(query);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR] Ürün güncellenirken hata: {ex.Message}");
                throw;
            }
        }

        // ==================== METOD: ÜRÜN SİL ====================
        public static bool DeleteProduct(int productId)
        {
            try
            {
                string query = $"DELETE FROM Products WHERE Id = {productId}";
                DatabaseManager.ExecuteQuery(query);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR] Ürün silinirken hata: {ex.Message}");
                throw;
            }
        }

        // ==================== METOD: ÜRÜN AL ====================
        public static Product GetProduct(int productId)
        {
            try
            {
                string query = $"SELECT * FROM Products WHERE Id = {productId}";
                using (var reader = DatabaseManager.ExecuteReader(query))
                {
                    if (reader.Read())
                        return MapProduct(reader);
                }
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR] Ürün alınırken hata: {ex.Message}");
                throw;
            }
        }

        // ==================== METOD: BARKODA GÖRE ÜRÜN AL ====================
        public static Product GetProductByBarcode(string barcode, int companyId)
        {
            try
            {
                string query = $"SELECT * FROM Products WHERE Barcode = '{barcode}' AND CompanyId = {companyId}";
                using (var reader = DatabaseManager.ExecuteReader(query))
                {
                    if (reader.Read())
                        return MapProduct(reader);
                }
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR] Ürün alınırken hata: {ex.Message}");
                throw;
            }
        }

        // ==================== METOD: TÜM ÜRÜNLER AL ====================
        public static List<Product> GetProductsByCompany(int companyId)
        {
            var products = new List<Product>();
            try
            {
                string query = $"SELECT * FROM Products WHERE CompanyId = {companyId} AND IsActive = 1 ORDER BY Name";
                using (var reader = DatabaseManager.ExecuteReader(query))
                {
                    while (reader.Read())
                        products.Add(MapProduct(reader));
                }
                return products;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR] Ürünler alınırken hata: {ex.Message}");
                throw;
            }
        }

        // ==================== METOD: ÜRÜN ARA ====================
        public static List<Product> SearchProducts(int companyId, string searchTerm)
        {
            var products = new List<Product>();
            try
            {
                string query = $@"
                    SELECT * FROM Products 
                    WHERE CompanyId = {companyId} AND IsActive = 1 AND 
                    (Name LIKE '%{searchTerm}%' OR Code LIKE '%{searchTerm}%' OR Barcode LIKE '%{searchTerm}%')
                    ORDER BY Name
                ";
                using (var reader = DatabaseManager.ExecuteReader(query))
                {
                    while (reader.Read())
                        products.Add(MapProduct(reader));
                }
                return products;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR] Ürün araması sırasında hata: {ex.Message}");
                throw;
            }
        }

        // ==================== METOD: DÜŞÜK STOK ÜRÜNLER ====================
        public static List<Product> GetLowStockProducts(int companyId)
        {
            var products = new List<Product>();
            try
            {
                string query = $@"
                    SELECT * FROM Products 
                    WHERE CompanyId = {companyId} AND IsActive = 1 AND StockQuantity <= MinimumStock
                    ORDER BY StockQuantity
                ";
                using (var reader = DatabaseManager.ExecuteReader(query))
                {
                    while (reader.Read())
                        products.Add(MapProduct(reader));
                }
                return products;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR] Düşük stok ürünleri alınırken hata: {ex.Message}");
                throw;
            }
        }

        // ==================== METOD: STOK GÜNCELLE ====================
        public static void UpdateStock(int productId, decimal quantityChange)
        {
            try
            {
                string query = $@"
                    UPDATE Products SET 
                    StockQuantity = StockQuantity + {quantityChange},
                    UpdatedDate = CURRENT_TIMESTAMP
                    WHERE Id = {productId}
                ";

                DatabaseManager.ExecuteQuery(query);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR] Stok güncellenirken hata: {ex.Message}");
            }
        }

        // ==================== HELPER: ÜRÜN HARITA ====================
        private static Product MapProduct(SQLiteDataReader reader)
        {
            return new Product
            {
                Id = Convert.ToInt32(reader["Id"]),
                CompanyId = Convert.ToInt32(reader["CompanyId"]),
                Code = reader["Code"]?.ToString() ?? "",
                Barcode = reader["Barcode"]?.ToString() ?? "",
                Name = reader["Name"]?.ToString() ?? "",
                Description = reader["Description"]?.ToString() ?? "",
                Category = reader["Category"]?.ToString() ?? "",
                Unit = reader["Unit"]?.ToString() ?? "Adet",
                SalesPrice = Convert.ToDecimal(reader["SalesPrice"] ?? 0m),
                CostPrice = Convert.ToDecimal(reader["CostPrice"] ?? 0m),
                KDVRate = Convert.ToDecimal(reader["KDVRate"] ?? 20m),
                StockQuantity = Convert.ToDecimal(reader["StockQuantity"] ?? 0m),
                MinimumStock = Convert.ToDecimal(reader["MinimumStock"] ?? 0m),
                MaximumStock = Convert.ToDecimal(reader["MaximumStock"] ?? 0m),
                ImagePath = reader["ImagePath"]?.ToString() ?? "",
                Supplier = reader["Supplier"]?.ToString() ?? "",
                IsActive = Convert.ToInt32(reader["IsActive"]) == 1,
                CreatedDate = DateTime.Parse(reader["CreatedDate"]?.ToString() ?? DateTime.Now.ToString()),
                UpdatedDate = reader["UpdatedDate"] != DBNull.Value ? DateTime.Parse(reader["UpdatedDate"].ToString()) : (DateTime?)null,
                Notes = reader["Notes"]?.ToString() ?? ""
            };
        }
    }
}
