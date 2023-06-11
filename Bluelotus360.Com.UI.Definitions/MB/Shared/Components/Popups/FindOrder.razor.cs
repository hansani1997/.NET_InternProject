using BlueLotus360.CleanArchitecture.Client.Infrastructure.Helpers;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using BlueLotus360.Com.UI.Definitions.Extensions;
using Microsoft.AspNetCore.Components;
using MudBlazor;


namespace BlueLotus360.Com.UI.Definitions.MB.Shared.Components.Popups
{
    public partial class FindOrder
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


        private OrderFindDto order;
        private MudTable<OrderFindResults> _table;

        private IList<OrderFindResults> FoundOrders;

        #endregion
        private BLUIElement formDefinition;

        protected override async Task OnParametersSetAsync()
        {
            order = new OrderFindDto();
            order.ObjectKey = 186581;
            FoundOrders = new List<OrderFindResults>();

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

            FoundOrders = await _orderManager.FindOrders(order, null);
            StateHasChanged();


        }


        private async void OnFindLocationChanged(UIInterectionArgs<CodeBaseResponse> args)
        {
            order.Location = args.DataObject;
            StateHasChanged();
            await Task.CompletedTask;
        }
        private async void OnFindPrefixChanged(UIInterectionArgs<CodeBaseResponse> args)
        {
            order.Prefix = args.DataObject;
            StateHasChanged();
            await Task.CompletedTask;
        }

        private async void OnFindDocNoClick(UIInterectionArgs<string> args)
        {
            order.DocumentNumber = args.DataObject;
            StateHasChanged();
            await Task.CompletedTask;
        }
        private async void OnFindOrdNoClick(UIInterectionArgs<string> args)
        {
            order.OrderNo = args.DataObject;
 
            StateHasChanged();
            await Task.CompletedTask;
        }
        private async void OnFindYourRefClick(UIInterectionArgs<string> args)
        {
            order.YourReference = args.DataObject;
            StateHasChanged();
            await Task.CompletedTask;
        }
        private async void OnFromdateClick(UIInterectionArgs<DateTime?> args)
        {
            StateHasChanged();
            await Task.CompletedTask;
        }

        private async void OnToDateClick(UIInterectionArgs<DateTime?> args)
        {

            StateHasChanged();
            await Task.CompletedTask;
        }
        private async void OpenTransaction(OrderFindResults item)
        {

            OrderOpenRequest request = new OrderOpenRequest();
            request.OrderKey = item.OrderKey;
            if (OnOpenClick.HasDelegate)
            {
                await OnOpenClick.InvokeAsync(request);

            }
        }
    }
}
