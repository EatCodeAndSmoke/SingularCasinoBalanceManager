using CasinoBalanceManager.Core.Models;
using MediatR;
using System.Threading.Tasks;

namespace CasinoBalanceManager.Application.Mediator {
    public interface IAppMediator {
        Task<AppResponse> Execute<TRequest>(TRequest request) where TRequest : IRequest<AppResponse>;
    }

    internal class AppMediator : IAppMediator {

        private readonly IMediator mediator;

        public AppMediator(IMediator mediator) {
            this.mediator = mediator;
        }

        public async Task<AppResponse> Execute<TRequest>(TRequest request) where TRequest : IRequest<AppResponse> {
            return await mediator.Send(request);
        }
    }
}
