using System;

namespace FinanceApp.Domain
{
    public abstract class AuditableEntity
    {
        public long Id { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAtUtc { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public byte[] RowVersion { get; set; }
    }

    public sealed class Company : AuditableEntity
    {
        public string Name { get; set; }
        public string TaxNumber { get; set; }
        public string CurrencyCode { get; set; }
    }

    public enum TransferOrderState
    {
        Draft = 1,
        PendingApproval = 2,
        Approved = 3,
        Queued = 4,
        Sending = 5,
        Sent = 6,
        Completed = 7,
        Failed = 8,
        Reversed = 9
    }

    public sealed class TransferOrder : AuditableEntity
    {
        public long CompanyId { get; set; }
        public string ReferenceNo { get; set; }
        public string SourceIban { get; set; }
        public string DestinationIban { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public TransferOrderState State { get; set; }
        public string IdempotencyKey { get; set; }
    }
}
