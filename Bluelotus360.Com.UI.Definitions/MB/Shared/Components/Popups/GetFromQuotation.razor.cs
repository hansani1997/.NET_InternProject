using BlueLotus360.CleanArchitecture.Client.Infrastructure.Helpers;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using BlueLotus360.Com.UI.Definitions.Extensions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace BlueLotus360.Com.UI.Definitions.MB.Shared.Components.Popups
{
    public partial class GetFromQuotation
    {
        #region Parameters
        [Parameter]
        public BLUIElement UIElement { get; set; }

        [Parameter]
        public object ComboDataObject { get; set; }

        [Parameter]
        public EventCallback OnCloseButtonClick { get; set; }

        [Parameter]
        public EventCallback<OrderOpenRequest> OnOpenClick { get; set; }


        public IDictionary<string, EventCallback> InteractionLogics { get; set; }


        public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }


        private GetFromQuoatationDTO order;
        private MudTable<GetFromQuotResults> _table;

        private IList<GetFromQuotResults> FoundOrders;
        private bool hasRecode = true;
        #endregion
        private BLUIElement formDefinition;
        bool fixedheader = true;
        protected override async Task OnParametersSetAsync()
        {
            order = new GetFromQuoatationDTO();
            order.ObjKy = 186581;
            FoundOrders = new List<GetFromQuotResults>();

            if (UIElement != null)
            {
                var formrequest = new ObjectFormRequest();
                formrequest.MenuKey = UIElement.ReferenceElementKey;
                formDefinition = await _navManger.GetMenuUIElement(formrequest);//get ui elements
                InteractionHelper helper = new InteractionHelper(this, formDefinition);//formdefinition has all form objects 
                order.ObjKy = UIElement.ReferenceElementKey;

                InteractionLogics = helper.GenerateEventCallbacks();//
                StateHasChanged();
            }
            await base.OnParametersSetAsync();
        }


        private async void OnCancelButtonClick(UIInterectionArgs<object> args)
        {

            if (OnCloseButtonClick.HasDelegate)
            {
                await OnCloseButtonClick.InvokeAsync();
            }

        }


        private async void OnFindButtonClick(UIInterectionArgs<object> args)
        {

            FoundOrders = await _orderManager.FindFromQuotation(order, null);
            if (FoundOrders != null && FoundOrders.Count() > 0)
            {
                hasRecode = false;
            }
            StateHasChanged();


        }

        private async void OnFromDateClick(UIInterectionArgs<DateTime?> args)
        {
            order.FromDate = (DateTime)args.DataObject;
            StateHasChanged();
            await Task.CompletedTask;
        }

        private async void OnToDateClick(UIInterectionArgs<DateTime?> args)
        {
            order.ToDate = (DateTime)args.DataObject;
            StateHasChanged();
            await Task.CompletedTask;
        }
        private async void OnPrefixChange(UIInterectionArgs<CodeBaseResponse> args)
        {
            order.PreFix = args.DataObject;
            StateHasChanged();
            await Task.CompletedTask;
        }
        private async void OnSupplierChange(UIInterectionArgs<AddressResponse> args)
        {
            order.Supplier = args.DataObject;
            StateHasChanged();
            await Task.CompletedTask;
        }
        private async void OnTrnNoClick(UIInterectionArgs<string> args)
        {
            order.SoNo = args.DataObject;
            StateHasChanged();
            await Task.CompletedTask;
        }
        private async void OnLocationChange(UIInterectionArgs<CodeBaseResponse> args)
        {
            order.Location = args.DataObject;
            StateHasChanged();
            await Task.CompletedTask;
        }
        
        private async void OnAdvAnlysisChange(UIInterectionArgs<AddressResponse> args)
        {
            order.AdvAnalysis = args.DataObject;
            StateHasChanged();
            await Task.CompletedTask;
        }

        private async void OpenTransaction(GetFromQuotResults item)
        {

            OrderOpenRequest request = new OrderOpenRequest();
            request.OrderKey = item.OrdKy;
            request.ObjKy = order.ObjKy;
            order.Project = new CodeBaseResponse();
            request.PrjKy = order.Project.CodeKey;
            if (OnOpenClick.HasDelegate)
            {
                await OnOpenClick.InvokeAsync(request);

            }
        }
    }
}
