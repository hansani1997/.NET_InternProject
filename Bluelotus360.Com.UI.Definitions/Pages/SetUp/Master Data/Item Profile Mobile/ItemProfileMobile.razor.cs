using BL10.CleanArchitecture.Domain.Entities.ItemProfleMobile;
using BlueLotus360.CleanArchitecture.Client.Infrastructure.Helpers;
using BlueLotus360.CleanArchitecture.Domain.DTO.MasterData;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using BlueLotus360.Com.UI.Definitions.Extensions;
using BlueLotus360.Com.UI.Definitions.MB.Settings;
using BlueLotus360.Com.UI.Definitions.MB.Shared.Components;
using BlueLotus360.Com.UI.Definitions.MB.Shared.Components.Popups.MasterDetailPopup;
using BlueLotus360.Com.UI.Definitions.MB.Shared.Components.TelerikComponents.Grid;
using BlueLotus360.Com.UI.Definitions.Pages.SetUp.Master_Data.Item_Profile_Mobile.Components;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace BlueLotus360.Com.UI.Definitions.Pages.SetUp.Master_Data.Item_Profile_Mobile
{
    public partial class ItemProfileMobile
    {
        #region parameter

        private BLUIElement formDefinition;
        private BLTransaction transaction = new();

        private ItemSelectListRequest itemSelectListRequest;
        private ItemSelectList insertRequest;
        private ItemSelectList updateRequest;

        private UIBuilder _refBuilder;
        private AddNewAddress _refNewAddressCreation;

        private IDictionary<string, EventCallback> _interactionLogic;
        private IDictionary<string, BLUIElement> _modalDefinitions;
        private IDictionary<string, IBLUIOperationHelper> _objectHelpers;

        private BLItemProfileInsertBasicDetails _bLItemProfileInsertBasicDetails;
        private BLItemProfileUpdateBasicDetails _bLItemProfileUpdateBasicDetails;
        private BLTelGrid<ItemSelectList> _blTb;

        private IList<ItemSelectList> gridDetails;
        private ItemSelectList currentDetails, updatedDetails;

        private bool ShowInsertDetails = false;
        private bool isTableLoading = false;
        private bool showsgrid = true;
        private bool ShowUpdateDetails = false;

        BLUIElement insertmodalUIElement, updatemodalUIElement, gridUIElement;

        long elementKey;

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
                insertmodalUIElement = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("InsertBasicDetails")).FirstOrDefault();
                updatemodalUIElement = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("UpdateBasicDetails")).FirstOrDefault();
                gridUIElement = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("DetailsTable")).FirstOrDefault();

                if (gridUIElement != null)
                {
                    gridUIElement.Children = formDefinition.Children.Where(x => x.ParentKey == gridUIElement.ElementKey).ToList();
                }
                if (insertmodalUIElement != null)
                {
                    insertmodalUIElement.Children = formDefinition.Children.Where(x => x.ParentKey == insertmodalUIElement.ElementKey).ToList();
                }
                if (updatemodalUIElement != null)
                {
                    updatemodalUIElement.Children = formDefinition.Children.Where(x => x.ParentKey == updatemodalUIElement.ElementKey).ToList();
                }
            }

            formDefinition.IsDebugMode = true;

            _interactionLogic = new Dictionary<string, EventCallback>();
            _modalDefinitions = new Dictionary<string, BLUIElement>();
            _objectHelpers = new Dictionary<string, IBLUIOperationHelper>();

            HookInteractions();
            RefreshGrid();

            itemSelectListRequest.ElementKey = elementKey;

            gridDetails = await _itemProfileMobileManager.GetItemProfileList(itemSelectListRequest);

        }

        private async void InitNewLine()
        {
            insertRequest = new ItemSelectList();
            _objectHelpers["InItemCode"].ResetToInitialValue();
            _objectHelpers["InItemName"].ResetToInitialValue();
            _objectHelpers["InItemType"].ResetToInitialValue();
            _objectHelpers["InUnit"].ResetToInitialValue();
            StateHasChanged();
        }
        private void RefreshGrid()
        {
            itemSelectListRequest = new ItemSelectListRequest();
            insertRequest = new ItemSelectList();
            updateRequest = new ItemSelectList();
            currentDetails = new ItemSelectList();
            updatedDetails = new ItemSelectList();
            _refNewAddressCreation = new AddNewAddress();
        }

        private void HookInteractions()
        {

            InteractionHelper helper = new InteractionHelper(this, formDefinition);
            _interactionLogic = helper.GenerateEventCallbacks();
            //AppSettings.RefreshTopBar("Item Profile Mobile");
            appStateService._AppBarName = "Item Profile Mobile";
        }

        private void UIStateChanged()
        {
            this.StateHasChanged();
        }

        #endregion

        #region Item Related Events

        private async void ShowAddNewCustomer(UIInterectionArgs<object> args) 
        {
            IDictionary<string, object> ParamDictionary = new Dictionary<string, object>();
            ParamDictionary.Add("InitiatorElement", args.InitiatorObject);
            ParameterView values = ParameterView.FromDictionary(ParamDictionary);
            await _refNewAddressCreation.SetParametersAsync(values);
            _refNewAddressCreation.ShowPopUp();

            UIStateChanged();
        }
        private async void OnCreateNewItem(UIInterectionArgs<object> args)
        {
            showsgrid = false;
            ShowInsertDetails = true;
            UIStateChanged();

        }
        #endregion

        #region customer creation 
        private async Task OnCustomerCreateSuccess(AddressMaster address)
        {
            await ReadData("Customer");
            await SetValue("Customer", address);

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

        #region Update Item Events 

        private async void UpdateHandler(UIInterectionArgs<object> args)
        {
            showsgrid = false;
            ShowUpdateDetails = true;

            currentDetails = (ItemSelectList)args.DataObject;

            updatedDetails = currentDetails;

            await SetValue("ItemCode", currentDetails.ItemCode);
            await SetValue("ItemName", currentDetails.ItemName);
            await SetValue("ItemType", currentDetails.ItemType.CodeKey);
            await SetValue("Unit", currentDetails.ItemUnit.UnitKey);
            await SetValue("IsAct", currentDetails.IsActive);
            await SetValue("IsApprove", currentDetails.IsApprove);

            UIStateChanged();
        }

        private async void OnSaveUpdateItem(UIInterectionArgs<object> args)
        {
            this.appStateService.IsLoaded = true;
            UIStateChanged();

            await _itemProfileMobileManager.UpdateItem(updatedDetails);
            gridDetails = await _itemProfileMobileManager.GetItemProfileList(itemSelectListRequest);

            ShowUpdateDetails = false;
            showsgrid = true;

            RefreshGrid();

            this.appStateService.IsLoaded = false;
            UIStateChanged();
        }
        private async void OnBackUpdateItem(UIInterectionArgs<object> args)
        {
            ShowUpdateDetails = false;
            showsgrid = true;

            UIStateChanged();
        }
        private async void OnUpdateItemCode(UIInterectionArgs<string> args)
        {
            updatedDetails.ItemCode = args.DataObject;
            UIStateChanged();
        }
        private async void OnUpdateItemName(UIInterectionArgs<string> args)
        {
            updatedDetails.ItemName = args.DataObject;
            UIStateChanged();
        }
        private async void OnUpdateItemType(UIInterectionArgs<CodeBaseResponse> args)
        {
            updatedDetails.ItemType = args.DataObject;
            UIStateChanged();
        }
        private async void OnUpdateItemUnit(UIInterectionArgs<UnitResponse> args)
        {
            updatedDetails.ItemUnit = args.DataObject;
            UIStateChanged();
        }
        private async void OnUpdateIsActive(UIInterectionArgs<bool> args)
        {
            updatedDetails.IsActive = args.DataObject;
            UIStateChanged();
        }
        private async void OnUpdateIsApprove(UIInterectionArgs<bool> args)
        {
            updatedDetails.IsApprove = args.DataObject;
            UIStateChanged();
        }

        #endregion

        #region Insert Item Events

        private async void OnInsertItemCode(UIInterectionArgs<string> args)
        {
            insertRequest.ItemCode = args.DataObject;
            UIStateChanged();
        }

        private async void OnInsertItemName(UIInterectionArgs<string> args)
        {
            insertRequest.ItemName = args.DataObject;
            UIStateChanged();
        }
        private async void OnInsertItemType(UIInterectionArgs<CodeBaseResponse> args)
        {
            insertRequest.ItemType = args.DataObject;
            UIStateChanged();
        }
        private async void OnInsertItemUnit(UIInterectionArgs<UnitResponse> args)
        {
            insertRequest.ItemUnit = args.DataObject;
            UIStateChanged();
        }
        private async void OnInsertIsActive(UIInterectionArgs<bool> args)
        {
            insertRequest.IsActive = args.DataObject;
            UIStateChanged();
        }
        private async void OnInsertIsApprove(UIInterectionArgs<bool> args)
        {
            insertRequest.IsApprove = args.DataObject;
            UIStateChanged();
        }
        private async void OnInsertItemSave(UIInterectionArgs<object> args)
        {
            this.appStateService.IsLoaded = true;
            UIStateChanged();

            await _itemProfileMobileManager.InsertItem(insertRequest);
            gridDetails = await _itemProfileMobileManager.GetItemProfileList(itemSelectListRequest);

            InitNewLine();

            ShowInsertDetails = false;
            showsgrid = true;

            this.appStateService.IsLoaded = false;
            UIStateChanged();
        }
        private async void OnInsertBack(UIInterectionArgs<object> args)
        {
            showsgrid = true;
            ShowInsertDetails = false;

            RefreshGrid();
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

        private void RefreshComponent(string name)
        {
            IBLUIOperationHelper helper;

            if (_objectHelpers.TryGetValue(name, out helper))
            {
                helper.Refresh();
                UIStateChanged();
            }
        }

        #endregion
    }
}
