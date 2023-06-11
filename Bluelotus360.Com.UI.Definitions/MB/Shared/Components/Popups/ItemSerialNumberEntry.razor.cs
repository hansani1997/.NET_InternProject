using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BlueLotus360.Com.UI.Definitions.MB.Shared.Components.Popups
{
    public partial class ItemSerialNumberEntry
    {


        private bool IsPopUpShown;

        [Parameter] public TransactionLineItem LineItem { get; set; }

        private string EnteringKey = "";


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
            if (LineItem.IsPersisted)
            {
                ItemTransactionSerialRequest request = new ItemTransactionSerialRequest();
                request.ItemTransactionKey = LineItem.ItemTransactionKey;
                LineItem.SerialNumbers = await _transactionManager.RetriveItemTransactionSerials(request);
            }
            await base.OnInitializedAsync();
        }

        public async void OnValueChange(string value)
        {
            
            if (!string.IsNullOrWhiteSpace(value))
            {
                ItemSerialNumber serialNumber = new ItemSerialNumber();
                serialNumber.SerialNumber = value;
                LineItem.SerialNumbers.Add(serialNumber);
                EnteringKey = "";
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
