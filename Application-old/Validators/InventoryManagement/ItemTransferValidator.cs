using BlueLotus360.CleanArchitecture.Application.Validators.MessageSystem;
using BlueLotus360.CleanArchitecture.Domain.Entities.InventoryManagement.ItemTransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.CleanArchitecture.Application.Validators.InventoryManagement
{
    public class ItemTransferValidator: IItemTransferValidator
    {
        private ItemTransfer _itemTransfer;
        public UserMessageManager UserMessages { get; set; }

        public ItemTransferValidator(ItemTransfer itemTransfer)
        {
            _itemTransfer=itemTransfer;
            UserMessages = new UserMessageManager();
        }

        public bool CanOpenScan()
        {
            UserMessages.UserMessages.Clear();

            if (_itemTransfer.FromLocation.CodeKey == 1)
            {
                UserMessages.AddErrorMessage("Please Enter Transfer Out Location");
            }
            if (_itemTransfer.ToLocation.CodeKey == 1)
            {
                UserMessages.AddErrorMessage("Please Enter Transfer In Location");
            }

            return UserMessages.UserMessages.Count == 0;
        }

        public void AddValidationErrors()
        {
            
            if (_itemTransfer.ValidationMessages.Count() > 0)
            {
                foreach (string msg in _itemTransfer.ValidationMessages)
                {
                    UserMessages.AddErrorMessage(msg);
                }
                _itemTransfer.ValidationMessages.Clear();
            }
        }
        public bool CanAddItemToGrid()
        {
            UserMessages.UserMessages.Clear();

            if (_itemTransfer.FromLocation.CodeKey == 1)
            {
                UserMessages.AddErrorMessage("Please Enter Transfer Out Location");
            }
            if (_itemTransfer.ToLocation.CodeKey == 1)
            {
                UserMessages.AddErrorMessage("Please Enter Transfer In Location");
            }
            
            return UserMessages.UserMessages.Count == 0;
        }

        public bool CanSaveOrUpdateItemTransfer()
        {
            UserMessages.UserMessages.Clear();

            if (_itemTransfer.LocationWiseSerialNoValidations!=null && _itemTransfer.LocationWiseSerialNoValidations.HasError)
            {
                UserMessages.AddErrorMessage(_itemTransfer.LocationWiseSerialNoValidations.Message);
            }

            return UserMessages.UserMessages.Count == 0;
        }

        public void AddAlertMessage(string msg, string type)
        {
            UserMessages.AddAlertMessage(msg,type);
        }
    }

}
