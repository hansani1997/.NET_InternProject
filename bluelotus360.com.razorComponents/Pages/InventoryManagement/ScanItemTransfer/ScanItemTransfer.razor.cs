using bluelotus360.com.razorComponents.Extensions;
using bluelotus360.com.razorComponents.MB.Shared.Components;
using bluelotus360.com.razorComponents.MB.Shared.Components.Dialogs;
using bluelotus360.Com.commonLib.Helpers;
using bluelotus360.Com.commonLib.Reports.Telerik;
using BlueLotus360.CleanArchitecture.Application.Validators.InventoryManagement;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain.Entities.InventoryManagement.ItemTransfer;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MudBlazor.CategoryTypes;

namespace bluelotus360.com.razorComponents.Pages.InventoryManagement.ScanItemTransfer
{
    public partial class ScanItemTransfer
    {
        #region parameter
        private BLUIElement formDefinition;
        private IDictionary<string, EventCallback> _interactionLogic;
        private IDictionary<string, BLUIElement> _modalDefinitions;
        private IDictionary<string, IBLUIOperationHelper> _objectHelpers;

        private UIBuilder _refBuilder;

        private ItemTransfer _itmTransfer;
        private ItemTransfer _afterSaveHeaderResponse;
        private ItemTransferLineItem _oneLineItm;
        private long elementKey;
        private bool IsExpand = false;
        private IItemTransferValidator validator;
        private int TrnkyForOut = 1, TrnKyForIn = 1;
        ItemTransfer _find_itm_trnsfer;
        private int total_scanned_session = 0;
        private bool FindTransferShown = false;
        private bool IsbukeySelected;
        private BLUIElement findTransferUI, TopButtonSection, HeaderSection, ApprovalSection;
        private TransferOpenRequest trnOpReq;
        private string _serialNo = "";
        private bool IsSerialNoValidationShown = false;
        private int isActiveZeroCount = 1;
        private string TrnNoText = "";
        bool IsValidTransfer;
        bool IsApprovalUIShown, IsSaveButtonDisbled;
        private IList<ItemTransferLineItem> _invoiceItemListForApproval = new List<ItemTransferLineItem>();
        string ButtonName = "Approve";
        CompletedUserAuth auth = new CompletedUserAuth();
        private bool ReportShown = false;
        private TerlrikReportOptions _itmTransferReportOptions = new TerlrikReportOptions();
        #endregion
        protected override async Task OnInitializedAsync()
        {
            elementKey = 1;
            _interactionLogic = new Dictionary<string, EventCallback>();
            _modalDefinitions = new Dictionary<string, BLUIElement>();
            _objectHelpers = new Dictionary<string, IBLUIOperationHelper>();

            _navigationManager.TryGetQueryString<long>("ElementKey", out elementKey);// get element key from url 

            if (elementKey > 10)
            {
                var formrequest = new ObjectFormRequest();

                formrequest.MenuKey = elementKey;
                formDefinition = await _navManger.GetMenuUIElement(formrequest);//get ui elements

                if (formDefinition != null)
                {
                    HookInteractions();

                    this.BreakComponent();
                    formDefinition.IsDebugMode = true;
                    TopButtonSection = formDefinition.Children.Where(x => x._internalElementName.Equals("FormButton")).FirstOrDefault();
                    HeaderSection = formDefinition.Children.Where(x => x._internalElementName.Equals("HeaderSection1")).FirstOrDefault();
                    findTransferUI = formDefinition.Children.Where(x => x._internalElementName.Equals("FindPopUp")).FirstOrDefault();
                    ApprovalSection = formDefinition.Children.Where(x => x._internalElementName.Equals("ApprovalSection")).FirstOrDefault();
                }


            }
            InitializeItemtransfer();
        }
        private void InitializeItemtransfer()
        {
            _itmTransfer = new ItemTransfer();
            _afterSaveHeaderResponse = new ItemTransfer();
            _oneLineItm = new ItemTransferLineItem();
            trnOpReq = new TransferOpenRequest();
            _find_itm_trnsfer = new ItemTransfer();

            total_scanned_session = 0;
            validator = new ItemTransferValidator(_itmTransfer);

            _itmTransfer = new ItemTransfer
            {
                ElementKey = elementKey,
                FromLocation = new CodeBaseResponse { CodeKey = 1, CodeName = "" },
                ToLocation = new CodeBaseResponse { CodeKey = 1, CodeName = "" },
            };

            TrnNoText = "New";
        }
        private void HookInteractions()
        {
            InteractionHelper helper = new InteractionHelper(this, formDefinition);//formdefinition has all form objects 
            _interactionLogic = helper.GenerateEventCallbacks();// generate all event callbacks 
            appStateService._AppBarName = "Scan Item Transfer";
        }
        private void BreakComponent()
        {
            if (formDefinition != null)
            {
                var childsHash = formDefinition.Children.ToLookup(elem => elem.ParentKey);
                foreach (var child in formDefinition.Children)
                {
                    child.Children = childsHash[child.ElementKey].ToList();
                }
                BLUIElement form = formDefinition.Children.Where(x => x.ElementKey == formDefinition.ElementKey).FirstOrDefault();
                if (form != null)
                {
                    formDefinition = form;

                }
            }
        }
        private void UIStateChanged()
        {
            this.StateHasChanged();
        }

