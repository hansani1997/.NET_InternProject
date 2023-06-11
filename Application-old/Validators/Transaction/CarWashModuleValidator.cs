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
	public class CarWashModuleValidator : ITransactionValidator
	{
		private BLTransaction _transaction;
		public UserMessageManager UserMessages { get; set; }

		public CarWashModuleValidator(BLTransaction transaction) 
		{
			this._transaction = transaction;
			UserMessages = new UserMessageManager();
		}
		public bool CanAddItemToGrid(decimal? AvailableStockQuantity = null)
		{
			UserMessages.UserMessages.Clear();

			if (_transaction.Account != null && _transaction.Account.AccountKey < 10 && _transaction.Account.IsMust)
			{
				UserMessages.AddErrorMessage("Customer is required ");
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
				UserMessages.AddErrorMessage("Transaction Unit Cannot be empty");
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

		public bool CanItemComboRequestFromServer()
		{
			return BaseResponse.IsValidData(_transaction.SelectedLineItem.ItemCategory1) && BaseResponse.IsValidData(_transaction.SelectedLineItem.ItemCategory2);
		}

		public bool CanSaveTransaction()
		{
			UserMessages.UserMessages.Clear();

			if (_transaction != null && !BaseResponse.IsValidData(_transaction.Account) && _transaction.Account.IsMust)
			{
				UserMessages.AddErrorMessage("Please Select a Customer Account");
			}

			if (_transaction.InvoiceLineItems.Count == 0)
			{
				UserMessages.AddErrorMessage("No Details Found");
			}

			return UserMessages.UserMessages.Count == 0;
		}
	}
}
