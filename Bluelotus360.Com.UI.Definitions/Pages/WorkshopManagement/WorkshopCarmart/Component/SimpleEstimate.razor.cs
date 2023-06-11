using BL10.CleanArchitecture.Domain.Entities.WorkShopManagement;
using BlueLotus360.CleanArchitecture.Client.Infrastructure.Helpers;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using BlueLotus360.Com.Application.Interfaces.Common;
using BlueLotus360.Com.UI.Definitions.Extensions;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using Telerik.Blazor.Components;
using static MudBlazor.CategoryTypes;
using static MudBlazor.Icons;
using static System.Formats.Asn1.AsnWriter;
using static Telerik.Blazor.ThemeConstants;

namespace BlueLotus360.Com.UI.Definitions.Pages.WorkshopManagement.WorkshopCarmart.Component
{
    public partial class SimpleEstimate
    {
        [Parameter] public BLUIElement MaterialSection { get; set; }
        [Parameter] public BLUIElement ServiceSection { get; set; }
        [Parameter] public WorkOrder DataObject { get; set; }
        [Parameter] public long ObjectKey { get; set; }

        private IDictionary<string, EventCallback> InteractionLogic1 { get; set; }
        private IDictionary<string, IBLUIOperationHelper> ObjectHelpers1 { get; set; }
        private IDictionary<string, EventCallback> InteractionLogic2 { get; set; }
        private IDictionary<string, IBLUIOperationHelper> ObjectHelpers2 { get; set; }
        private TelerikGrid<OrderItem> GridRef1 { get; set; }
        private TelerikGrid<OrderItem> GridRef2 { get; set; }
        //OrderItem material = new OrderItem();
        //OrderItem service = new OrderItem();

        protected override async Task OnParametersSetAsync()
        {
            if (MaterialSection != null)
            {
                InteractionHelper helper = new InteractionHelper(this, MaterialSection);
                InteractionLogic1 = helper.GenerateEventCallbacks();
                ObjectHelpers1=new Dictionary<string, IBLUIOperationHelper>();  
                InteractionHelper helper2 = new InteractionHelper(this, ServiceSection);
                InteractionLogic2 = helper2.GenerateEventCallbacks();
                ObjectHelpers2 = new Dictionary<string, IBLUIOperationHelper>();
                StateHasChanged();
            }
            await base.OnParametersSetAsync();
        }

        private async void OnMaterialComboChange(UIInterectionArgs<ItemResponse> args)
        {
            DataObject.SelectedOrderItem.TransactionItem = args.DataObject;
            await ReadData("EstimatedMaterialUnit");
            StateHasChanged();
        }
        private async void EstimatedMaterialUnit_OnBeforeDataFetch(UIInterectionArgs<ComboRequestDTO> args)
        {
            args.DataObject.AddtionalData.Add("ItemKey", DataObject.SelectedOrderItem.TransactionItem.ItemKey);
            StateHasChanged();
        }
        private async void OnEstimatedMaterialUnitChange(UIInterectionArgs<UnitResponse> args)
        {
            DataObject.SelectedOrderItem.TransactionUnit = args.DataObject;
            StateHasChanged();
            await Task.CompletedTask;
        }
        private async void OnSimpleEstimateMaterialAddtoGridClick(UIInterectionArgs<object> args)
        {
            ItemRateResponse rates = await RetriveRate(DataObject.SelectedOrderItem.TransactionItem);
            DataObject.SelectedOrderItem.Rate = rates.Rate;
            DataObject.SelectedOrderItem.TransactionRate = rates.TransactionRate;
            DataObject.SelectedOrderItem.DiscountPercentage = rates.DiscountPercentage;
            DataObject.SelectedOrderItem.AvailableStock = await RetriveStockAsAt(DataObject.SelectedOrderItem.TransactionItem);
            DataObject.SelectedOrderItem.IsActive = 1;
            DataObject.SelectedOrderItem.IsMaterialItem = true;
            DataObject.SelectedOrderItem.LineNumber = DataObject.WorkOrderSimpleEstimation.EstimatedMaterials.Count();

            DataObject.WorkOrderSimpleEstimation.EstimatedMaterials.Add(DataObject.SelectedOrderItem);
            DataObject.SelectedOrderItem = new();
            GridRef1?.Rebind();

            StateHasChanged();
            await Task.CompletedTask;
        }
        void EditMaterialHandler(GridCommandEventArgs args)
        {
            DataObject.EditingLineItem = (OrderItem)args.Item;
        }

