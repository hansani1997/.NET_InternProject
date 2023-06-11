using BL10.CleanArchitecture.Domain.Entities.Transaction;
using BL10.CleanArchitecture.Domain.Entities.Validation;
using BlueLotus360.CleanArchitecture.Application.Responses.ServerResponse;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.commonLib.Managers.TransactionManager
{
    public interface ITransactionManager:IManager
    {
        Task<ExtendedTransaction> SaveTransaction(BLTransaction transation);
        Task<ExtendedTransaction> SaveTransaction(string transation);
        Task SaveCashInOutTransaction(CashInOutTransaction transaction, URLDefinitions uRL);
        Task HoldTransaction(BLTransaction transaction);
        Task<StockAsAtResponse> GetStockAsAt(StockAsAtRequest rquest);
        Task<FindTransactionResponse> FindTransactions(TransactionFindRequest request, URLDefinitions uRL);
        Task<BLTransaction> OpenTransaction(TransactionOpenRequest request);
        Task<BLTransaction> OpenTransaction(TransactionOpenRequest request, URLDefinitions URL);
        Task<IList<RecieptDetailResponse>> GetRecieptDetailResponses(RecieptDetailRequest request, URLDefinitions urlDef);
        Task<BaseServerResponse<IList<GetFromTransactionResponse>>> GetFromTransactions(GetFromTransactionRequest request, URLDefinitions urlDef);
        Task SaveAccountRecieptPayement(AccoutRecieptPayment accoutReciept);
        Task<BaseServerResponse<BLTransaction>> ReadFromTransaction(FromTransactionOpenRequest request);
        Task<IList<DenominationEntry>> ReadDenominationEntries(DenominationRequest request);
        Task SaveDenominations(IList<DenominationEntry> denominations);
        Task<IList<ItemSerialNumber>> RetriveItemTransactionSerials(ItemTransactionSerialRequest request);
        Task<IList<BLTransaction>> LoadTransactionApprovals(FindTransactionStatus request);
        Task UpadteTransactionApprovals(BLTransaction request);
        Task<RecviedAmountResponse> GetTotalPayedAmount(RecieptDetailRequest request);
        Task<InvoiceDetailsByHdrSerNo> GetInvoiceFromSerialNumber(ItemSerialNumber serialNumber);
        Task SaveItemSerialNumber(ItemSerialNumber serialNumber);
        //Task SaveAccountRecieptPayementEx(PayementModeReciept reciept);
        Task<ValidateModel> SaveAccountRecieptPayementEx(PayementModeReciept accoutReciept, string URL);
        Task<TransactionPermission> CheckTransactionPermission(BLTransaction req);
        Task SaveHeaderSerialNumber(ItemSerialNumber serialNumber);
        Task<ApiServerResponse<CustomerOutStadingDetails>> GetCustomerOutStandingDetails(CustomerOutStanding request, string url);
        Task<ValidateModel> ValidateSerialNumberEntries(ItemSerialNumber request, string url);
    }
}
