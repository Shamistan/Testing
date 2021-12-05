using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.ViewModels
{
    public class SubTransactionViewModel
    {
        public int Id { get; set; }
        public string SettlementCategory { get; set; }
        public decimal TransactionAmountcredit { get; set; }
        public decimal ReconciliationAmntCredit { get; set; }
        public decimal FeeAmountCredit { get; set; }
        public decimal TransactionAmountDebit { get; set; }
        public decimal ReconciliationAmntDebit { get; set; }
        public decimal FeeAmountDebit { get; set; }
        public int CountTotal { get; set; }
        public decimal NetValue { get; set; }
        public int TransactionId { get; set; }
    }
}
