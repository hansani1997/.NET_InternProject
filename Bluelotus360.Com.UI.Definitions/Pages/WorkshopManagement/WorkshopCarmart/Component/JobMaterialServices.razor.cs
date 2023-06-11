using BL10.CleanArchitecture.Application.Validators.WorkShopManagement;
using BL10.CleanArchitecture.Domain.Entities.WorkShopManagement;
using BlueLotus360.CleanArchitecture.Client.Infrastructure.Helpers;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using BlueLotus360.Com.UI.Definitions.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using MudBlazor.Charts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Telerik.Blazor.Components;
using Telerik.DataSource.Extensions;
using static MudBlazor.Icons;

namespace BlueLotus360.Com.UI.Definitions.Pages.WorkshopManagement.WorkshopCarmart.Component
{
    public partial class JobMaterialServices
    {
        [Parameter] public EventCallback<int> Activate { get; set; }
        
        [Parameter] public BLUIElement UIScope { get; set; }

        [Parameter] public WorkOrder DataObject { get; set; }
        private IDictionary<string, EventCallback> InteractionLogic { get; set; }

        private IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }
        bool isAddMarterialPopUpShown, isAddServicePopUpShown;
        BLUIElement MaterialPopUp;
        BLUIElement ServicePopUp;
        BLUIElement AddOtherSection;
        private TelerikGrid<OrderItem> GridRef1;
        private TelerikGrid<OrderItem> GridRef2;
        private TelerikGrid<OrderItem> GridRef3;
        private string[] hiddenArray;
        private string[] displayedArray;
        private OrderItem other = new OrderItem();
        private int pagePosition;
        private bool isBackButtonDisabled = true;
        MudMessageBox mbox;
        string state = "Message box hasn't been opened yet";
        private bool IsButtonDisabled;
        private CarMaterialGridEditPopUp matGridRef=new CarMaterialGridEditPopUp();
        private ValidationPopUp _refUserMessage =new ValidationPopUp();
        private IWorkShopValidator validator;
        protected override async Task OnParametersSetAsync()
        {
            
            GridRef1 = new TelerikGrid<OrderItem>();
            if (UIScope != null)
            {
                MaterialPopUp = SplitUIComponent("MaterialAddingPopUp");
                ServicePopUp = SplitUIComponent("ServiceAddPopUp");
                AddOtherSection = SplitUIComponent("AddOther");

                InteractionHelper helper = new InteractionHelper(this, UIScope);
                InteractionLogic = helper.GenerateEventCallbacks();
                ObjectHelpers = new Dictionary<string, IBLUIOperationHelper>();

                StateHasChanged();
            }
            this.RefreshMaterialGrid();
            this.RefreshServicesGrid();
            foreach (var itm in DataObject.WorkOrderMaterials.Where(x => x.IsSelected == 1).ToList())
            {
                itm.IsJustAdded = true;
                itm.IsSelected = 0;
            }
            foreach (var itm in DataObject.WorkOrderServices.Where(x => x.IsSelected == 1).ToList())
            {
                itm.IsJustAdded = true;
                itm.IsSelected = 0;
            }

            if (!DataObject.OrderCategory1.Code.Equals("Good Will Warranty"))
            {
                ToggleViisbility("MaterialCarmartAccount", false);
                ToggleViisbility("MaterialCarmartPrecentage", false);
                ToggleViisbility("MaterialCarmartAmount", false);
                ToggleViisbility("MaterialPrincipalAccount", false);
                ToggleViisbility("MaterialPrinciplePrecentage", false);
                ToggleViisbility("MaterialPrincipalAmount", false);

                ToggleViisbility("ServiceCarmartAccount", false);
                ToggleViisbility("ServiceCarmartPrecentage", false);
                ToggleViisbility("ServiceCarmartAmount", false);
                ToggleViisbility("ServicePrincipalAccount", false);
                ToggleViisbility("ServicePrinciplePrecentage", false);
                ToggleViisbility("ServicePrincipleAmount", false);

            }

            IsButtonDisabled = DataObject.OrderStatus.Code.Equals("Closed");
            await base.OnParametersSetAsync();
        }
        protected override async Task OnInitializedAsync()
        {
            
            

        }

