using BlueLotus360.CleanArchitecture.Application.Responses.ServerResponse;
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
    public partial class GetFromTransaction
    {
        private GetFromTransactionRequest getFromTransaction = new GetFromTransactionRequest();

        [Parameter]
        public BLUIElement UIDefinition { get; set; }
        [Parameter]
        public IDictionary<string, EventCallback> InteractionLogics { get; set; }


        [Parameter]
        public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }


        [Parameter]
        public CodeBaseResponse FindLocation { get; set; }

        UserMessageManager messages;

        private BaseServerResponse<IList<GetFromTransactionResponse>> serverResponse;


        [Parameter]
        public EventCallback<BaseServerResponse<BLTransaction>> FromTransactionOpened { get; set; }


        protected override Task OnInitializedAsync()
        {
            serverResponse = new();
            serverResponse.DataObject = new List<GetFromTransactionResponse>();
            getFromTransaction = new GetFromTransactionRequest();

            messages = new UserMessageManager();
            return base.OnInitializedAsync();

        }



        protected override void OnParametersSet()
        {
            getFromTransaction.ElementKey = (UIDefinition == null ? 1 : UIDefinition.ElementKey);
            if (FindLocation != null)
            {
                getFromTransaction.Location = FindLocation;
            }
            base.OnParametersSet();
        }

        public void Refresh()
        {
            StateHasChanged();
        }

        public void Reset()
        {
            getFromTransaction = new GetFromTransactionRequest();
            getFromTransaction.ElementKey = UIDefinition.ElementKey;
            ToggleEditability("CashInSaveButton", true);
            StateHasChanged();
        }


        public async Task GetTransactionFromServer()
        {
            URLDefinitions uRLDefinitions = new URLDefinitions();
            messages.UserMessages.Clear();
            getFromTransaction.ElementKey = UIDefinition.ElementKey;
            uRLDefinitions.URL = UIDefinition.UrlController + "/" + UIDefinition.UrlAction;
            serverResponse = await _transactionManager.GetFromTransactions(getFromTransaction, uRLDefinitions);
            if (serverResponse.ResponseType == CleanArchitecture.Application.Responses.ServerResponse.ServerResponseType.Success)
            {

            }
            StateHasChanged();



            //
        }


        public async Task OpenFromTransaction(GetFromTransactionResponse item)
        {
            FromTransactionOpenRequest request = new();
            request.ElementKey = UIDefinition.ElementKey;
            request.TransactionKey = item.TransactionKey;
            BaseServerResponse<BLTransaction> transactionReadResponse = await _transactionManager.ReadFromTransaction(request);
            if (FromTransactionOpened.HasDelegate)
            {
               await FromTransactionOpened.InvokeAsync(transactionReadResponse);
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
