using Balances;
using CasinoBalanceManager.Application.Common;
using CasinoBalanceManager.Core.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CasinoBalanceManager.Application.Mediator.Queries.CasinoBalance {
    public class GetCasinoBalanceQuery : BaseCommand {
    }

    internal class GetCasinoBalanceQueryHandler : IRequestHandler<GetCasinoBalanceQuery, AppResponse> {

        private readonly IBalanceManager _casinoBalanceManager;

        public GetCasinoBalanceQueryHandler(ServiceResolver resolver) {
            _casinoBalanceManager = resolver(AppConstants.CasinoBalance);
        }

        public async Task<AppResponse> Handle(GetCasinoBalanceQuery request, CancellationToken cancellationToken) {
            return await Task.Factory.StartNew(() => {
                var currentBalance = _casinoBalanceManager.GetBalance();
                return AppResponse.Success(currentBalance);
            });
        }
    }
}
