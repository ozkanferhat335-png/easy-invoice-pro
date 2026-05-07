using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FinanceApp.BankAdapters
{
    public interface IBankAdapter
    {
        Task AuthenticateAsync(CancellationToken cancellationToken);
        Task<IReadOnlyList<BankAccountDto>> GetAccountsAsync(CancellationToken cancellationToken);
        Task<IReadOnlyList<BankTransactionDto>> GetTransactionsAsync(DateTime start, DateTime end, CancellationToken cancellationToken);
        Task<TransferResultDto> SendTransferAsync(TransferRequestDto request, CancellationToken cancellationToken);
        Task<TransferStatusDto> GetTransferStatusAsync(string referenceNo, CancellationToken cancellationToken);
        Task<IReadOnlyList<ExchangeRateDto>> GetExchangeRatesAsync(CancellationToken cancellationToken);
    }

    public sealed class BankAccountDto { public string Iban { get; set; } public string AccountName { get; set; } public decimal Balance { get; set; } public string CurrencyCode { get; set; } }
    public sealed class BankTransactionDto { public string ExternalId { get; set; } public DateTime BookingDate { get; set; } public decimal Amount { get; set; } public string CurrencyCode { get; set; } public string Description { get; set; } }
    public sealed class TransferRequestDto { public string ReferenceNo { get; set; } public string SourceIban { get; set; } public string DestinationIban { get; set; } public decimal Amount { get; set; } public string CurrencyCode { get; set; } }
    public sealed class TransferResultDto { public bool Accepted { get; set; } public string BankReference { get; set; } public string Message { get; set; } }
    public sealed class TransferStatusDto { public string ReferenceNo { get; set; } public string StatusCode { get; set; } public string StatusText { get; set; } }
    public sealed class ExchangeRateDto { public string BaseCurrency { get; set; } public string QuoteCurrency { get; set; } public decimal Rate { get; set; } public DateTime EffectiveAtUtc { get; set; } }
}
