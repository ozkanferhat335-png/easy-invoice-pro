using System;
using System.Collections.Generic;

namespace EasyInvoicePro.Banking
{
    public interface IBankAdapter
    {
        string BankName { get; }

        List<BankTransaction> GetTransactions(BankAccount account, DateTime from, DateTime to);

        TransferOrder SendTransfer(TransferOrder order);

        TransferStatus QueryTransferStatus(string bankRefNo);

        ExchangeRate GetExchangeRate(string currencyPair);
    }
}
