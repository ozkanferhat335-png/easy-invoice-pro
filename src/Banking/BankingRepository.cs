using System;
using System.Collections.Generic;
using System.Data.SQLite;
using EasyInvoicePro.Database;

namespace EasyInvoicePro.Banking
{
    public class BankingRepository
    {
        public void SaveTransactions(IEnumerable<BankTransaction> transactions)
        {
            using (var tx = DatabaseManager.Connection.BeginTransaction())
            {
                foreach (var trx in transactions)
                {
                    using (var cmd = DatabaseManager.Connection.CreateCommand())
                    {
                        cmd.CommandText = @"INSERT OR IGNORE INTO BankTransactions
(AccountId, TrxDate, Amount, Currency, Description, RefNo, UniqueHash, IsAccounted, CreatedDate)
VALUES (@AccountId, @TrxDate, @Amount, @Currency, @Description, @RefNo, @UniqueHash, 0, CURRENT_TIMESTAMP);";
                        cmd.Parameters.AddWithValue("@AccountId", trx.AccountId);
                        cmd.Parameters.AddWithValue("@TrxDate", trx.TrxDate);
                        cmd.Parameters.AddWithValue("@Amount", trx.Amount);
                        cmd.Parameters.AddWithValue("@Currency", trx.Currency);
                        cmd.Parameters.AddWithValue("@Description", trx.Description ?? string.Empty);
                        cmd.Parameters.AddWithValue("@RefNo", trx.RefNo ?? string.Empty);
                        cmd.Parameters.AddWithValue("@UniqueHash", trx.UniqueHash ?? string.Empty);
                        cmd.ExecuteNonQuery();
                    }
                }
                tx.Commit();
            }
        }

        public List<BankTransaction> GetTransactionsByDate(int accountId, DateTime from, DateTime to)
        {
            var result = new List<BankTransaction>();
            using (var cmd = DatabaseManager.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT Id, AccountId, TrxDate, Amount, Currency, Description, RefNo, UniqueHash
FROM BankTransactions
WHERE AccountId = @AccountId AND TrxDate >= @FromDate AND TrxDate <= @ToDate;";
                cmd.Parameters.AddWithValue("@AccountId", accountId);
                cmd.Parameters.AddWithValue("@FromDate", from);
                cmd.Parameters.AddWithValue("@ToDate", to);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new BankTransaction
                        {
                            Id = reader.GetInt32(0),
                            AccountId = reader.GetInt32(1),
                            TrxDate = Convert.ToDateTime(reader[2]),
                            Amount = Convert.ToDecimal(reader[3]),
                            Currency = reader[4].ToString(),
                            Description = reader[5].ToString(),
                            RefNo = reader[6].ToString(),
                            UniqueHash = reader[7].ToString()
                        });
                    }
                }
            }
            return result;
        }

        public int CreateTransferOrder(TransferOrder order)
        {
            using (var cmd = DatabaseManager.Connection.CreateCommand())
            {
                cmd.CommandText = @"INSERT INTO TransferOrders
(CompanyId, Type, Amount, Currency, Status, BankRefNo, IdempotencyKey, ReceiverIban, Description, CreatedDate)
VALUES (@CompanyId, @Type, @Amount, @Currency, @Status, @BankRefNo, @IdempotencyKey, @ReceiverIban, @Description, CURRENT_TIMESTAMP);
SELECT last_insert_rowid();";

                cmd.Parameters.AddWithValue("@CompanyId", order.CompanyId);
                cmd.Parameters.AddWithValue("@Type", (int)order.Type);
                cmd.Parameters.AddWithValue("@Amount", order.Amount);
                cmd.Parameters.AddWithValue("@Currency", order.Currency);
                cmd.Parameters.AddWithValue("@Status", (int)order.Status);
                cmd.Parameters.AddWithValue("@BankRefNo", order.BankRefNo ?? string.Empty);
                cmd.Parameters.AddWithValue("@IdempotencyKey", order.IdempotencyKey);
                cmd.Parameters.AddWithValue("@ReceiverIban", order.ReceiverIban ?? string.Empty);
                cmd.Parameters.AddWithValue("@Description", order.Description ?? string.Empty);

                return Convert.ToInt32((long)cmd.ExecuteScalar());
            }
        }
    }
}
