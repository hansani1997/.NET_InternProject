using BL10.CleanArchitecture.Domain.Entities.WorkShopManagement;
using BlueLotus360.CleanArchitecture.Client.Infrastructure.Helpers;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using BlueLotus360.Com.UI.Definitions.Extensions;
using BlueLotus360.Com.UI.Definitions.MB.Shared.Components.TelerikComponents.Grid;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using Telerik.DataSource.Extensions;

namespace BlueLotus360.Com.UI.Definitions.Pages.WorkshopManagement.InsuranceCarmart.Component
{
    public partial class IRNMaterialsAndServices
    {
        [Parameter] public EventCallback<int> Activate { get; set; }
        [Parameter] public EventCallback EnableTabs { get; set; }
        [Parameter] public EventCallback DisableTabs { get; set; }
        [Parameter] public BLUIElement UIScope { get; set; }

        [Parameter] public WorkOrder DataObject { get; set; }
        [Parameter] public IDictionary<string, EventCallback> InteractionLogic { get; set; }

        [Parameter] public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }

        private BLTelGrid<OrderItem> _servicesTrns;
        private BLTelGrid<OrderItem> _MaterialTrns;
		public bool Changed { get; set; }
        private string[] hiddenArray;
        private string[] displayedArray;

        BLUIElement AddServicesSection;
        BLUIElement AddMaterialSection;
        BLUIElement ServicesUISection;
        BLUIElement MaterialUISection;

        #region general
        protected override async Task OnParametersSetAsync()
        {
            if (UIScope != null) 
            {
                AddServicesSection = SplitUIComponent("AddServicesSection");
                AddMaterialSection = SplitUIComponent("AddMaterialSection");
                ServicesUISection = SplitUIComponent("AddServicesGrid");
                MaterialUISection = SplitUIComponent("AddMaterialsGrid");

				InteractionHelper helper = new InteractionHelper(this, UIScope);
                InteractionLogic = helper.GenerateEventCallbacks();

                StateHasChanged();
            }

                base.OnParametersSetAsync();
        }
        protected override async Task OnInitializedAsync()
        {
            InitializeMaterialAndServices();
            PageShowHide(new string[] { "add-materials" }, new string[] { "add-services" });
        }
        private void InitializeMaterialAndServices()
        {
            hiddenArray = new string[] { };
            displayedArray = new string[] { };
        }
        private BLUIElement SplitUIComponent(string domName)
        {
            BLUIElement parent = new BLUIElement();
            if (UIScope != null && !string.IsNullOrEmpty(domName))
            {
                parent = UIScope.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals(domName)).FirstOrDefault();

            }

            if (parent != null)
            {
                parent.Children = UIScope.Children.Where(x => x.ParentKey == parent.ElementKey).ToList();
            }

