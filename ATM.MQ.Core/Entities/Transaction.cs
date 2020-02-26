using System;

namespace ATM.MQ.Core.Entities
{
    public enum Operation {
        Debit,
        Credit
    }

    public class Transaction
    {
        public Account Account { get; set; }

        public double Amount { get; set; }

        public Operation Operation { get; set; }
    }
}
