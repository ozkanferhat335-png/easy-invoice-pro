using System;

namespace EasyInvoicePro.Banking
{
    public class BankConnection
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string BankName { get; set; }
        public string ApiConfigEncrypted { get; set; }
        public bool Active { get; set; }
    }

    public class BankAccount
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Iban { get; set; }
        public string Currency { get; set; }
        public string AccountType { get; set; }
    }

    public class BankTransaction
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public DateTime TrxDate { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Description { get; set; }
        public string RefNo { get; set; }
        public string UniqueHash { get; set; }
    }

    public class TransferOrder
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public TransferType Type { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public TransferStatus Status { get; set; }
        public string BankRefNo { get; set; }
        public string IdempotencyKey { get; set; }
        public string ReceiverIban { get; set; }
        public string Description { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? ApprovedByUserId { get; set; }
    }

    public class ExchangeRate
    {
        public int Id { get; set; }
        public string BankName { get; set; }
        public string CurrencyPair { get; set; }
        public decimal Rate { get; set; }
        public DateTime RateTime { get; set; }
    }

    public class AuditLog
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Action { get; set; }
        public string Entity { get; set; }
        public string EntityId { get; set; }
        public DateTime Timestamp { get; set; }
        public string DetailMasked { get; set; }
    }
}