        #region ui events
        private void OnEditClick(UIInterectionArgs<object> args)
        {
            UIStateChanged();
        }
        private async void OnNewClick(UIInterectionArgs<object> args)
        {
            InitializeItemtransfer();
            validator.UserMessages.AlertMessages.Clear();
            await SetValue("TrnNo", "");
            _objectHelpers["ScanBox"].ResetToInitialValue();
            this.ToggleEditability("FromLoc", true);
            IsSaveButtonDisbled = true;
            this.ToggleEditability("ButtonSave", IsSaveButtonDisbled);
            ToggleEditability("Find", true);
            ToggleEditability("Print", true);
            ToggleEditability("Cancel", true);
            this.ToggleEditability("ToLoc", true);
            _objectHelpers["TotalScan"].ResetToInitialValue();
            this.total_scanned_session = 0;
            UIStateChanged();
        }
        private async void OnClickSave(UIInterectionArgs<object> args)
        {
            validator.UserMessages.AlertMessages.Clear();


            total_scanned_session = 0;
            UIStateChanged();

            if (_itmTransfer != null && _itmTransfer.ScanItemTransferLineItem.Count() > 0)
            {
                _itmTransfer.SerialNoList.Clear();
                _itmTransfer.ElementKey = elementKey;
                foreach (var itm in _itmTransfer.ScanItemTransferLineItem.ToList())
                {
                    if (itm.IsJustScanned)
                    {
                        _itmTransfer.ScanItemTransferLineItemNew.Add(itm);
                    }
                    else
                    {
                        _itmTransfer.ScanItemTransferLineItemUpdated.Add(itm);
                    }
                }

                _itmTransfer.LocationWiseSerialNoValidations = await _itemTransferManager.TransferValidator(_itmTransfer);
                validator = new ItemTransferValidator(_itmTransfer);

                if (_itmTransfer.FromTransactionKey == 1)
                {
                    if (validator != null && validator.CanSaveOrUpdateItemTransfer())
                    {
                        this.appStateService.IsLoaded = true;
                        TrnkyForOut = await _itemTransferManager.CreateItemTransfer(_itmTransfer);
                        this.appStateService.IsLoaded = false;
                        //await saveTrnHeaderFrom();
                        //await saveTrnHeaderTo();
                        //await ItmSave();

                        if (!_itemTransferManager.IsExceptionthrown())
                        {
                            _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                            _snackBar.Add("Item Transfer has been  Saved Successfully", Severity.Success);
                            trnOpReq.TrnKy = TrnkyForOut;
                            await LoadTransfer(trnOpReq);
                        }
                        else
                        {
                            _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                            _snackBar.Add("Error Occured", Severity.Error);
                        }


                    }
                    else
                    {
                        IsSerialNoValidationShown = true;
                        UIStateChanged();
                    }

                }
                else
                {

                    //ItemTransfer ItemTransferWithNewlyAddedItem = new ItemTransfer();

                    //foreach (var itm in _itmTransfer.ScanItemTransferLineItem.ToList())
                    //{
                    //    if (itm.IsJustScanned)
                    //    {
                    //        ItemTransferWithNewlyAddedItem.ScanItemTransferLineItem.Add(itm);
                    //    }
                    //}

                    //ItemTransferWithNewlyAddedItem.ElementKey = elementKey;
                    //ItemTransferWithNewlyAddedItem.FromLocation = _itmTransfer.FromLocation;

                    //ItemTransferWithNewlyAddedItem.LocationWiseSerialNoValidations = await _itemTransferManager.TransferValidator(ItemTransferWithNewlyAddedItem);

                    //if (ItemTransferWithNewlyAddedItem != null && ItemTransferWithNewlyAddedItem.ScanItemTransferLineItem.Count() > 0)
                    //{
                    //    validator = new ItemTransferValidator(ItemTransferWithNewlyAddedItem);
                    //}
                    //else
                    //{
                    //    validator = new ItemTransferValidator(new ItemTransfer());
                    //}


                    if (validator != null && validator.CanSaveOrUpdateItemTransfer())
                    {
                        await EditItmTransfer();

                    }
                    else
                    {
                        IsSerialNoValidationShown = true;
                        UIStateChanged();
                    }


                }

                //AppSettings.Loading(false);

                UIStateChanged();
            }
            else
            {
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("Please Select at least one item", Severity.Error);
            }
        }
        private async void GotoApprovalMode()
        {
            IsApprovalUIShown = !IsApprovalUIShown;

            ToggleEditability("ButtonSave", IsSaveButtonDisbled && !IsApprovalUIShown);
            ToggleEditability("ButtonSearch", !IsApprovalUIShown);
            ToggleEditability("ButtonPrint", !IsApprovalUIShown);
            ToggleEditability("ButtonCancel", !IsApprovalUIShown);
            UIStateChanged();
        }
        private async void OnPrintClick(UIInterectionArgs<object> args)
        {
            if (_itmTransfer.FromTransactionKey > 1)
            {
                auth = await _authenticationManager.GetUserInformation();
                _itmTransferReportOptions.ReportParameters = new Dictionary<string, object>();

                if (_itmTransferReportOptions != null && _itmTransferReportOptions.ReportParameters != null)
                {
                    _itmTransferReportOptions.ReportParameters.Clear();

                    _itmTransferReportOptions.ReportParameters.Add("CKy", auth.AuthenticatedCompany.CompanyKey);
                    _itmTransferReportOptions.ReportParameters.Add("UsrKy", auth.AuthenticatedUser.UserKey);
                    _itmTransferReportOptions.ReportParameters.Add("TrnKy", _itmTransfer.FromTransactionKey);


                    _itmTransferReportOptions.ReportName = "DeliveryOrder_LND.trdp";
                    ReportShown = true;
                }
            }
            else
            {
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("Error!.", Severity.Error);
            }
            StateHasChanged();

        }
        private async void OnSearchClick(UIInterectionArgs<object> args)
        {
            //findTransferUI = args.InitiatorObject;
            await ShowFindTransferWindow();
            UIStateChanged();
        }
        private async void OnCancelClick(UIInterectionArgs<object> args)
        {

            InitializeItemtransfer();
            validator.UserMessages.AlertMessages.Clear();
            await SetValue("TrnNo", "");
            _objectHelpers["ScanBox"].ResetToInitialValue();
            this.ToggleEditability("FromLoc", true);
            IsSaveButtonDisbled = true;
            this.ToggleEditability("ButtonSave", IsSaveButtonDisbled);
            ToggleEditability("Find", true);
            ToggleEditability("Print", true);
            ToggleEditability("Cancel", true);
            this.ToggleEditability("ToLoc", true);
            _objectHelpers["TotalScan"].ResetToInitialValue();
            this.total_scanned_session = 0;
            await SetValue("Remark", string.Empty);
            UIStateChanged();
        }
        private void OnTrNoChange(UIInterectionArgs<string> args)
        {
            _itmTransfer.TransactionNumber = args.DataObject;
            UIStateChanged();
        }
        private void OnDateChange(UIInterectionArgs<DateTime?> args)
        {
            _itmTransfer.TransactionDate = (DateTime)args.DataObject;

            UIStateChanged();
        }
        private void OnFromLocChange(UIInterectionArgs<CodeBaseResponse> args)
        {
            _itmTransfer.FromLocation = args.DataObject;

            UIStateChanged();
        }
        private void OnToLocChange(UIInterectionArgs<CodeBaseResponse> args)
        {
            _itmTransfer.ToLocation = args.DataObject;

            UIStateChanged();
        }
        private void OnToAdvnlChange(UIInterectionArgs<AddressResponse> args)
        {
            _itmTransfer.Address = args.DataObject;

            UIStateChanged();
        }
        private async void OnSerialNoScan(UIInterectionArgs<object> args)
        {
            if (!string.IsNullOrEmpty(_serialNo)) { await Scan(); }
            _serialNo = "";
            UIStateChanged();

        }
        private async void OnAdornmentClick_ScanBox(UIInterectionArgs<object> args)
        {
            await Scan();
            _serialNo = "";
            UIStateChanged();
        }
        private async void OnTextBoxItmTransferChange(UIInterectionArgs<string> args)
        {

            this.UIStateChanged();
        }
        private async void OnSerialNoScanByType(UIInterectionArgs<string> args)
        {
            _serialNo = args.DataObject;
            UIStateChanged();
        }
        private async void OnHdrNumericBoxChanged(UIInterectionArgs<decimal> args)
        {
            StateHasChanged();
        }
        #endregion

