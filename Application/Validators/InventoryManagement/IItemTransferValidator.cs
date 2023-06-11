using BlueLotus360.CleanArchitecture.Application.Validators.MessageSystem;
using BlueLotus360.CleanArchitecture.Domain.Entities.InventoryManagement.ItemTransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.CleanArchitecture.Application.Validators.InventoryManagement
{
    public interface IItemTransferValidator
    {
        UserMessageManager UserMessages { get; set; }
        bool CanOpenScan();
        bool CanAddItemToGrid();
        bool CanSaveOrUpdateItemTransfer();
        void AddAlertMessage(string msg,string type);
        void AddValidationErrors();
        bool CanApproveByScan();
    }
}
