using BL10.CleanArchitecture.Application.Validators.Transaction;
using BL10.CleanArchitecture.Domain.Entities.Booking;
using BlueLotus360.CleanArchitecture.Application.Validators.Transaction;
using BlueLotus360.CleanArchitecture.Client.Infrastructure.Helpers;
using BlueLotus360.CleanArchitecture.Client.Infrastructure.Reports.Telerik;
using BlueLotus360.CleanArchitecture.Domain.DTO.MasterData;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using BlueLotus360.Com.UI.Definitions.Extensions;
using BlueLotus360.Com.UI.Definitions.MB.Shared.Components;
using BlueLotus360.Com.UI.Definitions.MB.Shared.Components.Popups.MasterDetailPopup;
using BlueLotus360.Com.UI.Definitions.MB.Shared.Components.TelerikComponents.Grid;
using BlueLotus360.Com.UI.Definitions.Pages.WorkshopManagement.BookingModule.Components;
using BlueLotus360.Com.UI.Definitions.Pages.WorkshopManagement.CarWash.Components;
using BlueLotus360.Com.UI.Definitions.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BlueLotus360.Com.Shared.Constants.Permission.Permissions;
using static MudBlazor.CategoryTypes;

namespace BlueLotus360.Com.UI.Definitions.Pages.WorkshopManagement.CarWash
{
    public partial class CarWashModule
    {
        #region parameter

        private BLUIElement formDefinition, gridUIElement, findTrandsactionUI, cusPopupUlement;
        private BLTransaction transaction = new();
        private IDictionary<string, EventCallback> _interactionLogic;
        private IDictionary<string, BLUIElement> _modalDefinitions;
        private IDictionary<string, IBLUIOperationHelper> _objectHelpers;
        private BLTelGrid<TransactionLineItem> _tranGrid = new BLTelGrid<TransactionLineItem>();
        private UIBuilder _refBuilder;
        private AddNewCustomer? _refNewCustomer;
		private ITransactionValidator validator;
		private UserMessageDialog _refUserMessage;
        private TerlrikReportOptions _dayEndReportOption;
        private IList<CustomerDetailsByVehicle> customerDetails;
        BookingVehicleDetails request;
        private SelectCustomerPopUp _customerSelectDialog;

        private bool FindTransactionShown = false;
        private bool ShowCusPopUp = false;

        long elementKey;

        #endregion

        #region General 

        protected override async Task OnInitializedAsync()
        {
            elementKey = 1;

            _navigationManager.TryGetQueryString<long>("ElementKey", out elementKey);

            InitilizeCarWash();
            await InitilizeFormDefinitions();

            transaction.Location.CodeKey = 424710;
            transaction.Location.CodeName = "Main02 - UP WorkShop";
            transaction.Location.Code = "UP WorkShop";

            //CompletedUserAuth auth = await _authenticationManager.GetUserInformation();
            //_dayEndReportOption = new TerlrikReportOptions();
            //_dayEndReportOption.ReportName = ".trdp";
            //_dayEndReportOption.ReportParameters = new Dictionary<string, object>();
            //_dayEndReportOption.ReportParameters.Add("Cky", auth.AuthenticatedCompany.CompanyId);
            //_dayEndReportOption.ReportParameters.Add("UsrKy", auth.AuthenticatedUser.UserKey);
            //_dayEndReportOption.ReportParameters.Add("ObjKy", elementKey);
        }
        private async Task InitilizeFormDefinitions() 
        {
            _tranGrid.Refresh();

            if (transaction == null)
			{
				transaction = new BLTransaction();

            }
			else
			{
				transaction.InvoiceLineItems.Clear();
				transaction.Address = new AddressResponse();
				transaction.Account = new AccountResponse();
				transaction.Amount = 0;
				transaction.Amount2 = 0;
				transaction.Amount3 = 0;
				transaction.Amount4 = 0;
				transaction.Amount5 = 0;
				transaction.Amount6 = 0;
				transaction.SubTotal = 0;
				transaction.NetAmount = 0;
				transaction.DeliveryDate = DateTime.Now;
				transaction.TransactionKey = 1;
				transaction.IsPersisted = false;
				transaction.PaymentTerm = new CodeBaseResponse();
				transaction.Code1 = new CodeBaseResponse();
				transaction.Location = new CodeBaseResponse();
				transaction.ContraAccount = new AccountResponse();
				transaction.Rep = new AddressResponse();
				transaction.SerialNumber = new ItemSerialNumber();
				transaction.SerialNumber.SerialNumber = string.Empty;
				transaction.Code2 = new CodeBaseResponse();
				transaction.SelectedLineItem = new TransactionLineItem();
				transaction.CalculateTotals();
			}

			if (elementKey > 10)
            {
                var formrequest = new ObjectFormRequest();
                formrequest.MenuKey = elementKey;
                formDefinition = await _navManger.GetMenuUIElement(formrequest);

                gridUIElement = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("DetailsTable")).FirstOrDefault();

                if (gridUIElement != null)
                {
                    gridUIElement.Children = formDefinition.Children.Where(x => x.ParentKey == gridUIElement.ElementKey).ToList();
                }
            }
            if (formDefinition != null)
            {
                formDefinition.IsDebugMode = true;

            }

