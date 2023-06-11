using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bluelotus360.com.razorComponents.MB.Shared.Components;
using bluelotus360.com.razorComponents.Extensions;
using Microsoft.AspNetCore.Components;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using bluelotus360.Com.commonLib.Helpers;
using MudBlazor;
using BL10.CleanArchitecture.Domain.Entities.ItemProfleMobile;
using bluelotus360.com.razorComponents.MB.Shared.Components.Popups.MasterDetailPopup;
using bluelotus360.com.razorComponents.StateManagement;
using Microsoft.JSInterop;
using BlueLotus.Com.Domain.Entity;
using BL10.CleanArchitecture.Domain.DTO;
using bluelotus360.Com.commonLib.Routes;
using MudBlazor.Charts;
using BL10.CleanArchitecture.Domain.Entities.Document;
using BlueLotus360.CleanArchitecture.Domain;
using System.Runtime.InteropServices.ComTypes;
using System.Xml.Linq;
using Telerik.DataSource;

namespace bluelotus360.com.razorComponents.Pages.SetUp.Master_Data.Item_Profile_Mobile_V3
{
    public partial class ItemProfileMobileV3
    {

        #region parameters

        private BLUIElement formDefinition;
        private UIBuilder _refBuilder = new UIBuilder();
        private AddNewAddress _refNewAddressCreation = new AddNewAddress();
        private BLImageBox _bLImageBox = new BLImageBox();

        private IDictionary<string, EventCallback> _interactionLogic;
        private IDictionary<string, BLUIElement> _modalDefinitions;
        private IDictionary<string, IBLUIOperationHelper> _objectHelpers;

        private BLTable<Item> _blTb = new BLTable<Item>();
        private BLTable<ItemUnit> _multiUnits = new BLTable<ItemUnit>();
        private BLTable<Item> _combinations = new BLTable<Item>();
        private BLTable<ItemComponent> _components = new BLTable<ItemComponent>();
        private BLTable<Item> _processComponentInputs = new BLTable<Item>();
        private BLTable<Item> _processComponentOutputs = new BLTable<Item>();
        private BLTable<Item> _advanceConfigGrid = new BLTable<Item>();
        private BLTable<Item> _multilingualGrid = new BLTable<Item>();
        private IList<Item> gridDetails = new List<Item>();
        private IList<ItemUnit> multiUnitsGridDetails = new List<ItemUnit>();
        private IList<ItemCombinations> combinationsGridDetails= new List<ItemCombinations>();
        //private IList<ComponentsTabGrid> componentsGridDetails = new List<ComponentsTabGrid>();
        private IList<Item> processComponentInputsGridDetails = new List<Item>();
        private IList<Item> processComponentOutputsGridDetails = new List<Item>();
        private IList<Item> advanceConfigGridDetails = new List<Item>();
        private IList<Item> multilingualGridDetails = new List<Item>();

        private bool isTableLoading = false;
        private bool showsgrid = true;
        private bool showstab = false;
        private bool basicdetailstabpage = false;
        private bool othertabpages = false;
        double ScreenWidth;
        WindowSize ws = new WindowSize();
        BLBreakpoint breakpointMargin = new BLBreakpoint();
        private readonly bool condition = true;
        private int _gridCount =0;
        private bool ImagePopupShown = false;
        private bool isSaving = false;
        private BLUIElement UIDefinition;
        long elementKey;
        private bool IsDisable01, IsDisable02, IsDisable03, IsDisable04;

