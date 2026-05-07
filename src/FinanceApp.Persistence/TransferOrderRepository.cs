using Dapper;
using FinanceApp.Domain;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace FinanceApp.Persistence
{
    public interface ITransferOrderRepository
    {
        Task<long> InsertAsync(TransferOrder order, IDbTransaction transaction, CancellationToken cancellationToken);
        Task<TransferOrder> GetByReferenceNoAsync(string referenceNo, CancellationToken cancellationToken);
        Task<IReadOnlyList<TransferOrder>> GetPendingAsync(int take, CancellationToken cancellationToken);
        Task UpdateStateAsync(long id, TransferOrderState state, byte[] rowVersion, IDbTransaction transaction, CancellationToken cancellationToken);
    }

    public sealed class TransferOrderRepository : ITransferOrderRepository
    {
        private readonly IDbConnection _connection;

        public TransferOrderRepository(IDbConnection connection) { _connection = connection; }

        public async Task<long> InsertAsync(TransferOrder order, IDbTransaction transaction, CancellationToken cancellationToken)
        {
            const string sql = @"INSERT INTO TransferOrders (CompanyId, ReferenceNo, SourceIban, DestinationIban, Amount, CurrencyCode, State, IdempotencyKey, CreatedAtUtc, CreatedBy, IsDeleted)
VALUES (@CompanyId,@ReferenceNo,@SourceIban,@DestinationIban,@Amount,@CurrencyCode,@State,@IdempotencyKey,SYSUTCDATETIME(),@CreatedBy,0);
SELECT CAST(SCOPE_IDENTITY() AS BIGINT);";
            return await _connection.ExecuteScalarAsync<long>(new CommandDefinition(sql, order, transaction, cancellationToken: cancellationToken)).ConfigureAwait(false);
        }

        public Task<TransferOrder> GetByReferenceNoAsync(string referenceNo, CancellationToken cancellationToken)
        {
            const string sql = "SELECT TOP 1 * FROM TransferOrders WHERE ReferenceNo=@referenceNo AND IsDeleted=0";
            return _connection.QueryFirstOrDefaultAsync<TransferOrder>(new CommandDefinition(sql, new { referenceNo }, cancellationToken: cancellationToken));
        }

        public async Task<IReadOnlyList<TransferOrder>> GetPendingAsync(int take, CancellationToken cancellationToken)
        {
            const string sql = "SELECT TOP (@take) * FROM TransferOrders WHERE State IN (4,5,6) AND IsDeleted=0 ORDER BY Id";
            var data = await _connection.QueryAsync<TransferOrder>(new CommandDefinition(sql, new { take }, cancellationToken: cancellationToken)).ConfigureAwait(false);
            return data.AsList();
        }

        public Task UpdateStateAsync(long id, TransferOrderState state, byte[] rowVersion, IDbTransaction transaction, CancellationToken cancellationToken)
        {
            const string sql = @"UPDATE TransferOrders SET State=@state, UpdatedAtUtc=SYSUTCDATETIME() WHERE Id=@id AND RowVersion=@rowVersion;";
            return _connection.ExecuteAsync(new CommandDefinition(sql, new { id, state, rowVersion }, transaction, cancellationToken: cancellationToken));
        }
    }
}
