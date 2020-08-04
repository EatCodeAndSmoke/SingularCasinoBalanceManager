using CasinoBalanceManager.Application.ErrorResolver.ResolverInfo;
using CasinoBalanceManager.Core.Models;
using System;
using System.Collections.Generic;

namespace CasinoBalanceManager.Application.ErrorResolver {
    internal interface IErrorResolver {
        AppError Resolve<TException>(TException exp) where TException : Exception;
    }

    internal class ErrorResolver : IErrorResolver {

        private readonly IDictionary<int, BaseResolverInfo> _lookUp;

        public ErrorResolver(IDictionary<int, BaseResolverInfo> lookUp) {
            this._lookUp = lookUp;
        }

        public AppError Resolve<TException>(TException exp) where TException : Exception {
            var key = exp.GetType().GetHashCode();
            if (!_lookUp.ContainsKey(key))
                return null;
            return _lookUp[key].GetError(exp);
        }
    }
}
