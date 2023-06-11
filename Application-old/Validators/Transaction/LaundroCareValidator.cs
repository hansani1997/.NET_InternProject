using BlueLotus360.CleanArchitecture.Application.Validators.MessageSystem;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.CleanArchitecture.Application.Validators.Transaction
{
    public class LaundroCareValidator : ITransactionValidator
    {
        private BLTransaction _transaction;
        public UserMessageManager UserMessages { get; set; }

        public LaundroCareValidator(BLTransaction transaction)
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

            //if (_transaction.Code1.CodeKey < 10 && _transaction.Code1.IsMust)
            //{
            //    UserMessages.AddErrorMessage("Please select a Price List");
            //}

            if (_transaction.Account != null && _transaction.Account.AccountKey < 10 && _transaction.Account.IsMust)
            {
                UserMessages.AddErrorMessage("Customer is required ");
            }

            if (_transaction.PaymentTerm.CodeKey < 10 && _transaction.IsMust)
            {
                UserMessages.AddErrorMessage("Payement Term is required ");
            }

            if (_transaction.SelectedLineItem.TransactionItem.ItemKey < 11)
            {
                UserMessages.AddErrorMessage("Transaction Item  is required ");
            }

            if (_transaction.SelectedLineItem.TransactionItem.ItemName == "")
            {
                UserMessages.AddErrorMessage("Transaction Item Name is required ");
            }

            if (_transaction.SelectedLineItem.TransactionQuantity <= 0)
            {
                UserMessages.AddErrorMessage("Transaction Quantity Cannot be Zero or less");
            }

            if (_transaction.SelectedLineItem.TransactionUnit.UnitKey < 11)
            {
               //UserMessages.AddErrorMessage("Transaction Unit Cannot be empty");
            }
            if (_transaction.InvoiceLineItems.Where(x => x.IsActive == 1 && x.TransactionItem.IsWeightItem() && !x.IsInEditMode).Count() > 0 && _transaction.SelectedLineItem.TransactionItem.IsWeightItem())
            {
                UserMessages.AddErrorMessage("You have already added one kilo wash item");
            }
            if (_transaction.InvoiceLineItems.Where(x => x.IsActive==1 && x.TransactionItem.IsWeightItem() && !x.IsInEditMode).Count() > 0 && !_transaction.SelectedLineItem.IsKiloWashItem() && !_transaction.SelectedLineItem.IsCommonItem())
            {
                UserMessages.AddErrorMessage("You can't add this service type");
            }

            if (_transaction.SelectedLineItem.IsKiloWashItem())
            {
                if (_transaction.SelectedLineItem.TransactionItem.IsWeightItem())
                {
                    if (_transaction.SelectedLineItem.TransactionItem.HasUptoTenWeight())
                    {
                        if (_transaction.SelectedLineItem.TransactionQuantity < 10)
                        {
                            UserMessages.AddErrorMessage("Transaction Quantity Can not be less than 10");
                        }
                        if (_transaction.SelectedLineItem.Quantity2 > 1)
                        {
                            UserMessages.AddErrorMessage("Transaction pices Can not be more than 1");
                        }
                    }
                    else
                    {
                        if (_transaction.SelectedLineItem.TransactionQuantity > 1)
                        {
                            UserMessages.AddErrorMessage("Transaction Quantity Can not be more than 1");
                        }
                        if (_transaction.SelectedLineItem.Quantity2 > 1)
                        {
                            UserMessages.AddErrorMessage("Transaction pices Can not be more than 1");
                        }
                    }

                }

                // UserMessages.AddErrorMessage("Transaction Unit Cannot be empty");

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
            if (_transaction != null && !BaseResponse.IsValidData(_transaction.Address))
            {
                UserMessages.AddErrorMessage("Please Select a Customer");
            }

            if ( _transaction != null && !BaseResponse.IsValidData(_transaction.Account) &&  _transaction.Account.IsMust)
            {
                UserMessages.AddErrorMessage("Please Select a Customer Account");
            }
            if (_transaction != null && !BaseResponse.IsValidData(_transaction.ContraAccount) && _transaction.ContraAccount.IsMust)
            {
                UserMessages.AddErrorMessage("Please Select a Sales Account");
            }

            //if (_transaction != null && _transaction.ContraAccountObjectKey < 10)
            //{
            //    UserMessages.AddErrorMessage("Contra Account Object is not set");
            //}
            //if (_transaction != null && _transaction.AccountObjectKey < 10)
            //{
            //    UserMessages.AddErrorMessage(" Account Object is not set");
            //}
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