            _interactionLogic = new Dictionary<string, EventCallback>();
            _modalDefinitions = new Dictionary<string, BLUIElement>();
			if (_objectHelpers == null)
			{
				_objectHelpers = new Dictionary<string, IBLUIOperationHelper>();
			}
            validator = new CarWashModuleValidator(transaction);
			transaction.ElementKey = elementKey;
            request.ElementKey = elementKey;

            HookInteractions();
   
        }

        private async void HideAllPopups()
        {
            FindTransactionShown = false;
            StateHasChanged();
            await Task.CompletedTask;
        }

        private void HookInteractions()
        {
            InteractionHelper helper = new InteractionHelper(this, formDefinition);
            _interactionLogic = helper.GenerateEventCallbacks();
            appStateService._AppBarName = "CarWash";

        }

        private async void InitilizeCarWash()
        {
            customerDetails = new List<CustomerDetailsByVehicle>();
            request = new BookingVehicleDetails();
            cusPopupUlement = new BLUIElement();
        }

        private void UIStateChanged()
        {
            this.StateHasChanged();
        }

        #endregion

        #region Grid Event

        private async void OnDeleteClick(UIInterectionArgs<object> args) 
        {
            transaction.InvoiceLineItems.Remove((TransactionLineItem)args.DataObject);

            _tranGrid.Refresh();
            StateHasChanged();
        }

        private async void OnEditClick(UIInterectionArgs<object> args)
        {
            TransactionLineItem transactionEditLineItem = new TransactionLineItem();
            transactionEditLineItem = args.DataObject as TransactionLineItem;

            transaction.InvoiceLineItems.Remove(transactionEditLineItem);

            transaction.SelectedLineItem.TransactionItem = transactionEditLineItem.TransactionItem;

            if (BaseResponse.IsValidData(transactionEditLineItem.TransactionItem))
            {
                ItemRateResponse response = await RetriveRate(transaction.SelectedLineItem);
                transaction.SelectedLineItem.TransactionRate = response.TransactionRate;
                transaction.SelectedLineItem.Rate = response.Rate;
            }
            await ReadData("Unit");
            await SetValue("Rate", transactionEditLineItem.Rate);
            await Focus("Rate");

            _tranGrid.Refresh();
            StateHasChanged();
        }

        #endregion

        #region UI Event

