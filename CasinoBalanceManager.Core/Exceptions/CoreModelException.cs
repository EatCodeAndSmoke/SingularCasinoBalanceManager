using System;
using System.Collections.Generic;
using System.Text;

namespace CasinoBalanceManager.Core.Exceptions {
    public class CoreModelException : Exception {
        public CoreModelException(string message) : base(message) {

        }
    }
}
