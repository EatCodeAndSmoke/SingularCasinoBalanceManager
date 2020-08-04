using Balances;
using CasinoBalanceManager.Application.Common;

namespace CasinoBalanceManager.Application.Mediator.Commands.CasinoBalance {
    public class DepositCommand : BaseBalanceCommand {
    }

    internal class DepositCommandHandler : BaseBalanceCommandHandler<DepositCommand> {

        public DepositCommandHandler(ServiceResolver resolver) : base(resolver) {

        }

        protected override IBalanceManager From => _gameBalanceManager;
        protected override IBalanceManager To => _casinoBalanceManager;
        protected override string FromBalanceName => AppConstants.GameBalance;
        protected override string ToBalanceName => AppConstants.CasinoBalance;
    }
}
