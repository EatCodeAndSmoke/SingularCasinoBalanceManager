using CasinoBalanceManager.Application.ErrorResolver.ResolverInfo;
using CasinoBalanceManager.Core.Models;
using System;
using System.Collections.Generic;

namespace CasinoBalanceManager.Application.ErrorResolver {
    internal interface IErrorResolverBuilder {
        IResolverInfo<TException, TError> Register<TException, TError>(int errorCode) where TException : Exception where TError : AppError, new();
        IErrorResolver Build();
    }

    internal class ErrorResolverBuilder : IErrorResolverBuilder {

        private readonly IDictionary<int, BaseResolverInfo> _lookUp;

        public ErrorResolverBuilder() {
            _lookUp = new Dictionary<int, BaseResolverInfo>();
        }

        public IResolverInfo<TException, TError> Register<TException, TError>(int errorCode) where TException : Exception where TError : AppError, new() {
            var resolverInfo = new ResolverInfo<TException, TError>(errorCode);
            _lookUp.Add(typeof(TException).GetHashCode(), resolverInfo);
            return resolverInfo;
        }

        public IErrorResolver Build() {
            return new ErrorResolver(_lookUp);
        }
    }
}