        #region scan

        private async Task Scan()
        {
            validator.UserMessages.AlertMessages.Clear();

            validator = new ItemTransferValidator(_itmTransfer);

            if (validator.CanOpenScan())
            {
                if (string.IsNullOrEmpty(_serialNo))
                {
                    var result = await _barcodeService.ReadBarcode();
                    if (result != null)
                    {
                        string res = result.ToString();
                        if (!string.IsNullOrEmpty(res))
                        {
                            _serialNo = res;
                            await LoadGrid(_serialNo);
                        }
                    }

                }
                else
                {
                    await LoadGrid(_serialNo);
                }

            }
            else
            {
                IsSerialNoValidationShown = true;
            }
            _objectHelpers["ScanBox"].ResetToInitialValue();
            UIStateChanged();
        }

        private async Task LoadGrid(string serial_num)
        {
            if (!_itmTransfer.SerialNoList.Contains(serial_num))
            {

                await GetGridData(serial_num);

                UIStateChanged();
            }
            else
            {
                ShowScannedValidations("This Item is already scanned");
                UIStateChanged();
            }
        }
        private async Task ShowFindTransferWindow()
        {
            HideAllPopups();
            FindTransferShown = true;
            UIStateChanged();
            await Task.CompletedTask;
        }

        private async Task GetGridData(string serNo)
        {

            try
            {
                this.appStateService.IsLoaded = true;
                _oneLineItm = new();
                _oneLineItm.ObjectKey = (int)elementKey;
                _oneLineItm.serialNo = serNo;

                if (_itmTransfer.FromLocation != null) { _oneLineItm.LocationKey = _itmTransfer.FromLocation.CodeKey; }

                _oneLineItm = await _itemTransferManager.GetItemsData(_oneLineItm);
                _oneLineItm.isActive = 1;
                _oneLineItm.IsJustScanned = true;
                if (_oneLineItm.LocationWiseitemSerialNoValidation != null && _oneLineItm.ItemKey > 1)
                {
                    if (!_oneLineItm.LocationWiseitemSerialNoValidation.HasError)
                    {
                        _itmTransfer.SerialNoList.Add(serNo);
                        total_scanned_session++;

                        await SetValue("ScanBox", serNo);
                        await SetValue("TotalScan", total_scanned_session);

                        _itmTransfer.ScanItemTransferLineItem.Add(_oneLineItm);
                        this.appStateService.IsLoaded = false;
                    }
                    else
                    {
                        this.appStateService.IsLoaded = false;
                        ShowScannedValidations(_oneLineItm.LocationWiseitemSerialNoValidation.Message);
                    }

                }
                else
                {
                    this.appStateService.IsLoaded = false;
                    ShowScannedValidations("There is no item with this serial number");
                }

                UIStateChanged();
            }
            catch (Exception e)
            {
                this.appStateService.IsLoaded = false;
                ShowScannedValidations("Some Error Occured,Please retry");
                UIStateChanged();
            }


        }

