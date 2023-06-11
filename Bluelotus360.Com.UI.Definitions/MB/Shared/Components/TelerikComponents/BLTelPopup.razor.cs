using BlueLotus360.CleanArchitecture.Client.Infrastructure.Helpers;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain.Entities.InventoryManagement.ItemTransfer;
using BlueLotus360.Com.UI.Definitions.Extensions;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Com.UI.Definitions.MB.Shared.Components.TelerikComponents
{
    public partial class BLTelPopup
    {
        [Parameter]
        public object DataObject { get; set; }

        [Parameter] public BLUIElement ModalUIElement { get; set; }
        [Parameter] public IDictionary<string, EventCallback> InteractionLogics { get; set; }

        [Parameter] public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }
        [Parameter]public string? PopupTitle { get; set; }
        [Parameter] public string? ActionButtonName { get; set; }
        [Parameter]public bool WindowIsVisible { get; set; }

        [Parameter]
        public EventCallback<TransferOpenRequest> OnOpenClick { get; set; }
        [Parameter]
        public EventCallback OnCloseButtonClick { get; set; }

        private FindItemTransferRequest findRequest;
        private List<FindItemTransferResponse> FoundTransfer;
        bool HideMinMax { get; set; } = false;
        private BLUIElement formDefinition;
        protected override async Task OnParametersSetAsync()
        {
            findRequest = new FindItemTransferRequest();
            findRequest.ObjKy = (int)ModalUIElement.ElementKey;
            FoundTransfer = new List<FindItemTransferResponse>();

            if (ModalUIElement != null)
            {
                var formrequest = new ObjectFormRequest();
                formrequest.MenuKey = ModalUIElement.ElementKey;
                formDefinition = await _navManger.GetMenuUIElement(formrequest);//get ui elements
                InteractionHelper helper = new InteractionHelper(this, formDefinition);//formdefinition has all form objects 
                InteractionLogics = helper.GenerateEventCallbacks();//
                StateHasChanged();
            }
            await base.OnParametersSetAsync();
        }

        private void OnFindFormFromDateClick(UIInterectionArgs<DateTime?> args)
        {
            findRequest.FrmDt = (DateTime)args.DataObject;
            StateHasChanged();
        }

        private void OnFindFormToDateClick(UIInterectionArgs<DateTime?> args)
        {
            findRequest.ToDt = (DateTime)args.DataObject;
            StateHasChanged();
        }

        private void OnFindFormTrnNoClick(UIInterectionArgs<string> args)
        {
            if (args.DataObject != string.Empty)
            {
                findRequest.TrnNo = args.DataObject;
            }
            else
            {
                findRequest.TrnNo = null;
            }

            StateHasChanged();
        }
        private void OnPrefixChange(UIInterectionArgs<CodeBaseResponse> args)
        {
            if (args.DataObject != null)
                findRequest.PreFixKy = args.DataObject.CodeKey;
        }

        private void OnFindFormFromLocChange(UIInterectionArgs<CodeBaseResponse> args)
        {
            findRequest.FrmLoc = args.DataObject;
            StateHasChanged();
        }
        private void OnFindFormToLocChange(UIInterectionArgs<CodeBaseResponse> args)
        {
            findRequest.ToLoc = args.DataObject;
            StateHasChanged();
        }

        private async void OnFindFormLoadClick(UIInterectionArgs<object> args)
        {

            FoundTransfer = await _itemTransferManager.Find(findRequest);

            StateHasChanged();
        }

        private async void OnFindFormCancelButtonClick(UIInterectionArgs<object> args)
        {

            //if (OnCloseButtonClick.HasDelegate)
            //{
            //    await OnCloseButtonClick.InvokeAsync();
            //}
            findRequest = new FindItemTransferRequest();
            FoundTransfer = new List<FindItemTransferResponse>();
            StateHasChanged();
        }


        private async void OpenTransfer(FindItemTransferResponse item)
        {
            TransferOpenRequest request = new TransferOpenRequest();
            request.TrnKy = item.TrnKy;

            if (OnOpenClick.HasDelegate)
            {
                await OnOpenClick.InvokeAsync(request);

            }
        }

    }
}
