using BlueLotus360.CleanArchitecture.Application.Validators.MessageSystem;
using BlueLotus360.CleanArchitecture.Application.Validators.SalesOrder;
using BlueLotus360.CleanArchitecture.Client.Infrastructure.Helpers;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain.Entities.InventoryManagement.ItemTransfer;
using BlueLotus360.Com.UI.Definitions.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlueLotus360.Com.UI.Definitions.MB.Shared.Components.Popups.InventoryManagement.ScanItemTransfer
{
    public partial class FindPopUp
    {
        #region Parameters
        [Parameter]
        public BLUIElement UIElement { get; set; }

        [Parameter]
        public EventCallback OnCloseButtonClick { get; set; }

        [Parameter]
        public EventCallback<TransferOpenRequest> OnOpenClick { get; set; }


        public IDictionary<string, EventCallback> InteractionLogics { get; set; }
        public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }
        private FindItemTransferRequest findRequest;
        private MudTable<FindItemTransferResponse> _table;

        private List<FindItemTransferResponse> FoundTransfer;
        private BLUIElement formDefinition;
        private bool isLoading = false;
        private bool isInitial = false;
        #endregion

        protected override async Task OnParametersSetAsync()
        {
            findRequest = new FindItemTransferRequest();
            findRequest.ObjKy = (int)UIElement.ElementKey;
            FoundTransfer = new List<FindItemTransferResponse>();

            if (UIElement != null)
            {
                var formrequest = new ObjectFormRequest();
                formrequest.MenuKey = UIElement.ElementKey;
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
            if (args.DataObject!=null)
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

        private async void OnFindFormCancelButtonClick(UIInterectionArgs<object> args)
        {

            if (OnCloseButtonClick.HasDelegate)
            {
                await OnCloseButtonClick.InvokeAsync();
            }

        }

        private async void OnFindFormLoadClick(UIInterectionArgs<object> args)
        {
           isInitial = true;
           
            isLoading = true;
            StateHasChanged();

            FoundTransfer = await _itemTransferManager.Find(findRequest);

            isLoading = false;
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
