using BL10.CleanArchitecture.Application.Validators.WorkShopManagement;
using BL10.CleanArchitecture.Domain.Entities.Booking;
using BL10.CleanArchitecture.Domain.Entities.Document;
using BL10.CleanArchitecture.Domain.Entities.WorkShopManagement;
using BlueLotus360.CleanArchitecture.Application.Validators.SalesOrder;
using BlueLotus360.CleanArchitecture.Client.Infrastructure.Helpers;
using BlueLotus360.CleanArchitecture.Client.Infrastructure.Reports.Telerik;
using BlueLotus360.CleanArchitecture.Domain.DTO.MasterData;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain.Entities.HR;
using BlueLotus360.CleanArchitecture.Domain.Entities.InventoryManagement.ItemTransfer;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using BlueLotus360.Com.UI.Definitions.Extensions;
using BlueLotus360.Com.UI.Definitions.MB.Shared.Components.Popups;
using BlueLotus360.Com.UI.Definitions.MB.Shared.Components.Popups.MasterDetailPopup;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static MudBlazor.Colors;

namespace BlueLotus360.Com.UI.Definitions.Pages.WorkshopManagement.WorkshopCarmart.Component
{
    public partial class OverView
    {
        [Parameter]public EventCallback<int> Activate { get; set; }

        [Parameter] public EventCallback EnableTabs { get; set; }
        [Parameter] public EventCallback DisableTabs { get; set; }

        [Parameter]public BLUIElement UIScope { get; set; }

        [Parameter] public WorkOrder DataObject { get; set; }
        [Parameter] public IDictionary<string, EventCallback> InteractionLogic { get; set; }
        
        [Parameter] public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }

        private VehicleSearch vehicleSearch;
        private IList<Vehicle> SelectedVehicleList { get; set; }
        private int SearchOption = 1;
        private string[] hiddenArray ;
        private string[] displayedArray ;
        private int pagePosition;
        private bool isBackButtonDisabled;

        BLUIElement SerachSection;
        BLUIElement CreateNewWorkOrderSection;
        BLUIElement PlainingSection;
        BLUIElement Estimate_MaterialCombo;
        BLUIElement EstImate_ServiceCombo;
        BLUIElement CustomerNotes;
        bool isAddCustomerComplaintPopUpShown;
        bool IsCrateNewWorkOrderSuccessfullPopUpShown;
        bool ImagePopupShown;
        bool IsAddProjectNamePopUopShown;
        bool IsValidationPopUpShown;
        bool IsProjectListPopUpShown;
        bool IsMaterialAndServiceItemNavButtonEnable = true;
        bool isFindWorkOrderPopUpShown;
        private FileUpload uploadObj;
        private IWorkShopValidator validator;
        private IList<ProjectResponse> ProjectList;
        private AddNewCustomer _refnewCustomer = new AddNewCustomer();
        private IList<WorkOrder> JobHistories;
        private ValidationPopUp _refUserMessage=new ValidationPopUp();
        CompletedUserAuth auth=new CompletedUserAuth();
        bool isVehicleLoading;
        #region general 
        protected override async Task OnParametersSetAsync()
        {
            
            if (UIScope != null)
            {
                SerachSection = SplitUIComponent("SearchSection");
                CreateNewWorkOrderSection = SplitUIComponent("CreateNewWorkOrderSection");
                PlainingSection= SplitUIComponent("PlaningSection");
                Estimate_MaterialCombo = SplitUIComponent("SimpleEstimateMaterial"); 
                EstImate_ServiceCombo= SplitUIComponent("SimpleEstimateService");
                CustomerNotes= SplitUIComponent("CustomerNotes");
                InteractionHelper helper = new InteractionHelper(this, UIScope);
                InteractionLogic = helper.GenerateEventCallbacks();

                
                StateHasChanged();
            }
            await base.OnParametersSetAsync();
        }
        protected override async Task OnInitializedAsync()
        {
            if (pagePosition == 0) { isBackButtonDisabled = true; }
            
            InitializeWorkShopOverview();
            
        }

