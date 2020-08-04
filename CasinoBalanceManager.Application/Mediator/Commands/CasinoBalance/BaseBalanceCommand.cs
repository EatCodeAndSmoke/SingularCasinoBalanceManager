using Balances;
using CasinoBalanceManager.Application.Common;
using CasinoBalanceManager.Application.Common.Exceptions;
using CasinoBalanceManager.Core.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CasinoBalanceManager.Application.Mediator.Commands.CasinoBalance {
    public class BaseBalanceCommand : BaseCommand {
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
    }

    internal abstract class BaseBalanceCommandHandler<TCommand> : IRequestHandler<TCommand, AppResponse>
        where TCommand : BaseBalanceCommand {

        protected readonly IBalanceManager _casinoBalanceManager, _gameBalanceManager;

        public BaseBalanceCommandHandler(ServiceResolver resolver) {
            _casinoBalanceManager = resolver(AppConstants.CasinoBalance);
            _gameBalanceManager = resolver(AppConstants.GameBalance);
        }

        protected abstract IBalanceManager From { get; }
        protected abstract IBalanceManager To { get; }
        protected abstract string FromBalanceName { get; }
        protected abstract string ToBalanceName { get; }
        protected TransactionFailedException Exception { get; private set; }

        public async Task<AppResponse> Handle(TCommand request, CancellationToken cancellationToken) {
            return await Task.Factory.StartNew(() => {
                Exception = new TransactionFailedException();
                var fromErrorCode = From.DecreaseBalance(request.Amount, request.TransactionId);
                CheckDecreaseBalanceResult(fromErrorCode, request);
                var toErrorCode = To.IncreaseBalance(request.Amount, request.TransactionId);
                CheckIncreaseBalanceResult(toErrorCode, request);
                return AppResponse.Success(null);
            });
        }

        protected virtual void CheckDecreaseBalanceResult(ErrorCode errorCode, TCommand request) {
            if (errorCode == ErrorCode.Success) {
                Exception.LogDecreaseSuccess(FromBalanceName);
            } else {
                Exception.LogDecreaseFailure(FromBalanceName, errorCode.ToString());
                if (errorCode == ErrorCode.UnknownError)
                    TryRollback(From, FromBalanceName, request.TransactionId);
                throw Exception;
            }
        }

        protected virtual void CheckIncreaseBalanceResult(ErrorCode errorCode, TCommand request) {
            if (errorCode == ErrorCode.Success) {
                Exception.LogIncreaseSuccess(ToBalanceName);
            } else {
                Exception.LogIncreaseFailure(ToBalanceName, errorCode.ToString());
                if (errorCode == ErrorCode.UnknownError)
                    TryRollback(To, ToBalanceName, request.TransactionId);
                TryRollback(From, FromBalanceName, request.TransactionId);
                throw Exception;
            }
        }

        protected virtual void TryRollback(IBalanceManager balanceManager, string balanceName, string transactionId) {
            var checkErroCode = balanceManager.CheckTransaction(transactionId);
            if (checkErroCode == ErrorCode.Success) {
                var rollbackErrorCode = balanceManager.Rollback(transactionId);
                if (rollbackErrorCode == ErrorCode.Success) {
                    Exception.LogRollbackSuccess(balanceName);
                } else {
                    if (rollbackErrorCode == ErrorCode.UnknownError && TransactionRollbacked(balanceManager, transactionId))
                        Exception.LogRollbackSuccess(balanceName);
                    else
                        Exception.LogRollbackFailure(balanceName, rollbackErrorCode.ToString());
                }
            }
        }

        private bool TransactionRollbacked(IBalanceManager balanceManager, string transactionId) {
            var checkRollbackErrorCode = balanceManager.CheckTransaction(transactionId);
            return checkRollbackErrorCode == ErrorCode.TransactionRollbacked;
        }
    }
}
