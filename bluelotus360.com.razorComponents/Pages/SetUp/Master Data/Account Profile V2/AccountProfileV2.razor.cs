using BL10.CleanArchitecture.Domain.DTO;
using BL10.CleanArchitecture.Domain.Entities.ItemProfleMobile;
using bluelotus360.com.razorComponents.Extensions;
using bluelotus360.com.razorComponents.MB.Shared.Components;
using bluelotus360.com.razorComponents.Pages.SetUp.Master_Data.Account_Profile;
using bluelotus360.com.razorComponents.StateManagement;
using bluelotus360.Com.commonLib.Helpers;
using bluelotus360.Com.commonLib.Routes;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain.Entities.AccountProfile;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace bluelotus360.com.razorComponents.Pages.SetUp.Master_Data.Account_Profile_V2
{
    public partial class AccountProfileV2
    {
        #region parameter

        private BLUIElement formDefinition, mainGridSection,topButtonGroup;
        //private BLTransaction transaction = new();

        //private AccountProfileRequest accountProfile;

        private IDictionary<string, EventCallback> _interactionLogic;
        private IDictionary<string, IBLUIOperationHelper> _objectHelpers;
        private IDictionary<string, BLUIElement> _modalDefinitions;
        private IDictionary<string, BLUIElement> _activeModalDefinitions;

        private UIBuilder _refBuilder;

        private IList<AccountProfileResponse> othergridDetails = new List<AccountProfileResponse>();
        private IList<AccountProfileResponse> adlAvlgridDetails = new List<AccountProfileResponse>();
        private IList<AccountProfileResponse> maingridDetails = new List<AccountProfileResponse>();


        private BLTable<AccountProfileResponse> _blOtherTb = new BLTable<AccountProfileResponse>();
        private BLTable<AccountProfileResponse> _blAdlAvlTb = new BLTable<AccountProfileResponse>();
        private BLTable<AccountProfileResponse> _blMainTb = new BLTable<AccountProfileResponse>();

        //private IList<AccountProfileResponse> gridDetails;
        private BaseServerFilterInfo AccountProfileGridListV2 = new BaseServerFilterInfo();

       
        //flags
        double ScreenWidth;
        private bool showstab = false;
        private bool Showtable = true;
        private int _mainGridCount = 0;

        long elementKey;

        BLBreakpoint breakpointMargin = new BLBreakpoint();
        BLUIElement tabSection, otherGrid,adlAvlGrid;
        WindowSize ws = new WindowSize();


        //insert section of Account Details
        private AccountProfileResponse accountDetailsTabInsertRequest, selectSignleAccountRequest, singleAccountData;
        //private AccountProfileResponse contactDetailsTabInsertRequest = new AccountProfileResponse();
        //private bool ShowTabsInsertDetails = false;


        #endregion


        #region General

        protected override async void OnParametersSet()
        {

            base.OnParametersSet();

            
            ws = await JS.InvokeAsync<WindowSize>("getWindowSize");

            if (ws.Width < 600)
            {
                breakpointMargin.BreakpointMargin = MudBlazor.Breakpoint.Xs;
            }
            else if (600 <= ws.Width && ws.Width < 960)
            {
                breakpointMargin.BreakpointMargin = MudBlazor.Breakpoint.Sm;
            }
            else if (960 <= ws.Width && ws.Width < 1280)
            {
                breakpointMargin.BreakpointMargin = MudBlazor.Breakpoint.Md;
            }
            else if (1280 <= ws.Width && ws.Width < 1920)
            {
                breakpointMargin.BreakpointMargin = MudBlazor.Breakpoint.Lg;
            }
            else if (1920 <= ws.Width && ws.Width < 2560)
            {
                breakpointMargin.BreakpointMargin = MudBlazor.Breakpoint.Xl;
            }

            StateHasChanged();
        }

        protected override async Task OnInitializedAsync()
        {
            _interactionLogic = new Dictionary<string, EventCallback>();
            _modalDefinitions = new Dictionary<string, BLUIElement>();
            _objectHelpers = new Dictionary<string, IBLUIOperationHelper>();
            _activeModalDefinitions = new Dictionary<string, BLUIElement>();

            elementKey = 1;

            _navigationManager.TryGetQueryString<long>("ElementKey", out elementKey);// get element key from url

            if (elementKey > 10)
            {
                var formrequest = new ObjectFormRequest();
                formrequest.MenuKey = elementKey;

                formDefinition = await _navManger.GetMenuUIElement(formrequest);//get ui elements
                HookInteractions();
                this.BreakComponent();
                formDefinition.IsDebugMode = false;
                BLUIElement tab= formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("TabSection")).FirstOrDefault();
                tabSection = tab.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("TabSection_Tab")).FirstOrDefault();
                otherGrid = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("OtherDetails_GridSection")).FirstOrDefault();
                adlAvlGrid = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("AdnAvlMultiSelect_GridSection")).FirstOrDefault();
                mainGridSection = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("MainGridSection")).FirstOrDefault();
                topButtonGroup= formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("ButtonSection")).FirstOrDefault();

            }
            
            RefreshGrid();


            //AccountProfileGridListV2.ObjKy = (int)elementKey;
            //AccountProfileGridListV2.RequestingURL = BaseEndpoint.BaseURL + mainGridSection.GetPathURL();
            //maingridDetails = await _profileManager.GetAccountProfileMainGridDetails(AccountProfileGridListV2) ?? new List<AccountProfileResponse>();
            //_mainGridCount = maingridDetails.FirstOrDefault() != null ? maingridDetails.FirstOrDefault().Count : 0;
            //RefreshGrid();
            
        }

        #region Initiate Newline for display inserted details

        private async void InitiateNewLine()
        {
            accountDetailsTabInsertRequest = new AccountProfileResponse();
            _objectHelpers["PISection_AccountCode"].ResetToInitialValue();
            _objectHelpers["PISection_AccountName"].ResetToInitialValue();
            _objectHelpers["PISection_AccountType"].ResetToInitialValue();
            _objectHelpers["PISection_Currency"].ResetToInitialValue();
            _objectHelpers["AccountDetails_ParentAccount"].ResetToInitialValue();
            _objectHelpers["AccountDetails_BusinessUnit"].ResetToInitialValue();
            _objectHelpers["AccountDetails_AccessLevel"].ResetToInitialValue();
            _objectHelpers["AccountDetails_ItemCategory2"].ResetToInitialValue();
            _objectHelpers["AccountDetails_AccountCategory1"].ResetToInitialValue();
            _objectHelpers["AccountDetails_AccountCategory2"].ResetToInitialValue();
            _objectHelpers["AccountDetails_AccountCategory3"].ResetToInitialValue();
            _objectHelpers["AccountDetails_AccountCategory4"].ResetToInitialValue();
            _objectHelpers["AccountDetails_AccountCategory5"].ResetToInitialValue();
            _objectHelpers["AccountDetails_AccountCategory6"].ResetToInitialValue();
            _objectHelpers["AccountProfile_Alias"].ResetToInitialValue();
            _objectHelpers["AccountDetails_IsAct"].ResetToInitialValue();
            _objectHelpers["AccountDetails_IsApp"].ResetToInitialValue();
            _objectHelpers["AccountDetails_IsParentAcc"].ResetToInitialValue();
            _objectHelpers["AccountDetails_IsBudget"].ResetToInitialValue();
            _objectHelpers["AccountDetails_IsCredit"].ResetToInitialValue();
            _objectHelpers["AccountDetails_IsAllTrn"].ResetToInitialValue();
            _objectHelpers["AccountDetails_IsControlAcc"].ResetToInitialValue();

            StateHasChanged();
        }
        #endregion

        private void RefreshGrid()
        {

            AccountProfileGridListV2 = new BaseServerFilterInfo();
            accountDetailsTabInsertRequest = new AccountProfileResponse();
            selectSignleAccountRequest = new AccountProfileResponse();
            singleAccountData = new AccountProfileResponse();
        }

        private void HookInteractions()
        {

            InteractionHelper helper = new InteractionHelper(this, formDefinition);//formdefinition has all form objects 
            _interactionLogic = helper.GenerateEventCallbacks();// generate all event callbacks
            appStateService._AppBarName = "Account Profile";
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

        #endregion

        #region Header Button Section

        //Add new Account Item
        private async void OnAddAccount(UIInterectionArgs<object> args)
        {
            showstab = true;
            Showtable = false;
            
            UIStateChanged();

        }

        private async void OnBackAction(UIInterectionArgs<object> args)
        {
            Showtable = true;
            showstab = false;

            RefreshGrid();
            UIStateChanged();
        }
        #endregion


        #region Main gird details Update Section

        private async void ShowRecord(UIInterectionArgs<object> args)
        {
            //select single account
            selectSignleAccountRequest = (AccountProfileResponse)args.DataObject;
            selectSignleAccountRequest.IsInEditMode= true;
            accountDetailsTabInsertRequest.CopyFrom(selectSignleAccountRequest);

            singleAccountData = await _profileManager.SelectSignleAccountRecord(accountDetailsTabInsertRequest);

            Showtable = false;
            showstab= true;

            //Account Details tab
            await SetValue("PISection_AccountCode", selectSignleAccountRequest.AccountCode);
            await SetValue("PISection_AccountName", selectSignleAccountRequest.AccountName);
            await SetValue("PISection_AccountType", selectSignleAccountRequest.AccountType.CodeName);
            await SetValue("PISection_Currency", selectSignleAccountRequest.Currency.CodeName);
            await SetValue("AccountDetails_BusinessUnit", selectSignleAccountRequest.BusinessUnit.CodeName);
            await SetValue("AccountDetails_ParentAccount", selectSignleAccountRequest.ParentAccount.AccountName);
            await SetValue("AccountDetails_AccessLevel", selectSignleAccountRequest.AccessLevel.CodeName);
            await SetValue("AccountDetails_ItemCategory2", selectSignleAccountRequest.ItemCategory2.CodeName);
            await SetValue("AccountDetails_AccountCategory1", selectSignleAccountRequest.AccountCategory1.CodeName);
            await SetValue("AccountDetails_AccountCategory2", selectSignleAccountRequest.AccountCategory2.CodeName);
            await SetValue("AccountDetails_AccountCategory3", selectSignleAccountRequest.AccountCategory3.CodeName);
            await SetValue("AccountDetails_AccountCategory4", selectSignleAccountRequest.AccountCategory4.CodeName);
            await SetValue("AccountDetails_AccountCategory5", selectSignleAccountRequest.AccountCategory5.CodeName);
            await SetValue("AccountDetails_AccountCategory6", selectSignleAccountRequest.AccountCategory6.CodeName);
            await SetValue("AccountProfile_Alias", selectSignleAccountRequest.Alias);
            await SetValue("AccountDetails_IsAct", selectSignleAccountRequest.isProfileActive);
            await SetValue("AccountDetails_IsApp", selectSignleAccountRequest.isApprove);
            await SetValue("AccountDetails_IsParentAcc", selectSignleAccountRequest.IsParentAccount);
            await SetValue("AccountDetails_IsBudget", selectSignleAccountRequest.IsBudget);
            await SetValue("AccountDetails_IsCredit", selectSignleAccountRequest.IsCredit);
            await SetValue("AccountDetails_IsAllTrn", selectSignleAccountRequest.IsAllowedFroTransaction);
            await SetValue("AccountDetails_IsControlAcc", selectSignleAccountRequest.IsControlAccount);

            UIStateChanged();

        }

        #endregion


        #region Account Details Insert
        //Primary information section insert
        private async void OnCodeInsertPISetion(UIInterectionArgs<string> args)
        {
            accountDetailsTabInsertRequest.AccountCode = args.DataObject;
            UIStateChanged();
        }
        private async void OnNameInsertPISetion(UIInterectionArgs<string> args)
        {
            accountDetailsTabInsertRequest.AccountName = args.DataObject;
            UIStateChanged();
        }

        private async void OnAccountTypeInsertPISetion(UIInterectionArgs<CodeBaseResponse> args)
        {
            accountDetailsTabInsertRequest.AccountType = args.DataObject;
            UIStateChanged();
        }

        private async void OnCurrencyInsertPISetion(UIInterectionArgs<CodeBaseResponse> args)
        {
            accountDetailsTabInsertRequest.Currency = args.DataObject;
            UIStateChanged();
        }

        //Secondary information
        private async void OnParentAccountInsertSeSetion(UIInterectionArgs<AccountResponse> args)
        {
            accountDetailsTabInsertRequest.ParentAccount = args.DataObject;
            UIStateChanged();
        }

        private async void OnBusinessUnitInsertSeSetion(UIInterectionArgs<CodeBaseResponse> args)
        {
            accountDetailsTabInsertRequest.BusinessUnit = args.DataObject;
            UIStateChanged();
        }
        private async void OnAccesssLevelInsertSeSetion(UIInterectionArgs<CodeBaseResponse> args)
        {
            accountDetailsTabInsertRequest.AccessLevel = args.DataObject;
            UIStateChanged();
        }
        private async void OnItemCategory2InsertSeSetion(UIInterectionArgs<CodeBaseResponse> args)
        {
            accountDetailsTabInsertRequest.ItemCategory2 = args.DataObject;
            UIStateChanged();
        }
        private async void OnAccountCategory1InsertSeSetion(UIInterectionArgs<CodeBaseResponse> args)
        {
            accountDetailsTabInsertRequest.AccountCategory1 = args.DataObject;
            UIStateChanged();
        }
        private async void OnAccountCategory2InsertSeSetion(UIInterectionArgs<CodeBaseResponse> args)
        {
            accountDetailsTabInsertRequest.AccountCategory2 = args.DataObject;
            UIStateChanged();
        }
        private async void OnAccountCategory3InsertSeSetion(UIInterectionArgs<CodeBaseResponse> args)
        {
            accountDetailsTabInsertRequest.AccountCategory3 = args.DataObject;
            UIStateChanged();
        }
        private async void OnAccountCategory4InsertSeSetion(UIInterectionArgs<CodeBaseResponse> args)
        {
            accountDetailsTabInsertRequest.AccountCategory4 = args.DataObject;
            UIStateChanged();
        }
        private async void OnAccountCategory5InsertSeSetion(UIInterectionArgs<CodeBaseResponse> args)
        {
            accountDetailsTabInsertRequest.AccountCategory5 = args.DataObject;
            UIStateChanged();
        }
        private async void OnAccountCategory6InsertSeSetion(UIInterectionArgs<CodeBaseResponse> args)
        {
            accountDetailsTabInsertRequest.AccountCategory6 = args.DataObject;
            UIStateChanged();
        }
        private async void OnAliasInsertSeSetion(UIInterectionArgs<string> args)
        {
            accountDetailsTabInsertRequest.Alias = args.DataObject;
            UIStateChanged();
        }

        //Configuration Section
        private async void OnIsActiveInsertConfigSection(UIInterectionArgs<bool> args)
        {
            accountDetailsTabInsertRequest.isProfileActive = args.DataObject;
            UIStateChanged();
        }
        private async void OnIsApproveInsertConfigSection(UIInterectionArgs<bool> args)
        {
            accountDetailsTabInsertRequest.isApprove = args.DataObject;
            UIStateChanged();
        }
        private async void OnIsParentAccountInsertConfigSection(UIInterectionArgs<bool> args)
        {
            accountDetailsTabInsertRequest.IsParentAccount = args.DataObject;
            UIStateChanged();
        }
        private async void OnIsBudgetInsertConfigSection(UIInterectionArgs<bool> args)
        {
            accountDetailsTabInsertRequest.IsBudget = args.DataObject;
            UIStateChanged();
        }
        private async void OnIsCreditInsertConfigSection(UIInterectionArgs<bool> args)
        {
            accountDetailsTabInsertRequest.IsCredit = args.DataObject;
            UIStateChanged();
        }
        private async void OnIsAllTrnInsertConfigSection(UIInterectionArgs<bool> args)
        {
            accountDetailsTabInsertRequest.IsAllowedFroTransaction = args.DataObject;
            UIStateChanged();
        }
        private async void OnIsControlAccountInsertConfigSection(UIInterectionArgs<bool> args)
        {
            accountDetailsTabInsertRequest.IsControlAccount = args.DataObject;
            UIStateChanged();
        }
        private async void OnIsCustomerSupplierConfigSection(UIInterectionArgs<bool> args)
        {
            accountDetailsTabInsertRequest.IsCustomerSupplier = args.DataObject;
            UIStateChanged();
        }
        private async void OnIsDefaultConfigSection(UIInterectionArgs<bool> args)
        {
            accountDetailsTabInsertRequest.IsDefault = args.DataObject;
            UIStateChanged();
        }
        private async void OnIsSystemRecordtConfigSection(UIInterectionArgs<bool> args)
        {
            accountDetailsTabInsertRequest.IsSystemRecord = args.DataObject;
            UIStateChanged();
        }
        private async void OnNotesInsertConfigSection(UIInterectionArgs<decimal> args)
        {
            accountDetailsTabInsertRequest.Notes = args.DataObject;
            UIStateChanged();
        }
        //private async void OnOurAccountCodeInsertConfigSection(UIInterectionArgs<string> args)
        //{
        //    accountDetailsTabInsertRequest.OurAccountCode = args.DataObject;
        //    UIStateChanged();
        //}
        private async void OnIsLmpCostingConfigSection(UIInterectionArgs<bool> args)
        {
            accountDetailsTabInsertRequest.IsImpCosting = args.DataObject;
            UIStateChanged();
        }
        #endregion

        #region Save Data 

        private async void OnSaveAccount(UIInterectionArgs<object> args)
        {
            this.appStateService.IsLoaded = true;

            if (accountDetailsTabInsertRequest.IsInEditMode)
            {
                await _profileManager.UpdateAccountProfileDetails(accountDetailsTabInsertRequest);
                accountDetailsTabInsertRequest.IsInEditMode = false;

                //AccountProfileGridListV2.ObjKy = (int)elementKey;
                //AccountProfileGridListV2.RequestingURL = BaseEndpoint.BaseURL + mainGridSection.GetPathURL();
                //maingridDetails = await _profileManager.GetAccountProfileMainGridDetails(AccountProfileGridListV2) ?? new List<AccountProfileResponse>();
                //_mainGridCount = maingridDetails.FirstOrDefault() != null ? maingridDetails.FirstOrDefault().Count : 0;

                _blMainTb.ServerFilterRefrersh();
                showstab = false;
                Showtable = true;

                RefreshGrid();
            }
            else
            {
                await _profileManager.InsertAccountProfileDetails(accountDetailsTabInsertRequest);

                //AccountProfileGridListV2.ObjKy = (int)elementKey;
                //AccountProfileGridListV2.RequestingURL = BaseEndpoint.BaseURL + mainGridSection.GetPathURL();
                //maingridDetails = await _profileManager.GetAccountProfileMainGridDetails(AccountProfileGridListV2) ?? new List<AccountProfileResponse>();
                //_mainGridCount = maingridDetails.FirstOrDefault() != null ? maingridDetails.FirstOrDefault().Count : 0;


                _blMainTb.ServerFilterRefrersh();
                InitiateNewLine();

                showstab = false;
                Showtable = true;

                RefreshGrid();

            }

            
            this.appStateService.IsLoaded = false;
            UIStateChanged();

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

            private void ToggleViisbility(string name, bool visible)
            {
                IBLUIOperationHelper helper;

                if (_objectHelpers.TryGetValue(name, out helper))
                {
                    helper.UpdateVisibility(visible);
                    UIStateChanged();
                }
            }
            #endregion

    }
        
}
