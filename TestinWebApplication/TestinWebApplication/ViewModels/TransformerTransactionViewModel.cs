using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.ViewModels
{
    public class TransformerTransactionViewModel
    {
        public List<TransactionEntity> TransactionViewModels { get; set; }
        public List<SubTransactionEntity> SubTransactionViewModels { get; set; }
    }
}