        private async void OnCustomerClick(UIInterectionArgs<string> args) 
        {
            StateHasChanged();
        }
        private async void OnVehicleClick(UIInterectionArgs<AddressResponse> args) 
        {
            transaction.Address2 = args.DataObject;

            request.Registration = args.DataObject;
            if (request.Registration.AddressKey > 1) 
            {
                customerDetails = await _bookingManager.GetBookingCustomerDetails(request);

                if (customerDetails.Count() > 1)
                {
                    if (!_modalDefinitions.TryGetValue("CarRegistrationNumberPopUps", out cusPopupUlement))
                    {
                        cusPopupUlement = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("CarRegistrationNumberPopUp")).FirstOrDefault();
                        if (formDefinition != null && formDefinition.Children.Count > 0)
                        {
                            _modalDefinitions.Add("CarRegistrationNumberPopUps", cusPopupUlement);

                        }
                    }
                    ShowCusPopUp = true;
                }
                else 
                {
                    await SetValue("Customer", customerDetails.FirstOrDefault().Customer.AddressName);

                    transaction.Account.AccountName = customerDetails.FirstOrDefault().Customer.AddressName;
                    transaction.Account.AccountCode = customerDetails.FirstOrDefault().Customer.AddressID;
                    transaction.Account.AccountKey = customerDetails.FirstOrDefault().Customer.AddressKey;
                }

            }

