using BlueLotus360.CleanArchitecture.Domain.Entities.InventoryManagement.ItemTransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.commonLib.Managers.ItemTransferManager
{
    public interface IItemTransferManager : IManager
    {
        Task<int> CreateItemTransfer(ItemTransfer itm);

        Task<ItemTransferLineItem> GetItemsData(ItemTransferLineItem res);

        Task<ItmtrnsferValidationResponse> TransferValidator(ItemTransfer itm);
        Task<ItmtrnsferValidationResponse> InvoiceTransferValidator(LNDInvoice invoice);
        Task<List<FindItemTransferResponse>> Find(FindItemTransferRequest req);
        Task<ItemTransfer> RefreshForm(TransferOpenRequest req);
        //Task<ItemTransfer> RefreshScanInvoice(TransferOpenRequest req);

        Task<int> UpdateItemTransfer(ItemTransfer req);

        Task<List<ItemTransferLineItem>> GetInvoiceData(LNDInvoice res);
        Task<List<string>> GetInvoiceSerialNoList(LNDInvoice res);
        Task<ItmtrnsferValidationResponse> TransferMultiAprLock(ItemTransfer req);
        Task<IList<ItemTransferLineItem>> GetItemtransferLineItemForApproval(ItemTransfer req);
        Task ItmTrnSerAprInsertWeb(IList<ItemTransferLineItem> reqList);
        bool IsExceptionthrown();

    }
}
