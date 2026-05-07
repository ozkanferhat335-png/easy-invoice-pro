using FinanceApp.BankAdapters;
using FinanceApp.Domain;
using FinanceApp.Persistence;
using FinanceApp.Shared;
using FluentValidation;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace FinanceApp.Application
{
    public sealed class CreateTransferOrderCommand
    {
        public long CompanyId { get; set; }
        public string ReferenceNo { get; set; }
        public string SourceIban { get; set; }
        public string DestinationIban { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string InitiatedBy { get; set; }
    }

    public sealed class CreateTransferOrderValidator : AbstractValidator<CreateTransferOrderCommand>
    {
        public CreateTransferOrderValidator()
        {
            RuleFor(x => x.ReferenceNo).NotEmpty().MaximumLength(50);
            RuleFor(x => x.SourceIban).NotEmpty().Length(26);
            RuleFor(x => x.DestinationIban).NotEmpty().Length(26);
            RuleFor(x => x.Amount).GreaterThan(0);
            RuleFor(x => x.CurrencyCode).Length(3);
        }
    }

    public sealed class TransferWorkflowService
    {
        private readonly ITransferOrderRepository _repository;
        private readonly IBankAdapter _bankAdapter;
        private readonly IDbConnection _connection;

        public TransferWorkflowService(ITransferOrderRepository repository, IBankAdapter bankAdapter, IDbConnection connection)
        { _repository = repository; _bankAdapter = bankAdapter; _connection = connection; }

        public async Task<Result> CreateAndQueueAsync(CreateTransferOrderCommand command, CancellationToken cancellationToken)
        {
            var validation = new CreateTransferOrderValidator().Validate(command);
            if (!validation.IsValid) return Result.Failure(validation.ToString());

            var existing = await _repository.GetByReferenceNoAsync(command.ReferenceNo, cancellationToken).ConfigureAwait(false);
            if (existing != null) return Result.Failure("Duplicate transfer reference detected.");

            using (var tx = _connection.BeginTransaction())
            {
                var order = new TransferOrder
                {
                    CompanyId = command.CompanyId,
                    ReferenceNo = command.ReferenceNo,
                    SourceIban = command.SourceIban,
                    DestinationIban = command.DestinationIban,
                    Amount = command.Amount,
                    CurrencyCode = command.CurrencyCode,
                    State = TransferOrderState.Queued,
                    IdempotencyKey = Guid.NewGuid().ToString("N"),
                    CreatedBy = command.InitiatedBy
                };
                await _repository.InsertAsync(order, tx, cancellationToken).ConfigureAwait(false);
                tx.Commit();
            }

            return Result.Success();
        }
    }
}
