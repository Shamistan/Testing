using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.ViewModels
{
    public class TransactionViewModel
    {
        public int Id { get; set; }
        public string FinancialInstitution { get; set; }
        public string FXSettlementDate { get; set; }
        public string ReconciliationFileID { get; set; }
        public string TransactionCurrency { get; set; }
        public string ReconciliationCurrency { get; set; }
    }
}
