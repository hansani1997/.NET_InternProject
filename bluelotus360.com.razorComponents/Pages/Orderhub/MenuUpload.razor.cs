using BL10.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using bluelotus360.Com.commonLib.Helpers;
using bluelotus360.Com.commonLib.Routes;
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
using static BlueLotus360.Com.Infrastructure.OrderPlatforms.Ubereats.UberEntities.UberMenuItems;

namespace bluelotus360.com.razorComponents.Pages.Orderhub
{
    public partial class MenuUpload
    {
        #region parameter

        private BLUIElement formDefinition;
        private BLUIElement Grid;
        private IDictionary<string, EventCallback> _interactionLogic;
        private IDictionary<string, IBLUIOperationHelper> _objectHelpers;
        private UIBuilder _refBuilder;
        private BLTable<PartnerMenuItem> _blTb;
        private IList<PartnerMenuItem> _ListOfItems;
        private CodeBaseResponse Location = new CodeBaseResponse();
        private CodeBaseResponse BU = new CodeBaseResponse();

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
            _refBuilder = new UIBuilder();
            _blTb = new BLTable<PartnerMenuItem>();
            _ListOfItems = new List<PartnerMenuItem>();
            HookInteractions();
            if (formDefinition != null)
            {
                Grid = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("DetailTable")).FirstOrDefault();
                formDefinition.IsDebugMode = true;
            }

            if (Grid != null)
            {
                Grid.Children = formDefinition.Children.Where(x => x.ParentKey == Grid.ElementKey).ToList();
            }
            GetAllItemsDetails();
            UIStateChanged();
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
            appStateService._AppBarName = "Menu Upload";
        }



        #endregion

        #region Ui Events
        private async Task GetAllItemsDetails()
        {
            RequestParameters rec = new RequestParameters()
            {
                LocationKey = Location.CodeKey,
                PlatformName = BaseEndpoint.ProductImageURL

            };
            _ListOfItems = await _orderManager.GetOrderItemsToUpload(rec);
            //if (_blTb != null)
            //{
            //    _blTb.Refresh();
            //}
            UIStateChanged();
        }

        private void LocationOnChange(UIInterectionArgs<CodeBaseResponse> args)
        {
            Location = args.DataObject;
            GetAllItemsDetails();
            UIStateChanged();
        }

        private void BUOnChange(UIInterectionArgs<CodeBaseResponse> args)
        {
            BU = args.DataObject;
            UIStateChanged();
        }

        private async void SyncItems(UIInterectionArgs<object> args)
        {
            appStateService.IsLoaded = true;
            UberMenuHandler ubermenu = new UberMenuHandler(_codebaseManager, _apiManager, _orderManager);
            bool issucess = await ubermenu.SyncItemsToUber(Location.CodeKey, BU.CodeKey);
            appStateService.IsLoaded = false;
            if (issucess)
            {
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("Menu has been Uploaded Successfully ", Severity.Success);
            }
            else
            {
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("Menu Upload has been Failed. Please Try Again", Severity.Error);
            }
        }



        //private async void GetMenu()
        //{
        //    UberMenuHandler uberMenuHandler = new UberMenuHandler(_codebaseManager,_apiManager,_orderManager);
        //    uberMenuHandler.GetAllMenuByStore();
        //}




        #endregion
    }
}
