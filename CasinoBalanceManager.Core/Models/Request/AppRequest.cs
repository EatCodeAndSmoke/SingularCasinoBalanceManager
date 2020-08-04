using System;

namespace CasinoBalanceManager.Core.Models {
    public class AppRequest {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime RequestDate { get; set; } = DateTime.Now;
    }
}
