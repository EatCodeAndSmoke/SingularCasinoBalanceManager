using Balances;
using CasinoBalanceManager.Application.Common;
using CasinoBalanceManager.Application.ErrorResolver;
using CasinoBalanceManager.Application.Mediator;
using CasinoBalanceManager.Application.Mediator.Commands.CasinoBalance;
using CasinoBalanceManager.Application.Mediator.Pipeline;
using CasinoBalanceManager.Application.Validation;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace CasinoBalanceManager.Application.Extensions {
    public static class ServiceCollectionExtensions {

        public static IServiceCollection AddCasinoBalanceManagerServices(this IServiceCollection services) {
            AddInternalServices(services);
            AddMediatRServices(services);
            AddValidators(services);
            AddBalanceManagerServices(services);
            return services;
        }

        private static void AddInternalServices(IServiceCollection services) {
            services.AddScoped<IAppMediator, AppMediator>();

            IErrorResolverBuilder builder = new ErrorResolverBuilder();
            builder.AddApplicationErrors();

            services.AddSingleton(builder);
            services.AddSingleton(sp => sp.GetService<IErrorResolverBuilder>().Build());
        }

        private static void AddMediatRServices(IServiceCollection services) {
            services.AddMediatR(typeof(AppMediator).Assembly);
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ExceptionHandler<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationHandler<,>));
        }

        private static void AddValidators(IServiceCollection services) {
            services.AddScoped<IValidator<BaseBalanceCommand>, BaseBalanceCommandValidator<BaseBalanceCommand>>();
            services.AddScoped<IValidator<DepositCommand>, BaseBalanceCommandValidator<DepositCommand>>();
            services.AddScoped<IValidator<WithdrawCommand>, BaseBalanceCommandValidator<WithdrawCommand>>();
        }

        private static void AddBalanceManagerServices(IServiceCollection services) {
            services.AddScoped<Balances.CasinoBalanceManager>();
            services.AddScoped<GameBalanceManager>();

            services.AddTransient<ServiceResolver>(provider => key => {
                switch (key) {
                    case AppConstants.CasinoBalance:
                        return provider.GetService<Balances.CasinoBalanceManager>();
                    case AppConstants.GameBalance:
                        return provider.GetService<GameBalanceManager>();
                    default:
                        throw new KeyNotFoundException("Invalid key for IBalanceManager to resolve");
                }
            });
        }
    }
}
