using BlueLotus360.CleanArchitecture.Application.Validators.Transaction;
using BlueLotus360.CleanArchitecture.Client.Infrastructure.Helpers;
using BlueLotus360.CleanArchitecture.Client.Infrastructure.Reports.Telerik;
using BlueLotus360.CleanArchitecture.Domain;
using BlueLotus360.CleanArchitecture.Domain.DTO.MasterData;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.DTO.Report;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using BlueLotus360.Com.UI.Definitions.Extensions;
using BlueLotus360.Com.UI.Definitions.MB.Settings;
using BlueLotus360.Com.UI.Definitions.MB.Shared;
using BlueLotus360.Com.UI.Definitions.MB.Shared.Components;
using BlueLotus360.Com.UI.Definitions.MB.Shared.Components.Dialogs;
using BlueLotus360.Com.UI.Definitions.MB.Shared.Components.Popups;
using BlueLotus360.Com.UI.Definitions.MB.Shared.Components.Popups.MasterDetailPopup;
using BlueLotus360.Com.UI.Definitions.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueLotus360.Com.UI.Definitions.Pages.Transaction
{
    public partial class LaundroCareTransaction
    {
        private long elementKey = 1;
        private BLUIElement UIDefinition;//wht
        private BLUIElement findTrandsactionUI;//wht
        private IDictionary<string, EventCallback> _interactionLogic;
        private IDictionary<string, IBLUIOperationHelper> _objectHelpers;
        private IDictionary<string, BLUIElement> _modalDefinitions;
        private LaundercareItemPiicker _refItemPicker;
        private BLTransaction transaction = new();
        private IList<CodeBaseResponse> Services = new List<CodeBaseResponse>();
        private IList<CodeBaseResponse> HumanTypes = new List<CodeBaseResponse>();
        private IList<ItemResponse> Items = new List<ItemResponse>();
        private ITransactionValidator validator;
        private string _tableHeight = "200px";
        private MudTable<TransactionLineItem> _table;
        private bool isInEditMode = false;
        private bool isSaving = false;
        private bool FindTransactionShown = false;
        private AddNewAddress _refNewAddressCreation;
        private UserMessageDialog _refUserMessage;
        private GenericReciept _refgenericReciept;
        private bool ReportShown = false;
        private ReportCompanyDetailsResponse _companyDetails;
        private TerlrikReportOptions _dayEndReportOption;

        protected override async Task OnInitializedAsync()
        {

            _navigationManager.TryGetQueryString<long>("ElementKey", out elementKey);
            await InitilizeFormDefinitions();
            await base.OnInitializedAsync();
            CompletedUserAuth auth = await _authenticationManager.GetUserInformation();
            _dayEndReportOption = new TerlrikReportOptions();
            _dayEndReportOption.ReportName = "Invoice_LND.trdp";
            _dayEndReportOption.ReportParameters = new Dictionary<string, object>();
            _dayEndReportOption.ReportParameters.Add("Cky", auth.AuthenticatedCompany.CompanyKey);
            _dayEndReportOption.ReportParameters.Add("UsrKy", auth.AuthenticatedUser.UserKey);
            _dayEndReportOption.ReportParameters.Add("ObjKy", elementKey);



        }

        private async Task InitilizeFormDefinitions()
        {
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
                transaction.SerialNumber.SerialNumber=string.Empty;
                transaction.Code2 = new CodeBaseResponse();
                transaction.SelectedLineItem = new TransactionLineItem();
                Items = new List<ItemResponse>();
                transaction.CalculateTotals();
                if (_objectHelpers!=null)
                {
                    _objectHelpers["SerialNumber"].ResetToInitialValue();
                    _objectHelpers["Comment"].ResetToInitialValue();
                }
                
            }

            if (_objectHelpers == null)
            {
                _objectHelpers = new Dictionary<string, IBLUIOperationHelper>();
            }
            validator = new LaundroCareValidator(transaction);
            var formrequest = new ObjectFormRequest();
            formrequest.MenuKey = elementKey; 
            if (UIDefinition == null || UIDefinition.Children.Count == 0)
            {
                UIDefinition = await _navManger.GetMenuUIElement(formrequest);
                UIDefinition.CssClass = "laund";
            }
            
            _interactionLogic = new Dictionary<string, EventCallback>();
            _modalDefinitions = new Dictionary<string, BLUIElement>();
            transaction.ElementKey = elementKey;
            if (Services != null && Services.Count()>0) { Services.ToList().ForEach(x => x.AddtionalData["IsDisabled"] = false); }
            HookInteractions();
        }


        private async void NewTransaction(UIInterectionArgs<object> uIInterectionArgs)
        {
            await InitilizeFormDefinitions();
            StateHasChanged();
        }


        private void HookInteractions()
        {
            InteractionHelper helper = new InteractionHelper(this, UIDefinition);//formdefinition has all form objects 
            _interactionLogic = helper.GenerateEventCallbacks();//
            InitilizeOtherUIControle();
            //AppSettings.RefreshTopBar("Invoice");
            appStateService._AppBarName = "Invoice";
        }



        private void InitilizeOtherUIControle()
        {
            //    HookInteractions();
        }


        #region UIEvents

        private async void OnTransactionLocationChanged(UIInterectionArgs<CodeBaseResponse> args)
        {

            StateHasChanged();
            await Task.CompletedTask;
        }


        private async void ServiceName_AfterComboLoaded(UIInterectionArgs<IList<CodeBaseResponse>> args)
        {
            Services = args.DataObject;

            if (Services != null) 
            { 
                //Services.ToList().ForEach(x => x.AddtionalData["IsDisabled"] = false);

                foreach (CodeBaseResponse c in Services)
                {
                    c.AddtionalData["IsDisabled"] = false;
                    c.SeperateServiceType();
                }
            }

            
            await Task.CompletedTask;
            StateHasChanged();
        }
        private async void HumanType_AfterComboLoaded(UIInterectionArgs<IList<CodeBaseResponse>> args)
        {
            HumanTypes = args.DataObject;
            await Task.CompletedTask;
            StateHasChanged();
        }


        private async void OnItemCategory1Change(UIInterectionArgs<CodeBaseResponse> args)
        {
            transaction.SelectedLineItem.ItemCategory1 = args.DataObject;
            await ReadData("Article");
            StateHasChanged();
        }

        private async void OnItemCategory2Change(UIInterectionArgs<CodeBaseResponse> args)
        {
            transaction.SelectedLineItem.ItemCategory2 = args.DataObject;
            await ReadData("Article");
            StateHasChanged();
        }

        private async void OnContraAccountChange(UIInterectionArgs<AccountResponse> args)
        {
            await Task.CompletedTask;
            transaction.ContraAccountObjectKey = args.InitiatorObject.ElementKey;
            StateHasChanged();

        }

        private async void OnAccountChange(UIInterectionArgs<AccountResponse> args)
        {
            await Task.CompletedTask;
            transaction.AccountObjectKey = args.InitiatorObject.ElementKey;
            StateHasChanged();

        }

        private async void Article_OnBeforeDataFetch(UIInterectionArgs<ComboRequestDTO> args)
        {
            args.DataObject.CancelRead = !validator.CanItemComboRequestFromServer();
            args.DataObject.AddtionalData.Add("ItmCat2Ky", transaction.SelectedLineItem.ItemCategory2 == null
                ? 1 : transaction.SelectedLineItem.ItemCategory2.CodeKey
                );
            args.DataObject.AddtionalData.Add("ItmCat1Ky", transaction.SelectedLineItem.ItemCategory1 == null ?
                1 : transaction.SelectedLineItem.ItemCategory1.CodeKey
                );

            await Task.CompletedTask;
        }

        private async void OpenInvoiceReport(UIInterectionArgs<object> args)
        {
            HideAllPopups();
            if (transaction.TransactionKey > 10)
            {
                object varLockKy;

                if (_dayEndReportOption.ReportParameters.TryGetValue("TrnKy", out varLockKy))
                {
                    _dayEndReportOption.ReportParameters["TrnKy"] = transaction.TransactionKey;
                }
                else
                {
                    _dayEndReportOption.ReportParameters.Add("TrnKy", transaction.TransactionKey);
                }
                ReportShown = true;
            }
            else
            {
                bool? result = await _dialogService.ShowMessageBox(
                  "Warning",
                  $"Please Select a Transaction"
                );
                return;
            }

            StateHasChanged();
        }
        private async void Article_AfterComboLoaded(UIInterectionArgs<IList<ItemResponse>> args)
        {
            Items = args.DataObject;
            StateHasChanged();
            await Task.CompletedTask;
        }


        private async void OnTransactionItemChanged(UIInterectionArgs<ItemResponse> args)
        {
            transaction.SelectedLineItem.TransactionItem = args.DataObject;
            if (BaseResponse.IsValidData(args.DataObject))
            {
                ItemRateResponse response = await RetriveRate(transaction.SelectedLineItem);
                transaction.SelectedLineItem.TransactionRate = response.TransactionRate;
                transaction.SelectedLineItem.MarkupPercentage = response.MarkUpPercentage;
                transaction.MarkupPercentage = response.MarkUpPercentage;
                transaction.SelectedLineItem.Rate = response.Rate;
            }
            await Refresh("Article");
            await Task.CompletedTask;
            StateHasChanged();
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

        private async Task Refresh(string name)
        {
            IBLUIOperationHelper helper;

            if (_objectHelpers.TryGetValue(name, out helper))
            {
                await helper.Refresh();

                StateHasChanged();
            }
        }


        private async void OnInvoiceSaveClick(UIInterectionArgs<object> args)
        {
            await SaveTransaction();

        }

        private async Task SaveTransaction()
        {
            if (validator.CanSaveTransaction())
            {
                await _transactionManager.SaveTransaction(transaction);
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("Invoice has been  Saved Successfully", Severity.Success);
                await SetValue("HeaderTitle", transaction.TransactionNumber);
                TransactionOpenRequest request = new TransactionOpenRequest();
                request.TransactionKey = transaction.TransactionKey;
                await LoadTransaction(request);
            }
            else
            {
                _refUserMessage.ShowUserMessageWindow();
            }

            await Task.CompletedTask;
        }

        private async void OnPaymementTermChange(UIInterectionArgs<CodeBaseResponse> args)
        {
            await Task.CompletedTask;
            StateHasChanged();
        }



        private async void OnCodeBaseComboChange(UIInterectionArgs<CodeBaseResponse> args)
        {

            await Task.CompletedTask;
            StateHasChanged();
        }

        public async Task<ItemRateResponse> RetriveRate(TransactionLineItem transactionItem)
        {
            ItemService itemService = new ItemService();
            itemService.ComboManager = _comboManager;
            return await itemService.RequestItemRateForTransaction(transaction, transactionItem);
        }

        public async Task<CodeBaseResponseExtended> RetriveAddtionalChanges(CodeBaseResponse response)
        {
            ItemService itemService = new ItemService();
            itemService.ComboManager = _comboManager;
            return await itemService.GetItemAditionalCharges(response);
        }



        private async void OnItemEditClick(TransactionLineItem line)
        {
            //isInEditMode = true;
            line.IsInEditMode = true;
            //transaction.SelectedLineItem = line;
            transaction.EditingLineItem = new();
            transaction.EditingLineItem = line;
            transaction.SelectedLineItem.CopyFrom(line);
            
            StateHasChanged();
            await Task.CompletedTask;

        }

        private async void OnOrderItemDelete(TransactionLineItem item)
        {
            bool? result = await _dialogService.ShowMessageBox(
                "Warning",
                $"Do you want to remove Item {item.TransactionItem.ItemName}",
                yesText: "Delete!", cancelText: "Cancel");

            if (result.HasValue && result.Value)
            {
                item.IsActive = 0;
                transaction.CalculateCBalances();
                transaction.CalculateTotals();
                StateHasChanged();
            }
            await Task.CompletedTask;

        }
        private async void OnAddtoGridClick(UIInterectionArgs<object> args)
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

                CodeBaseResponseExtended responseExtended = await RetriveAddtionalChanges(transaction.Code2);
                transaction.SelectedLineItem.Amount1 = responseExtended.CodeNumber1;
                transaction.SelectedLineItem.Amount2 = transaction.SelectedLineItem.GetLineTotalWithDiscount() * responseExtended.CodeNumber1;
                transaction.CalculateTotals();
                transaction.InitilizeNewLineItem();
                transaction.SelectedLineItem.IsInEditMode = false;
                

            }
            else
            {
                _refUserMessage.ShowUserMessageWindow();
            }
            
            StateHasChanged();

            await Task.CompletedTask;
        }

        private async void OnCancelClick(UIInterectionArgs<object> args)
        {
            transaction.SelectedLineItem = new();
            _objectHelpers["Comment"].ResetToInitialValue();
            StateHasChanged();
        }
        private async void ShowScanSerialNumber(TransactionLineItem item)
        {
            DialogOptions dialogOptions = new DialogOptions();
            dialogOptions.MaxWidth = MaxWidth.Medium;
            DialogParameters dialogParam = new DialogParameters();
            dialogParam.Add("LineItem", item);
            var dialog = _dialogService.Show<ScanSerial>("Select Barcode Number", dialogParam, dialogOptions);

            StateHasChanged();
            await Task.CompletedTask;
        }


        private async void OnNumericBoxChnaged(UIInterectionArgs<decimal> args)
        {
            transaction.CalculateTotals();
            StateHasChanged();
            await Task.CompletedTask;

        }
        #endregion

        private async Task SetValue(string name, object value)
        {
            IBLUIOperationHelper helper;

            if (_objectHelpers.TryGetValue(name, out helper))
            {
                await helper.SetValue(value);
                StateHasChanged();
                await Task.CompletedTask;
            }
        }

        private async Task LoadTransaction(TransactionOpenRequest request)
        {
            HideAllPopups();
            DateTime dateTime1 = DateTime.Now;
            isSaving = true;
            URLDefinitions urlDefinitions = new URLDefinitions();
            urlDefinitions.URL = UIDefinition.ReadController + "/" + UIDefinition.ReadAction;
            BLTransaction otransaction = await _transactionManager.OpenTransaction(request, urlDefinitions);
            otransaction.ElementKey = transaction.ElementKey;

            transaction.CopyFrom(otransaction);
            await ReadCurrentPayedTotal();
            string valueN = "";

            IList<KeyValuePair<string, IBLUIOperationHelper>> pairs = _objectHelpers.ToList();

            foreach (KeyValuePair<string, IBLUIOperationHelper> helper in pairs)
            {
                await helper.Value.Refresh();

            }

            transaction.CalculateTotals();
            // transaction.CalculateCBalances();
            await SetValue("HeaderTitle", transaction.TransactionNumber);

            AppSettings.RefreshTopBar("Invoice - " + transaction.TransactionNumber);

            isSaving = false;
        }


        private async void HideAllPopups()
        {
            FindTransactionShown = false;
            ReportShown = false;
            StateHasChanged();
            await Task.CompletedTask;
        }

        private async void OnSearchTracsactionClick(UIInterectionArgs<object> args)
        {
            findTrandsactionUI = args.InitiatorObject;

            await ShowFindTransactionWindow();

        }

        private async void ShowAddNewCustomer(UIInterectionArgs<object> args)
        {
            IDictionary<string, object> ParamDictionary = new Dictionary<string, object>();
            ParamDictionary.Add("InitiatorElement", args.InitiatorObject);
            ParameterView values = ParameterView.FromDictionary(ParamDictionary);
            await _refNewAddressCreation.SetParametersAsync(values);
            _refNewAddressCreation.ShowPopUp();

        }

        private async Task ShowFindTransactionWindow()
        {
            HideAllPopups();
            FindTransactionShown = true;
            StateHasChanged();
            await Task.CompletedTask;
        }


        private async Task OnCustomerCreateSuccess(AddressMaster address)
        {
            await ReadData("Customer");
            await SetValue("Customer", address);

        }

        private async void OnRecieptsClick(UIInterectionArgs<object> args)
        {
            HideAllPopups();
            if (transaction.TransactionKey > 10)
            {
                IDictionary<string, object> ParamDictionary = new Dictionary<string, object>();
                ParamDictionary.Add("InitiatorElement", args.InitiatorObject);
                ParameterView values = ParameterView.FromDictionary(ParamDictionary);
                await _refgenericReciept.SetParametersAsync(values);
                _refgenericReciept.ShowPopUp();
                StateHasChanged();
                await Task.CompletedTask;

            }

            else
            {
                bool? result = await _dialogService.ShowMessageBox(
                  "Warning",
                  $"Please Select a Transaction"
                );
                return;

            }

        }

        private async void OnAmount5Change(UIInterectionArgs<decimal> args)
        {
            if (!transaction.AddtionalData.ContainsKey("AdvancePaymentObjectKey"))
            {
                transaction.AddtionalData.Add("AdvancePaymentObjectKey", args.InitiatorObject.ElementKey);
            }
            else
            {
                transaction.AddtionalData["AdvancePaymentObjectKey"] = args.InitiatorObject.ElementKey;

            }

            transaction.CalculateTotals();
            StateHasChanged();
            await Task.CompletedTask;
        }



        private async void OnPriceListChange(UIInterectionArgs<CodeBaseResponse> uIInterectionArgs)
        {

            await ReadData("ServiceName");
            StateHasChanged();

            await Task.CompletedTask;
        }


        private async void ServiceName_OnBeforeDataFetch(UIInterectionArgs<ComboRequestDTO> args)
        {
            args.DataObject.AddtionalData.Add("PreKy", BaseResponse.GetKeyValue(transaction.Code1));
            await Task.CompletedTask;
        }
        private async void OnCheckOutClick(UIInterectionArgs<object> args)
        {
            DialogOptions dialogOptions = new DialogOptions();
            dialogOptions.MaxWidth = MaxWidth.Medium;
            DialogParameters dialogParam = new DialogParameters();
            dialogParam.Add("InitiatorElement", args.InitiatorObject);
            var dialog = _dialogService.Show<LaundrocareCheckOut>("Check Out", dialogParam, dialogOptions);
        }


        private async Task ReadCurrentPayedTotal()
        {
            RecieptDetailRequest request = new RecieptDetailRequest();
            request.ElementKey = transaction.ElementKey;
            request.TransactionKey = transaction.TransactionKey;
            RecviedAmountResponse response = await _transactionManager.GetTotalPayedAmount(request);
            transaction.Amount5 = response.TotalPayedAmount;
            StateHasChanged();
        }


        private async Task OnRecieptSavedSuccessfully()
        {
            await ReadCurrentPayedTotal();
            transaction.CalculateCBalances();
            transaction.CalculateTotals();
            if (transaction.IsPersisted)
            {
                await SaveTransaction();
            }
            StateHasChanged();

        }

        private async Task OnRecipetClose()
        {

            await Task.CompletedTask;
        }

    }
}