        async Task UpdateMaterialHandler(GridCommandEventArgs args)
        {
            DataObject.EditingLineItem = (OrderItem)args.Item;

            var index = DataObject.WorkOrderSimpleEstimation.EstimatedMaterials.ToList().FindIndex(x => x.LineNumber == DataObject.EditingLineItem.LineNumber);
            if (index != -1)
            {
                DataObject.EditingLineItem.CalculateLineBalance();
                DataObject.WorkOrderSimpleEstimation.EstimatedMaterials[index].CopyFrom(DataObject.EditingLineItem);
                
            }
            DataObject.EditingLineItem = new OrderItem();

            GridRef1?.Rebind();
            StateHasChanged();
            Console.WriteLine("Update event is fired.");
        }
        async Task DeleteMaterialHandler(GridCommandEventArgs args)
        {
            DataObject.EditingLineItem = (OrderItem)args.Item;
            var index = DataObject.WorkOrderSimpleEstimation.EstimatedMaterials.ToList().FindIndex(x => x.LineNumber == DataObject.EditingLineItem.LineNumber);
            DataObject.WorkOrderSimpleEstimation.EstimatedMaterials[index].IsActive = 0;

            DataObject.EditingLineItem = new();

            GridRef1?.Rebind();
            StateHasChanged();
            Console.WriteLine("Delete event is fired.");
        }
        private async void OnServiceComboChange(UIInterectionArgs<ItemResponse> args)
        {
            DataObject.SelectedOrderItem.TransactionItem = args.DataObject;
            await ReadData("EstimatedServiceUnit");
            StateHasChanged();
        }
        
        private async void OnEstimatedServiceUnitChange(UIInterectionArgs<UnitResponse> args)
        {
            DataObject.SelectedOrderItem.TransactionUnit = args.DataObject;
            StateHasChanged();
            await Task.CompletedTask;
        }
        private async void EstimatedServiceUnit_OnBeforeDataFetch(UIInterectionArgs<ComboRequestDTO> args)
        {
            args.DataObject.AddtionalData.Add("ItemKey", DataObject.SelectedOrderItem.TransactionItem.ItemKey);
            StateHasChanged();
        }
        private async void OnSimpleEstimateServiceAddtoGridClick(UIInterectionArgs<object> args)
        {
           
            ItemRateResponse rates = await RetriveRate(DataObject.SelectedOrderItem.TransactionItem);
            DataObject.SelectedOrderItem.Rate = rates.Rate;
            DataObject.SelectedOrderItem.TransactionRate = rates.TransactionRate;
            DataObject.SelectedOrderItem.DiscountPercentage = rates.DiscountPercentage;
            DataObject.SelectedOrderItem.AvailableStock = await RetriveStockAsAt(DataObject.SelectedOrderItem.TransactionItem);
            DataObject.SelectedOrderItem.IsActive = 1;
            DataObject.SelectedOrderItem.IsServiceItem = true;
            DataObject.SelectedOrderItem.LineNumber = DataObject.WorkOrderSimpleEstimation.EstimatedServices.Count();
            DataObject.WorkOrderSimpleEstimation.EstimatedServices.Add(DataObject.SelectedOrderItem);
            DataObject.SelectedOrderItem = new();

            GridRef2?.Rebind();
            StateHasChanged();
            await Task.CompletedTask;
        }

        void EditServiceHandler(GridCommandEventArgs args)
        {
            DataObject.EditingLineItem = (OrderItem)args.Item;
        }

