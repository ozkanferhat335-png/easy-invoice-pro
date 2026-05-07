using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyInvoicePro.Banking
{
    public class BankingService
    {
        private readonly IBankAdapter _adapter;

        public BankingService(IBankAdapter adapter)
        {
            _adapter = adapter;
        }

        public List<BankTransaction> SyncTransactions(BankAccount account, DateTime from, DateTime to, IEnumerable<BankTransaction> existing)
        {
            var incoming = _adapter.GetTransactions(account, from, to);
            var existingHashes = new HashSet<string>(existing.Select(x => x.UniqueHash));

            return incoming.Where(x => !existingHashes.Contains(x.UniqueHash)).ToList();
        }

        public TransferOrder CreateTransfer(TransferOrder order)
        {
            if (string.IsNullOrWhiteSpace(order.IdempotencyKey))
            {
                order.IdempotencyKey = Guid.NewGuid().ToString("N");
            }

            order.Status = TransferStatus.Beklemede;
            return order;
        }
    }
}
