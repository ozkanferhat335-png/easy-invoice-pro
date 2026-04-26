using System;
using System.Text;

namespace EasyInvoicePro.Database
{
    /// <summary>
    /// Veritabanı Şemasını oluşturan SQL scriptı
    /// </summary>
    public static class DatabaseInitializer
    {
        /// <summary>
        /// Tüm tabloların CREATE scriptıni döner
        /// </summary>
        /// <returns>SQL CREATE scriptı</returns>
        public static string GetCreateTableScript()
        {
            var sb = new StringBuilder();

            // ==================== COMPANIES TABLE ====================
            sb.Append(@"
CREATE TABLE IF NOT EXISTS Companies (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    TaxNumber TEXT NOT NULL UNIQUE,
    LegalName TEXT NOT NULL,
    Address TEXT,
    City TEXT,
    District TEXT,
    ZipCode TEXT,
    Phone TEXT,
    Fax TEXT,
    Email TEXT,
    Website TEXT,
    eFaturaAlias TEXT,
    eFaturaPassword TEXT,
    CertificatePath TEXT,
    CertificatePassword TEXT,
    CertificateStartDate DATETIME,
    CertificateEndDate DATETIME,
    AccountantName TEXT,
    AccountantPhone TEXT,
    AccountantEmail TEXT,
    DefaultKDVRate DECIMAL DEFAULT 20.00,
    DefaultCurrency TEXT DEFAULT 'TRY',
    DefaultPaymentTermDays INTEGER DEFAULT 30,
    LogoPath TEXT,
    LowStockWarningLevel INTEGER DEFAULT 10,
    IsActive INTEGER DEFAULT 1,
    CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedDate DATETIME,
    Notes TEXT
);
CREATE INDEX IF NOT EXISTS IX_Companies_TaxNumber ON Companies(TaxNumber);
");

            // ==================== CUSTOMERS TABLE ====================
            sb.Append(@"
CREATE TABLE IF NOT EXISTS Customers (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    CompanyId INTEGER NOT NULL,
    Name TEXT NOT NULL,
    CustomerType INTEGER NOT NULL DEFAULT 0,
    TaxNumber TEXT NOT NULL,
    TaxOffice TEXT,
    Address TEXT,
    City TEXT,
    District TEXT,
    ZipCode TEXT,
    Phone TEXT,
    MobilePhone TEXT,
    Fax TEXT,
    Email TEXT,
    eFaturaEmail TEXT,
    Website TEXT,
    ContactPerson TEXT,
    ContactPersonPhone TEXT,
    TotalDebt DECIMAL DEFAULT 0.00,
    OverdueDebt DECIMAL DEFAULT 0.00,
    CreditLimit DECIMAL DEFAULT 0.00,
    RiskLevel INTEGER DEFAULT 0,
    DefaultPaymentMethod INTEGER DEFAULT 0,
    DefaultPaymentTermDays INTEGER DEFAULT 0,
    DiscountRate DECIMAL DEFAULT 0.00,
    IsActive INTEGER DEFAULT 1,
    IsBlocked INTEGER DEFAULT 0,
    CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedDate DATETIME,
    Notes TEXT,
    FOREIGN KEY (CompanyId) REFERENCES Companies(Id) ON DELETE CASCADE
);
CREATE INDEX IF NOT EXISTS IX_Customers_CompanyId ON Customers(CompanyId);
CREATE INDEX IF NOT EXISTS IX_Customers_TaxNumber ON Customers(TaxNumber);
");

            // ==================== PRODUCTS TABLE ====================
            sb.Append(@"
CREATE TABLE IF NOT EXISTS Products (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    CompanyId INTEGER NOT NULL,
    Code TEXT NOT NULL,
    Barcode TEXT,
    Name TEXT NOT NULL,
    Description TEXT,
    Category TEXT,
    Unit TEXT DEFAULT 'Adet',
    SalesPrice DECIMAL NOT NULL DEFAULT 0.00,
    CostPrice DECIMAL DEFAULT 0.00,
    KDVRate DECIMAL DEFAULT 20.00,
    StockQuantity DECIMAL DEFAULT 0.00,
    MinimumStock DECIMAL DEFAULT 0.00,
    MaximumStock DECIMAL DEFAULT 0.00,
    ImagePath TEXT,
    Supplier TEXT,
    IsActive INTEGER DEFAULT 1,
    CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedDate DATETIME,
    Notes TEXT,
    FOREIGN KEY (CompanyId) REFERENCES Companies(Id) ON DELETE CASCADE,
    UNIQUE(CompanyId, Code, Barcode)
);
CREATE INDEX IF NOT EXISTS IX_Products_CompanyId ON Products(CompanyId);
CREATE INDEX IF NOT EXISTS IX_Products_Barcode ON Products(Barcode);
");

            // ==================== INVOICES TABLE ====================
            sb.Append(@"
CREATE TABLE IF NOT EXISTS Invoices (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    CompanyId INTEGER NOT NULL,
    CustomerId INTEGER NOT NULL,
    InvoiceNumber TEXT NOT NULL,
    InvoiceDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    InvoiceType INTEGER DEFAULT 0,
    DueDate DATETIME,
    SubTotal DECIMAL DEFAULT 0.00,
    DiscountAmount DECIMAL DEFAULT 0.00,
    TotalTax DECIMAL DEFAULT 0.00,
    GrandTotal DECIMAL DEFAULT 0.00,
    PaidAmount DECIMAL DEFAULT 0.00,
    PaymentStatus INTEGER DEFAULT 0,
    Currency TEXT DEFAULT 'TRY',
    PaymentMethod INTEGER DEFAULT 0,
    IsSentToEFatura INTEGER DEFAULT 0,
    eFaturaStatus INTEGER DEFAULT 0,
    eFaturaUUID TEXT,
    eFaturaXML TEXT,
    QRCode TEXT,
    Notes TEXT,
    CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedDate DATETIME,
    FOREIGN KEY (CompanyId) REFERENCES Companies(Id) ON DELETE CASCADE,
    FOREIGN KEY (CustomerId) REFERENCES Customers(Id) ON DELETE RESTRICT
);
CREATE INDEX IF NOT EXISTS IX_Invoices_CompanyId ON Invoices(CompanyId);
CREATE INDEX IF NOT EXISTS IX_Invoices_CustomerId ON Invoices(CustomerId);
CREATE INDEX IF NOT EXISTS IX_Invoices_InvoiceNumber ON Invoices(InvoiceNumber);
CREATE INDEX IF NOT EXISTS IX_Invoices_InvoiceDate ON Invoices(InvoiceDate);
");

            // ==================== INVOICE_ITEMS TABLE ====================
            sb.Append(@"
CREATE TABLE IF NOT EXISTS InvoiceItems (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    InvoiceId INTEGER NOT NULL,
    ProductId INTEGER,
    Description TEXT NOT NULL,
    Quantity DECIMAL NOT NULL DEFAULT 1.00,
    UnitPrice DECIMAL NOT NULL DEFAULT 0.00,
    Unit TEXT DEFAULT 'Adet',
    DiscountRate DECIMAL DEFAULT 0.00,
    DiscountAmount DECIMAL DEFAULT 0.00,
    TaxRate DECIMAL DEFAULT 20.00,
    LineTotal DECIMAL DEFAULT 0.00,
    LineTotalWithTax DECIMAL DEFAULT 0.00,
    LineTaxAmount DECIMAL DEFAULT 0.00,
    LineNumber INTEGER,
    Notes TEXT,
    FOREIGN KEY (InvoiceId) REFERENCES Invoices(Id) ON DELETE CASCADE,
    FOREIGN KEY (ProductId) REFERENCES Products(Id) ON DELETE SET NULL
);
CREATE INDEX IF NOT EXISTS IX_InvoiceItems_InvoiceId ON InvoiceItems(InvoiceId);
");

            // ==================== PAYMENTS TABLE ====================
            sb.Append(@"
CREATE TABLE IF NOT EXISTS Payments (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    InvoiceId INTEGER NOT NULL,
    PaymentDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    Amount DECIMAL NOT NULL DEFAULT 0.00,
    PaymentMethod INTEGER DEFAULT 0,
    ReferenceNumber TEXT,
    Notes TEXT,
    CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (InvoiceId) REFERENCES Invoices(Id) ON DELETE CASCADE
);
CREATE INDEX IF NOT EXISTS IX_Payments_InvoiceId ON Payments(InvoiceId);
");

            // ==================== LICENSES TABLE ====================
            sb.Append(@"
CREATE TABLE IF NOT EXISTS Licenses (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    LicenseKey TEXT NOT NULL UNIQUE,
    MachineId TEXT NOT NULL,
    LicenseLevel INTEGER DEFAULT 0,
    ActivationDate DATETIME,
    ExpirationDate DATETIME,
    IsActive INTEGER DEFAULT 1,
    OwnerName TEXT,
    OwnerEmail TEXT,
    CompanyName TEXT,
    CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    LastValidationDate DATETIME,
    Notes TEXT
);
CREATE INDEX IF NOT EXISTS IX_Licenses_MachineId ON Licenses(MachineId);
");

            // ==================== KDV_DECLARATIONS TABLE ====================
            sb.Append(@"
CREATE TABLE IF NOT EXISTS KDVDeclarations (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    CompanyId INTEGER NOT NULL,
    Year INTEGER NOT NULL,
    Month INTEGER NOT NULL,
    KDV1_20_Basis DECIMAL DEFAULT 0.00,
    KDV1_20_Tax DECIMAL DEFAULT 0.00,
    KDV1_10_Basis DECIMAL DEFAULT 0.00,
    KDV1_10_Tax DECIMAL DEFAULT 0.00,
    KDV1_5_Basis DECIMAL DEFAULT 0.00,
    KDV1_5_Tax DECIMAL DEFAULT 0.00,
    KDV1_1_Basis DECIMAL DEFAULT 0.00,
    KDV1_1_Tax DECIMAL DEFAULT 0.00,
    DiscountedSales DECIMAL DEFAULT 0.00,
    TotalKDV DECIMAL DEFAULT 0.00,
    PeriodStartDate DATETIME,
    PeriodEndDate DATETIME,
    CalculationDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    IsSubmitted INTEGER DEFAULT 0,
    SubmissionDate DATETIME,
    Notes TEXT,
    FOREIGN KEY (CompanyId) REFERENCES Companies(Id) ON DELETE CASCADE,
    UNIQUE(CompanyId, Year, Month)
);
CREATE INDEX IF NOT EXISTS IX_KDVDeclarations_CompanyId ON KDVDeclarations(CompanyId);
");

            // ==================== CERTIFICATES TABLE ====================
            sb.Append(@"
CREATE TABLE IF NOT EXISTS Certificates (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    CompanyId INTEGER NOT NULL,
    FileName TEXT NOT NULL,
    FilePath TEXT NOT NULL,
    SubjectName TEXT,
    Issuer TEXT,
    SerialNumber TEXT,
    Thumbprint TEXT,
    ValidFrom DATETIME NOT NULL,
    ValidTo DATETIME NOT NULL,
    Status INTEGER DEFAULT 0,
    UploadDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    IsActive INTEGER DEFAULT 1,
    Notes TEXT,
    FOREIGN KEY (CompanyId) REFERENCES Companies(Id) ON DELETE CASCADE
);
CREATE INDEX IF NOT EXISTS IX_Certificates_CompanyId ON Certificates(CompanyId);
");

            // ==================== EFATURA_LOGS TABLE ====================
            sb.Append(@"
CREATE TABLE IF NOT EXISTS eFaturaLogs (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    InvoiceId INTEGER NOT NULL,
    SendDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    UUID TEXT NOT NULL,
    Status INTEGER DEFAULT 0,
    ResponseCode TEXT,
    ResponseMessage TEXT,
    XMLContent TEXT,
    ReceiverEmail TEXT,
    Notes TEXT,
    FOREIGN KEY (InvoiceId) REFERENCES Invoices(Id) ON DELETE CASCADE
);
CREATE INDEX IF NOT EXISTS IX_eFaturaLogs_InvoiceId ON eFaturaLogs(InvoiceId);
");

            // ==================== BACKUP_LOGS TABLE ====================
            sb.Append(@"
CREATE TABLE IF NOT EXISTS BackupLogs (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    BackupDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    FileName TEXT NOT NULL,
    FilePath TEXT NOT NULL,
    FileSize INTEGER,
    BackupType TEXT,
    IsSuccessful INTEGER DEFAULT 1,
    ErrorMessage TEXT,
    Notes TEXT
);
CREATE INDEX IF NOT EXISTS IX_BackupLogs_BackupDate ON BackupLogs(BackupDate);
");

            // ==================== AUDIT_LOGS TABLE ====================
            sb.Append(@"
CREATE TABLE IF NOT EXISTS AuditLogs (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    CompanyId INTEGER,
    ActionDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    ActionType INTEGER NOT NULL,
    TableName TEXT,
    RecordId INTEGER,
    Description TEXT,
    OldValue TEXT,
    NewValue TEXT,
    Username TEXT,
    IPAddress TEXT,
    Notes TEXT,
    FOREIGN KEY (CompanyId) REFERENCES Companies(Id) ON DELETE SET NULL
);
CREATE INDEX IF NOT EXISTS IX_AuditLogs_CompanyId ON AuditLogs(CompanyId);
CREATE INDEX IF NOT EXISTS IX_AuditLogs_ActionDate ON AuditLogs(ActionDate);
");

            return sb.ToString();
        }
    }
}