        private async void ShowScannedValidations(string msg)
        {
            _itmTransfer.ValidationMessages.Add(msg);

            validator = new ItemTransferValidator(_itmTransfer);
            validator.AddValidationErrors();
            IsSerialNoValidationShown = true;
        }

        private async void DeleteRow(int index, string serNo)
        {
            var parameters = new DialogParameters
            {
                ["ContentText"] = "Are you sure to delete this item?"
            };
            DialogOptions options = new DialogOptions();
            var dialog = _dialogService.Show<DeleteConfirmation>("Confirm", parameters, options);
            DialogResult dialogResult = await dialog.Result;

            if (dialogResult!=null && !dialogResult.Canceled)
            {

                if (_itmTransfer.ScanItemTransferLineItem[index].IsJustScanned)
                {
                    _itmTransfer.ScanItemTransferLineItem.RemoveAt(index);
                    _itmTransfer.SerialNoList.Remove(serNo);
                }
                else
                {
                    _itmTransfer.ScanItemTransferLineItem[index].isActive = 0;
                }

                if (total_scanned_session > 0)
                    total_scanned_session--;
                total_scanned_session = 0;


                await SetValue("TotalScan", total_scanned_session);
            }



            UIStateChanged();
        }
        private void GridClear()
        {
            _itmTransfer.ScanItemTransferLineItem.Clear();
        }
        private void Closed(string chipToRemove)
        {
            _itmTransfer.SerialNoList.Remove(chipToRemove);
        }
        private async Task CloseFindTransferWindow()
        {
            HideAllPopups();

            await Task.CompletedTask;

        }
        private void HideAllPopups()
        {
            FindTransferShown = false;
            IsSerialNoValidationShown = false;
            UIStateChanged();
        }
        private async void OnOpenTransferClick(TransferOpenRequest request)
        {
            HideAllPopups();
            InitializeItemtransfer();
            this.appStateService.IsLoaded = true;
            await LoadTransfer(request);
            this.appStateService.IsLoaded = false;
            UIStateChanged();
        }

