using BL10.CleanArchitecture.Domain.Entities.WorkShopManagement;
using BlueLotus360.CleanArchitecture.Client.Infrastructure.Helpers;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.Com.UI.Definitions.Extensions;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Blazor.Components;
using static MudBlazor.Icons;

namespace BlueLotus360.Com.UI.Definitions.Pages.WorkshopManagement.WorkshopCarmart.Component
{
    public partial class AddServicePopUp
    {
        [Parameter] public BLUIElement PopUI { get; set; }
        [Parameter] public WorkOrder DataObject { get; set; }
        [Parameter] public long ObjectKey { get; set; }
        [Parameter] public EventCallback OnCloseButtonClick { get; set; }
        [Parameter] public bool WindowIsVisible { get; set; }
        [Parameter] public EventCallback ItemAddedSuccessfull { get; set; }
        private IList<OrderItem> Services { get; set; }
        private IDictionary<string, EventCallback> InteractionLogic { get; set; }
        private IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }

        string Complaint = "";
        bool HideMinMax { get; set; } = false;
        private TelerikGrid<OrderItem> GridRef1 { get; set; }
        protected override async Task OnParametersSetAsync()
        {
            ObjectHelpers = new Dictionary<string, IBLUIOperationHelper>();
 
            InteractionHelper helper = new InteractionHelper(this, PopUI);
            InteractionLogic = helper.GenerateEventCallbacks();
            

            if (!DataObject.OrderCategory1.Code.Equals("Good Will Warranty"))
            {
                ToggleViisbility("CarmartPrecentage", false);
                ToggleViisbility("CarmartAmount", false);
                ToggleViisbility("PrinciplePrecentage", false);
                ToggleViisbility("PrincipleAmount", false);

            }

            DataObject.SelectedOrderItem = new OrderItem();
            Services = new List<OrderItem>();
            await base.OnParametersSetAsync();
        }


        private async void OnCloseClick()
        {
            if (OnCloseButtonClick.HasDelegate)
            {
                await OnCloseButtonClick.InvokeAsync();
            }
        }


        private async void OnAddClick()
        {
            if (OnCloseButtonClick.HasDelegate)
            {
                await OnCloseButtonClick.InvokeAsync();
            }
            if (ItemAddedSuccessfull.HasDelegate)
            {
                await ItemAddedSuccessfull.InvokeAsync();
            }


        }

        private async void OnServicePopUpServiceComboChange(UIInterectionArgs<ItemResponse> args)
        {
            DataObject.SelectedOrderItem.TransactionItem = args.DataObject;
            ItemRateResponse rates = await RetriveRate(DataObject.SelectedOrderItem.TransactionItem);
            DataObject.SelectedOrderItem.Rate = rates.Rate;
            DataObject.SelectedOrderItem.TransactionRate = rates.Rate;
            DataObject.SelectedOrderItem.DiscountPercentage = rates.DiscountPercentage;
            DataObject.SelectedOrderItem.AvailableStock = await RetriveStockAsAt(DataObject.SelectedOrderItem.TransactionItem);
            DataObject.SelectedOrderItem.LineNumber = DataObject.WorkOrderMaterials.Count()+1;

            await ReadData("ServiceUnit");
            StateHasChanged();
        }

        private async void OnTechnicianChange(UIInterectionArgs<AddressResponse> args)
        {
            DataObject.SelectedOrderItem.ResourceAddress = args.DataObject;
            StateHasChanged();
        }
        private async void OnServiceCompanyAccountChange(UIInterectionArgs<AccountResponse> args)
        {
            DataObject.SelectedOrderItem.BaringCompany = args.DataObject;
            StateHasChanged();
        }
       
        private async void OnServicePrincipalAccountChange(UIInterectionArgs<AccountResponse> args)
        {
            DataObject.SelectedOrderItem.BaringPrinciple = args.DataObject;
            StateHasChanged();
        }
        private async void OnServicePopUpNumericBoxChange(UIInterectionArgs<decimal> args)
        {
            DataObject.SelectedOrderItem.CalculateLineBalance();
            StateHasChanged();
            await Task.CompletedTask;
        }

