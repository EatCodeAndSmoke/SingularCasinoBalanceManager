using CasinoBalanceManager.Application.Common.Exceptions;
using CasinoBalanceManager.Core.Models;
using System.Collections.Generic;

namespace CasinoBalanceManager.Application.Models {
    public class TransactionFailedErrorModel : AppError {
        public IEnumerable<TransactionExecutionLog> ExecutionLog { get; set; }
    }
}
