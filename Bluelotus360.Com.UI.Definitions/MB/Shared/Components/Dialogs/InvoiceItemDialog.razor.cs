using BlueLotus360.CleanArchitecture.Application.Validators.SalesOrder;
using BlueLotus360.CleanArchitecture.Application.Validators.Transaction;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using BlueLotus360.Com.UI.Definitions.Extensions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlueLotus360.Com.UI.Definitions.MB.Shared.Components.Dialogs
{
    public partial class InvoiceItemDialog
    {
        [CascadingParameter]
        MudDialogInstance MudDialog { get; set; }

        [Parameter]
        public TransactionLineItem InvoiceItem { get; set; }

        [Parameter]
        public BLUIElement ModalUIElement { get; set; }

        [Parameter]
        public IDictionary<string, EventCallback> InteractionLogics { get; set; }

        [Parameter]
        public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }

        [Parameter]
        public CodeBaseResponse ParentLocation { get; set; }


        [Parameter]
        public ITransactionValidator Validaor { get; set; }

        protected override Task OnInitializedAsync()
        {

            return base.OnInitializedAsync();
        }


        MudMessageBox mbox;

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        public async void AddItem()
        {
            if (Validaor != null)
            {
                if (Validaor.CanAddItemToGrid())
                {
                    MudDialog.Close(DialogResult.Ok(InvoiceItem));
                }
                else
                {


                }
            }

        }


    }
}
