using BL10.CleanArchitecture.Domain.Entities.Validation;
using bluelotus360.Com.commonLib.Routes;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace bluelotus360.com.razorComponents.Pages.Transaction.TransactionComponent
{
    public partial class ItemSerialNumberEntry
    {


        private bool IsPopUpShown;

        [Parameter] public TransactionLineItem LineItem { get; set; }
        [Parameter] public IList<string> SerialNumberList { get; set; }

        private string EnteringKey = "";

        ValidateModel validate = new ValidateModel();
        bool _isSerialNumberValid = true;
        public decimal GetTotalScans()
        {
            return LineItem.TransactionQuantity * LineItem.Quantity2;
        }

        public decimal GetPendingScans()
        {
            return GetTotalScans() - LineItem.SerialNumbers.Count;

        }

        protected override async Task OnInitializedAsync()
        {
            //if (LineItem.IsPersisted)
            //{
            //    ItemTransactionSerialRequest request = new ItemTransactionSerialRequest();
            //    request.ItemTransactionKey = LineItem.ItemTransactionKey;
            //    LineItem.SerialNumbers = await _transactionManager.RetriveItemTransactionSerials(request);
            //}
            await base.OnInitializedAsync();
        }

        public async void OnValueChange(string value)
        {
            validate = new ValidateModel();
            _isSerialNumberValid = true;

            if (!string.IsNullOrWhiteSpace(value))
            {
                ItemSerialNumber serialNumber = new ItemSerialNumber();
                serialNumber.SerialNumber = value;
                serialNumber.ElementKey = LineItem.ElementKey;
                serialNumber.IsCompanyValidation = true;

                if (SerialNumberList.Where(x => x.Equals(value)).Count() == 0)
                {
                    validate = await _transactionManager.ValidateSerialNumberEntries(serialNumber, BaseEndpoint.BaseURL + "Transaction/validateSerialNumberEntries");
                    if (validate != null && validate.HasError)
                    {
                        _isSerialNumberValid = false;
                    }
                }
                else
                {
                    _isSerialNumberValid = false;
                }

                if (_isSerialNumberValid)
                {
                    LineItem.SerialNumbers.Add(serialNumber);
                    EnteringKey = "";
                }
                else
                {
                    EnteringKey = value;
                }

            }


            StateHasChanged();
            await Task.CompletedTask;
        }


        private bool DoneAddingSerialNumbers()
        {
            return GetPendingScans() == 0;
        }


        bool success;
        string[] errors = { };

        MudForm form;











    }
}