        BLUIElement topButtonGroup, tabSection, gridSection, imageSection, PrimaryInformationSection, multiUnitsGrid, combinationsGrid, componentsGrid, processComponentInputsGrid, processComponentOutputsGrid, advanceConfigGrid, multilingualGrid;
        private Item insertBasicDetailsRequest, selectSingleItemRequest, singleItemData, multiUnitGridDetailsRequest, insertMultiUnitsDetailsRequest, multiUnitUpdateRequest, CombinationsGridDetailsRequest;
        private BaseServerFilterInfo itemSelectListRequestV3 = new BaseServerFilterInfo(); //grid
        private ItemUnit selectSingleMultiUnitItemRequest, editedMultiUnitLineItem;
        private ItemCombinations selectSingleCombinationItemRequest;
        private IList<Item> _combinationItemList=new List<Item>();  
        private ItemComponent selectSingleComponentItemRequest,editedComponentLineItem;
        private IList<ItemComponent> _componentItemList = new List<ItemComponent>();


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
            elementKey = 1;

            _navigationManager.TryGetQueryString<long>("ElementKey", out elementKey);

            if (elementKey > 10)
            {

                var formrequest = new ObjectFormRequest();
                formrequest.MenuKey = elementKey;

                formDefinition = await _navManger.GetMenuUIElement(formrequest);

                if (formDefinition!=null)
                {
                    HookInteractions();
                    this.BreakComponent();
                    formDefinition.IsDebugMode = false;

                    BLUIElement tab = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("TabSection")).FirstOrDefault();
                    tabSection = tab.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("Tab")).FirstOrDefault();

