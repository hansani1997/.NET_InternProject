using bluelotus360.com.razorComponents.Extensions;
using bluelotus360.com.razorComponents.MB.Shared.Components;
using bluelotus360.com.razorComponents.Pages.SetUp.Master_Data.Account_Profile.Components;
using bluelotus360.Com.commonLib.Helpers;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain.Entities.AccountProfile;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using Microsoft.AspNetCore.Components;
using MudBlazor.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.razorComponents.Pages.SetUp.Master_Data.Account_Profile
{
    public partial class AccountProfile
    {
        #region parameter

        private BLUIElement formDefinition, gridUIElement;
        private BLTransaction transaction = new();

        private AccountProfileRequest accountProfile;

        private IDictionary<string, EventCallback> _interactionLogic;
        private IDictionary<string, BLUIElement> _modalDefinitions;
        private IDictionary<string, BLUIElement> _activeModalDefinitions;
        BLUIElement modalUIElement, insertmodalUIElement;
        private IDictionary<string, IBLUIOperationHelper> _objectHelpers;

        private UIBuilder _refBuilder;
        private BLAccountProfileHeader _refBLAccountProfileHeader;
        private BLAccountProfileDetails _refBLAccountProfileDetails;
        private BLTable<AccountProfileResponse> _blTb;

        private IList<AccountProfileResponse> gridDetails;
        private AccountProfileResponse accountProfileResponse;
        private AccountProfileResponse updatedDetails;

        private AccountProfileInsertRequest newItem;

        private bool Showtable = true;
        private bool ShowRecordItems = false;
        private bool ShowInsertDetails = false;
        private bool isTableLoading = false;

        long elementKey;

        string searchterm = string.Empty;

        #endregion


        #region General

        protected override void OnParametersSet()
        {

            base.OnParametersSet();
        }

        protected override async Task OnInitializedAsync()
        {
            elementKey = 1;

            _navigationManager.TryGetQueryString<long>("ElementKey", out elementKey);// get element key from url

            if (elementKey > 10)
            {
                var formrequest = new ObjectFormRequest();
                formrequest.MenuKey = elementKey;

                formDefinition = await _navManger.GetMenuUIElement(formrequest);//get ui elements
                modalUIElement = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("SecondSection")).FirstOrDefault();
                insertmodalUIElement = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("InsertSection")).FirstOrDefault();
                gridUIElement = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("DetailsTable")).FirstOrDefault();

                if (modalUIElement != null)
                {
                    modalUIElement.Children = formDefinition.Children.Where(x => x.ParentKey == modalUIElement.ElementKey).ToList();
                }

                if (insertmodalUIElement != null)
                {
                    insertmodalUIElement.Children = formDefinition.Children.Where(x => x.ParentKey == insertmodalUIElement.ElementKey).ToList();
                }
                if (gridUIElement != null)
                {
                    gridUIElement.Children = formDefinition.Children.Where(x => x.ParentKey == gridUIElement.ElementKey).ToList();
                }

                formDefinition.IsDebugMode = false;
            }
            
             _interactionLogic = new Dictionary<string, EventCallback>();
            _modalDefinitions = new Dictionary<string, BLUIElement>();
            _objectHelpers = new Dictionary<string, IBLUIOperationHelper>();
            _activeModalDefinitions = new Dictionary<string, BLUIElement>();

            
            HookInteractions();
            RefreshGrid();

            accountProfile.ElementKey = elementKey;
            gridDetails = await _profileManager.GetAccountProfileList(accountProfile);

        }

        private void RefreshGrid()
        {
            accountProfile = new AccountProfileRequest();
            newItem = new AccountProfileInsertRequest();
            accountProfileResponse = new AccountProfileResponse();
            updatedDetails = new AccountProfileResponse();

        }
        private void HookInteractions()
        {

            InteractionHelper helper = new InteractionHelper(this, formDefinition);//formdefinition has all form objects 
            _interactionLogic = helper.GenerateEventCallbacks();// generate all event callbacks
            //AppSettings.RefreshTopBar("Account Profile Mobile");
            appStateService._AppBarName = "Account Profile";
        }

        private void UIStateChanged()
        {

            this.StateHasChanged();
        }

        #endregion

        #region ui events

        //private async void OnIsActive(UIInterectionArgs<bool> args)
        //{
        //    updatedDetails.IsActive = args.DataObject;
        //    UIStateChanged();
        //}

        private async void OnCodeUpdated(UIInterectionArgs<string> args)
        {

            updatedDetails.AccountCode = args.DataObject;

            UIStateChanged();
        }

        private async void OnNameUpdated(UIInterectionArgs<string> args)
        {

            updatedDetails.AccountName = args.DataObject;
            UIStateChanged();
        }

        private async void OnAccountTypeUpdated(UIInterectionArgs<CodeBaseResponse> args)
        {
            updatedDetails.AccountType = args.DataObject;
            UIStateChanged();
        }

        private async void OnBackButton(UIInterectionArgs<object> args)
        {
            Showtable = true;
            ShowRecordItems = false;

            RefreshGrid();
            UIStateChanged();
        }

        private async void OnSaveUpdated(UIInterectionArgs<object> args)
        {
            this.appStateService.IsLoaded = true;
            UIStateChanged();

            await _profileManager.UpdatedAccountProfile(updatedDetails);

            gridDetails = await _profileManager.GetAccountProfileList(accountProfile);

            isTableLoading = false;

            ShowInsertDetails = false;
            Showtable = true;
            ShowRecordItems = false;

            this.appStateService.IsLoaded = false;
            UIStateChanged();

        }

        private async void OnInsertBackButton(UIInterectionArgs<object> args)
        {
            ShowInsertDetails = false;
            Showtable = true;
            ShowRecordItems = false;

            RefreshGrid();
            UIStateChanged();
        }

        private async void OnInsertSaveButton(UIInterectionArgs<object> args)
        {
            this.appStateService.IsLoaded = true;
            UIStateChanged();

            await _profileManager.InsertAccountProfile(newItem);

            ShowInsertDetails = false;
            Showtable = true;
            ShowRecordItems = false;

            gridDetails = await _profileManager.GetAccountProfileList(accountProfile);

            this.appStateService.IsLoaded = false;
            UIStateChanged();
        }

        private async void OnCodeInsert(UIInterectionArgs<string> args)
        {
            newItem.AccountCode = args.DataObject;
            UIStateChanged();
        }
        private async void OnNameInsert(UIInterectionArgs<string> args)
        {
            newItem.AccountName = args.DataObject;
            UIStateChanged();
        }

        private async void OnAccountTypeInsert(UIInterectionArgs<CodeBaseResponse> args)
        {
            newItem.AccountType = args.DataObject;
            UIStateChanged();
        }

        private async void OnCreateItem(UIInterectionArgs<object> args)
        {
            ShowInsertDetails = true;
            Showtable = false;
            ShowRecordItems = false;

            UIStateChanged();
        }

        #endregion

        #region UI Interaction Logics
        private void OnInsertIsActive(UIInterectionArgs<bool> args)
        {
            newItem.IsActive = args.DataObject;
            UIStateChanged();
        }

        private void OnCodeItem(UIInterectionArgs<string> args)
        {
            newItem.AccountCode = args.DataObject;
            UIStateChanged();
        }

        private void OnNameItem(UIInterectionArgs<string> args)
        {
            newItem.AccountName = args.DataObject;
            UIStateChanged();
        }

        private void OnAccount(UIInterectionArgs<CodeBaseResponse> args)
        {
            newItem.AccountType = args.DataObject;
            UIStateChanged();
        }
        #endregion

        #region Grid Loading
        private async void ShowRecord(UIInterectionArgs<object> args)
        {

            Showtable = false;
            ShowRecordItems = true;

            accountProfileResponse = (AccountProfileResponse)args.DataObject;

            updatedDetails.AccountKey = accountProfileResponse.AccountKey;
            updatedDetails.AccountType.CodeKey = accountProfileResponse.AccountType.CodeKey;
            updatedDetails.AccountCode = accountProfileResponse.AccountCode;
            updatedDetails.AccountName = accountProfileResponse.AccountName;
            updatedDetails.IsActive = accountProfileResponse.IsActive;

            await SetValue("Code", accountProfileResponse.AccountCode);
            await SetValue("Name", accountProfileResponse.AccountName);
            await SetValue("AccountType", accountProfileResponse.AccountType.CodeName);
            await SetValue("IsAct", accountProfileResponse.IsActive);

            UIStateChanged();
        }

        private bool FilterCycleCheck(AccountProfileResponse searchItem)
        {

            if (searchItem.AccountCode.Contains(searchterm) || searchItem.AccountName.Contains(searchterm) || searchItem.AccountType.CodeName.Contains(searchterm))
            {
                return true;
            }

            return false;
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
