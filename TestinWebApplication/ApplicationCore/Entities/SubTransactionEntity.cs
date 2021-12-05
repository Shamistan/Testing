using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Entities
{
    public class SubTransactionEntity
    {
        [Key]
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
        public TransactionEntity Transaction { get; set; }
    }
}