        async Task UpdateServiceHandler(GridCommandEventArgs args)
        {
            DataObject.EditingLineItem = (OrderItem)args.Item;
            var index = DataObject.WorkOrderSimpleEstimation.EstimatedServices.ToList().FindIndex(x=>x.LineNumber== DataObject.EditingLineItem.LineNumber);
            if (index != -1)
            {
                DataObject.EditingLineItem.CalculateLineBalance();
                DataObject.WorkOrderSimpleEstimation.EstimatedServices[index].CopyFrom(DataObject.EditingLineItem);
            }

            DataObject.EditingLineItem = new();
            GridRef2?.Rebind();
            StateHasChanged();
            Console.WriteLine("Update event is fired.");
        }

        async Task DeleteServiceHandler(GridCommandEventArgs args)
        {
            DataObject.EditingLineItem = (OrderItem)args.Item;
            var index = DataObject.WorkOrderSimpleEstimation.EstimatedServices.ToList().FindIndex(x => x.LineNumber == DataObject.EditingLineItem.LineNumber);
            DataObject.WorkOrderSimpleEstimation.EstimatedServices[index].IsActive = 0 ;
            DataObject.EditingLineItem = new();
            GridRef2?.Rebind() ;
            StateHasChanged();
            Console.WriteLine("Delete event is fired.");
        }

        async Task CreateHandler(GridCommandEventArgs args)
        {
            DataObject.SelectedOrderItem = (OrderItem)args.Item;

                if (DataObject.WorkOrderSimpleEstimation.EstimatedServices.Count() == 0)
                {
                    DataObject.WorkOrderSimpleEstimation.EstimatedServices = new List<OrderItem>();
                }
            DataObject.SelectedOrderItem.LineNumber = DataObject.WorkOrderSimpleEstimation.EstimatedServices.Count();
            DataObject.SelectedOrderItem.IsActive = 1;
            DataObject.SelectedOrderItem.IsServiceItem = true;
            DataObject.WorkOrderSimpleEstimation.EstimatedServices.Add(DataObject.SelectedOrderItem);

            DataObject.SelectedOrderItem=new OrderItem();


        }

        void OnCancelHandler(GridCommandEventArgs args)
        {
            DataObject.SelectedOrderItem = (OrderItem)args.Item;
            Console.WriteLine("Cancel event is fired. Can be useful when people decide to not satisfy validation");
        }

        protected void OnSelectMaterial(IEnumerable<OrderItem> materials)
        {
            foreach (var itm in DataObject.WorkOrderSimpleEstimation.EstimatedMaterials)
            {
                itm.IsSelected = 0;
            }
            foreach (var itm in materials)
            {
                DataObject.WorkOrderSimpleEstimation.EstimatedMaterials[(int)itm.LineNumber].IsSelected = 1;
            }
        }

        protected void OnSelectService(IEnumerable<OrderItem> services)
        {
            foreach (var itm in DataObject.WorkOrderSimpleEstimation.EstimatedServices)
            {
                itm.IsSelected = 0;
            }
            foreach (var itm in services)
            {
                DataObject.WorkOrderSimpleEstimation.EstimatedServices[(int)itm.LineNumber].IsSelected = 1;
            }
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
            request.TrnTypKy = DataObject.OrderType.CodeKey;
            StockAsAtResponse response = await _workshopManager.GetAvailableStock(request);
            return Math.Max(response.StockAsAt, 0);
        }

        private decimal calculateEstimatedMaterial()
        {
            return DataObject.WorkOrderSimpleEstimation.EstimatedMaterials.Where(x=>x.IsActive==1).Sum(x=>x.SubTotal);
        }

        private decimal calculateEstimatedServices()
        {
            return DataObject.WorkOrderSimpleEstimation.EstimatedServices.Where(x => x.IsActive == 1).Sum(x => x.SubTotal);
        }

        private async Task ReadData(string name, bool UseLocalStorage = false)
        {
            IBLUIOperationHelper helper1;
            IBLUIOperationHelper helper2;

            if (ObjectHelpers1.TryGetValue(name, out helper1))
            {
                await (helper1 as IBLServerDependentComponent).FetchData(UseLocalStorage);

                StateHasChanged();
            }

            if (ObjectHelpers2.TryGetValue(name, out helper2))
            {
                await (helper2 as IBLServerDependentComponent).FetchData(UseLocalStorage);

                StateHasChanged();
            }
        }
    }
}
