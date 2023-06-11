using BlueLotus360.CleanArchitecture.Application.Validators.MessageSystem;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BlueLotus360.CleanArchitecture.Application.Validators.Transaction
{
    public class SalesReturnValidator: ITransactionValidator
    {
        private BLTransaction _transaction;
        public UserMessageManager UserMessages { get; set; }

        public SalesReturnValidator(BLTransaction transaction)
        {
            this._transaction = transaction;
            UserMessages = new UserMessageManager();
        }
        public bool CanAddItemToGrid(decimal ? AvailableStockQuantity=null)
        {
            UserMessages.UserMessages.Clear();

            if (_transaction.Location.CodeKey<10)
            {
                UserMessages.AddErrorMessage("Transaction Location is required ");
            }


            if (_transaction.Account.AccountKey <10)
            {
                UserMessages.AddErrorMessage("Customer is required ");
            }

            if (_transaction.PaymentTerm.CodeKey<10)
            {
                UserMessages.AddErrorMessage("Payement Term is required ");
            }

            if (_transaction.SelectedLineItem.TransactionItem.ItemKey<11)
            {
                UserMessages.AddErrorMessage("Transaction Item  is required ");
            }

            
            if (_transaction.SelectedLineItem.TransactionQuantity <= 0)
            {
                UserMessages.AddErrorMessage("Transaction Quantity Cannot be Zero or less");
            }

            if (_transaction.SelectedLineItem.TransactionUnit.UnitKey < 11)
            {
                UserMessages.AddErrorMessage("Transaction Unit Cannot be empty");
            }

            if (AvailableStockQuantity.HasValue)
            {
                if (_transaction.SelectedLineItem.TransactionQuantity>AvailableStockQuantity.Value)
                {
                    UserMessages.AddErrorMessage($"Cannot Add {_transaction.SelectedLineItem.TransactionQuantity.ToString()} as Balance Quantity is {AvailableStockQuantity.Value.ToString()}");

                }
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
            throw new NotImplementedException();
        }

        public bool CanItemComboRequestFromServer()
        {
            throw new NotImplementedException();
        }
    }
}
