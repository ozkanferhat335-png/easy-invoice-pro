using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyInvoicePro.Banking
{
    public class BankingService
    {
        private readonly IBankAdapter _adapter;
        private readonly BankingRepository _repository;

        public BankingService(IBankAdapter adapter, BankingRepository repository)
        {
            _adapter = adapter;
            _repository = repository;
        }

        public List<BankTransaction> SyncTransactions(BankAccount account, DateTime from, DateTime to)
        {
            var existing = _repository.GetTransactionsByDate(account.Id, from, to);
            var incoming = _adapter.GetTransactions(account, from, to);
            var existingHashes = new HashSet<string>(existing.Select(x => x.UniqueHash));

            var newItems = incoming.Where(x => !existingHashes.Contains(x.UniqueHash)).ToList();
            _repository.SaveTransactions(newItems);
            return newItems;
        }

        public TransferOrder CreateTransfer(TransferOrder order, bool makerCheckerEnabled)
        {
            if (!IBANValidator.IsValid(order.ReceiverIban))
            {
                throw new InvalidOperationException("Geçersiz alıcı IBAN bilgisi.");
            }

            if (string.IsNullOrWhiteSpace(order.IdempotencyKey))
            {
                order.IdempotencyKey = Guid.NewGuid().ToString("N");
            }

            order.Status = makerCheckerEnabled ? TransferStatus.Beklemede : TransferStatus.Onaylandi;
            order.Id = _repository.CreateTransferOrder(order);
            return order;
        }

        public TransferOrder ApproveTransfer(TransferOrder order, int approverUserId)
        {
            if (order.CreatedByUserId.HasValue && order.CreatedByUserId.Value == approverUserId)
            {
                throw new InvalidOperationException("Maker-checker kuralı ihlali: oluşturan kullanıcı onay veremez.");
            }

            order.ApprovedByUserId = approverUserId;
            order.Status = TransferStatus.Onaylandi;
            return order;
        }
    }
}
