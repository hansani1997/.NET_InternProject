using BL10.CleanArchitecture.Domain.Entities.ItemProfleMobile;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using bluelotus360.com.razorComponents.Extensions;
using bluelotus360.com.razorComponents.MB.Shared.Components;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using bluelotus360.com.razorComponents.Pages.SetUp.Master_Data.Item_Profile_Mobile.Components;
using bluelotus360.com.razorComponents.MB.Shared.Components.Popups.MasterDetailPopup;
using BlueLotus360.CleanArchitecture.Domain.DTO.MasterData;
using bluelotus360.Com.commonLib.Helpers;
using System.Reflection.Metadata;
using bluelotus360.Com.commonLib.Managers.ItemProfileMobile;
using MudBlazor.Extensions;

namespace bluelotus360.com.razorComponents.Pages.SetUp.Master_Data.Item_Profile_Mobile
{
    public partial class ItemProfileMobile
    {
        #region parameter

        private BLUIElement formDefinition;
        private BLTransaction transaction = new();

        private ItemSelectListRequest itemSelectListRequest;  //grid
        private ItemSelectList insertRequest;  //insert
        private ItemSelectList updateRequest; //update

        private UIBuilder _refBuilder;
        private AddNewAddress _refNewAddressCreation;

        private IDictionary<string, EventCallback> _interactionLogic;
        private IDictionary<string, BLUIElement> _modalDefinitions;  //grid
        private IDictionary<string, IBLUIOperationHelper> _objectHelpers;

        private BLItemProfileInsertDetails _bLItemProfileInsertDetails; //insert
        private BLItemProfileUpdateDetails _bLItemProfileUpdateDetails; //update
        private BLTable<ItemSelectList> _blTb;    //Gird

        private IList<ItemSelectList> gridDetails;
        private ItemSelectList currentDetails, updatedDetails; 

        private bool ShowInsertDetails = false;  //insert
        private bool ShowUpdateDetails = false;  //update
        private bool isTableLoading = false;
        private bool showsgrid = true;

        BLUIElement insertmodalUIElement, gridUIElement, updatemodalUIElement;

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
                insertmodalUIElement = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("InsertBasicDetails")).FirstOrDefault();  //insert 
                updatemodalUIElement = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("UpdateBasicDetails")).FirstOrDefault();
                gridUIElement = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("DetailsTable")).FirstOrDefault();      //grid


                //grid
                if (gridUIElement != null)
                {
                    gridUIElement.Children = formDefinition.Children.Where(x => x.ParentKey == gridUIElement.ElementKey).ToList();
                }

                //insert section
                if (insertmodalUIElement != null)
                {
                    insertmodalUIElement.Children = formDefinition.Children.Where(x => x.ParentKey == insertmodalUIElement.ElementKey).ToList();
                }

                //update
                if (updatemodalUIElement != null)
                {
                    updatemodalUIElement.Children = formDefinition.Children.Where(x => x.ParentKey == updatemodalUIElement.ElementKey).ToList();
                }
            }

            formDefinition.IsDebugMode = true;

            _interactionLogic = new Dictionary<string, EventCallback>();
            _modalDefinitions = new Dictionary<string, BLUIElement>();    //grid
            _objectHelpers = new Dictionary<string, IBLUIOperationHelper>();


            HookInteractions();
            RefreshGrid();


            //grid
            itemSelectListRequest.ElementKey = elementKey;

            gridDetails = await _itemProfileMobileManager.GetItemProfileList(itemSelectListRequest); //request API to getItemList /(IItemProfileManager)


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
            appStateService._AppBarName = "Item Profile Mobile";
        }

        private void UIStateChanged()
        {
            this.StateHasChanged();
        }
        #endregion

        #region Item Related Events

        //add new customer
        private async void ShowAddNewCustomer(UIInterectionArgs<object> args)
        {
            IDictionary<string, object> ParamDictionary = new Dictionary<string, object>();
            ParamDictionary.Add("InitiatorElement", args.InitiatorObject);
            ParameterView values = ParameterView.FromDictionary(ParamDictionary);
            await _refNewAddressCreation.SetParametersAsync(values);
            _refNewAddressCreation.ShowPopUp();

            UIStateChanged();
        }

        //add new item btn
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

        //func of save btn for new item 
        private async void OnInsertItemSave(UIInterectionArgs<object> args)
        {
            this.appStateService.IsLoaded = true;
            UIStateChanged();
                await _itemProfileMobileManager.InsertItem(insertRequest);  //insert reuquest
                gridDetails = await _itemProfileMobileManager.GetItemProfileList(itemSelectListRequest); // request API to getItemList after insertion 

                InitNewLine();

                ShowInsertDetails = false;
                showsgrid = true;

            this.appStateService.IsLoaded = false;
            UIStateChanged();

        }

        //back btn from insert section
        private async void OnInsertBack(UIInterectionArgs<object> args)
        {
            showsgrid = true;
            ShowInsertDetails = false;

            RefreshGrid();
            UIStateChanged();

        }
        #endregion

        #region Detele Events
        //Delete action
        //private async void DeleteHandler(UIInterectionArgs<object> args)
        //{
        //    int index = gridDetails.ToList().IndexOf((ItemSelectList)args.DataObject);
        //    if (index != -1)
        //    {
        //        gridDetails[index].IsAct = false;
        //        await _itemProfileMobileManager.UpdateItem((ItemSelectList)args.DataObject);  // request API for the update after deleted(isActive=false)
        //    }
        //    if (_blTb != null)
        //    {
        //        _blTb.Refresh();
        //    }

        //    UIStateChanged();
        //}

        #endregion

       
        #region Update Item Events 

        private async void UpdateHandler(UIInterectionArgs<object> args)
        {
            showsgrid= false;
            ShowUpdateDetails= true;


            currentDetails = args.DataObject as ItemSelectList;

            updatedDetails = currentDetails;

            await SetValue("ItemCode", currentDetails.ItemCode);
            await SetValue("ItemName", currentDetails.ItemName);
            await SetValue("ItemType", currentDetails.ItemType.CodeName);
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
