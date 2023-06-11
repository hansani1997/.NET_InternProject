using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.razorComponents.MB.Shared.Components.Dialogs
{
	public partial class AddNewAddressDialog
	{
		private AddressResponse _addressReponse;

		[CascadingParameter]
		MudDialogInstance MudDialog { get; set; }


		private string ErrorMessage = "";

		public AddNewAddressDialog()
		{
			_addressReponse = new AddressResponse();
			_addressReponse.AddressName = "";
			ErrorMessage = "No Errors";
		}
		void Cancel() => MudDialog.Cancel();

		private void Submit()
		{
			if (string.IsNullOrEmpty(_addressReponse.AddressID) || _addressReponse.AddressID.Trim().Length < 3)
			{
				ErrorMessage = "Customer ID Cannot be empty,Should contain atleast 3 Characters";
				return;
			}

			if (string.IsNullOrEmpty(_addressReponse.AddressName) || _addressReponse.AddressName.Trim().Length < 3)
			{
				ErrorMessage = "Customer Name Cannot be empty,Should Contain atleast 3 characters";
				return;
			}
		}
	}
}
