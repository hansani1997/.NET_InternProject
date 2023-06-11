using BlueLotus360.CleanArchitecture.Domain.DTO.Report;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.commonLib.Managers.Printer
{
    public interface IInvoicePrinterManager : IManager
    {
        Task PrintTransactionBillLocalAsync(TransactionReportLocal report, URLDefinitions definitions);

    }

}