        public void VisibleElement()
        {
            if (DataObject != null && !DataObject.OrderCategory1.Code.Equals("Good Will Warranty"))
            {
                ToggleViisbility("MaterialCarmartAccount", false);
                ToggleViisbility("MaterialCarmartPrecentage", false);
                ToggleViisbility("MaterialCarmartAmount", false);
                ToggleViisbility("MaterialPrincipalAccount", false);
                ToggleViisbility("MaterialPrinciplePrecentage", false);
                ToggleViisbility("MaterialPrincipalAmount", false);

                ToggleViisbility("ServiceCarmartAccount", false);
                ToggleViisbility("ServiceCarmartPrecentage", false);
                ToggleViisbility("ServiceCarmartAmount", false);
                ToggleViisbility("ServicePrincipalAccount", false);
                ToggleViisbility("ServicePrinciplePrecentage", false);
                ToggleViisbility("ServicePrincipleAmount", false);

            }
            else
            {
                ToggleViisbility("MaterialCarmartAccount", true);
                ToggleViisbility("MaterialCarmartPrecentage", true);
                ToggleViisbility("MaterialCarmartAmount", true);
                ToggleViisbility("MaterialPrincipalAccount", true);
                ToggleViisbility("MaterialPrinciplePrecentage", true);
                ToggleViisbility("MaterialPrincipalAmount", true);

                ToggleViisbility("ServiceCarmartAccount", true);
                ToggleViisbility("ServiceCarmartPrecentage", true);
                ToggleViisbility("ServiceCarmartAmount", true);
                ToggleViisbility("ServicePrincipalAccount", true);
                ToggleViisbility("ServicePrinciplePrecentage", true);
                ToggleViisbility("ServicePrincipleAmount", true);

            }
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
        private void HideAllPopups()
        {
            isAddMarterialPopUpShown = false;
            isAddServicePopUpShown = false;
            StateHasChanged();
        }
        private void RefreshMaterialGrid()
        {
            GridRef1?.Rebind();
        }
        private void RefreshServicesGrid()
        {
            GridRef2?.Rebind();
        }
        private async void OnBack()
        {
            if (pagePosition > 0)
            {
                pagePosition--;
            }
            if (pagePosition == 0)
            {
                isBackButtonDisabled = true;

                PageShowHide(new string[] { "job-material-service-section3" }, new string[] { "job-material-service-section1", "job-material-service-section2" });
            }
            else if (pagePosition == 1)
            {
                isBackButtonDisabled = false;
                PageShowHide(new string[] { "job-material-service-section1", "job-material-service-section2" }, new string[] { "job-material-service-section3" });
            }
            else
            {
                isBackButtonDisabled = false;
            }
            this.StateHasChanged();
        }

        private async void OnProceedToJobSummery()
        {
            this.appStateService.IsLoaded = true;
            DataObject.OrderItems.AddRange(DataObject.WorkOrderMaterials);
            DataObject.OrderItems.AddRange(DataObject.WorkOrderServices);
            DataObject.OrderItems.AddRange(DataObject.OtherServices);
            DataObject.OrderItems.ToList().ForEach(x => x.BussinessUnit.CodeKey = DataObject.Cd1Ky);

            DataObject.OrderStatus = new CodeBaseResponse() { OurCode = "WIP" };
            DataObject.FormObjectKey = UIScope.ElementKey;

            validator = new WorkShopValidator(DataObject);
            if (validator!=null && validator.CanSaveWorkOrder())
            {
                await _workshopManager.EditWorkOrder(DataObject);

                DataObject.WorkOrderMaterials.ToList().ForEach(x => x.IsJustAdded = false);
                DataObject.WorkOrderServices.ToList().ForEach(x => x.IsJustAdded = false);

                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("Items has been added Successfully", Severity.Success);

                OrderOpenRequest req = new OrderOpenRequest();
                req.ObjKy = UIScope.ElementKey;
                req.OrderKey = DataObject.OrderKey;

                WorkOrder workOrd = await _workshopManager.OpenWorkOrderV2(req);

                
                if (workOrd != null)
                {
                    workOrd.SelectedVehicle.CopyFrom(DataObject.SelectedVehicle);
                    if (DataObject.WorkOrderTransaction!=null)
                    {
                        workOrd.WorkOrderTransaction.CopyFrom(DataObject.WorkOrderTransaction);
                    }
                    
                    DataObject.CopyFrom(workOrd);
                }

                if (Activate.HasDelegate)
                {
                    await Activate.InvokeAsync(3);
                    StateHasChanged();
                }
            }
            else
            {
                _refUserMessage.ShowUserMessageWindow();
                StateHasChanged();
            }
            
            this.appStateService.IsLoaded = false;
            StateHasChanged();
            
        }

        public async void checkTabActivating(int index)
        {
            bool condition1=DataObject.WorkOrderMaterials.Where(x=>x.IsJustAdded && x.IsActive==1).Count()>0;  
            bool condition2=DataObject.WorkOrderServices.Where(x => x.IsJustAdded && x.IsActive == 1).Count()>0;

            if (condition1 || condition2)
            {
                if (Activate.HasDelegate)
                {
                    await Activate.InvokeAsync(1);
                }
                bool? result = await mbox.Show();
                if (result != null)
                {
                    mbox.Close();
                } 
                
                StateHasChanged();
            }

        }

        public async void gotoCreateNewWorkOrder()
        {
            if (Activate.HasDelegate)
            {
                await Activate.InvokeAsync(0);
            }
        }



        #region add material

        private async void OnPopUpMaterialComboChange(UIInterectionArgs<ItemResponse> args)
        {
            DataObject.SelectedOrderItem.TransactionItem = args.DataObject;
            ItemRateResponse rates = await RetriveRate(DataObject.SelectedOrderItem.TransactionItem);
            DataObject.SelectedOrderItem.Rate = rates.Rate;
            DataObject.SelectedOrderItem.TransactionRate = rates.TransactionRate;
            DataObject.SelectedOrderItem.DiscountPercentage = rates.DiscountPercentage;
            DataObject.SelectedOrderItem.AvailableStock = await RetriveStockAsAt(DataObject.SelectedOrderItem.TransactionItem);

            await ReadData("MaterialTransactionUnit");




            StateHasChanged();
        }
        private async void OnMaterialCarmartAccountChange(UIInterectionArgs<AccountResponse> args)
        {
            DataObject.SelectedOrderItem.BaringCompany=args.DataObject;
            StateHasChanged();
        }

        private async void OnMaterialPrincipleAccountChange(UIInterectionArgs<AccountResponse> args)
        {
            DataObject.SelectedOrderItem.BaringPrinciple = args.DataObject;
            StateHasChanged();
        }
        private async void OnMaterialPopUpNumericBoxChange(UIInterectionArgs<decimal> args)
        {
            DataObject.SelectedOrderItem.CalculateLineBalance();

            StateHasChanged();
            await Task.CompletedTask;
        }

        private async void OnMaterialTransactionUnitChange(UIInterectionArgs<UnitResponse> args)
        {
            DataObject.SelectedOrderItem.TransactionUnit = args.DataObject;
            StateHasChanged();
            await Task.CompletedTask;
        }
        private async void MaterialTransactionUnit_OnBeforeDataFetch(UIInterectionArgs<ComboRequestDTO> args)
        {
            args.DataObject.AddtionalData.Add("ItemKey", DataObject.SelectedOrderItem.TransactionItem.ItemKey);
            StateHasChanged();
        }
        private async void OnMaterialPopUpAddToGridClick(UIInterectionArgs<object> args)
        {
            DataObject.SelectedOrderItem.IsMaterialItem = true;
            validator = new WorkShopValidator(DataObject);

            if (validator.CanAddToGridItem())
            {
                if (DataObject.SelectedOrderItem.IsInEditMode)
                {
                    if (DataObject.EditingLineItem != null)
                    {
                        DataObject.SelectedOrderItem.IsInEditMode = false;
                        DataObject.SelectedOrderItem.CalculateLineBalance();
                        DataObject.EditingLineItem.CopyFrom(DataObject.SelectedOrderItem);

                        var index = DataObject.WorkOrderMaterials.ToList().FindIndex(x => x.LineNumber == DataObject.EditingLineItem.LineNumber);
                        if (index != -1)
                        {
                            DataObject.WorkOrderMaterials[index].CopyFrom(DataObject.EditingLineItem);
                        }
                        DataObject.SelectedOrderItem = new OrderItem();
                    }
                }
                else
                {
                    DataObject.SelectedOrderItem.IsJustAdded = true;
                    DataObject.SelectedOrderItem.IsActive = 1;
                    DataObject.SelectedOrderItem.LineNumber = DataObject.WorkOrderMaterials.Count() + 1;

                    if (!DataObject.OrderCategory1.Code.Equals("Good Will Warranty"))
                    {
                        DataObject.SelectedOrderItem.BaringCompany = new AccountResponse();
                        DataObject.SelectedOrderItem.BaringPrinciple = new AccountResponse();
                    }
                    DataObject.WorkOrderMaterials.Add(DataObject.SelectedOrderItem);
                    DataObject.SelectedOrderItem = new OrderItem();
                }
            }
            else
            {
                _refUserMessage.ShowUserMessageWindow();
                StateHasChanged();
            }
            GridRef1?.Rebind();
            
            
            StateHasChanged();
        }
        private async void OnMaterialClearClick(UIInterectionArgs<object> args)
        {
            DataObject.SelectedOrderItem= new OrderItem();
            StateHasChanged();
            await Task.CompletedTask;
        }
        void MaterialEditHandler(OrderItem item)
        {
            item.IsInEditMode = true;
            item.OrderLineLocation = DataObject.OrderLocation;
            item.IsMaterialItem = true;
            DataObject.EditingLineItem = new();
            DataObject.EditingLineItem = item;

            DataObject.SelectedOrderItem.CopyFrom(item);
            DataObject.EditingLineItem = new();

            StateHasChanged();
            //matGridRef.ShowEditPopUp(DataObject);

        }
        async Task MaterialUpdateHandler(OrderItem item)
        {
            var index = DataObject.WorkOrderMaterials.ToList().FindIndex(x => x.LineNumber == item.LineNumber);
            if (index != -1)
            {
                DataObject.WorkOrderMaterials[index] = item;
            }

            GridRef1?.Rebind();
            StateHasChanged();
            Console.WriteLine("Update event is fired.");
        }
        async Task MaterialDeleteHandler(OrderItem item)
        {

            bool? result = await _dialogService.ShowMessageBox(
                "Warning",
                $"Do you want to remove Item {item.TransactionItem.ItemName}",
                yesText: "Delete!", cancelText: "Cancel");

            if (result.HasValue && result.Value)
            {
                item.IsActive = 0;
                
                StateHasChanged();
            }
            
        }
        #endregion

        #region add  service
        private async void OnServicePopUpServiceComboChange(UIInterectionArgs<ItemResponse> args)
        {
            DataObject.SelectedOrderItem.TransactionItem = args.DataObject;
            ItemRateResponse rates = await RetriveRate(DataObject.SelectedOrderItem.TransactionItem);
            DataObject.SelectedOrderItem.Rate = rates.Rate;
            DataObject.SelectedOrderItem.TransactionRate = rates.Rate;
            DataObject.SelectedOrderItem.DiscountPercentage = rates.DiscountPercentage;
            DataObject.SelectedOrderItem.AvailableStock = await RetriveStockAsAt(DataObject.SelectedOrderItem.TransactionItem);
            

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
            DataObject.SelectedOrderItem.IsServiceItem = true;
            validator = new WorkShopValidator(DataObject);
            if (validator.CanAddToGridItem())
            {
                if (DataObject.SelectedOrderItem.IsInEditMode)
                {
                    if (DataObject.EditingLineItem != null)
                    {
                        DataObject.SelectedOrderItem.IsInEditMode = false;

                        DataObject.SelectedOrderItem.GetLineTotalWithDiscount();
                        DataObject.SelectedOrderItem.GetLineCompanyAmount();
                        DataObject.SelectedOrderItem.GetLinePrincipleAmount();
                        DataObject.SelectedOrderItem.GetLineCustomerAmount();
                        if (DataObject.SelectedOrderItem.TransactionQuantity > 0)
                        {
                            DataObject.SelectedOrderItem.SubTotal = 1000 * DataObject.SelectedOrderItem.TransactionQuantity;
                        }

                        DataObject.EditingLineItem.CopyFrom(DataObject.SelectedOrderItem);

                        var index = DataObject.WorkOrderServices.ToList().FindIndex(x => x.LineNumber == DataObject.EditingLineItem.LineNumber);
                        if (index != -1)
                        {
                            DataObject.WorkOrderServices[index].CopyFrom(DataObject.EditingLineItem);
                        }
                    }
                }
                else
                {
                    DataObject.SelectedOrderItem.IsJustAdded = true;
                    DataObject.SelectedOrderItem.IsActive = 1;
                    //DataObject.SelectedOrderItem.IsServiceItem = true;
                    DataObject.SelectedOrderItem.LineNumber = DataObject.WorkOrderMaterials.Count() + 1;
                    if (!DataObject.OrderCategory1.Code.Equals("Good Will Warranty"))
                    {
                        DataObject.SelectedOrderItem.BaringCompany = new AccountResponse();
                        DataObject.SelectedOrderItem.BaringPrinciple = new AccountResponse();
                    }
                    DataObject.SelectedOrderItem.GetLineTotalWithDiscount();
                    DataObject.SelectedOrderItem.GetLineCompanyAmount();
                    DataObject.SelectedOrderItem.GetLinePrincipleAmount();
                    DataObject.SelectedOrderItem.GetLineCustomerAmount();
                    if (DataObject.SelectedOrderItem.TransactionQuantity > 0)
                    {
                        DataObject.SelectedOrderItem.SubTotal = 1000 * DataObject.SelectedOrderItem.TransactionQuantity;
                    }

                    DataObject.WorkOrderServices.Add(DataObject.SelectedOrderItem);
                }
            }
            else
            {
                _refUserMessage.ShowUserMessageWindow();
                StateHasChanged();
            }

            DataObject.SelectedOrderItem = new OrderItem();
            GridRef2?.Rebind();
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
        private async void OnServiceClearClick(UIInterectionArgs<object> args)
        {
            DataObject.SelectedOrderItem = new OrderItem();
            StateHasChanged();
            await Task.CompletedTask;
        }
        void ServiceEditHandler(OrderItem item)
        {
            item.IsInEditMode = true;
            item.OrderLineLocation = DataObject.OrderLocation;
            item.IsServiceItem = true;
            DataObject.EditingLineItem = new();
            DataObject.EditingLineItem = item;

            DataObject.SelectedOrderItem.CopyFrom(item);
            DataObject.EditingLineItem = new();
            StateHasChanged();
        }
        async Task ServiceUpdateHandler(GridCommandEventArgs args)
        {
            OrderItem item = (OrderItem)args.Item;
            var index = DataObject.WorkOrderServices.ToList().FindIndex(x => x.LineNumber == item.LineNumber);
            if (index != -1)
            {
                DataObject.WorkOrderServices[index] = item;
            }

            GridRef2?.Rebind();
            StateHasChanged();
            Console.WriteLine("Update event is fired.");
        }
        async Task ServiceDeleteHandler(OrderItem item)
        {
            bool? result = await _dialogService.ShowMessageBox(
               "Warning",
               $"Do you want to remove Item {item.TransactionItem.ItemName}",
               yesText: "Delete!", cancelText: "Cancel");

            if (result.HasValue && result.Value)
            {
                item.IsActive = 0;

                StateHasChanged();
            }
        }
        #endregion

        #region add Misc
        private async void ShowAddOther()
        {
            hiddenArray = new string[] { "job-material-service-section1", "job-material-service-section2" };
            displayedArray = new string[] { "job-material-service-section3" };
            pagePosition = 1;
            isBackButtonDisabled = false;
            PageShowHide(hiddenArray, displayedArray);

            StateHasChanged();
        }
        private async void OnSupplierChange(UIInterectionArgs<AccountResponse> args)
        {
            other.Supplier = args.DataObject;
            StateHasChanged();
        }
        
        private async void OnOtherServiceUnitChange(UIInterectionArgs<UnitResponse> args)
        {
            other.TransactionUnit = args.DataObject;
            StateHasChanged();
        }

        private void OtherServiceTransactionUnit_OnBeforeDataFetch(UIInterectionArgs<ComboRequestDTO> args)
        {
            args.DataObject.AddtionalData.Add("ItemKey", other.TransactionItem.ItemKey);
        }
        private async void OnAddOtherNumericBoxChange(UIInterectionArgs<decimal> args)
        {
            other.CalculateLineBalance();
            StateHasChanged();
        }
        private async void OtherServiceAddToGridClick(UIInterectionArgs<object> args)
        {
            other.IsServiceItem = true;
            validator = new WorkShopValidator(new WorkOrder() { SelectedOrderItem=other});
            if (validator.CanAddToGridItem())
            {
                if (other.IsInEditMode)
                {
                    other.IsInEditMode = false;
                    //DataObject.EditingLineItem.CopyFrom(DataObject.SelectedOrderItem);

                    var index = DataObject.OtherServices.ToList().FindIndex(x => x.LineNumber == other.LineNumber);
                    if (index != -1)
                    {
                        DataObject.OtherServices[index].CopyFrom(other);
                    }
                    other = new OrderItem();

                }
                else
                {
                    other.IsActive = 1;
                    other.IsJustAdded = true;
                    other.BaringCompany = new AccountResponse();
                    other.BaringPrinciple = new AccountResponse();
                    //other.IsServiceItem = true;
                    other.LineNumber = DataObject.OtherServices.Count() + 1;

                    DataObject.OtherServices.Add(other);
                    other = new OrderItem();
                }
            }
            else
            {
                _refUserMessage.ShowUserMessageWindow();
                StateHasChanged();
            }

            GridRef3?.Rebind();

            StateHasChanged();
            await Task.CompletedTask;
        }
        private async void OtherServiceClearClick(UIInterectionArgs<object> args)
        {
            other = new OrderItem();
            StateHasChanged();
            await Task.CompletedTask;
        }
        private async void OnOtherItemComboChange(UIInterectionArgs<ItemResponse> args)
        {
            other.TransactionItem = args.DataObject;
            await ReadData("OtherServiceTransactionUnit");
            StateHasChanged();
        }
        void MiscEditHandler(OrderItem item)
        {
            item.IsInEditMode = true;
            item.OrderLineLocation = DataObject.OrderLocation;
            item.IsServiceItem = true;
            DataObject.EditingLineItem = new();
            DataObject.EditingLineItem = item;

            other.CopyFrom(item);
            DataObject.EditingLineItem = new();
        }
        async Task MiscUpdateHandler(GridCommandEventArgs args)
        {
            OrderItem item = (OrderItem)args.Item;
            var index = DataObject.OtherServices.ToList().FindIndex(x => x.LineNumber == item.LineNumber);
            if (index != -1)
            {
                DataObject.OtherServices[index] = item;
            }

            GridRef3?.Rebind();
            StateHasChanged();
            Console.WriteLine("Update event is fired.");
        }
        async Task MiscDeleteHandler(OrderItem item)
        {
            bool? result = await _dialogService.ShowMessageBox(
               "Warning",
               $"Do you want to remove Item {item.TransactionItem.ItemName}",
               yesText: "Delete!", cancelText: "Cancel");

            if (result.HasValue && result.Value)
            {
                item.IsActive = 0;

                StateHasChanged();
            }
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
            request.ObjectKey = UIScope.ElementKey ;
            return (await _comboManager.GetRate(request));
        }

        public async Task<decimal> RetriveStockAsAt(ItemResponse workOrderItem)
        {
            StockAsAtRequest request = new StockAsAtRequest();
            request.ElementKey = UIScope.ElementKey ;
            request.LocationKey = DataObject.OrderLocation.CodeKey;
            request.ItemKey = workOrderItem.ItemKey;
            request.TrnTypKy = DataObject.OrderType.CodeKey;
            StockAsAtResponse response = await _workshopManager.GetAvailableStock(request);
            return Math.Max(response.StockAsAt, 0);
        }
        #endregion

        #region ERP Link Event

        private async void OnGenerateMaterialRequisition() 
        {
            string URL = "https://bl360x.com/BL10/Object/LoadMenu?ObjKy=112160&InilizeRecordKey=1&RefOrdKy=1&Token=abscd";
            //_navigationManager.NavigateTo(URL);

            if (!string.IsNullOrEmpty(URL))
            {
                string url = URL;
                await _jsRuntime.InvokeVoidAsync("open", url, "_blank");
            }

            StateHasChanged();
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