                    BLUIElement mainGrid = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("MainGridSection")).FirstOrDefault();
                    gridSection = mainGrid.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("MainGrid")).FirstOrDefault();

                    topButtonGroup = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("ButtonSection")).FirstOrDefault();
                }
                

                //imageSection = tabSection.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("BasicDetails_ImageSection")).FirstOrDefault();
                //PrimaryInformationSection = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals(" BasicDetails_PrimaryInformationSection")).FirstOrDefault();
                //multiUnitsGrid = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("GridSection_MultiUnits")).FirstOrDefault();
                //combinationsGrid = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("GridSection_Combinations")).FirstOrDefault();
                //componentsGrid = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("GridSection_Components")).FirstOrDefault();
                //processComponentInputsGrid = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("InputsGrid_ProcessComponent")).FirstOrDefault();
                //processComponentOutputsGrid = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("OutputsGrid_ProcessComponent")).FirstOrDefault();
                //advanceConfigGrid = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("GridSection_AdvanceConfig")).FirstOrDefault();
                //multilingualGrid = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("GridSection_Multilingual")).FirstOrDefault();


            }

            //grid
            //itemSelectListRequestV3.ObjKy = (int)elementKey;
            //itemSelectListRequestV3.RequestingURL = BaseEndpoint.BaseURL + gridSection.GetPathURL();
			//gridDetails = await _itemProfileMobileManagerV3.GetItemServerfilterDetails(itemSelectListRequestV3) ??new List<Item>();
            //_gridCount = gridDetails.FirstOrDefault() != null?gridDetails.FirstOrDefault().Count:0;
            InitializeItemProfile();


        }

        private async void InitNewLine()
        {
             insertBasicDetailsRequest = new Item(); 
            _objectHelpers["ItemCode_PISection"].ResetToInitialValue();
            _objectHelpers["ItemName_PISection"].ResetToInitialValue();
            _objectHelpers["ItemUnit_PISection"].ResetToInitialValue();
            _objectHelpers["ItemType_PISection"].ResetToInitialValue();
            _objectHelpers["PartNumber_PISection"].ResetToInitialValue();
            _objectHelpers["Description_PISection"].ResetToInitialValue();
            _objectHelpers["ItemCategory1_BasicDetails"].ResetToInitialValue();
            _objectHelpers["ItemCategory2_BasicDetails"].ResetToInitialValue();
            _objectHelpers["ItemCategory3_BasicDetails"].ResetToInitialValue();
            _objectHelpers["ItemCategory4_BasicDetails"].ResetToInitialValue();
            _objectHelpers["ReOrderLevel_BasicDetails"].ResetToInitialValue();
            _objectHelpers["Model_BasicDetails"].ResetToInitialValue();
            _objectHelpers["Brand_BasicDetails"].ResetToInitialValue();
            _objectHelpers["OldItemCode_BasicDetails"].ResetToInitialValue();
            _objectHelpers["SupplierWarranty_BasicDetails"].ResetToInitialValue();
            _objectHelpers["DefaultSupplier_BasicDetails"].ResetToInitialValue();
            StateHasChanged();
        }
        // / RefreshGrid
        private void InitializeItemProfile() 
        {
            itemSelectListRequestV3 = new BaseServerFilterInfo();
            insertBasicDetailsRequest = new Item();
            selectSingleItemRequest = new Item();
            selectSingleMultiUnitItemRequest = new ItemUnit();
            multiUnitGridDetailsRequest = new Item();
            insertMultiUnitsDetailsRequest = new Item();
            multiUnitUpdateRequest = new Item();
            selectSingleCombinationItemRequest = new ItemCombinations();
            CombinationsGridDetailsRequest = new Item();
            selectSingleComponentItemRequest = new ItemComponent();
            _refNewAddressCreation = new AddNewAddress();
            singleItemData=new Item();
        }

        private void HookInteractions()
        {
            InteractionHelper helper = new InteractionHelper(this, formDefinition);
            _interactionLogic = helper.GenerateEventCallbacks();
            appStateService._AppBarName = "Item Profile";
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


        private async void OnTabChanged(int tabIndex)
        {
        }

        #region Header Button Section

        //Add new Item
        private async void OnAdd(UIInterectionArgs<object> args)
        {
            InitializeItemProfile();

            showsgrid = false;
            showstab = true;

            IsDisable02=true;
            IsDisable03=true;
            IsDisable04=true;

            await SetValue("ItemCode_PISection", string.Empty); 
            await SetValue("ItemName_PISection", string.Empty);
            await SetValue("ItemType_PISection", string.Empty);
            await SetValue("PartNumber_PISection", string.Empty);
            await SetValue("Description_PISection", string.Empty);
            await SetValue("ItemCategory1_BasicDetails", string.Empty);
            await SetValue("ItemCategory2_BasicDetails", string.Empty);
            await SetValue("ItemCategory3_BasicDetails", string.Empty);
            await SetValue("ItemCategory4_BasicDetails", string.Empty);
            await SetValue("Model_BasicDetails", string.Empty);
            await SetValue("Brand_BasicDetails", string.Empty);
            await SetValue("OldItemCode_BasicDetails", string.Empty);
            await SetValue("DefaultSupplier_BasicDetails", string.Empty);

            UIStateChanged();

        }

      

        //Back btn to main grid

        private async void OnBack(UIInterectionArgs<object> args)
        {
            showsgrid = true;
            showstab = false;

            UIStateChanged();

        }

        ////Save btn after entering data to tabs
        private async void OnSave(UIInterectionArgs<object> args)
        {
            this.appStateService.IsLoaded = true;

                if (insertBasicDetailsRequest.IsInEditMode)
                {
                    await _itemProfileMobileManagerV3.UpdateItem(insertBasicDetailsRequest);
                    insertBasicDetailsRequest.IsInEditMode = false;

                    _blTb.ServerFilterRefrersh();
                    showstab = false;
                    showsgrid = true;

                    InitializeItemProfile();
                }
                else
                {
                    await _itemProfileMobileManagerV3.InsertItemV3(insertBasicDetailsRequest);

                    _blTb.ServerFilterRefrersh();
                    InitNewLine();

                    showstab = false;
                    showsgrid = true;
                    InitializeItemProfile();
                }

                this.appStateService.IsLoaded = false;
                UIStateChanged();
            
        }

        #endregion

        #region Main Grid Button (Update)

        private async void OnEditGridSection(UIInterectionArgs<object> args)
        {
            //select single item
            selectSingleItemRequest = (Item)args.DataObject;

            insertBasicDetailsRequest = await _itemProfileMobileManagerV3.SelectSingleItem(selectSingleItemRequest);
            insertBasicDetailsRequest.IsInEditMode = true;


            //image upload
            DocumentRetrivaltDTO document = new DocumentRetrivaltDTO();
            
            if (insertBasicDetailsRequest.ItemKey>11)
            {
                document.ItemKey = (int)insertBasicDetailsRequest.ItemKey;
                insertBasicDetailsRequest.Base64Documents = await _uploadManager.getBase64DocumentsV2(document);
            }
            

            LoadCombinations();
            LoadItemComponents();

            showsgrid = false;
            showstab = true;

            IsDisable02 = false;
            IsDisable03 = false;
            IsDisable04 = false;

            //Basic details tab
            await ReadData("ItemUnit_PISection");
            await ReadData("TargetUnit_MultiUnits");
            await SetValue("ItemCode_PISection", insertBasicDetailsRequest.ItemCode);
            await SetValue("ItemName_PISection", insertBasicDetailsRequest.ItemName);
            await SetValue("ItemType_PISection", insertBasicDetailsRequest.ItemType.CodeName);
            await SetValue("PartNumber_PISection", insertBasicDetailsRequest.PartNumber);
            await SetValue("Description_PISection", insertBasicDetailsRequest.Description);
            await SetValue("ReOrderLevel_BasicDetails", insertBasicDetailsRequest.ReOrderLevel);
            await SetValue("OldItemCode_BasicDetails", insertBasicDetailsRequest.OldItemCode);
            await SetValue("SupplierWarranty_BasicDetails", insertBasicDetailsRequest.SupplierWarranty);

            UIStateChanged();
        }

        #endregion

        #region Item Navigation

        public async void OnItemNavigation(int singleItem)
        {
            selectSingleItemRequest.ItemKey = singleItem;

            //select single item
            insertBasicDetailsRequest = await _itemProfileMobileManagerV3.SelectSingleItem(selectSingleItemRequest);
            insertBasicDetailsRequest.IsInEditMode = true;

            //image upload
            DocumentRetrivaltDTO document = new DocumentRetrivaltDTO();
                
            if (insertBasicDetailsRequest.ItemKey > 11)
            {
                    document.ItemKey = (int)insertBasicDetailsRequest.ItemKey;
                    insertBasicDetailsRequest.Base64Documents = await _uploadManager.getBase64DocumentsV2(document);
            }

            LoadCombinations();
            LoadItemComponents();

            showsgrid = false;
            showstab = true;

            IsDisable02 = false;
            IsDisable03 = false;
            IsDisable04 = false;

            //Basic details tab
            await ReadData("ItemUnit_PISection");
            await ReadData("TargetUnit_MultiUnits");
            await SetValue("ItemCode_PISection", insertBasicDetailsRequest.ItemCode);
            await SetValue("ItemName_PISection", insertBasicDetailsRequest.ItemName);
            await SetValue("ItemType_PISection", insertBasicDetailsRequest.ItemType.CodeName);
            await SetValue("PartNumber_PISection", insertBasicDetailsRequest.PartNumber);
            await SetValue("Description_PISection", insertBasicDetailsRequest.Description);
            await SetValue("ReOrderLevel_BasicDetails", insertBasicDetailsRequest.ReOrderLevel);
            await SetValue("OldItemCode_BasicDetails", insertBasicDetailsRequest.OldItemCode);
            await SetValue("SupplierWarranty_BasicDetails", insertBasicDetailsRequest.SupplierWarranty);

            UIStateChanged();
        }

        #endregion

        #region Tab Section

        #region Basic Details (Insert)

        private async void OnItemCodePISection(UIInterectionArgs<string> args)
        {
            insertBasicDetailsRequest.ItemCode = args.DataObject;
            UIStateChanged();
        }

        private async void OnItemNamePISection(UIInterectionArgs<string> args)
        {
            insertBasicDetailsRequest.ItemName = args.DataObject;
            UIStateChanged();
        }
        private async void OnItemUnitPISection(UIInterectionArgs<UnitResponse> args)
        {
            insertBasicDetailsRequest.ItemUnit = args.DataObject;
            UIStateChanged();
        }
        private async void OnItemTypePISection(UIInterectionArgs<CodeBaseResponse> args)
        {
            insertBasicDetailsRequest.ItemType = args.DataObject;
            UIStateChanged();
        }

        private async void OnPartNumberPISection(UIInterectionArgs<string> args)
        {
            insertBasicDetailsRequest.PartNumber = args.DataObject;
            UIStateChanged();
        }

        private async void OnDescriptionPISection(UIInterectionArgs<string> args)
        {
            insertBasicDetailsRequest.Description = args.DataObject;
            UIStateChanged();
        }

        private async void OnItemCategory1BasicDetails(UIInterectionArgs<CodeBaseResponse> args)
        {
            insertBasicDetailsRequest.ItemCategory1 = args.DataObject;
            UIStateChanged();
        }

        private async void OnItemCategory2BasicDetails(UIInterectionArgs<CodeBaseResponse> args)
        {
            insertBasicDetailsRequest.ItemCategory2 = args.DataObject;
            UIStateChanged();
        }

        private async void OnItemCategory3BasicDetails(UIInterectionArgs<CodeBaseResponse> args)
        {
            insertBasicDetailsRequest.ItemCategory3 = args.DataObject;
            UIStateChanged();
        }

        private async void OnItemCategory4BasicDetails(UIInterectionArgs<CodeBaseResponse> args)
        {
            insertBasicDetailsRequest.ItemCategory4 = args.DataObject;
            UIStateChanged();
        }

        //private async void OnReOrderLevelBasicDetails(UIInterectionArgs<decimal> args)
        //{
        //    insertBasicDetailsRequest.ReOrderLevel = args.DataObject;
        //    UIStateChanged();
        //}

        private async void OnModelBasicDetails(UIInterectionArgs<CodeBaseResponse> args)
        {
            insertBasicDetailsRequest.Model = args.DataObject;
            UIStateChanged();
        }

        private async void OnBrandBasicDetails(UIInterectionArgs<CodeBaseResponse> args)
        {
            insertBasicDetailsRequest.Brand = args.DataObject;
            UIStateChanged();
        }

        private async void OnOldItemCodeBasicDetails(UIInterectionArgs<string> args)
        {
            insertBasicDetailsRequest.OldItemCode = args.DataObject;
            UIStateChanged();
        }

        //private async void OnSupplierWarrantyBasicDetails(UIInterectionArgs<decimal> args)
        //{
        //    insertBasicDetailsRequest.SupplierWarranty = args.DataObject;
        //    UIStateChanged();
        //}

        private async void OnDefaultSupplierBasicDetails(UIInterectionArgs<AddressResponse> args)
        {
            insertBasicDetailsRequest.DefaultSupplier = args.DataObject;
            UIStateChanged();
        }

        private async void OnIsActBasicDetails(UIInterectionArgs<bool> args)
        {
            //byte isActive = Convert.ToByte(args.DataObject);
            insertBasicDetailsRequest.IsProfActive = args.DataObject;
            UIStateChanged();
        }

        private async void OnIsAppBasicDetails(UIInterectionArgs<bool> args)
        {
           // byte isApprove = Convert.ToByte(args.DataObject);
            insertBasicDetailsRequest.IsProfApprove = args.DataObject;
            UIStateChanged();
        }
        private void ItemUnit_PISection_OnBeforeDataFetch(UIInterectionArgs<ComboRequestDTO> args)
        {
            args.DataObject.AddtionalData.Add("ItemKey", insertBasicDetailsRequest.ItemKey);
        }

        #endregion

        #region MultiUnits
        //Add new record button

        private async void OnAddNewRecordMultiUnits(UIInterectionArgs<Object> args)
        {
            if (!selectSingleMultiUnitItemRequest.IsInEditMode)
            {
                selectSingleMultiUnitItemRequest.ItemKey = insertBasicDetailsRequest.ItemKey;
                insertBasicDetailsRequest.MultiUnits.Add(selectSingleMultiUnitItemRequest);

                await _itemProfileMobileManagerV3.MultiUnitsInsert(selectSingleMultiUnitItemRequest);
            }
            else
            {
                editedMultiUnitLineItem.CopyFrom(selectSingleMultiUnitItemRequest);
                selectSingleMultiUnitItemRequest.IsInEditMode= false;

                await _itemProfileMobileManagerV3.MultiUnitsUpdate(selectSingleMultiUnitItemRequest);
            }
            
            selectSingleMultiUnitItemRequest = new();
            StateHasChanged();
        }

        //Update button on MultiUnits Grid
        private async void OnEditGridSectionMultiUnits(UIInterectionArgs<object> args)
        {
            editedMultiUnitLineItem = (ItemUnit)args.DataObject;

            if (editedMultiUnitLineItem != null)
            {
                editedMultiUnitLineItem.IsInEditMode = true;
                selectSingleMultiUnitItemRequest.CopyFrom(editedMultiUnitLineItem);
            }
            await SetValue("TargetUnit_MultiUnits", selectSingleMultiUnitItemRequest.TargetUnit.UnitKey);
            StateHasChanged();
        }


        //Input Section

        private async void OnBaseUnitConRateMultiUnits(UIInterectionArgs<decimal> args)
        {
            selectSingleMultiUnitItemRequest.BaseUnitConversionRate = args.DataObject;
            UIStateChanged();
        }

        private async void OnTargetUnitMultiUnits(UIInterectionArgs<UnitResponse> args)
        {
            selectSingleMultiUnitItemRequest.TargetUnit = args.DataObject;
            UIStateChanged();
        }

        private async void OnUnitConRateMultiUnits(UIInterectionArgs<decimal> args)
        {
            selectSingleMultiUnitItemRequest.UnitConversionRate = args.DataObject;
            UIStateChanged();
        }

        private async void OnConRateMultiUnits(UIInterectionArgs<decimal> args)
        {
            selectSingleMultiUnitItemRequest.ConversionRate = args.DataObject;
            UIStateChanged();
        }

        private async void OnIsSalesUnitMultiUnits(UIInterectionArgs<bool> args)
        {
            selectSingleMultiUnitItemRequest.IsSalesUnit = args.DataObject;
            UIStateChanged();
        }

        private async void OnIsPurchaseUnitMultiUnits(UIInterectionArgs<bool> args)
        {
            selectSingleMultiUnitItemRequest.IsPurchaseUnit = args.DataObject;
            UIStateChanged();
        }

        private async void OnIsBaseUnitMultiUnits(UIInterectionArgs<bool> args)
        {
            selectSingleMultiUnitItemRequest.IsBaseUnit = args.DataObject;
            UIStateChanged();
        }

        private void TargetUnit_MultiUnits_OnBeforeDataFetch(UIInterectionArgs<ComboRequestDTO> args)
        {
            args.DataObject.AddtionalData.Add("ItemKey", selectSingleMultiUnitItemRequest.ItemKey);
        }

        //private async void OnMultiUnitCancelClick(UIInterectionArgs<object> args)
        //{
        //    selectSingleMultiUnitItemRequest= new();    
        //    await ReadData("TargetUnit_MultiUnits");    
        //    StateHasChanged();
        //}
        #endregion

        #region Combinations

        private async void OnColorCombinations(UIInterectionArgs<IList<CodeBaseResponse>> args)
        {
            selectSingleCombinationItemRequest.ItemProperty1 = args.DataObject;
            UIStateChanged();
        }

        private async void OnSizeCombinations(UIInterectionArgs<IList<CodeBaseResponse>> args)
        {
            selectSingleCombinationItemRequest.ItemProperty2 = args.DataObject;
            UIStateChanged();
        }

        private async void OnProperty3Combinations(UIInterectionArgs<IList<CodeBaseResponse>> args)
        {
            selectSingleCombinationItemRequest.ItemProperty3 = args.DataObject;
            UIStateChanged();
        }

        private async void OnProperty4Combinations(UIInterectionArgs<IList<CodeBaseResponse>> args)
        {
            selectSingleCombinationItemRequest.ItemProperty4 = args.DataObject;
            UIStateChanged();
        }

        //Insert Combination Item
        private async void OnGenerateCombinations(UIInterectionArgs<object> args)
        {
            if (!selectSingleCombinationItemRequest.IsInEditMode)
            {
                selectSingleCombinationItemRequest.ParentItemKey = insertBasicDetailsRequest.ItemKey;
                selectSingleCombinationItemRequest.RequestingObjectKey = insertBasicDetailsRequest.RequestingObjectKey;
                await _itemProfileMobileManagerV3.GenerateCombinations(selectSingleCombinationItemRequest);
            }

            selectSingleCombinationItemRequest = new();

            LoadCombinations();
            
            StateHasChanged();

        }

        private async void LoadCombinations()
        {

            if(insertBasicDetailsRequest != null)
            {
                _combinationItemList = await _itemProfileMobileManagerV3.GetCombinationItems(new Item() { ItemKey = insertBasicDetailsRequest.ItemKey, RequestingObjectKey = (int)elementKey });

            }
        }

        ////update combination Item
        //public async void OnEditGridSectionCombinations(UIInterectionArgs<object> args)
        //{
        //    //await SetValue("Color_Combinations", );
        //    //await SetValue("Size_Combinations", );
        //    //await SetValue("Property3_Combinations", );
        //    //await SetValue("Property4_Combinations", );
        //}

        ////Delete Combnation Item
        //public async void OnDeleteGridSectionCombinations(UIInterectionArgs<object> args)
        //{

        //}

        #endregion

        #region Components

        private async void OnAnalysisQuantityComponents(UIInterectionArgs<decimal> args)
        {
            selectSingleComponentItemRequest.AnalysisQuantity = args.DataObject;
            UIStateChanged();
        }

        private async void OnItemComponents(UIInterectionArgs<ItemResponse> args)
        {
            selectSingleComponentItemRequest.ComponentItem = args.DataObject;
            UIStateChanged();
        }

        private async void OnUnitComponents(UIInterectionArgs<UnitResponse> args)
        {
            selectSingleComponentItemRequest.TransactionUnit = args.DataObject;
            UIStateChanged();
        }

        private async void OnTranQtyComponents(UIInterectionArgs<decimal> args)
        {
            selectSingleComponentItemRequest.Quantity = args.DataObject;
            UIStateChanged();
        }

        private async void OnWastageComponents(UIInterectionArgs<decimal> args)
        {
            selectSingleComponentItemRequest.WastagePercentage = args.DataObject;
            UIStateChanged();
        }

        private async void OnWastageQtyComponents(UIInterectionArgs<decimal> args)
        {
            selectSingleComponentItemRequest.WastageQuantity = args.DataObject;
            UIStateChanged();
        }
        private async void OnUsageComponents(UIInterectionArgs<decimal> args)
        {
            selectSingleComponentItemRequest.UsagePercentage = args.DataObject;
            UIStateChanged();
        }
        private async void OnIsAdrWiseComponents(UIInterectionArgs<bool> args)
        {
            selectSingleComponentItemRequest.isAdrWise = args.DataObject;
            UIStateChanged();
        }

        private async void OnDescriptionComponents(UIInterectionArgs<string> args)
        {
            selectSingleComponentItemRequest.Description = args.DataObject;
            UIStateChanged();
        }

        private async void OnCompactFactorComponents(UIInterectionArgs<decimal> args)
        {
            selectSingleComponentItemRequest.CompactFactor = args.DataObject;
            UIStateChanged();
        }


        private async void OnGenerateCombinationsComponents(UIInterectionArgs<Object> args)
        {
            if (!selectSingleComponentItemRequest.IsInEditMode)
            {
                selectSingleComponentItemRequest.FInishedItem.ItemKey = insertBasicDetailsRequest.ItemKey;
                //_componentItemList.Add(selectSingleComponentItemRequest);

                await _itemProfileMobileManagerV3.CreateComponents(selectSingleComponentItemRequest);
            }
            else
            {
                editedComponentLineItem.CopyFrom(selectSingleComponentItemRequest);
                selectSingleComponentItemRequest.IsInEditMode = false;

                await _itemProfileMobileManagerV3.UpdateItemComponent(selectSingleComponentItemRequest);
            }

            selectSingleComponentItemRequest = new();

            LoadItemComponents();
            StateHasChanged();
        
        }

        //update component Item
        private async void OnEditGridSectionComponents(UIInterectionArgs<object> args)
        {
            editedComponentLineItem = (ItemComponent)args.DataObject;
            editedComponentLineItem = await _itemProfileMobileManagerV3.GetSingleItemComponent(editedComponentLineItem);
            selectSingleComponentItemRequest.IsInEditMode = true;

            selectSingleComponentItemRequest.CopyFrom(editedComponentLineItem);
            

            //await SetValue("AnalysisQuantity_Components", selectSingleComponentItemRequest.AnalysisQuantity);
            //await SetValue("Item_Components", selectSingleComponentItemRequest.ItemName);
            //await SetValue("Unit_Components", selectSingleComponentItemRequest.TransactionUnit.UnitName);
            //await SetValue("TranQty_Components", selectSingleComponentItemRequest.TransactionQuantity);
            //await SetValue("Wastage_Components", selectSingleComponentItemRequest.WastagePercentage);
            //await SetValue("WastageQty_Components", selectSingleComponentItemRequest.WastageQuantity);
            //await SetValue("Usage_Components", selectSingleComponentItemRequest.UsagePercentage);
            //await SetValue("IsAdrWise_Components", selectSingleComponentItemRequest.IsAddressWise);
            //await SetValue("Description_Components", selectSingleComponentItemRequest.Description);

            StateHasChanged();
        }

        //Delete Component Item
        private async void OnDeleteGridSectionComponents(UIInterectionArgs<object> args)
        {
            if (args.DataObject != null)
            {
                ItemComponent item = args.DataObject as ItemComponent;

                bool? result = await _dialogService.ShowMessageBox(
                    "Warning",
                    $"Do you want to remove Item {item.ComponentItem.ItemName}",
                    yesText: "Delete!", cancelText: "Cancel");

                if (result.HasValue && result.Value)
                {
                    if (item != null)
                    {
                        await _itemProfileMobileManagerV3.DeleteItemComponent(item);
                    }


                }

                LoadItemComponents();
                StateHasChanged();
            }
        }

        private async void LoadItemComponents()
        {
            if(insertBasicDetailsRequest != null)
            {
                _componentItemList = await _itemProfileMobileManagerV3.GetItemComponents(new Item() { ItemKey = insertBasicDetailsRequest.ItemKey, RequestingObjectKey = (int)elementKey });

            }
        }
        #endregion

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

        private async Task ReadData(string name, bool UseLocalStorage = false)
        {
            IBLUIOperationHelper helper;

            if (_objectHelpers.TryGetValue(name, out helper))
            {
                await (helper as IBLServerDependentComponent).FetchData(UseLocalStorage);

                StateHasChanged();
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
