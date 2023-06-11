using ApexCharts;
using BL10.CleanArchitecture.Application.Validators.WorkShopManagement;
using BL10.CleanArchitecture.Domain.Entities.Booking;
using BL10.CleanArchitecture.Domain.Entities.WorkShopManagement;
using BlueLotus360.CleanArchitecture.Client.Infrastructure.Helpers;
using BlueLotus360.CleanArchitecture.Domain.DTO.MasterData;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using BlueLotus360.Com.UI.Definitions.Extensions;
using BlueLotus360.Com.UI.Definitions.MB.Shared.Components.Popups.MasterDetailPopup;
using BlueLotus360.Com.UI.Definitions.Pages.WorkshopManagement.WorkshopCarmart.Component;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static MudBlazor.CategoryTypes;

namespace BlueLotus360.Com.UI.Definitions.Pages.WorkshopManagement.InsuranceCarmart.Component
{
    public partial class InsuranceOverView
    {
        [Parameter] public EventCallback<int> Activate { get; set; }
        [Parameter] public EventCallback EnableTabs { get; set; }
        [Parameter] public EventCallback DisableTabs { get; set; }
        [Parameter] public BLUIElement UIScope { get; set; }

        [Parameter] public WorkOrder DataObject { get; set; }
        [Parameter] public IDictionary<string, EventCallback> InteractionLogic { get; set; }

        [Parameter] public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }

        private VehicleSearch vehicleSearch;
        private IList<Vehicle> SelectedVehicleList { get; set; }
        private int SearchOption = 1;
        private string[] hiddenArray;
        private string[] displayedArray;
        private int pagePosition;
        private bool isBackButtonDisabled;
        private bool IsCrateNewIRNSuccessfullPopUpShown = false;
        bool IsAddProjectNamePopUopShown;
        bool IsProjectListPopUpShown;
        private AddNewCustomer _refnewCustomer;
        CompletedUserAuth auth;
        private IInsuranceModuleValidator validator;
        private ValidationPopUp _refUserMessage = new ValidationPopUp();
        private IList<ProjectResponse> ProjectList;

        BLUIElement SerachSection;
        BLUIElement BranchSection;
        BLUIElement CreateNewIRNSection;

        #region general

        protected override async Task OnParametersSetAsync()
        {
            if (UIScope != null) 
            {
                SerachSection = SplitUIComponent("SearchSection");
                BranchSection = SplitUIComponent("BranchSection");
                CreateNewIRNSection = SplitUIComponent("CreateNewIRNSection");

                InteractionHelper helper = new InteractionHelper(this, UIScope);
                InteractionLogic = helper.GenerateEventCallbacks();

                auth = await _authenticationManager.GetUserInformation();

                StateHasChanged();
            }
            await base.OnParametersSetAsync();
        }
        protected override async Task OnInitializedAsync()
        {
            if (pagePosition == 0) { isBackButtonDisabled = true; }

            InitializeInsurenceOverview();
            DataObject.OrderLocation = new CodeBaseResponse { Code = "Rathmalana", CodeKey = 424708, CodeName = "RA01 - Rathmalana" };
            //DataObject.OrderLocation.Code = "Rathmalana";
            //DataObject.OrderLocation.CodeKey = 424708;
            //DataObject.OrderLocation.CodeName = "RA01 - Rathmalana";
        }

