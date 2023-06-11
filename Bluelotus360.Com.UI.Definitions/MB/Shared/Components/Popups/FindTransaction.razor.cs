using BlueLotus360.CleanArchitecture.Client.Infrastructure.Helpers;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.Com.UI.Definitions.Extensions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlueLotus360.Com.UI.Definitions.MB.Shared.Components.Popups
{

    public partial class FindTransaction
    {
        #region Parameters
        [Parameter]
        public BLUIElement UIElement { get; set; }

        [Parameter]
        public object ComboDataObject { get; set; }

        [Parameter]
        public EventCallback OnCloseButtonClick { get; set; }

        [Parameter]
        public EventCallback<TransactionOpenRequest> OnOpenClick { get; set; }

 
        public IDictionary<string, EventCallback> InteractionLogics { get; set; }


        public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }


        private TransactionFindRequest transaction;
        private MudTable<FindTransactionLineItem> _table;

        private FindTransactionResponse FoundTransactions;

        #endregion
        private BLUIElement formDefinition;

        protected override async Task OnParametersSetAsync()
        {
            transaction = new TransactionFindRequest();
            transaction.ElementKey = UIElement.ElementKey;
            FoundTransactions = new();

            if (UIElement != null)
            {
                var formrequest = new ObjectFormRequest();
                formrequest.MenuKey = UIElement.ReferenceElementKey;
                formDefinition = await _navManger.GetMenuUIElement(formrequest);//get ui elements
                InteractionHelper helper = new InteractionHelper(this, formDefinition);//formdefinition has all form objects 
                InteractionLogics = helper.GenerateEventCallbacks();//
                StateHasChanged();
            }
            await base.OnParametersSetAsync();
        }


        private async void OnFindCancelButtonClick(UIInterectionArgs<object> args)
        {

            if (OnCloseButtonClick.HasDelegate)
            {
                await OnCloseButtonClick.InvokeAsync();
            }

        }


        private async void OnFindButtonClick(UIInterectionArgs<object> args)
        {

            FoundTransactions = await _transactionManager.FindTransactions(transaction, null);
            StateHasChanged();


        }

        private string RowClassSelection(FindTransactionLineItem item, int RowNumber)
        {
            string value = string.Empty;

            if (item.IsApprove == 3)
            {
                value = "hold";
            }


            return value;
        }


        private async void OnFindLocationChanged(UIInterectionArgs<CodeBaseResponse> args)
        {
            StateHasChanged();
            await Task.CompletedTask;
        }
        private async void OnFindPrefixChanged(UIInterectionArgs<CodeBaseResponse> args)
        {
            StateHasChanged();
            await Task.CompletedTask;
        }
           private async void OnFindPayementTermChanged(UIInterectionArgs<CodeBaseResponse> args)
        {
            StateHasChanged();
            await Task.CompletedTask;
        }

        private async void OnFindFromDateChanged(UIInterectionArgs<DateTime?> args)
        {
            StateHasChanged();
            await Task.CompletedTask;
        }

        private async void OnFindToDateChanged(UIInterectionArgs<DateTime?> args)
        {
           
            StateHasChanged();
            await Task.CompletedTask;
        }
        private  async void OpenTransaction(FindTransactionLineItem item)
        {

            TransactionOpenRequest  request= new TransactionOpenRequest();
            request.TransactionKey = item.TransactionKey;
            if (OnOpenClick.HasDelegate)
            {
               await OnOpenClick.InvokeAsync(request);

            }
        }

    }
}
