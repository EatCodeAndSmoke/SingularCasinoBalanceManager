using CasinoBalanceManager.Application.ErrorResolver;
using CasinoBalanceManager.Core.Models;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CasinoBalanceManager.Application.Mediator.Pipeline {
    internal class ExceptionHandler<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TResponse : AppResponse {

        private readonly IErrorResolver _errorResolver;

        public ExceptionHandler(IErrorResolver errorResolver) {
            this._errorResolver = errorResolver;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next) {
            try {
                return await next();
            } catch (Exception ex) {
                return AppResponse.Failure(_errorResolver.Resolve(ex)) as TResponse;
            }
        }
    }
}
