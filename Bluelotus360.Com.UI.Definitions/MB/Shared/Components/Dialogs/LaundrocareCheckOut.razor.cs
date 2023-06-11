using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Com.UI.Definitions.MB.Shared.Components.Dialogs
{
    public partial class LaundrocareCheckOut
    {
        [CascadingParameter] public MudDialogInstance MudDialog { get; set; }
        [Parameter] public BLUIElement InitiatorElement { get; set; }

        string searchvalue = "";

        TransactionViewer _viewer;
        bool IsLoading = false;
        private BLTransaction loaded_Transaction;
        private InvoiceDetailsByHdrSerNo resp;

        async void Submit()
        {
            DialogResult result = DialogResult.Ok<int>(1);
            await TransactionLoadBySerial();
        }
        void Cancel() => MudDialog.Cancel();


        public async void OnValueChange(string value)
        {
            searchvalue = value;
            await TransactionLoadBySerial();
            if (_viewer != null)
            {
                await _viewer.Refesh();
            }
            StateHasChanged();
            await Task.CompletedTask;
        }

        private async Task TransactionLoadBySerial()
        {
            IsLoading = true;
            loaded_Transaction = null;
            await _viewer.Refesh();
            StateHasChanged();
            ItemSerialNumber serialNumber = new ItemSerialNumber();
            if (InitiatorElement != null)
            {
                serialNumber.ElementKey = InitiatorElement.ElementKey;

            }

            serialNumber.SerialNumber = searchvalue;

            if (!string.IsNullOrWhiteSpace(serialNumber.SerialNumber))
            {
                resp = await _transactionManager.GetInvoiceFromSerialNumber(serialNumber);
                TransactionOpenRequest openRequest = new TransactionOpenRequest();
                openRequest.ElementKey = InitiatorElement.ElementKey;
                openRequest.TransactionKey = resp.TransactionKey;
                loaded_Transaction = await _transactionManager.OpenTransaction(openRequest);
                loaded_Transaction.ElementKey = InitiatorElement.ElementKey;
                loaded_Transaction.TransactionKey = 1;
                loaded_Transaction.TransactionDate = System.DateTime.Now;
                loaded_Transaction.IsPersisted = false;
                loaded_Transaction.IsDirty = true;
                loaded_Transaction.DocumentNumber = "GIN-" + loaded_Transaction.TransactionNumber;
                loaded_Transaction.TransactionNumber = "";
                foreach (var item in loaded_Transaction.InvoiceLineItems)
                {
                    item.ItemTransactionKey = 1;
                    item.ObjectKey = InitiatorElement.ElementKey;
                    item.IsPersisted = false;
                    item.IsDirty = true;
                }

            }

            IsLoading = false;
            StateHasChanged();
        }
    }
}