            return parent;
        }

        #endregion

        #region UI Event
        public void OnToggledChanged(bool toggled)
        {
            Changed = toggled;

            if (toggled)
            {
                PageShowHide(new string[] { "add-services" }, new string[] { "add-materials" });
            }
            else 
            {
                PageShowHide(new string[] { "add-materials" }, new string[] { "add-services" });
            }
            
        }
        private async void OnProceed()
        {
            appStateService.IsLoaded = true;

            DataObject.IsInWorkOrderEditMode = true;

            DataObject.OrderItems.AddRange(DataObject.WorkOrderServices);
            DataObject.OrderItems.AddRange(DataObject.WorkOrderMaterials);
            DataObject.OrderStatus = new CodeBaseResponse() { OurCode = "Pending" };

            if (DataObject.OrderKey == 1)
            {
                await _workshopManager.SaveIRNWorkOrder(DataObject);
            }
            else
            {
                await _workshopManager.EditIRNWorkOrder(DataObject);
            }

            if (Activate.HasDelegate)
            {
                await Activate.InvokeAsync(2);
            }

           

            appStateService.IsLoaded = false;
            StateHasChanged();
        }

        #endregion

        #region ui logics

        private async void PageShowHide(string[] hidden, string[] displayed)
        {
            foreach (string div in hidden)
            {
                await _jsRuntime.InvokeVoidAsync("HideDiv", div);
            }
            foreach (string div in displayed)
            {
                await _jsRuntime.InvokeVoidAsync("ShowDiv", div);
            }
            StateHasChanged();
        }
        #endregion

        #region Add Service UI

        private async void OnServiceComboChange(UIInterectionArgs<ItemResponse> args) 
        {
            DataObject.SelectedOrderItem.TransactionItem = args.DataObject;
            ItemRateResponse rates = await RetriveRate(DataObject.SelectedOrderItem.TransactionItem);
            DataObject.SelectedOrderItem.Rate = rates.Rate;
            DataObject.SelectedOrderItem.TransactionRate = rates.TransactionRate;
            await ReadData("ServiceUnit");
            await SetValue("ServiceRate", DataObject.SelectedOrderItem.TransactionRate);
            StateHasChanged();
        }
        private async void OnServiceUnitChange(UIInterectionArgs<UnitResponse> args) 
        {
            DataObject.SelectedOrderItem.TransactionUnit = args.DataObject;
            StateHasChanged();
        }
        private async void ServiceUnit_OnBeforeDataFetch(UIInterectionArgs<ComboRequestDTO> args)
        {
            args.DataObject.AddtionalData.Add("ItemKey", DataObject.SelectedOrderItem.TransactionItem.ItemKey);
            StateHasChanged();
        }
        private async void OnServiceQuantityChanged(UIInterectionArgs<decimal> args)
        {
            DataObject.SelectedOrderItem.TransactionQuantity = args.DataObject;
            StateHasChanged();
        }
        private async void OnServiceRateChange(UIInterectionArgs<decimal> args)
        {
            DataObject.SelectedOrderItem.TransactionRate = args.DataObject;
            StateHasChanged();
        }
        private async void OnServiceDescriptionChanged(UIInterectionArgs<string> args)
        {
            DataObject.SelectedOrderItem.Description = args.DataObject;
            StateHasChanged();
        }
        private async void OnServicePackagesChanged(UIInterectionArgs<ItemResponse> args)
        {
            DataObject.SelectedOrderItem.Packages = args.DataObject;
            StateHasChanged();
        }
        private async void OnServiceVatChanged(UIInterectionArgs<decimal> args)
        {
            DataObject.SelectedOrderItem.VAT = args.DataObject;
            StateHasChanged();
        }
        private async void OnServiceVATAmountChanged(UIInterectionArgs<decimal> args)
        {
            DataObject.SelectedOrderItem.VATAmount = args.DataObject;
            StateHasChanged();
        }
        private async void OnServiceSubTotalClick(UIInterectionArgs<decimal> args)
        {
            DataObject.SelectedOrderItem.SubTotal = args.DataObject;
            StateHasChanged();
        }

        #endregion

        #region Service Grid UI Event

        private async void OnServiceAddToGridClick(UIInterectionArgs<object> args)
        {
            DataObject.WorkOrderServices.Add(DataObject.SelectedOrderItem);
            DataObject.SelectedOrderItem = new OrderItem();
            _servicesTrns.Refresh();
            StateHasChanged();
        }
        private async void OnServiceClearClick(UIInterectionArgs<object> args)
        {
            DataObject.WorkOrderServices.Clear();
            _servicesTrns.Refresh();
            StateHasChanged();
        }
        private async void OnServiceEdit(UIInterectionArgs<object> args) 
        {
            StateHasChanged();
        }
        private async void OnServiceDelete(UIInterectionArgs<object> args) 
        {
            StateHasChanged();
        }

        #endregion

        #region retrivel

        public async Task<ItemRateResponse> RetriveRate(ItemResponse workOrderItem)
        {
            ItemRateRequest request = new ItemRateRequest();
            request.LocationKey = DataObject.OrderLocation.CodeKey;
            request.ItemKey = workOrderItem.ItemKey;
            request.EffectiveDate = DateTime.Now.Date;
            request.ConditionCode = "OrdTyp";
            request.ObjectKey = UIScope.ElementKey;
            return (await _comboManager.GetRate(request));
        }

        #endregion

        #region Add Material UI

        private async void OnMaterialComboChange(UIInterectionArgs<ItemResponse> args)
        {
            DataObject.SelectedOrderItem.TransactionItem = args.DataObject;
            ItemRateResponse rates = await RetriveRate(DataObject.SelectedOrderItem.TransactionItem);
            DataObject.SelectedOrderItem.Rate = rates.Rate;
            DataObject.SelectedOrderItem.TransactionRate = rates.TransactionRate;
            await ReadData("MaterialUnit");
            await SetValue("MaterialRate", DataObject.SelectedOrderItem.TransactionRate);
            StateHasChanged();
        }
        private async void OnMaterialUnitChange(UIInterectionArgs<UnitResponse> args)
        {
            DataObject.SelectedOrderItem.TransactionUnit = args.DataObject;
            StateHasChanged();
        }
        private async void MaterialUnit_OnBeforeDataFetch(UIInterectionArgs<ComboRequestDTO> args)
        {
            args.DataObject.AddtionalData.Add("ItemKey", DataObject.SelectedOrderItem.TransactionItem.ItemKey);
            StateHasChanged();
        }
        private async void OnMaterialQuantityChanged(UIInterectionArgs<decimal> args)
        {
            DataObject.SelectedOrderItem.TransactionQuantity = args.DataObject;
            StateHasChanged();
        }
        private async void OnMaterialRateChanged(UIInterectionArgs<decimal> args)
        {
            DataObject.SelectedOrderItem.TransactionRate = args.DataObject;
            StateHasChanged();
        }
        private async void OnMaterialDescriptionChanged(UIInterectionArgs<string> args)
        {
            DataObject.SelectedOrderItem.Description = args.DataObject;
            StateHasChanged();
        }
        private async void OnMaterialVATChanged(UIInterectionArgs<decimal> args)
        {
            DataObject.SelectedOrderItem.VAT = args.DataObject;
            StateHasChanged();
        }
        private async void OnMaterialVATAmountChanged(UIInterectionArgs<decimal> args)
        {
            DataObject.SelectedOrderItem.VATAmount = args.DataObject;
            StateHasChanged();
        }
        private async void OnMaterialSubTotalChanged(UIInterectionArgs<decimal> args)
        {
            DataObject.SelectedOrderItem.SubTotal = args.DataObject;
            StateHasChanged();
        }

        #endregion

        #region Service Grid UI Event

        private async void OnMaterialAddToGrid(UIInterectionArgs<object> args) 
        {
            DataObject.WorkOrderMaterials.Add(DataObject.SelectedOrderItem);
            DataObject.SelectedOrderItem = new OrderItem();
            _MaterialTrns.Refresh();
            StateHasChanged();
        }
        private async void OnMaterialClear(UIInterectionArgs<object> args) 
        {
            DataObject.WorkOrderMaterials.Clear();
            _MaterialTrns.Refresh();
            StateHasChanged();
        }
        private async void OnMetirialEdit(UIInterectionArgs<object> args) 
        {
            StateHasChanged();
        }
        private async void OnMetirialDelete(UIInterectionArgs<object> args) 
        {
            
        }

        #endregion

        #region Object Helpers

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