        private async void OnServicePopUpAddToGridClick(UIInterectionArgs<object> args)
        {
            DataObject.SelectedOrderItem.IsInEditMode = true;
            DataObject.SelectedOrderItem.IsActive = 1;
            DataObject.SelectedOrderItem.IsServiceItem = true;

            if (!DataObject.OrderCategory1.Code.Equals("Good Will Warranty"))
            {
                DataObject.SelectedOrderItem.BaringCompany = new AccountResponse();
                DataObject.SelectedOrderItem.BaringPrinciple = new AccountResponse();
            }

            Services.Add(DataObject.SelectedOrderItem);
            DataObject.WorkOrderServices.Add(DataObject.SelectedOrderItem);
            DataObject.SelectedOrderItem = new OrderItem();
            GridRef1?.Rebind();
            StateHasChanged();
        }
        private async void OnServiceUnitChange(UIInterectionArgs<UnitResponse> args)
        {
            DataObject.SelectedOrderItem.TransactionUnit = args.DataObject;
            StateHasChanged();
            await Task.CompletedTask;
        }
        private async void OnServicePopUpTimeChange(UIInterectionArgs<decimal> args)
        {
            DataObject.SelectedOrderItem.TransactionQuantity = args.DataObject;
            StateHasChanged();
            await Task.CompletedTask;
        }
        private async void ServiceUnit_OnBeforeDataFetch(UIInterectionArgs<ComboRequestDTO> args)
        {
            args.DataObject.AddtionalData.Add("ItemKey", DataObject.SelectedOrderItem.TransactionItem.ItemKey);
            StateHasChanged();
        }
        public async Task<ItemRateResponse> RetriveRate(ItemResponse workOrderItem)
        {
            ItemRateRequest request = new ItemRateRequest();
            request.LocationKey = DataObject.OrderLocation.CodeKey;
            request.ItemKey = workOrderItem.ItemKey;
            request.EffectiveDate = DateTime.Now.Date;
            request.ConditionCode = "OrdTyp";
            request.ObjectKey = ObjectKey;
            return (await _comboManager.GetRate(request));
        }

        public async Task<decimal> RetriveStockAsAt(ItemResponse workOrderItem)
        {
            StockAsAtRequest request = new StockAsAtRequest();
            request.ElementKey = ObjectKey;
            request.LocationKey = DataObject.OrderLocation.CodeKey;
            request.ItemKey = workOrderItem.ItemKey;
            StockAsAtResponse response = await _transactionManager.GetStockAsAt(request);
            return Math.Max(response.StockAsAt, 0);
        }

        async Task DeleteHandler(GridCommandEventArgs args)
        {
            OrderItem item = (OrderItem)args.Item;
            var index = DataObject.WorkOrderServices.ToList().FindIndex(x => x.LineNumber == item.LineNumber);
            DataObject.WorkOrderServices[index].IsActive = 0;

            GridRef1?.Rebind();
            StateHasChanged();
        }

        #region object helpers

        private async Task SetValue(string name, object value)
        {
            IBLUIOperationHelper helper;

            if (ObjectHelpers.TryGetValue(name, out helper))
            {
                await helper.SetValue(value);
                StateHasChanged();
                await Task.CompletedTask;
            }
        }
        private async Task SetDataSource(string name, object dataSource)
        {
            IBLUIOperationHelper helper;

            if (ObjectHelpers.TryGetValue(name, out helper))
            {
                await (helper as IBLServerDependentComponent).SetDataSource(dataSource);
                StateHasChanged();
            }
        }

        private void ToggleViisbility(string name, bool visible)
        {
            IBLUIOperationHelper helper;

            if (ObjectHelpers.TryGetValue(name, out helper))
            {
                helper.UpdateVisibility(visible);
                StateHasChanged();
            }
        }
        private async Task ReadData(string name, bool UseLocalStorage = false)
        {
            IBLUIOperationHelper helper;

            if (ObjectHelpers.TryGetValue(name, out helper))
            {
                await (helper as IBLServerDependentComponent).FetchData(UseLocalStorage);

                StateHasChanged();
            }
        }
        #endregion
    }
}
