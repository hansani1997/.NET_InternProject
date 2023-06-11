using BlueLotus360.CleanArchitecture.Application.Validators.MessageSystem;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BlueLotus360.CleanArchitecture.Application.Validators.Transaction
{
    public class TransactionValidator: ITransactionValidator
    {
        private BLTransaction _transaction;
        public UserMessageManager UserMessages { get; set; }

        public TransactionValidator(BLTransaction transaction)
        {
            this._transaction = transaction;
            UserMessages = new UserMessageManager();
        }


      


        public bool CanAddItemToGrid(decimal ? AvailableStockQuantity=null)
        {
            UserMessages.UserMessages.Clear();

            if (_transaction.Location.CodeKey<10 && _transaction.Location.IsMust)
            {
                UserMessages.AddErrorMessage("Transaction Location is required ");
            }

            if (_transaction.Code1.CodeKey <10  && _transaction.Code1.IsMust)
            {
                UserMessages.AddErrorMessage("Please select a Price List");
            }

            if (_transaction.Account!=null  && _transaction.Account.AccountKey <10 && _transaction.Account.IsMust)
            {
                UserMessages.AddErrorMessage("Customer is required ");
            }

            if (_transaction.PaymentTerm.CodeKey<10 && _transaction.IsMust)
            {
                UserMessages.AddErrorMessage("Payement Term is required ");
            }

            if (_transaction.SelectedLineItem.TransactionItem.ItemKey<11)
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
                UserMessages.AddErrorMessage("Transaction Unit Cannot be empty");
            }

            if (AvailableStockQuantity.HasValue)
            {
                if (_transaction.SelectedLineItem.TransactionQuantity>AvailableStockQuantity.Value)
                {
                    UserMessages.AddErrorMessage($"Cannot Add {_transaction.SelectedLineItem.TransactionQuantity} as Available stock is {AvailableStockQuantity.Value.ToString()}");

                }
            }

            if (_transaction.ContraAccountObjectKey<11)
            {
                UserMessages.AddErrorMessage("ContraAccount Object Not Set");

            }

            if (_transaction.AccountObjectKey < 11)
            {
                UserMessages.AddErrorMessage("Account Object Not Set");

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

            if (_transaction.PaymentTerm!=null && (_transaction.PaymentTerm.CodeName.Contains("Cash", StringComparison.InvariantCultureIgnoreCase) || _transaction.PaymentTerm.CodeName.Contains("Card", StringComparison.InvariantCultureIgnoreCase) ))
            {
                decimal transactionTotal=_transaction.GetOrderTotalWithDiscounts();
                decimal payedTotal = this._transaction.Amount2 + this._transaction.Amount3;
                if (payedTotal < transactionTotal)
                {
                    UserMessages.AddErrorMessage($"Cannot Process since Total Payable {transactionTotal} is more than Payed Amount {payedTotal}");
                }
            }
            return UserMessages.UserMessages.Count == 0;
        }

        public bool CanItemComboRequestFromServer()
        {
            throw new NotImplementedException();
        }
    }
}
