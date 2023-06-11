using BlueLotus360.CleanArchitecture.Application.Validators.Transaction;
using BlueLotus360.CleanArchitecture.Client.Infrastructure.Helpers;
using BlueLotus360.CleanArchitecture.Client.Infrastructure.Reports.Telerik;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.DTO.Report;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using BlueLotus360.Com.UI.Definitions.Extensions;
using BlueLotus360.Com.UI.Definitions.MB.Settings;
using BlueLotus360.Com.UI.Definitions.MB.Shared.Components;
using BlueLotus360.Com.UI.Definitions.MB.Shared.Components.Dialogs;
using BlueLotus360.Com.UI.Definitions.MB.Shared.Components.Popups;
using BlueLotus360.Com.UI.Definitions.MB.Shared.Components.Popups.MasterDetailPopup;
using BlueLotus360.Com.UI.Definitions.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;


namespace BlueLotus360.Com.UI.Definitions.Pages.Transaction
{
    public partial class StockCount
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

        private bool IsContinuesScanMode = false;
        protected override async Task OnInitializedAsync()
        {
            validator = new StockCountValidatorMobile(transaction); 
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
                transaction.SelectedLineItem = new TransactionLineItem();
                transaction.CalculateTotals();
            }

            if (_objectHelpers == null)
            {
                _objectHelpers = new Dictionary<string, IBLUIOperationHelper>();
            }

            var formrequest = new ObjectFormRequest();
            formrequest.MenuKey = elementKey;
            if (UIDefinition == null || UIDefinition.Children.Count == 0)
            {
                UIDefinition = await _navManger.GetMenuUIElement(formrequest);
            }
            UIDefinition.CssClass = "laund";
            _interactionLogic = new Dictionary<string, EventCallback>();
            _modalDefinitions = new Dictionary<string, BLUIElement>();
            transaction.ElementKey = elementKey;
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
            appStateService._AppBarName = "Stock Count - New";
        }

        #region UIEvents    
    
        private async void OnTransactionItemChanged(UIInterectionArgs<ItemResponse> args)
        {
            transaction.SelectedLineItem.TransactionItem = args.DataObject;
            if (BaseResponse.IsValidData(args.DataObject))
            {
                ItemRateResponse response = await RetriveRate(transaction.SelectedLineItem);
                transaction.SelectedLineItem.TransactionRate = response.TransactionRate;
                transaction.SelectedLineItem.Rate = response.Rate;
            }
            await ReadData("Unit");
            if (IsContinuesScanMode)
            {
                OnAddtoGridClick(null);
            }
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
        private async void OnInvoiceSaveClick(UIInterectionArgs<object> args)
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


        private async void CountUnit_OnBeforeDataFetch(UIInterectionArgs<ComboRequestDTO> args)
        {
            args.DataObject.AddtionalData.Add("ItemKey", BaseResponse.GetKeyValue(this.transaction.SelectedLineItem.TransactionItem));
            args.CancelChange = !BaseResponse.IsValidData(this.transaction.SelectedLineItem.TransactionItem);
            await Task.CompletedTask;

        }


        private async void OnItemEditClick(TransactionLineItem line)
        {
            isInEditMode = true;
            transaction.SelectedLineItem = line;
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
                StateHasChanged();
            }
            await Task.CompletedTask;

        }
        private async void OnAddtoGridClick(UIInterectionArgs<object> args)
        {
            if (validator.CanAddItemToGrid())
            {
                if (!isInEditMode)
                {

                    var item = transaction.InvoiceLineItems.Where(x => x.TransactionItem.ItemKey == transaction.SelectedLineItem.TransactionItem.ItemKey).FirstOrDefault();
                    if (item != null)
                    {
                        item.IsDirty = true;
                        item.TransactionQuantity += transaction.SelectedLineItem.TransactionQuantity;
                    }
                    else
                    {
                        transaction.InvoiceLineItems.Add(transaction.SelectedLineItem);

                    }

                }
                transaction.CalculateTotals();
                transaction.InitilizeNewLineItem();
                isInEditMode = false;
            }
            else
            {
                _refUserMessage.ShowUserMessageWindow();
            }
            await SetValue("ItemCode", "");
            await Focus("ItemCode");
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
            }
        }

        private async Task LoadTransaction(TransactionOpenRequest request)
        {
            HideAllPopups();
            DateTime dateTime1 = DateTime.Now;
            isSaving = true;
            URLDefinitions urlDefinitions = new URLDefinitions();
            urlDefinitions.URL = UIDefinition.ReadController + "/" + UIDefinition.ReadAction;
            BLTransaction otransaction = await _transactionManager.OpenTransaction(request);
            otransaction.ElementKey = transaction.ElementKey;
            transaction.CopyFrom(otransaction);
            string valueN = "";
            IList<KeyValuePair<string, IBLUIOperationHelper>> pairs = _objectHelpers.ToList();

            foreach (KeyValuePair<string, IBLUIOperationHelper> helper in pairs)
            {
                await helper.Value.Refresh();

            }
            transaction.CalculateTotals();
            // transaction.CalculateCBalances();

            AppSettings.RefreshTopBar("Stock Count - " + transaction.TransactionNumber);
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
        private async Task ShowFindTransactionWindow()
        {
            HideAllPopups();
            FindTransactionShown = true;
            StateHasChanged();
            await Task.CompletedTask;
        }


        private async void OnItemCodeChanged(UIInterectionArgs<object> args)
        {
            if (args.DataObject != null && !string.IsNullOrWhiteSpace(args.DataObject.ToString()))
            {
                ItemRequestModel requestModel = new ItemRequestModel();

                requestModel.ItemCode = args.DataObject.ToString();
                var resp = await _comboManager.GetItemByItemCode(requestModel);
                if (resp.Count > 0)
                {
                    await SetValue("Item", resp[0].ItemKey);
                }
                else
                {
                    _snackBar.Add($"Item With Item Code {requestModel.ItemCode} Not Found", Severity.Error);
                }

             
            }
            await Task.CompletedTask;

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

        private async void OnScanModeChange(UIInterectionArgs<bool> args)
        {

            IsContinuesScanMode = args.DataObject;
            await Task.CompletedTask;

        }

    }
}
