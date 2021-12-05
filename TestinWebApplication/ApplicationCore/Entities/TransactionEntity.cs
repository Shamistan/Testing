using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Entities
{
    public class TransactionEntity
    {
        [Key]
        public int Id { get; set; }
        public string FinancialInstitution { get; set; }
        public string FXSettlementDate { get; set; }
        public string ReconciliationFileID { get; set; }
        public string TransactionCurrency { get; set; }
        public string ReconciliationCurrency { get; set; }
        public ICollection<SubTransactionEntity> SubTransactions { get; set; }

    }
}