        private async void InitializeWorkShopOverview()
        { 
            hiddenArray = new string[] { };
            displayedArray = new string[] { };
            vehicleSearch=new VehicleSearch();
            ProjectList=new List<ProjectResponse>();
            JobHistories=new List<WorkOrder>();
            uploadObj = new();
            auth = new();
            validator = new WorkShopValidator(DataObject);
            SelectedVehicleList = new List<Vehicle>();
            auth = await _authenticationManager.GetUserInformation();
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

        #region ui logics
        private async void OnBack()
        {
            if (pagePosition>0)
            {
                pagePosition--;
            }
            if (pagePosition==0) { 
                isBackButtonDisabled = true;
                InitializeWorkShopOverview();
                PageShowHide(new string[] { "overview-section3", "overview-section4", "overview-section5", "overview-section6", "overview-section7", "overview-section8" }, new string[] { "overview-section1", "overview-section2" });
            }
            else if (pagePosition == 1)
            {
                isBackButtonDisabled = false;
                PageShowHide(new string[] { "overview-section1", "overview-section2","overview-section5", "overview-section6", "overview-section7", "overview-section8" }, new string[] { "overview-section3", "overview-section4" });
            }
            else if (pagePosition == 2)
            {
                isBackButtonDisabled = false;
               PageShowHide(new string[] { "overview-section1", "overview-section2", "overview-section3", "overview-section6", "overview-section7", "overview-section8" },new string[] { "overview-section5" });
            }
            else if (pagePosition == 3)
            {
                isBackButtonDisabled = false;
                // PageShowHide(new string[] { "create-new-work-order-section" }, new string[] { "search-section", "search-result-section", "veh-cus-info-section", "job-history-section" });
            }
            else
            {
                isBackButtonDisabled = false;
            }
            this.StateHasChanged();
        }
        private async void ActivateTab(int index)
        {
            if (Activate.HasDelegate)
            {
                await Activate.InvokeAsync(index);
            }
        }
        private async void LoadCusVehicleDetails(Vehicle vehicle)
        {
            hiddenArray = new string[] { "overview-section2" };
            displayedArray= new string[] { "overview-section3", "overview-section4" };
            PageShowHide(hiddenArray, displayedArray);
            DataObject.SelectedVehicle = vehicle;
            DataObject.SelectedVehicle.ObjectKey = UIScope.ElementKey;
            DataObject.SelectedVehicle.JobHistory = await _workshopManager.GetJobHistory(DataObject.SelectedVehicle);

            IList<BookingDetails> booked_list = await _workshopManager.GetRecentBookingDetails(DataObject.SelectedVehicle);
            if (booked_list != null) {
                DataObject.SelectedVehicle.LatestBook = booked_list.FirstOrDefault();
            }
            
            isBackButtonDisabled = false;
            pagePosition = 1;
            StateHasChanged();
        }
        private async void ShowCreateNewOrder()
        {
            hiddenArray = new string[] { "overview-section1", "overview-section2", "overview-section3", "overview-section4", "overview-section6", "overview-section7", "overview-section8" };
            displayedArray = new string[] { "overview-section5" };
            PageShowHide(hiddenArray, displayedArray);
            isBackButtonDisabled = false;
            pagePosition = 2;
            IsMaterialAndServiceItemNavButtonEnable = true;

            if (DisableTabs.HasDelegate)
            {
                await DisableTabs.InvokeAsync();
            }

            StateHasChanged();
        }
        private async void WorkOrderDetailShow()
        {
            hiddenArray = new string[] { "overview-section1", "overview-section2", "overview-section4", "overview-section5" };
            displayedArray = new string[] { "overview-section3", "overview-section6", "overview-section7", "overview-section8" };
            PageShowHide(hiddenArray, displayedArray);
            isBackButtonDisabled = false;
            pagePosition = 3;
            IsCrateNewWorkOrderSuccessfullPopUpShown = false;
            StateHasChanged();
        }
        private async void PageShowHide(string[] hidden,string[] displayed)
        {
            try
            {
                foreach (string div in hidden)
                {
                    await _jsRuntime.InvokeVoidAsync("HideDiv", div);
                }
                foreach (string div in displayed)
                {
                    await _jsRuntime.InvokeVoidAsync("ShowDiv", div);
                }
            }
            catch (Exception ex)
            {

            }
            
            StateHasChanged();
        }
        
        #endregion

        #region ui events

        private async void OnRegNoSearchChange(UIInterectionArgs<AddressResponse> args)
        {
            DataObject.WorkOrderClear();
            isVehicleLoading = true;
            vehicleSearch = new VehicleSearch();
            vehicleSearch.VehicleRegistration = args.DataObject;
            vehicleSearch.ObjectKey = UIScope.ElementKey;           
            SelectedVehicleList = await _workshopManager.GetVehicleDetailsByVehregNo(vehicleSearch);
            isVehicleLoading = false;

            if (pagePosition == 1)
            {
                OnBack();
            }
            
            StateHasChanged();
            await Task.CompletedTask;
        }
        private async void OnChasisNoSearchChange(UIInterectionArgs<ItemSerialNumber> args)
        {
            DataObject.WorkOrderClear();
            isVehicleLoading = true;
            vehicleSearch = new VehicleSearch();
            vehicleSearch.VehicleSerialNumber = args.DataObject;
            vehicleSearch.ObjectKey = UIScope.ElementKey;
            SelectedVehicleList = await _workshopManager.GetVehicleDetailsByVehregNo(vehicleSearch);
            isVehicleLoading = false;
            if (pagePosition == 1)
            {
                OnBack();
            }
            StateHasChanged();
            await Task.CompletedTask;
        }
        private async void OnNICNumberSearchChange(UIInterectionArgs<AddressResponse> args)
        {
            DataObject.WorkOrderClear();
            isVehicleLoading = true;
            vehicleSearch = new VehicleSearch();
            vehicleSearch.RegisteredCustomer = args.DataObject;
            vehicleSearch.ObjectKey = UIScope.ElementKey;
            SelectedVehicleList = await _workshopManager.GetVehicleDetailsByVehregNo(vehicleSearch);
            isVehicleLoading = false;

            if (pagePosition == 1)
            {
                OnBack();
            }
            StateHasChanged();
            await Task.CompletedTask;
        }
        private async void OnNameSearchChange(UIInterectionArgs<AddressResponse> args)
        {
            DataObject.WorkOrderClear();
            isVehicleLoading = true;
            vehicleSearch = new VehicleSearch();
            vehicleSearch.RegisteredCustomer = args.DataObject;
            vehicleSearch.ObjectKey = UIScope.ElementKey;
            SelectedVehicleList = await _workshopManager.GetVehicleDetailsByVehregNo(vehicleSearch);
            isVehicleLoading = false;
            if (pagePosition == 1)
            {
                OnBack();
            }
            StateHasChanged();
            await Task.CompletedTask;
        }
        private async void OnVehRegNoTypeSelected(UIInterectionArgs<object> args)
        {
            SearchOption = 1;
            ToggleViisbility("RegNoSearchField", true);
            ToggleViisbility("ChasisNoField",false);
            ToggleViisbility("NICSearch", false);
            ToggleViisbility("NameSearch", false);
            if(SelectedVehicleList!=null && SelectedVehicleList.Count > 0) { SelectedVehicleList.Clear(); }
                
            this.StateHasChanged();
            await Task.CompletedTask;
        }
        private async void OnChasisNoTypeSelected(UIInterectionArgs<object> args)
        {
            SearchOption = 2;
            ToggleViisbility("RegNoSearchField", false);
            ToggleViisbility("ChasisNoField", true);
            ToggleViisbility("NICSearch", false);
            ToggleViisbility("NameSearch", false);
            if (SelectedVehicleList != null && SelectedVehicleList.Count > 0) { SelectedVehicleList.Clear(); }
            this.StateHasChanged();
            await Task.CompletedTask;
        }
        private async void OnNICTypeSelected(UIInterectionArgs<object> args)
        {
            SearchOption = 3;
            ToggleViisbility("RegNoSearchField", false);
            ToggleViisbility("ChasisNoField", false);
            ToggleViisbility("NICSearch", true);
            ToggleViisbility("NameSearch", false);
            if (SelectedVehicleList != null && SelectedVehicleList.Count > 0) { SelectedVehicleList.Clear(); }
            this.StateHasChanged();
            await Task.CompletedTask;
        }
        private async void OnNameTypeSelected(UIInterectionArgs<object> args)
        {
            SearchOption = 4;
            ToggleViisbility("RegNoSearchField", false);
            ToggleViisbility("ChasisNoField", false);
            ToggleViisbility("NICSearch", false);
            ToggleViisbility("NameSearch", true);
            if (SelectedVehicleList != null && SelectedVehicleList.Count > 0) { SelectedVehicleList.Clear(); }
            this.StateHasChanged();
            await Task.CompletedTask;
        }
        private async void OnSearchClick(UIInterectionArgs<object> args)
        {
            this.appStateService.IsLoaded = true;
            await SetDataSource("SearchCombo", SearchOption);
            this.appStateService.IsLoaded = false;
            this.StateHasChanged();
            await Task.CompletedTask;
        }
        private async void OnWorkOrderCatChange(UIInterectionArgs<CodeBaseResponse> args)
        {
            DataObject.OrderCategory1 = new();
            DataObject.OrderPaymentTerm = new();
            DataObject.OrderCategory2 = new();
            DataObject.Department = new();
            DataObject.SelectedVehicle.CurrentMilage = 0;
            DataObject.PrincipalPercentage = 0;
            DataObject.PrincipalValue = 0;
            DataObject.CarmartPercentage = 0;
            DataObject.CarmartValue = 0;

            DataObject.OrderCategory1 = args.DataObject;
            if (args.DataObject.Code.Equals("Retail"))
            {
                ToggleViisbility("PaymentMode", true);
                ToggleViisbility("PrincipalPrecentage", false);
                ToggleViisbility("CarmartPrecentage", false);
                ToggleViisbility("PricipalValue", false);
                ToggleViisbility("CarmartValue", false);
                ToggleViisbility("Department", false);
            }
            else if (args.DataObject.Code.Equals("Internal"))
            {
                ToggleViisbility("PaymentMode", true);
                ToggleViisbility("PrincipalPrecentage", false);
                ToggleViisbility("CarmartPrecentage", false);
                ToggleViisbility("PricipalValue", false);
                ToggleViisbility("CarmartValue", false);
                ToggleViisbility("Department", true);
            }
            else if (args.DataObject.Code.Equals("Warranty"))
            {
                ToggleViisbility("PaymentMode", false);
                ToggleViisbility("PrincipalPrecentage", false);
                ToggleViisbility("CarmartPrecentage", false);
                ToggleViisbility("PricipalValue", false);
                ToggleViisbility("CarmartValue", false);
                ToggleViisbility("Department", false);
            }
            else if (args.DataObject.Code.Equals("Good Will Warranty"))
            {
                ToggleViisbility("PaymentMode", true);
                ToggleViisbility("PrincipalPrecentage", true);
                ToggleViisbility("CarmartPrecentage", true);
                ToggleViisbility("PricipalValue", true);
                ToggleViisbility("CarmartValue", true);
                ToggleViisbility("Department", false);
            }
            else if (args.DataObject.Code.Equals("Free"))
            {
                ToggleViisbility("PaymentMode", false);
                ToggleViisbility("PrincipalPrecentage", false);
                ToggleViisbility("CarmartPrecentage", false);
                ToggleViisbility("PricipalValue", false);
                ToggleViisbility("CarmartValue", false);
                ToggleViisbility("Department", true);
            }
            else
            {

            }

            StateHasChanged();
            await Task.CompletedTask;
        }
        private async void OnPaymentModeChange(UIInterectionArgs<CodeBaseResponse> args)
        {
            DataObject.OrderPaymentTerm = args.DataObject;
            StateHasChanged();
            await Task.CompletedTask;
        }
        private async void OnWorkOrderTypeChange(UIInterectionArgs<CodeBaseResponse> args)
        {
            DataObject.OrderCategory2 = args.DataObject;
            StateHasChanged();
            await Task.CompletedTask;
        }
        private async void OnDepartmentChange(UIInterectionArgs<CodeBaseResponse> args)
        {
            DataObject.Department = args.DataObject;
            DataObject.Cd1Ky = DataObject.Department.CodeKey;
            StateHasChanged();
            await Task.CompletedTask;
        }
        private async void OnWorkOrderLocationChange(UIInterectionArgs<CodeBaseResponse> args)
        {
            DataObject.OrderLocation = args.DataObject;
            
            StateHasChanged();
            await Task.CompletedTask;
        }
        private async void OnNumericBoxChange(UIInterectionArgs<decimal> args)
        {
            StateHasChanged();
            await Task.CompletedTask;
        }
        private void DeleteComplain(string complain)
        {

        }
        private async void OnProceedToCreateNewWorkOrder()
        {
            appStateService.IsLoaded = true;

            DataObject.FormObjectKey = UIScope.ElementKey;
            validator = new WorkShopValidator(DataObject);
            //UserRequestValidation v = await _workshopManager.GetWorkShopValidatoion(DataObject);
            //if (v.IsError)
            //{
            //    validator.UserMessages.AddErrorMessage(v.Message);
            //}

            if (validator != null && validator.CanCreateWorkOrder())
            {
                DataObject.FormObjectKey = UIScope.ElementKey;
                DataObject.OrderCustomer = DataObject.SelectedVehicle.RegisteredCustomer;
                DataObject.SelectedVehicle.ObjectKey = UIScope.ElementKey;
                DataObject.OrderStatus = new CodeBaseResponse() { OurCode = "WIP" };
                DataObject.EnteredUser = await _addressManager.GetAddressByUserKy();
                DataObject.OrderRepAddress = DataObject.EnteredUser;
               // DataObject.OrderLocation = new CodeBaseResponse() { CodeKey= 424710 , CodeName = "Main02 - UP WorkShop" ,Code= "UP WorkShop" };

                if (DataObject.OrderCategory2.Code.Equals("Main"))
                {
                    IsAddProjectNamePopUopShown = true;
                }
                if (DataObject.OrderCategory2.Code.Equals("Supplimentary"))
                {
                    ProjectList = await _workshopManager.SelectOngoingProjectDetails(DataObject.SelectedVehicle);
                    IsProjectListPopUpShown = true;
                }
                
                
            }
            else
            {
                _refUserMessage.ShowUserMessageWindow();
                StateHasChanged();
            }
            appStateService.IsLoaded = false;
            StateHasChanged();
        }
        private void OnProceedEnDateUpdate()
        {
            appStateService.IsLoaded = true;

            if (DataObject.WorkOrderSimpleEstimation.EstimatedMaterials!=null && DataObject.WorkOrderSimpleEstimation.EstimatedMaterials.Count()>0)
            {
                DataObject.WorkOrderMaterials = DataObject.WorkOrderSimpleEstimation.EstimatedMaterials.Where(x => x.IsActive == 1 && x.IsSelected == 1 && !x.TransactionItem.ItemName.Equals("-")).ToList();
                
            }
            if (DataObject.WorkOrderSimpleEstimation.EstimatedServices != null && DataObject.WorkOrderSimpleEstimation.EstimatedServices.Count() > 0)
            {
                DataObject.WorkOrderServices = DataObject.WorkOrderSimpleEstimation.EstimatedServices.Where(x => x.IsActive == 1 && x.IsSelected == 1 && !x.TransactionItem.ItemName.Equals("-")).ToList();
                
            }

            ShowCreateNewOrder();

            if (Activate.HasDelegate)
            {
                Activate.InvokeAsync(1);
                Activate.InvokeAsync(2);
            }

            appStateService.IsLoaded = false;
            StateHasChanged();
        }
        private void NavigateToMaterialAddPage()
        {
            appStateService.IsLoaded = true;
            if (Activate.HasDelegate)
            {
                Activate.InvokeAsync(1);
            }
            appStateService.IsLoaded = false;
            StateHasChanged();
        }

        private void ClearWorkOrder()
        {
            appStateService.IsLoaded = true;
            DataObject.WorkOrderClear();
            IsMaterialAndServiceItemNavButtonEnable = true;
            if (DisableTabs.HasDelegate)
            {
                DisableTabs.InvokeAsync();
            }

            appStateService.IsLoaded = false;
            StateHasChanged();
        }

        private async void FindWorkOrder()
        {
            appStateService.IsLoaded = true;

            JobHistories = await _workshopManager.GetJobHistory(DataObject.SelectedVehicle);
            isFindWorkOrderPopUpShown = true;

            appStateService.IsLoaded = false;
            StateHasChanged();
        }

        #endregion

        #region Erp link Event
        private async void OnSimpleEstimateClick(UIInterectionArgs<object> args) 
        {
            string URL = "https://bl360x.com/BL10/Object/LoadMenu?ObjKy=107752&InilizeRecordKey=1&RefOrdKy=1&Token=abscd";
            //string url = "https://bl360x.com/BL10/BOQ/BOQ?ObjKy=107752&OurCd=Estimate&ObjCaptn=Estimate&ChildKy=107752&InilizeRecordKey=1";
            //_navigationManager.NavigateTo(URL);

            if (!string.IsNullOrEmpty(URL))
            {
                string url = URL;
                await _jsRuntime.InvokeVoidAsync("open", url, "_blank");
            }

            StateHasChanged();
            await Task.CompletedTask;
        }

        #endregion

        #region customer creation

        private async void OnRegisterNewCustomerClick(UIInterectionArgs<object> args)
        {
            IDictionary<string, object> ParamDictionary = new Dictionary<string, object>();
            ParamDictionary.Add("InitiatorElement", args.InitiatorObject);
            ParameterView values = ParameterView.FromDictionary(ParamDictionary);
            await _refnewCustomer.SetParametersAsync(values);
            _refnewCustomer.ShowPopUp();
        }
        private async Task OnCustomerCreateSuccess(AddressMaster customer)
        {
            await ReadData("Customer");
            await SetValue("Customer", customer);

        }

        #endregion

        #region popup events
        private void HideAllPopups()
        {
            isAddCustomerComplaintPopUpShown = false;
            IsCrateNewWorkOrderSuccessfullPopUpShown = false;
            IsAddProjectNamePopUopShown = false;
            ImagePopupShown = false;
            IsValidationPopUpShown = false;
            IsProjectListPopUpShown = false;
            isFindWorkOrderPopUpShown = false;
            StateHasChanged();
        }

        private void AddCustomerComplainPopUpShow()
        {
            isAddCustomerComplaintPopUpShown = true;
        }
        private void CreateNewWorkOrderSuccessfullPopUpShow()
        {
            IsCrateNewWorkOrderSuccessfullPopUpShown = true;
            if (Activate.HasDelegate)
            {
                Activate.InvokeAsync(2);
            }
            StateHasChanged();
        }

        private void ShowUploadPopUp()
        {
            uploadObj = new FileUpload();
            //uploadObj.ItemTransactionKey = (int)line.ItemTransactionKey;

            ImagePopupShown = true;
            StateHasChanged();
        }
        private async void UploadSuccess()
        {
            TransactionOpenRequest request = new TransactionOpenRequest();
            StateHasChanged();
        }

        #endregion

        #region Load
        private async void LoadWorkorder(OrderOpenRequest req)
        {
            this.appStateService.IsLoaded = true;
            WorkOrder workOrd = await _workshopManager.OpenWorkOrderV2(req);

            TransactionOpenRequest trn_req=new TransactionOpenRequest() { TransactionKey=req.TransactionKey};
            BLTransaction otransaction = await _workshopManager.OpenWorkOrderTransaction(trn_req);
            

            
            if (workOrd!=null)
            {
                workOrd.SelectedVehicle.CopyFrom(DataObject.SelectedVehicle);
                DataObject.WorkOrderTransaction.CopyFrom(otransaction);
                DataObject.CopyFrom(workOrd);
            }

            ShowCreateNewOrder();
            IsMaterialAndServiceItemNavButtonEnable = false;
            if (DataObject.OrderCategory1.Code.Equals("Retail"))
            {
                ToggleViisbility("PaymentMode", true);
                ToggleViisbility("PrincipalPrecentage", false);
                ToggleViisbility("CarmartPrecentage", false);
                ToggleViisbility("PricipalValue", false);
                ToggleViisbility("CarmartValue", false);
                ToggleViisbility("Department", false);
            }
            else if (DataObject.OrderCategory1.Code.Equals("Internal"))
            {
                ToggleViisbility("PaymentMode", true);
                ToggleViisbility("PrincipalPrecentage", false);
                ToggleViisbility("CarmartPrecentage", false);
                ToggleViisbility("PricipalValue", false);
                ToggleViisbility("CarmartValue", false);
                ToggleViisbility("Department", true);
            }
            else if (DataObject.OrderCategory1.Code.Equals("Warranty"))
            {
                ToggleViisbility("PaymentMode", false);
                ToggleViisbility("PrincipalPrecentage", false);
                ToggleViisbility("CarmartPrecentage", false);
                ToggleViisbility("PricipalValue", false);
                ToggleViisbility("CarmartValue", false);
                ToggleViisbility("Department", false);
            }
            else if (DataObject.OrderCategory1.Code.Equals("Good Will Warranty"))
            {
                ToggleViisbility("PaymentMode", true);
                ToggleViisbility("PrincipalPrecentage", true);
                ToggleViisbility("CarmartPrecentage", true);
                ToggleViisbility("PricipalValue", true);
                ToggleViisbility("CarmartValue", true);
                ToggleViisbility("Department", false);
            }
            else if (DataObject.OrderCategory1.Code.Equals("Free"))
            {
                ToggleViisbility("PaymentMode", false);
                ToggleViisbility("PrincipalPrecentage", false);
                ToggleViisbility("CarmartPrecentage", false);
                ToggleViisbility("PricipalValue", false);
                ToggleViisbility("CarmartValue", false);
                ToggleViisbility("Department", true);
            }
            else
            {

            }

            if (EnableTabs.HasDelegate)
            {
                await EnableTabs.InvokeAsync();
            }
            StateHasChanged();
            await Task.CompletedTask;

            this.appStateService.IsLoaded = false;
            StateHasChanged();
        }
        #endregion

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
