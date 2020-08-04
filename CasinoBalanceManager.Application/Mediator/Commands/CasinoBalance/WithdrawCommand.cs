using Balances;
using CasinoBalanceManager.Application.Common;

namespace CasinoBalanceManager.Application.Mediator.Commands.CasinoBalance {
    public class WithdrawCommand : BaseBalanceCommand {
    }

    internal class WithdrawCommandHandler : BaseBalanceCommandHandler<WithdrawCommand> {

        public WithdrawCommandHandler(ServiceResolver resolver) : base(resolver) {

        }

        protected override IBalanceManager From => _casinoBalanceManager;
        protected override IBalanceManager To => _gameBalanceManager;
        protected override string FromBalanceName => AppConstants.CasinoBalance;
        protected override string ToBalanceName => AppConstants.GameBalance;
    }
}