        private async Task LoadTransfer(TransferOpenRequest request)
        {
            IsValidTransfer = false;
            request.ObjKy = elementKey;
            _find_itm_trnsfer = await _itemTransferManager.RefreshForm(request);

            if (_find_itm_trnsfer != null)
            {
                _itmTransfer = new ItemTransfer();
                _itmTransfer.CopyFrom(_find_itm_trnsfer);

                if (_itmTransfer.SerialNoList == null) { _itmTransfer.SerialNoList = new List<string>(); }

                if (_itmTransfer != null)
                {
                    await SetValue("TrnNo", _find_itm_trnsfer.TransactionNumber);
                    await SetValue("Remark", _find_itm_trnsfer.Remark);
                    TrnNoText = _find_itm_trnsfer.TransactionNumber;
                    //TrnkyForOut = _find_itm_trnsfer.FromTransactionKey;

                    UIStateChanged();
                    IsbukeySelected = false;
                    if (_find_itm_trnsfer.ScanItemTransferLineItem != null && _find_itm_trnsfer.ScanItemTransferLineItem.Count > 0)
                    {
                        foreach (var itm in _find_itm_trnsfer.ScanItemTransferLineItem)
                        {
                            if (_itmTransfer != null && itm.isActive == 1)
                            {
                                if (!IsbukeySelected)
                                {
                                    _itmTransfer.FromBuKy = itm.FromBuKy;
                                    _itmTransfer.ToBuKy = itm.ToBuKy;
                                    IsbukeySelected = true;
                                }
                                if (!string.IsNullOrEmpty(itm.serialNo))
                                {
                                    _itmTransfer.SerialNoList.Add(itm.serialNo.ToString());
                                }

                            }

                        }
                    }

                    ItmtrnsferValidationResponse validateResponse = new ItmtrnsferValidationResponse();
                    _itmTransfer.ElementKey = elementKey;
                    validateResponse = await _itemTransferManager.TransferMultiAprLock(_itmTransfer);
                    IsValidTransfer = !validateResponse.HasError;

                    if (!IsValidTransfer)
                    {
                        IsSaveButtonDisbled = false;
                        ToggleEditability("ButtonSave", IsSaveButtonDisbled);

                        _itmTransfer.ValidationMessages.Clear();
                        _itmTransfer.ValidationMessages.Add(validateResponse.Message);

                        validator = new ItemTransferValidator(_itmTransfer);
                        validator.AddValidationErrors();
                        IsSerialNoValidationShown = true;
                    }
                }

                this.ToggleEditability("FromLoc", false);

                if (_itmTransfer != null && !_itmTransfer.CanUpdateToLocation)
                {
                    IsSaveButtonDisbled = false;
                    this.ToggleEditability("ButtonSave", IsSaveButtonDisbled);
                    this.ToggleEditability("ToLoc", false);
                }

                UIStateChanged();
            }
        }

