using bluelotus360.com.razorComponents.Extensions;
using bluelotus360.com.razorComponents.MB.Shared.Components.RigidComponents;
using bluelotus360.Com.commonLib.Helpers;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using static MudBlazor.CategoryTypes;


namespace bluelotus360.com.razorComponents.MB.Shared.Components.Popups.Order
{
    public partial class FindOrder
    {
        [Parameter]
        public BLUIElement UIElement { get; set; }

        [Parameter]
        public object ComboDataObject { get; set; }

        [Parameter]
        public EventCallback OnCloseButtonClick { get; set; }

        [Parameter]
        public EventCallback<OrderOpenRequest> OnOpenClick { get; set; }
        [Parameter] public string? PopupTitle { get; set; }
        [Parameter] public string? ActionButtonName { get; set; }
        [Parameter] public bool WindowIsVisible { get; set; }
        [Parameter] public IDictionary<string, EventCallback> InteractionLogics { get; set; }
        [Parameter] public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }
        [Parameter] public long ElementKey { get; set; }

        private OrderFindDto order;
        private IList<OrderFindResults> FoundOrders;
        private BLUIElement formDefinition;
        bool HideMinMax { get; set; } = false;
        private DialogOptions dialogOptions = new() { FullScreen=true };
        private bool IsGriddShown;
        private string searchString = "";
        protected override async Task OnParametersSetAsync()
        {
            order = new OrderFindDto();
            order.ObjectKey = Convert.ToInt32(ElementKey);
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
            IsGriddShown= true;
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
        private async void OpenOrder(OrderFindResults item)
        {

            OrderOpenRequest request = new OrderOpenRequest();
            request.OrderKey = item.OrderKey;
            if (OnOpenClick.HasDelegate)
            {
                await OnOpenClick.InvokeAsync(request);

            }
        }

        private async void OnToolbarSearchButtonClick()
        {
            IsGriddShown = !IsGriddShown;
            if (IsGriddShown)
            {
                FoundOrders = await _orderManager.FindOrders(order, null);
            }
            StateHasChanged();
        }
        private bool FilterFunc(OrderFindResults element)
        {
            if (string.IsNullOrWhiteSpace(searchString))
                return true;
            if (element.DocumentNumber.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.OrderNumber.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.Prefix.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.YourReference.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        }
    }
}
