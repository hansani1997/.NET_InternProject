using BlueLotus360.CleanArchitecture.Application.Validators.MessageSystem;
using BlueLotus360.CleanArchitecture.Application.Validators.Transaction;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL10.CleanArchitecture.Application.Validators.Transaction
{
    public class WorkShopValidator : ITransactionValidator
    {
        private BLTransaction _transaction;
        public UserMessageManager UserMessages { get; set; }
        public WorkShopValidator(BLTransaction transaction) 
        {
            this._transaction = transaction;
            UserMessages = new UserMessageManager();
        }

        public bool CanAddItemToGrid(decimal? AvailableStockQuantity = null)
        {
            UserMessages.UserMessages.Clear();

            if (_transaction.Location.CodeKey < 10 && _transaction.Location.IsMust)
            {
                UserMessages.AddErrorMessage("Transaction Location is required ");
            }
            if (_transaction.PaymentTerm.CodeKey < 10 && _transaction.IsMust)
            {
                UserMessages.AddErrorMessage("Payement Term is required ");
            }

            return UserMessages.UserMessages.Count == 0;
        }

        public bool CanChangeHeaderInformatiom()
        {
            return _transaction.InvoiceLineItems.Count == 0;
        }

        public bool CanItemCodeSearch()
        {
            return _transaction.Code1.CodeKey != 1;
        }

        public bool CanSaveTransaction()
        {
            UserMessages.UserMessages.Clear();

            if (_transaction != null && !BaseResponse.IsValidData(_transaction.Location))
            {
                UserMessages.AddErrorMessage("Please Select a location");
            }
            //if (_transaction != null && !BaseResponse.IsValidData(_transaction.Address))
            //{
            //    UserMessages.AddErrorMessage("Please Select a Customer");
            //}

            if (_transaction != null && !BaseResponse.IsValidData(_transaction.Account) && _transaction.Account.IsMust)
            {
                UserMessages.AddErrorMessage("Please Select a Customer Account");
            }
            if (_transaction != null && !BaseResponse.IsValidData(_transaction.ContraAccount) && _transaction.ContraAccount.IsMust)
            {
                UserMessages.AddErrorMessage("Please Select a Sales Account");
            }
            if (_transaction != null && !BaseResponse.IsValidData(_transaction.PaymentTerm))
            {
                UserMessages.AddErrorMessage("Please select a payement Term");
            }

            if (_transaction.InvoiceLineItems.Count == 0)
            {
                UserMessages.AddErrorMessage("No Details Found");
            }

            return UserMessages.UserMessages.Count == 0;

        }

        public bool CanItemComboRequestFromServer()
        {
            return BaseResponse.IsValidData(_transaction.SelectedLineItem.ItemCategory1) && BaseResponse.IsValidData(_transaction.SelectedLineItem.ItemCategory2);
        }
    }
}
