using BlueLotus360.CleanArchitecture.Application.Validators.MessageSystem;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using BlueLotus360.Com.UI.Definitions.Extensions;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlueLotus360.Com.UI.Definitions.MB.Shared.Components.Popups
{
    public partial class CashIn
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

        protected override Task OnInitializedAsync()
        {
            cashtransaction = new CashInOutTransaction();

            messages = new UserMessageManager();
            return base.OnInitializedAsync();

        }



        protected override void OnParametersSet()
        {
            cashtransaction.ElementKey = (CashInOutUIDefeinition==null ?1 : CashInOutUIDefeinition.ElementKey);
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
                ToggleEditability("CashInSaveButton", false);
                cashtransaction.TransactionDate = System.DateTime.Now;
                await _transactionManager.SaveCashInOutTransaction(cashtransaction, uRLDefinitions);
                //CashInSaveButton
             
            }
           // ToggleEditability("CashInSaveButton", false);

            StateHasChanged();



            //
        }

        private void ToggleEditability(string name, bool visible)
        {
            IBLUIOperationHelper helper;

            if (ObjectHelpers.TryGetValue(name, out helper))
            {
                helper.ToggleEditable(visible);
                StateHasChanged();
            }
        }
    }
}