        private void InitializeInsurenceOverview()
        {
            _refnewCustomer = new AddNewCustomer();
            hiddenArray = new string[] { };
            displayedArray = new string[] { };
            vehicleSearch = new VehicleSearch();
            auth = new();
            validator = new InsuranceModuleValidator(DataObject);
            ProjectList = new List<ProjectResponse>();
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

        #region popup events

        private void ShowUploadPopUp()
        {

            StateHasChanged();
        }

        #endregion

        #region ui logics

        private async void OnBack() 
        {
            if (pagePosition > 0)
            {
                pagePosition--;
            }
            if (pagePosition == 0)
            {
                isBackButtonDisabled = true;
                InitializeInsurenceOverview();
                PageShowHide(new string[] { "overview-section3", "overview-section4", "overview-section5" }, new string[] { "overview-section1", "overview-section2" });
            }
            else if (pagePosition == 1) 
            {
                isBackButtonDisabled = false;
                PageShowHide(new string[] { "overview-section1", "overview-section2", "overview-section6" }, new string[] { "overview-section4", "overview-section5" });
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
                Activate.InvokeAsync(index);
            }
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

        private async void IRNDetailShow()
        {
            IsCrateNewIRNSuccessfullPopUpShown = false;
            StateHasChanged();

        }
        private async void LoadCusVehicleDetails(Vehicle vehicle) 
        {
            hiddenArray = new string[] { "overview-section2" };
            displayedArray = new string[] { "overview-section3", "overview-section4", "overview-section5" };
            PageShowHide(hiddenArray, displayedArray);
            DataObject.SelectedVehicle = vehicle;
            DataObject.SelectedVehicle.ObjectKey = UIScope.ElementKey;
            DataObject.SelectedVehicle.IsInsurence = true;

            DataObject.SelectedVehicle.JobHistory = await _workshopManager.GetJobHistory(DataObject.SelectedVehicle);

            IList<BookingDetails> booked_list = await _workshopManager.GetRecentBookingDetails(DataObject.SelectedVehicle);
            if (booked_list != null)
            {
                DataObject.SelectedVehicle.LatestBook = booked_list.FirstOrDefault();
            }

            isBackButtonDisabled = false;
            pagePosition = 1;
            StateHasChanged();
        }
        private async void ShowCreateNewIRN() 
        {
            hiddenArray = new string[] { "overview-section1", "overview-section2", "overview-section3", "overview-section4", "overview-section5"};
            displayedArray = new string[] { "overview-section6" };
            PageShowHide(hiddenArray, displayedArray);
            isBackButtonDisabled = false;
            pagePosition = 2;

            StateHasChanged();
        }
        #endregion
        #region Load
        private async void LoadWorkorder(OrderOpenRequest req) 
        {
            
        }

        #endregion

        #region ui events

        private async void OnRegNoSearchChange(UIInterectionArgs<AddressResponse> args)
        {

            vehicleSearch = new VehicleSearch();
            vehicleSearch.VehicleRegistration = args.DataObject;
            vehicleSearch.ObjectKey = UIScope.ElementKey;
            SelectedVehicleList = await _workshopManager.GetVehicleDetailsByVehregNo(vehicleSearch);
            if (pagePosition == 1)
            {
                OnBack();
            }
            StateHasChanged();
            await Task.CompletedTask;
        }
        private async void OnChasisNoSearchChange(UIInterectionArgs<ItemSerialNumber> args)
        {
            vehicleSearch = new VehicleSearch();
            vehicleSearch.VehicleSerialNumber = args.DataObject;
            vehicleSearch.ObjectKey = UIScope.ElementKey;
            SelectedVehicleList = await _workshopManager.GetVehicleDetailsByVehregNo(vehicleSearch);
            if (pagePosition == 1)
            {
                OnBack();
            }
            StateHasChanged();
            await Task.CompletedTask;
        }
        private async void OnNICNumberSearchChange(UIInterectionArgs<AddressResponse> args)
        {
            vehicleSearch = new VehicleSearch();
            vehicleSearch.RegisteredCustomer = args.DataObject;
            vehicleSearch.ObjectKey = UIScope.ElementKey;
            SelectedVehicleList = await _workshopManager.GetVehicleDetailsByVehregNo(vehicleSearch);
            if (pagePosition == 1)
            {
                OnBack();
            }
            StateHasChanged();
            await Task.CompletedTask;
        }
        private async void OnNameSearchChange(UIInterectionArgs<AddressResponse> args)
        {
            vehicleSearch = new VehicleSearch();
            vehicleSearch.RegisteredCustomer = args.DataObject;
            vehicleSearch.ObjectKey = UIScope.ElementKey;
            SelectedVehicleList = await _workshopManager.GetVehicleDetailsByVehregNo(vehicleSearch);
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
            ToggleViisbility("ChasisNoField", false);
            ToggleViisbility("NICSearch", false);
            ToggleViisbility("NameSearch", false);
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
        private async void OnClickInsurance(UIInterectionArgs<ItemResponse> args) 
        {
            DataObject.Insurance = args.DataObject;
            StateHasChanged();
        }
        private async void OnClickCurrentMilage(UIInterectionArgs<decimal> args) 
        {
            DataObject.MeterReading = args.DataObject;
            StateHasChanged();
        }
        private async void OnClickIRNType(UIInterectionArgs<CodeBaseResponse> args)
        { 
            DataObject.OrderCategory2 = args.DataObject;
            StateHasChanged();
        }
        private async void OnSimpleEstimateClick(UIInterectionArgs<object> args) 
        {
            string URL = "https://bl360x.com/BL10/Object/LoadMenu?ObjKy=107752&InilizeRecordKey=1&RefOrdKy=1&Token=abscd";

            if (!string.IsNullOrEmpty(URL))
            {
                string url = URL;
                await _jsRuntime.InvokeVoidAsync("open", url, "_blank");
            }

            this.StateHasChanged();
            await Task.CompletedTask;
        }
        private async void OnProceedToCreateNewIRN() 
        {
            appStateService.IsLoaded = true;
            validator = new InsuranceModuleValidator(DataObject);

            if (validator != null && validator.CanCreateIRNOrder()) 
            {
                DataObject.FormObjectKey = UIScope.ElementKey;
                DataObject.OrderStatus = new CodeBaseResponse() { OurCode = "Pending" };

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

        #endregion

        #region popup events
        private void HideAllPopups()
        {
            IsAddProjectNamePopUopShown = false;
            IsProjectListPopUpShown = false;        
            StateHasChanged();
        }
        private async void CreateNewIRNOrderSuccessfullPopUpShow() 
        {
            if (DataObject.OrderKey == 1)
            {
                await _workshopManager.SaveIRNWorkOrder(DataObject);
                StateHasChanged();
            }
            else
            {
                await _workshopManager.EditIRNWorkOrder(DataObject);
                StateHasChanged();
            }

            IsCrateNewIRNSuccessfullPopUpShown = true;
            if (Activate.HasDelegate)
            {
               await Activate.InvokeAsync(1);
            }
            StateHasChanged();
        }

        #endregion

        #region customer creation
        private async void OnAddNewCustomer(UIInterectionArgs<object> args) 
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

        #region object helpers
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