            StateHasChanged();
        }
        private async void OnSelectCustomerName(UIInterectionArgs<AddressResponse> args)
        {
            if (args.DataObject != null)
            {
                await SetValue("Customer", args.DataObject.AddressName);

                transaction.Account.AccountName = args.DataObject.AddressName;
                transaction.Account.AccountCode = args.DataObject.AddressID;
                transaction.Account.AccountKey = args.DataObject.AddressKey;
            }
            UIStateChanged();
        }
        private async void OnAddToGrid(UIInterectionArgs<object> args)
        {
            if (validator.CanAddItemToGrid()) 
            {
				if (!transaction.SelectedLineItem.IsInEditMode)
				{
					transaction.InvoiceLineItems.Add(transaction.SelectedLineItem);
				}
				else
				{
					if (transaction.EditingLineItem != null)
					{
						transaction.SelectedLineItem.IsInEditMode = false;
						transaction.EditingLineItem.CopyFrom(transaction.SelectedLineItem);
						transaction.SelectedLineItem = new();
					}

				}

				transaction.CalculateTotals();
				transaction.InitilizeNewLineItem();
				transaction.SelectedLineItem.IsInEditMode = false;
			}
			else
			{
				_refUserMessage.ShowUserMessageWindow();
			}

            _tranGrid.Refresh(); 

			StateHasChanged();
            await Task.CompletedTask;
        }
        //private async void OnLocationClick(UIInterectionArgs<CodeBaseResponse> args)
        //{
        //    transaction.Location = args.DataObject;

        //    await Task.CompletedTask;
        //    StateHasChanged();
        //}
        private async void OnItemClick(UIInterectionArgs<ItemResponse> args)
        {
            transaction.SelectedLineItem.TransactionItem = args.DataObject;

            if (BaseResponse.IsValidData(args.DataObject))
            {
                ItemRateResponse response = await RetriveRate(transaction.SelectedLineItem);
                transaction.SelectedLineItem.TransactionRate = response.TransactionRate;
                transaction.SelectedLineItem.Rate = response.Rate;
            }
            await ReadData("Unit");

            StateHasChanged();
        }
        public async Task<ItemRateResponse> RetriveRate(TransactionLineItem transactionItem)
        {
            ItemService itemService = new ItemService();
            itemService.ComboManager = _comboManager;
            return await itemService.RequestItemRateForTransaction(transaction, transactionItem);
        }

        private async void Unit_OnBeforeDataFetch(UIInterectionArgs<ComboRequestDTO> args)
        {
            args.DataObject.AddtionalData.Add("ItemKey", BaseResponse.GetKeyValue(this.transaction.SelectedLineItem.TransactionItem));
            args.CancelChange = !BaseResponse.IsValidData(this.transaction.SelectedLineItem.TransactionItem);
            await Task.CompletedTask;

        }
        private async void OnUnitClick(UIInterectionArgs<UnitResponse> args) 
        {
            StateHasChanged();
        }
        private async void OnClickTransacionDate(UIInterectionArgs<DateTime?> args)
        {
            transaction.TransactionDate = (DateTime)args.DataObject;
            StateHasChanged();
        }

        #endregion

        #region On Header Button UI Event

        private async void OnPrintClick(UIInterectionArgs<object> args)
        {

            StateHasChanged();
        }
        //private async Task ShowFindTransactionWindow()
        //{
        //    HideAllPopups();
        //    FindTransactionShown = true;
        //    StateHasChanged();
        //    await Task.CompletedTask;
        //}
        private async Task LoadTransaction(TransactionOpenRequest request) 
        {
            HideAllPopups();

            request.ElementKey = transaction.ElementKey;
            request.TransactionKey = transaction.TransactionKey;

            URLDefinitions urlDefinitions = new URLDefinitions();
            urlDefinitions.URL = formDefinition.ReadController + "/" + formDefinition.ReadAction;
            BLTransaction otransaction = await _transactionManager.OpenTransaction(request, urlDefinitions);
            otransaction.ElementKey = transaction.ElementKey;

            transaction.CopyFrom(otransaction);

            IList<KeyValuePair<string, IBLUIOperationHelper>> pairs = _objectHelpers.ToList();

            foreach (KeyValuePair<string, IBLUIOperationHelper> helper in pairs)
            {
                await helper.Value.Refresh();

            }

            transaction.CalculateTotals();
        }

        private async void OnSaveTransaction(UIInterectionArgs<object> args)
        {
            await SaveTransaction();
            UIStateChanged();
        }

        private async Task SaveTransaction()
        {
            if (validator.CanSaveTransaction())
            {
                transaction = await _transactionManager.SaveTransaction(transaction);
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("Saved Successfully", Severity.Success);
                //TransactionOpenRequest request = new TransactionOpenRequest();
                //request.TransactionKey = transaction.TransactionKey;
                //await LoadTransaction(request);
            }
            else
            {
                _refUserMessage.ShowUserMessageWindow();
            }

            await Task.CompletedTask;
        }

        private async void OnClearClick(UIInterectionArgs<object> args)
        {

            _objectHelpers["Unit"].ResetToInitialValue();
            _objectHelpers["Vehicle"].ResetToInitialValue();
            _objectHelpers["Customer"].ResetToInitialValue();
            _objectHelpers["Item"].ResetToInitialValue();
            _objectHelpers["Rate"].ResetToInitialValue();

            transaction = new BLTransaction();

            StateHasChanged();
        }
        private async void OnAddNewCustomer(UIInterectionArgs<object> args) 
        {
            IDictionary<string, object> ParamDictionary = new Dictionary<string, object>();
            ParamDictionary.Add("InitiatorElement", args.InitiatorObject);
            ParameterView values = ParameterView.FromDictionary(ParamDictionary);
            await _refNewCustomer.SetParametersAsync(values);
            _refNewCustomer.ShowPopUp();
        }
        private async void NewTransaction(UIInterectionArgs<object> args)
        {
            _objectHelpers["Unit"].ResetToInitialValue();
            await InitilizeFormDefinitions();
            StateHasChanged();
        }

        #endregion

        #region customer creation 
        private async Task OnCustomerCreateSuccess(AddressMaster customer)
        {
            await ReadData("Customer");
            await SetValue("Customer", customer);

        }

        private async Task ReadData(string name, bool UseLocalStorage = false)
        {
            IBLUIOperationHelper helper;

            if (_objectHelpers.TryGetValue(name, out helper))
            {
                await (helper as IBLServerDependentComponent).FetchData(UseLocalStorage);

                StateHasChanged();
            }
        }
        #endregion

        #region object helpers

        private async Task SetValue(string name, object value)
        {
            IBLUIOperationHelper helper;

            if (_objectHelpers.TryGetValue(name, out helper))
            {
                await helper.SetValue(value);
                UIStateChanged();
                await Task.CompletedTask;
            }
        }

        private async Task Focus(string name)
        {
            IBLUIOperationHelper helper;

            if (_objectHelpers.TryGetValue(name, out helper))
            {
                await helper.FocusComponentAsync();
                StateHasChanged();
            }
        }

        #endregion
    }
}