        private async Task EditItmTransfer()
        {
            this.appStateService.IsLoaded = true;
            _itmTransfer.FromTransactionKey = _find_itm_trnsfer.FromTransactionKey;
            _itmTransfer.ToTransactionKey = _find_itm_trnsfer.ToTransactionKey;
            _itmTransfer.ElementKey = elementKey;
            _itmTransfer.FromTmStmp = _find_itm_trnsfer.FromTmStmp;
            _itmTransfer.ToTmStmp = _find_itm_trnsfer.ToTmStmp;

            await _itemTransferManager.UpdateItemTransfer(_itmTransfer);

            if (!_itemTransferManager.IsExceptionthrown())
            {
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("Item Transfer has been  updated Successfully", Severity.Success);
                trnOpReq.TrnKy = _itmTransfer.FromTransactionKey;
                await LoadTransfer(trnOpReq);
                this.appStateService.IsLoaded = false;
            }
            else
            {
                this.appStateService.IsLoaded = false;
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("Error Occured", Severity.Error);
            }


        }

        private bool CheckIsctiveZero()
        {
            foreach (var itm in _itmTransfer.ScanItemTransferLineItem)
            {
                if (itm.isActive == 0)
                {
                    isActiveZeroCount++;
                }
            }

            return isActiveZeroCount == 0;
        }
        #endregion

        #region object helpers
        private void ToggleEditability(string name, bool editable)
        {
            IBLUIOperationHelper helper;

            if (_objectHelpers.TryGetValue(name, out helper))
            {
                helper.ToggleEditable(editable);
                UIStateChanged();
            }
        }
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

        #endregion


    }
}
