using BL10.CleanArchitecture.Application.Validators.FileUpload;
using BL10.CleanArchitecture.Domain.Entities.Document;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using bluelotus360.Com.commonLib.Helpers;
using bluelotus360.com.razorComponents.Extensions;

namespace bluelotus360.com.razorComponents.MB.Shared.Components.Popups.Transaction
{
    public partial class FindTransactionPopUp
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
        [Parameter] public string? PopupTitle { get; set; }
        [Parameter] public string? ActionButtonName { get; set; }
        [Parameter] public bool WindowIsVisible { get; set; }
        [Parameter] public IDictionary<string, EventCallback> InteractionLogics { get; set; }
        [Parameter] public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }


        private TransactionFindRequest transaction;

        private FindTransactionResponse FoundTransactions;
        private BLUIElement formDefinition;
        bool HideMinMax { get; set; } = false;
        private DialogOptions dialogOptions = new() { CloseButton = true,Position=DialogPosition.Center };
        #endregion


        protected override async Task OnParametersSetAsync()
        {
            transaction = new TransactionFindRequest();
            FoundTransactions = new();

            if (UIElement != null)
            {
                transaction.ElementKey = UIElement.ElementKey;
                var formrequest = new ObjectFormRequest();
                formrequest.MenuKey = UIElement.ReferenceElementKey;
                formDefinition = await _navManger.GetMenuUIElement(formrequest);//get ui elements
                InteractionHelper helper = new InteractionHelper(this, formDefinition);//formdefinition has all form objects 
                InteractionLogics = helper.GenerateEventCallbacks();//
                StateHasChanged();
            }
            await base.OnParametersSetAsync();
        }

        private async void OnCloseClick()
        {
            if (OnCloseButtonClick.HasDelegate)
            {
                await OnCloseButtonClick.InvokeAsync();
            }
        }
        private async void OnFindCancelButtonClick(UIInterectionArgs<object> args)
        {

            if (OnCloseButtonClick.HasDelegate)
            {
                await OnCloseButtonClick.InvokeAsync();
            }

        }

        async void Cancel() {
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
        private async void OpenTransaction(FindTransactionLineItem item)
        {

            TransactionOpenRequest request = new TransactionOpenRequest();
            request.TransactionKey = item.TransactionKey;
            if (OnOpenClick.HasDelegate)
            {
                await OnOpenClick.InvokeAsync(request);

            }
        }
    }
}
