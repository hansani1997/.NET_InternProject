using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using bluelotus360.Com.commonLib.Helpers;
using bluelotus360.com.razorComponents.Extensions;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MudBlazor;
using bluelotus360.com.razorComponents.MB.Shared.Components.RigidComponents;

namespace bluelotus360.com.razorComponents.MB.Shared.Components.Popups.Order
{
    public partial class GetFromQuoatation
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
        [Parameter] public string? PopupTitle { get; set; }
        [Parameter] public string? ActionButtonName { get; set; }
        [Parameter] public bool WindowIsVisible { get; set; }
        [Parameter] public IDictionary<string, EventCallback> InteractionLogics { get; set; }
        [Parameter] public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }
        [Parameter] public long ElementKey { get; set; }
        bool HideMinMax { get; set; } = false;
        private GetFromQuoatationDTO order;
        private IList<GetFromQuotResults> FoundOrders;
        private BLUIElement formDefinition;
        private DialogOptions dialogOptions = new() { FullScreen = true };
        private bool IsGriddShown;
        private string searchString = "";
        #endregion


        protected override async Task OnParametersSetAsync()
        {
            order = new GetFromQuoatationDTO();
            order.ObjKy = Convert.ToInt32(ElementKey);
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
            IsGriddShown = true;
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

        private async void OpenQuotation(GetFromQuotResults item)
        {

            OrderOpenRequest request = new OrderOpenRequest();
            request.OrderKey = item.OrdKy;
            request.ObjKy = order.ObjKy;
            order.Project = new ProjectResponse();
            request.PrjKy = order.Project.ProjectKey;
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
                FoundOrders = await _orderManager.FindFromQuotation(order, null);
            }
            StateHasChanged();
        }
        private bool FilterFunc(GetFromQuotResults element)
        {
            if (string.IsNullOrWhiteSpace(searchString))
                return true;
            if (element.OrdNo.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.DocNo.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.Prefix.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.LocCd.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        }
    }
}
