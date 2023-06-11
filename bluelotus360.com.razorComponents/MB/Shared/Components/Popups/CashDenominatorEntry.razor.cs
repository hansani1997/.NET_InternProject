using BlueLotus360.CleanArchitecture.Application.Validators.MessageSystem;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using bluelotus360.com.razorComponents.Extensions;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MudBlazor;

namespace bluelotus360.com.razorComponents.MB.Shared.Components.Popups
{
    public partial class CashDenominatorEntry
    {
        private CashInOutTransaction cashtransaction;

        [Parameter]
        public BLUIElement CashInOutUIDefeinition { get; set; }
        [Parameter]
        public IDictionary<string, EventCallback> InteractionLogics { get; set; }


        [Parameter]
        public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }


        [Parameter]
        public CodeBaseResponse CashInOutLocation { get; set; }

        UserMessageManager messages;


        [Parameter]
        public EventCallback CloseDenominatorWindow { get; set; }

        protected override Task OnInitializedAsync()
        {
            cashtransaction = new CashInOutTransaction();

            messages = new UserMessageManager();
            return base.OnInitializedAsync();

        }


        private CashDenominator _refDenominator;
        private bool IsPopUpShown = false;
        private DialogOptions dialogOptions = new() { FullScreen = true };


        protected override void OnParametersSet()
        {
            cashtransaction.ElementKey = (CashInOutUIDefeinition == null ? 1 : CashInOutUIDefeinition.ElementKey);
            if (CashInOutLocation != null)
            {
                cashtransaction.Location = CashInOutLocation;
            }
            base.OnParametersSet();
        }

        public void Refresh()
        {
            StateHasChanged();
        }

        public void Reset()
        {
            cashtransaction = new CashInOutTransaction();
            cashtransaction.ElementKey = CashInOutUIDefeinition.ElementKey;
            ToggleEditability("CashInSaveButton", true);
            StateHasChanged();
        }
       

        public async Task SaveCashInOut()
        {
            URLDefinitions uRLDefinitions = new URLDefinitions();
            messages.UserMessages.Clear();
            cashtransaction.ElementKey = CashInOutUIDefeinition.ElementKey;
            uRLDefinitions.URL = CashInOutUIDefeinition.UrlController + "/" + CashInOutUIDefeinition.UrlAction;
            if (cashtransaction.Location == null || cashtransaction.Location.CodeKey < 11)
            {

                messages.AddErrorMessage("Please select a Location");
            }
            if (cashtransaction.Address == null || cashtransaction.Address.AddressKey < 11)
            {

                messages.AddErrorMessage("Please select a Supplier or a Customer");
            }
            if (cashtransaction.Amount <= 0)
            {

                messages.AddErrorMessage("Amount Cannot be Zero");
            }

            if (messages.IsValidForm())
            {
                await _transactionManager.SaveCashInOutTransaction(cashtransaction, uRLDefinitions);
                //CashInSaveButton
                ToggleEditability("CashInSaveButton", false);
            }

            StateHasChanged();



            //
        }

        public async void CloseWindow()
        {
            IsPopUpShown = false;
            if (CloseDenominatorWindow.HasDelegate)
            {
                await CloseDenominatorWindow.InvokeAsync();
            }
        }
        public void ShowPopUp()
        {
            IsPopUpShown = true;
            StateHasChanged();
        }

        public async void SaveDenominations()
        {
            if (_refDenominator != null)
            {
                await _transactionManager.SaveDenominations(_refDenominator.Entries);

            }
        }

        private void ToggleEditability(string name, bool visible)
        {
            IBLUIOperationHelper helper;

            if (ObjectHelpers.TryGetValue(name, out helper))
            {
                //helper.ToggleEditable(visible);
                StateHasChanged();
            }
        }
    }
}
