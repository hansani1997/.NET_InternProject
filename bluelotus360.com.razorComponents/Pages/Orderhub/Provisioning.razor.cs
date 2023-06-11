using BL10.CleanArchitecture.Domain.Entities.APIInfo;
using BlueLotus.Com.Domain.PartnerEntity;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using bluelotus360.Com.commonLib.Helpers;
using BlueLotus360.Com.Infrastructure.OrderPlatforms.Ubereats.UberEntities;
using BlueLotus360.Com.Infrastructure.OrderPlatforms.Ubereats;
using bluelotus360.com.razorComponents.Extensions;
using bluelotus360.com.razorComponents.MB.Shared.Components;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.razorComponents.Pages.Orderhub
{
    public partial class Provisioning
    {
        #region parameter

        private BLUIElement formDefinition;
        private BLUIElement Grid;
        private IDictionary<string, EventCallback> _interactionLogic;
        private IDictionary<string, IBLUIOperationHelper> _objectHelpers;
        private UIBuilder _refBuilder;
        private BLTable<PartnerStore> _blTb;
        private bool isOpenPopup = false;
        List<PartnerStore> UberStoreList;
        List<PartnerStore> UberStoreData;
        UberStoreHandler UberStoreHandler;

        #endregion

        #region General
        protected override async Task OnInitializedAsync()
        {
            long elementKey = 1;
            _navigationManager.TryGetQueryString<long>("ElementKey", out elementKey);// get element key from url 

            if (elementKey > 10)
            {
                var formrequest = new ObjectFormRequest();

                formrequest.MenuKey = elementKey;
                formDefinition = await _navManger.GetMenuUIElement(formrequest);//get ui elements
            }

            if (formDefinition != null)
            {
                formDefinition.IsDebugMode = true;
            }


            _interactionLogic = new Dictionary<string, EventCallback>();
            _objectHelpers = new Dictionary<string, IBLUIOperationHelper>();
            UberStoreHandler = new UberStoreHandler(_apiManager);
            _blTb = new BLTable<PartnerStore>();
            UberStoreData = new List<PartnerStore>();
            HookInteractions();
            var childsHash = formDefinition.Children.ToLookup(elem => elem.ParentKey);
            foreach (var child in formDefinition.Children)
            {
                child.Children = childsHash[child.ElementKey].ToList();
            }
            if (formDefinition != null)
            {
                Grid = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("DetailTable")).FirstOrDefault();
                formDefinition.IsDebugMode = true;
            }

            //if (Grid != null)
            //{
            //    Grid.Children = formDefinition.Children.Where(x => x.ParentKey == Grid.ElementKey).ToList();
            //}

        }

        private void UIStateChanged()
        {

            this.StateHasChanged();
        }

        private void HookInteractions()
        {
            InteractionHelper helper = new InteractionHelper(this, formDefinition);//formdefinition has all form objects 
            _interactionLogic = helper.GenerateEventCallbacks();// generate all event callbacks 
            //AppSettings.RefreshTopBar("Geo Attendence");
            appStateService._AppBarName = "Provisioning & Store Setup";
        }



        #endregion

        #region Ui Events

        private void LoadPopup()
        {
            HideAllPopups();
            isOpenPopup = true;
            UIStateChanged();
        }

        private void LocationOnChange(UIInterectionArgs<CodeBaseResponse> args)
        {
            UIStateChanged();
        }

        private async Task LoadGrid()
        {
            appStateService.IsLoaded = true;
            HideAllPopups();
            UberStoreList = new List<PartnerStore>();
            /*
             * 1. get uberStoreList from uber
             * 2. get user locations
             * 3. check stores are provisioned
             * 4. send stores to frontend with locations
             */
            IList<UberStore> UberStores = await UberStoreHandler.GetUberStoreDetails();
            CodeBaseResponse code = new CodeBaseResponse()
            {
                ConditionCode = "Loc"
            };
            IList<CodeBaseResponse> AvailableBLLocations = await _codebaseManager.GetCodesByConditionCode(code);
            foreach (UberStore item in UberStores)
            {
                PartnerStore partnerStore = new PartnerStore();
                APIRequestParameters request = new APIRequestParameters()
                {
                    APIName = item.Store_id
                };
                APIInformation StoreDetailsByMerchantID = await _apiManager.GetApiDetailsByMerchantID(request);
                if (StoreDetailsByMerchantID != null && StoreDetailsByMerchantID.MappedLocationKey > 1)
                {
                    partnerStore.PartnerStoreId = item.Store_id;
                    partnerStore.PartnerStoreName = item.Name;
                    partnerStore.IsProvisionEnabled = StoreDetailsByMerchantID.IsActive == 1 ? true : false;
                    partnerStore.IsProvisionOnOff = StoreDetailsByMerchantID.IsActive == 1 ? true : false;
                    partnerStore.UserLocationInOurSystem.CodeKey = StoreDetailsByMerchantID.Location.CodeKey;
                    partnerStore.UserLocationInOurSystem.CodeName = StoreDetailsByMerchantID.Location.CodeName;
                    partnerStore.BU.CodeKey = StoreDetailsByMerchantID.BU.CodeKey;
                    partnerStore.BU.CodeName = StoreDetailsByMerchantID.BU.CodeName;

                }
                else
                {
                    partnerStore.PartnerStoreId = item.Store_id;
                    partnerStore.PartnerStoreName = item.Name;
                    partnerStore.IsProvisionEnabled = false;
                    partnerStore.IsProvisionOnOff = false;
                    partnerStore.UserLocationInOurSystem.CodeKey = 1;
                    partnerStore.UserLocationInOurSystem.CodeName = "-";
                    partnerStore.BU.CodeKey = 1;
                    partnerStore.BU.CodeName = "-";

                }

                UberStoreList.Add(partnerStore);

            }

            UberStoreData = UberStoreList;
            //if (_blTb != null)
            //{
            //    _blTb.Refresh();
            //}
            appStateService.IsLoaded = false;
        }

        private async void SetupStoreProvision(UIInterectionArgs<object> args)
        {
            appStateService.IsLoaded = true;
            PartnerStore selectedPartnerStore = args.DataObject as PartnerStore;
            if (selectedPartnerStore.IsProvisionOnOff)
            {
                selectedPartnerStore.IsProvisionEnabled = true;
            }
            else
            {
                selectedPartnerStore.IsProvisionEnabled = false;
            }
            UberProvisionSetupForStore uberProvisionSetupForStore = new UberProvisionSetupForStore()
            {
                // Integrator_store_id = selectedPartnerStore.UserLocationInOurSystem.Code,
                Integration_enabled = selectedPartnerStore.IsProvisionEnabled,
                Is_order_manager = selectedPartnerStore.IsProvisionEnabled
            };
            bool isSuccess = await UberStoreHandler.SetUberStoreProvisioning(selectedPartnerStore.UserLocationInOurSystem.CodeKey, selectedPartnerStore.PartnerStoreId, uberProvisionSetupForStore, selectedPartnerStore.BU.CodeKey);

            if (isSuccess)
            {
                APIInformation apiinfomation = new APIInformation()
                {
                    APIIntegrationNmae = "UberEatsStore_" + selectedPartnerStore.UserLocationInOurSystem.CodeKey + "_" + selectedPartnerStore.BU.CodeKey,
                    ApplicationID = selectedPartnerStore.PartnerStoreId,
                    Location = selectedPartnerStore.UserLocationInOurSystem,
                    BU = selectedPartnerStore.BU,
                    IsActive = Convert.ToByte(selectedPartnerStore.IsProvisionOnOff)

                };
                bool success = await _orderManager.UberProvision_InsertUpdate(apiinfomation);
                appStateService.IsLoaded = false;
                if (success)
                {
                    _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                    _snackBar.Add("Setup Successful ", Severity.Success);
                }
                else
                {
                    _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                    _snackBar.Add("Setup has been Failed. Please Try Again", Severity.Error);
                }
            }
            else
            {
                appStateService.IsLoaded = false;
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("Store Provisioning has been Failed. Please Try Again", Severity.Error);
            }

        }

        #endregion

        #region PopupEvents
        private void HideAllPopups()
        {
            isOpenPopup = false;
            UIStateChanged();
        }

        #endregion
    }
}
