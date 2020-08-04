using System;
using System.Collections.Generic;
using System.Text;

namespace CasinoBalanceManager.Core.Models {
    public class AppResponseBody {

        public AppResponseBody() {

        }

        public AppResponseBody(object value) : base() {
            Value = value;
        }

        public bool HasValue => Value != null;
        public object Value { get; set; }

        public bool TryParse<T>(out T result) {
            if (Value is T parsedValue) {
                result = parsedValue;
                return true;
            }
            result = default;
            return false;
        }
    }
}
