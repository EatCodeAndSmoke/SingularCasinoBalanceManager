using CasinoBalanceManager.Core.Models;
using MediatR;

namespace CasinoBalanceManager.Application.Mediator {
    public class BaseCommand : AppRequest, IRequest<AppResponse> {
    }
}
