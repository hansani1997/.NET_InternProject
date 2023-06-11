using BL10.CleanArchitecture.Domain.Entities.Document;
using bluelotus360.com.razorComponents.Extensions;
using bluelotus360.com.razorComponents.MB.Shared.Components;
using bluelotus360.com.razorComponents.MB.Shared.Components.Popups.MasterDetailPopup;
using bluelotus360.com.razorComponents.MB.Shared.Components.Popups.Transaction;
using bluelotus360.com.razorComponents.Pages.Transaction.TransactionComponent;
using bluelotus360.com.razorComponents.Services;
using bluelotus360.Com.commonLib.Helpers;
using bluelotus360.Com.commonLib.Reports.Telerik;
using BlueLotus360.CleanArchitecture.Application.Validators.Transaction;
using BlueLotus360.CleanArchitecture.Domain;
using BlueLotus360.CleanArchitecture.Domain.DTO.MasterData;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.DTO.Report;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;

namespace bluelotus360.com.razorComponents.Pages.Transaction
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

        private bool ImagePopupShown = false;
        private FileUpload uploadObj;
        string LatestAprroveState = "Open";
        string NextApproveState;
        TransactionPermission permission=new TransactionPermission();
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
                transaction.Quantity1 = 0;
                transaction.SubTotal = 0;
                transaction.NetAmount = 0;
                //transaction.DeliveryDate = GetDeliveryDate("17:00");
                //transaction.YourReferenceDate = GetDeliveryDate("17:00");
                transaction.TransactionKey = 1;
                transaction.IsPersisted = false;
                transaction.IsVarcar1On = false;
                transaction.PaymentTerm = new CodeBaseResponse();
                transaction.Code1 = new CodeBaseResponse();
                transaction.Location = new CodeBaseResponse();
                transaction.ContraAccount = new AccountResponse();
                transaction.Rep = new AddressResponse();
                transaction.SerialNumber = new ItemSerialNumber();
                transaction.SerialNumber.SerialNumber = string.Empty;
                transaction.Code2 = new CodeBaseResponse();
                transaction.SelectedLineItem = new TransactionLineItem();
                Items = new List<ItemResponse>();
                transaction.CalculateTotals();
                if (_objectHelpers != null)
                {
                    _objectHelpers["SerialNumber"].ResetToInitialValue();
                    _objectHelpers["Comment"].ResetToInitialValue();
                    _objectHelpers["DeliDate"].ResetToInitialValue();
                    _objectHelpers["ConsentSigned"].ResetToInitialValue();
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
            if (Services != null && Services.Count() > 0) { Services.ToList().ForEach(x => x.AddtionalData["IsDisabled"] = false); }
            uploadObj = new FileUpload();

            //CheckkTransactionPermission();
            HookInteractions();
        }

       

        private async void NewTransaction(UIInterectionArgs<object> uIInterectionArgs)
        {
            await InitilizeFormDefinitions();
            await ResetToInitialValue("DeliDate");
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
        private async void OnLNDTrnDateChange(UIInterectionArgs<DateTime?> args)
        {
            transaction.TransactionDate = (DateTime)args.DataObject;
            StateHasChanged();
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
        private async void OnDeliveryDateChange(UIInterectionArgs<DateTime?> args)
        {
            transaction.DeliveryDate = (DateTime)args.DataObject;
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
                //transaction.SelectedLineItem.Quantity2 = response.Length;
            }
            //await Refresh("Article");
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
            _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
            _snackBar.Add("Invoice has been  Saved Successfully", Severity.Success);

        }

        private async Task SaveTransaction()
        {
            if (validator.CanSaveTransaction())
            {
                await _transactionManager.SaveTransaction(transaction);
                
                await SetValue("HeaderTitle", transaction.TransactionNumber);
                TransactionOpenRequest request = new TransactionOpenRequest();
                request.TransactionKey = transaction.TransactionKey;

                ItemSerialNumber ser = new ItemSerialNumber();
                ser.TransactionKey = transaction.TransactionKey;
                ser.SerialNumber = transaction.TransactionNumber;
                await _transactionManager.SaveHeaderSerialNumber(ser);

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
                    transaction.SelectedLineItem.LineNumber = transaction.InvoiceLineItems.Count + 1;
                    transaction.InvoiceLineItems.Add(transaction.SelectedLineItem);
                }
                else
                {
                    if (transaction.EditingLineItem != null)
                    {
                        transaction.SelectedLineItem.IsInEditMode = false;
                        transaction.SelectedLineItem.IsDirty = true;
                        transaction.EditingLineItem.CopyFrom(transaction.SelectedLineItem);
                        transaction.SelectedLineItem = new();
                    }

                }

                CodeBaseResponseExtended responseExtended = await RetriveAddtionalChanges(transaction.Code2);
                if (responseExtended != null)
                {
                    transaction.SelectedLineItem.Amount1 = responseExtended.CodeNumber1;
                    transaction.SelectedLineItem.Amount2 = transaction.SelectedLineItem.GetLineTotalWithDiscount() * responseExtended.CodeNumber1;
                }
                    
                transaction.CalculateTotals();
                transaction.InitilizeNewLineItem();
                _objectHelpers["Comment"].ResetToInitialValue();
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

            List<string> serialNumbers = new List<string>();
            foreach (var itm in transaction.InvoiceLineItems)
            {
                if (itm.SerialNumbers.Count > 0)
                {
                    serialNumbers.AddRange(itm.SerialNumbers.Select(x => x.SerialNumber));
                }
            }

            dialogParam.Add("SerialNumberList", serialNumbers);
            var dialog = _dialogService.Show<ScanSerial>("Select Barcode Number", dialogParam, dialogOptions);
            //DialogResult dialogResult = await dialog.Result;

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

        

        private async Task LoadTransaction(TransactionOpenRequest request)
        {
            HideAllPopups();
            DateTime dateTime1 = DateTime.Now;
            isSaving = true;
            URLDefinitions urlDefinitions = new URLDefinitions();
            urlDefinitions.URL = UIDefinition.ReadController + "/" + UIDefinition.ReadAction;
            BLTransaction otransaction = await _transactionManager.OpenTransaction(request, urlDefinitions);
            if (otransaction!=null && otransaction.InvoiceLineItems!=null)
            {
                TransactionLineItem trnitm=otransaction.InvoiceLineItems.Where(x => x.IsActive == 1).FirstOrDefault();
                if (trnitm!=null)
                {
                    otransaction.YourReferenceDate = (DateTime)(trnitm.DeliveryDate);
                }
                otransaction.ElementKey = transaction.ElementKey;

                foreach (var itm in otransaction.InvoiceLineItems)
                {
                    DocumentRetrivaltDTO document = new DocumentRetrivaltDTO();
                    document.ItemTransactionKey = (int)itm.ItemTransactionKey;
                    itm.Base64Documents = await _uploadManager.getBase64Documents(document);
                }

                transaction.CopyFrom(otransaction);
                await ReadCurrentPayedTotal();
                CheckkTransactionPermission();

            }
            
            
            string valueN = "";

            IList<KeyValuePair<string, IBLUIOperationHelper>> pairs = _objectHelpers.ToList();

            foreach (KeyValuePair<string, IBLUIOperationHelper> helper in pairs)
            {
                await helper.Value.Refresh();

            }

            transaction.CalculateTotals();
            // transaction.CalculateCBalances();
            await SetValue("HeaderTitle", transaction.TransactionNumber);

            //AppSettings.RefreshTopBar("Invoice - " + transaction.TransactionNumber);
            appStateService._AppBarName = "Invoice - " + transaction.TransactionNumber +" ("+transaction.ApproveState.CodeName+")";

            isSaving = false;
        }


        private async void HideAllPopups()
        {
            FindTransactionShown = false;
            ReportShown = false;
            ImagePopupShown = false;
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

            URLDefinitions urlDefinitions = new URLDefinitions();
            urlDefinitions.URL = UIDefinition.ReadController + "/" + UIDefinition.ReadAction;
            dialogParam.Add("UrlDefinition", urlDefinitions);

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

            _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
            _snackBar.Add("Reciept is Saved Successfully", Severity.Success);

            StateHasChanged();

        }

        private async Task OnRecipetClose()
        {
            HideAllPopups();
            await Task.CompletedTask;
        }

        private DateTime GetDeliveryDate(string date_time)
        {
            if (!string.IsNullOrEmpty(date_time))
            {

                DateTime dt = DateTime.Parse(date_time);

                string[] default_date = new string[7];
                int i = 0;

                if (date_time.Contains('/') && date_time.Contains(':'))
                {
                    return dt;
                }
                else if (date_time.Contains('/'))
                {
                    foreach (string dts in dt.ToString("yyyy/MM/dd").Split("/"))
                    {
                        default_date[i] = dts;
                        i++;
                    }
                    default_date[3] = DateTime.Now.ToString("HH", CultureInfo.InvariantCulture);
                    default_date[4] = DateTime.Now.ToString("mm", CultureInfo.InvariantCulture);
                    default_date[5] = DateTime.Now.ToString("ss", CultureInfo.InvariantCulture);
                    default_date[6] = DateTime.Now.ToString("tt", CultureInfo.InvariantCulture);


                    if (!string.IsNullOrEmpty(default_date[0]) && !string.IsNullOrEmpty(default_date[1]) && !string.IsNullOrEmpty(default_date[2]) && !string.IsNullOrEmpty(default_date[3]) && !string.IsNullOrEmpty(default_date[4]) && !string.IsNullOrEmpty(default_date[5]) && !string.IsNullOrEmpty(default_date[6]))
                    {
                        return new DateTime(Convert.ToInt32(default_date[0]), Convert.ToInt32(default_date[1]), Convert.ToInt32(default_date[2]), Convert.ToInt32(default_date[3]), Convert.ToInt32(default_date[4]), Convert.ToInt32(default_date[5]));
                    }
                    else
                    {
                        return DateTime.Now;
                    }
                }
                else if (date_time.Contains(':'))
                {
                    foreach (string dts in dt.ToString("HH:mm").Split(":"))
                    {
                        default_date[i] = dts;
                        i++;
                    }
                    if (string.IsNullOrEmpty(default_date[2]))
                    {
                        default_date[2] = "00";
                    }
                    default_date[3] = dt.ToString("tt", CultureInfo.InvariantCulture);
                    default_date[4] = DateTime.Now.ToString("dd", CultureInfo.InvariantCulture);
                    default_date[5] = DateTime.Now.ToString("MM", CultureInfo.InvariantCulture);
                    default_date[6] = DateTime.Now.ToString("yyyy", CultureInfo.InvariantCulture);

                    if (!string.IsNullOrEmpty(default_date[0]) && !string.IsNullOrEmpty(default_date[1]) && !string.IsNullOrEmpty(default_date[2]) && !string.IsNullOrEmpty(default_date[3]) && !string.IsNullOrEmpty(default_date[4]) && !string.IsNullOrEmpty(default_date[5]) && !string.IsNullOrEmpty(default_date[6]))
                    {
                        return new DateTime(Convert.ToInt32(default_date[6]), Convert.ToInt32(default_date[5]), Convert.ToInt32(default_date[4]), Convert.ToInt32(default_date[0]), Convert.ToInt32(default_date[1]), Convert.ToInt32(default_date[2]));
                    }
                    else
                    {
                        return DateTime.Now;
                    }
                }
                else
                {
                    return DateTime.Now;
                }

            }
            else
            {
                return DateTime.Now;
            }
        }

        private void ShowUploadPopUp(TransactionLineItem line)
        {
            uploadObj = new FileUpload();
            uploadObj.ItemTransactionKey = (int)line.ItemTransactionKey;

            ImagePopupShown = true;
            StateHasChanged();
        }
        private async void UploadSuccess()
        {
            TransactionOpenRequest request = new TransactionOpenRequest();
            request.TransactionKey = transaction.TransactionKey;
            await LoadTransaction(request);
            StateHasChanged();
        }

        private async void CheckkTransactionPermission()
        {
            BLTransaction permission_req = new BLTransaction()
            {
                ApproveState = transaction.ApproveState,
                ElementKey = elementKey,
                TransactionKey = transaction.TransactionKey
            };
            permission = await _transactionManager.CheckTransactionPermission(permission_req) ?? new TransactionPermission();
            LatestAprroveState = transaction?.ApproveState.CodeName ?? "";

            if (permission!=null && !Convert.ToBoolean(permission.IsAllowUpdate))
            {
                this.ToggleEditability("SaveButton",false);
                StateHasChanged();
            }
        }

        private void ToggleEditability(string name, bool visible)
        {
            IBLUIOperationHelper helper;

            if (_objectHelpers.TryGetValue(name, out helper))
            {
                helper.ToggleEditable(visible);
                StateHasChanged();
            }
        }

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

        private async Task ResetToInitialValue(string name)
        {
            IBLUIOperationHelper helper;

            if (_objectHelpers.TryGetValue(name, out helper))
            {
                helper.ResetToInitialValue();
                StateHasChanged();
                await Task.CompletedTask;
            }
        }

    }
}
