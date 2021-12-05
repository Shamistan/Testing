using ApplicationCore.Entities;
using ApplicationCore.Extentions;
using Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication.ViewModels;
using static WebApplication.ViewModels.FileUploadViewModel;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFileProvider fileProvider;
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(IFileProvider fileProvider, IUnitOfWork unitOfWork)
        {
            this.fileProvider = fileProvider;
            _unitOfWork = unitOfWork;
        }
        #region Index
        public IActionResult Index()
        {

            return View();
        }
        #endregion

        #region Upload File
        public IActionResult UploadFile()
        {
            ViewBag.TransformerTransactionViewModel = _unitOfWork.Repository<TransactionEntity>().GetAll().ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(FileInputModel fileInputModel)
        {
            if (fileInputModel.File == null || fileInputModel.File.Length == 0)
                return Content("file not selected");
            var fileName = Guid.NewGuid().ToString() + fileInputModel.File.GetFilename();
            var path = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot",
                        fileName);
            var returnPath = IFormFileExtensions.GetContentType(path);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await fileInputModel.File.CopyToAsync(stream);
            }
            ViewBag.FileName = fileName;

            return View();
        }
        #endregion

        #region Display File
        [HttpPost]
        public IActionResult DisplayFile(string fileName)
        {
            var transactionList = new List<TransactionEntity>();

            using (var reader = new StreamReader(Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot",
                        fileName)))
            {
                //var t = reader.ReadToEnd();
                var subTransactionList = new List<SubTransactionEntity>();
                var transaction = new TransactionEntity();
                var subTransaction = new SubTransactionEntity();
                var isSubTransactionHeaderStart = false;
                var isSubTransactionHeaderBodyStart = false;
                var isSubTransactionHeaderBodyEnd = false;
                while (reader.Peek() >= 0)
                {
                    var line = reader.ReadLine().Trim();

                    if (string.IsNullOrEmpty(line))
                        continue;
                    if (!isSubTransactionHeaderStart && line.Trim() == "+--------------------------------------+--------------------+---------------------+---------------------+---------------------+---------------------+---------------------+---------+---------------------+")
                    {
                        isSubTransactionHeaderStart = true;
                        continue;
                    }
                    if (isSubTransactionHeaderStart && line.Trim() == "+--------------------------------------+--------------------+---------------------+---------------------+---------------------+---------------------+---------------------+---------+---------------------+")
                    {
                        isSubTransactionHeaderBodyStart = true;
                        continue;
                    }

                    if (isSubTransactionHeaderStart && isSubTransactionHeaderBodyStart && line.Trim() == "----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------")
                    {
                        isSubTransactionHeaderBodyEnd = true;
                        continue;
                    }
                    if (isSubTransactionHeaderBodyEnd)
                    {
                        isSubTransactionHeaderStart = false;
                        isSubTransactionHeaderBodyStart = false;
                        isSubTransactionHeaderBodyEnd = false;
                        transaction.SubTransactions = subTransactionList;
                        transactionList.Add(transaction);
                        transaction = new TransactionEntity();
                        subTransactionList = new List<SubTransactionEntity>();
                        continue;
                    }
                    if (isSubTransactionHeaderStart && isSubTransactionHeaderBodyStart)
                    {
                        subTransactionList.Add(GetSubTransactionInfo(line));
                        continue;
                    }
                    if (isSubTransactionHeaderStart)
                        continue;

                    var lineSplite = line.Split(":");

                    switch (lineSplite[0].Trim())
                    {
                        case "Financial Institution":
                            transaction.FinancialInstitution = lineSplite[1].Trim();
                            break;
                        case "FX Settlement Date":
                            transaction.FXSettlementDate = lineSplite[1].Trim();
                            break;
                        case "Reconciliation File ID":
                            transaction.ReconciliationFileID = lineSplite[1].Trim();
                            break;
                        case "Transaction Currency":
                            transaction.TransactionCurrency = lineSplite[1].Trim();
                            break;
                        case "Reconciliation Currency":
                            transaction.ReconciliationCurrency = lineSplite[1].Trim();
                            break;
                        default:
                            break;
                    }

                }

            }
            foreach (var item in transactionList)
            {
                _unitOfWork.Repository<TransactionEntity>().Add(item);
            }
            return RedirectToAction("UploadFile");
        }

        private SubTransactionEntity GetSubTransactionInfo(string line)
        {
            line = line.Substring(1);
            var start = true;
            var position = 1;
            var value = new StringBuilder();
            var subTransaction = new SubTransactionEntity();
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == '!')
                {
                    subTransaction.NetValue = Convert.ToDecimal(value.ToString());
                    break;
                }
                if (line[i] == ' ' && line[i + 1] == ' ' && start)
                {
                    start = false;
                    switch (position)
                    {
                        case 1:
                            subTransaction.SettlementCategory = value.ToString().Trim();
                            break;
                        case 2:
                            subTransaction.TransactionAmountcredit = Convert.ToDecimal(value.ToString().Trim());
                            break;
                        case 3:
                            subTransaction.ReconciliationAmntCredit = Convert.ToDecimal(value.ToString().Trim());
                            break;
                        case 4:
                            subTransaction.FeeAmountCredit = Convert.ToDecimal(value.ToString().Trim());
                            break;
                        case 5:
                            subTransaction.TransactionAmountDebit = Convert.ToDecimal(value.ToString().Trim());
                            break;
                        case 6:
                            subTransaction.ReconciliationAmntDebit = Convert.ToDecimal(value.ToString().Trim());
                            break;
                        case 7:
                            subTransaction.FeeAmountDebit = Convert.ToDecimal(value.ToString().Trim());
                            break;
                        case 8:
                            subTransaction.CountTotal = Convert.ToInt32(value.ToString());
                            break;
                        default:
                            break;
                    }
                    position++;
                    value = new StringBuilder();
                }
                if (line[i] != ' ' && !start)
                {
                    start = true;
                }
                if (!start)
                    continue;

                value.Append(line[i]);
            }
            return subTransaction;
        }

        #endregion

        #region Detail

        public IActionResult Detail(int Id)
        {
            ViewBag.SubTransactionModel = _unitOfWork.Repository<SubTransactionEntity>().FindAll(x => x.TransactionId == Id);
            return View();
        }

        #endregion
    }
}
