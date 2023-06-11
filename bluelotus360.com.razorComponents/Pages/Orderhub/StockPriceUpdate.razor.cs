using BL10.CleanArchitecture.Domain.Entities;
using BlueLotus.Com.Domain.PartnerEntity;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using bluelotus360.Com.commonLib.Helpers;
using bluelotus360.Com.commonLib.Routes;
using BlueLotus360.Com.Infrastructure.OrderPlatforms.PickMe;
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
    public partial class StockPriceUpdate
    {
        #region parameter

        private BLUIElement formDefinition;
        private BLUIElement UberDefinition;
        private BLUIElement PickmeDefinition;
        private BLUIElement UberGrid;
        private BLUIElement PickmeGrid;
        private IDictionary<string, EventCallback> _interactionLogic;
        private IDictionary<string, IBLUIOperationHelper> _objectHelpers;
        private UIBuilder _refBuilder;
        private BLTable<data> _blTb;
        private BLTable<PartnerMenuItem> _blTbx;
        //private BLTelGrid<PickmeMenu> _blTb;
        private CodeBaseResponse UberLocation = new CodeBaseResponse();
        private CodeBaseResponse UberBU = new CodeBaseResponse();
        private CodeBaseResponse PickmeLocation = new CodeBaseResponse();
        private CodeBaseResponse PickmeBU = new CodeBaseResponse();
        private CodeBaseResponse StockStatus = new CodeBaseResponse();
        private string SuspenditemTill = DateTime.Now.ToString("yyyy/MM/dd");
        private IList<data> _ListOfPickmeMenu;
        private IList<PartnerMenuItem> _ListOfUberMenu;

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
            _blTb = new BLTable<data>();
            _blTbx = new BLTable<PartnerMenuItem>();
            _ListOfPickmeMenu = new List<data>();
            UberLocation = new CodeBaseResponse();
            UberBU = new CodeBaseResponse();
            PickmeLocation = new CodeBaseResponse();
            PickmeBU = new CodeBaseResponse();
            StockStatus = new CodeBaseResponse();
            _ListOfUberMenu = new List<PartnerMenuItem>();
            HookInteractions();
            var childsHash = formDefinition.Children.ToLookup(elem => elem.ParentKey);
            foreach (var child in formDefinition.Children)
            {
                child.Children = childsHash[child.ElementKey].ToList();
            }
            if (formDefinition != null)
            {
                PickmeDefinition = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("PickmeContents")).FirstOrDefault();
                UberDefinition = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("UberContents")).FirstOrDefault();
                //if(PickmeDefinition != null)
                //{
                //    PickmeDefinition.Children= formDefinition.Children.Where(x => x.ParentKey == PickmeDefinition.ElementKey).ToList();
                //}
                PickmeGrid = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("PickMeGrid")).FirstOrDefault();
                UberGrid = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("UberGrid")).FirstOrDefault();
                formDefinition.IsDebugMode = true;
            }

            //if (PickmeGrid != null)
            //{
            //    PickmeGrid.Children = formDefinition.Children.Where(x => x.ParentKey == PickmeGrid.ElementKey).ToList();
            //}
            // GetAllItemsDetails();
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
            appStateService._AppBarName = "Stock & Price Update";
        }



        #endregion

        #region Ui Events

        private void StockStatusOnChange(UIInterectionArgs<CodeBaseResponse> args)
        {
            StockStatus = args.DataObject;
            UIStateChanged();
        }
        private void PickmeLocationOnChange(UIInterectionArgs<CodeBaseResponse> args)
        {
            PickmeLocation = args.DataObject;
            UIStateChanged();
        }

        private void PickmeBUOnChange(UIInterectionArgs<CodeBaseResponse> args)
        {
            PickmeBU = args.DataObject;
            UIStateChanged();
        }
        private void UberLocationOnChange(UIInterectionArgs<CodeBaseResponse> args)
        {
            UberLocation = args.DataObject;
            UIStateChanged();
        }

        private void UberBUOnChange(UIInterectionArgs<CodeBaseResponse> args)
        {
            UberBU = args.DataObject;
            UIStateChanged();
        }

        private void DateOnChange(UIInterectionArgs<DateTime?> args)
        {
            SuspenditemTill = args.DataObject.Value.ToString("yyyy/MM/dd");
            UIStateChanged();
        }

        private async void PickmeItemsLoad(UIInterectionArgs<object> args)
        {
            if (PickmeLocation.CodeKey > 11 && PickmeBU.CodeKey > 11)
            {
                appStateService.IsLoaded = true;
                PickmeStockPriceUpdateHandler pickmeMenu = new PickmeStockPriceUpdateHandler(_apiManager, _addressManager);
                PickmeMenu Menu = await pickmeMenu.GetPickItems(PickmeLocation.CodeKey, PickmeBU.CodeKey);
                if (Menu.Data.Count > 0)
                {
                    _ListOfPickmeMenu = Menu.Data;
                }
                appStateService.IsLoaded = false;
                //if (_blTb != null)
                //{
                //    _blTb.Refresh();
                //}
                UIStateChanged();
            }
            else
            {
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("Please select Location & Business Unit", Severity.Info);
                UIStateChanged();
            }

        }

        private async void PushToPickMe(UIInterectionArgs<object> args)
        {
            data menu = args.DataObject as data;
            PickmeStockPriceUpdateHandler pickmeMenu = new PickmeStockPriceUpdateHandler(_apiManager, _addressManager);

            bool success = await pickmeMenu.UpdatePickMeMenu(PickmeLocation.CodeKey, PickmeBU.CodeKey, menu.ID, menu.Price, StockStatus.Code);
            if (success)
            {
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("Item has been Uploaded Successfully ", Severity.Success);
                UIStateChanged();
            }
            else
            {
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("Item Update has been Failed. Please Try Again", Severity.Error);
                UIStateChanged();
            }

            //if (_blTb != null)
            //{
            //    _blTb.Refresh();
            //}

            UIStateChanged();
        }



        private async void UberItemsLoad(UIInterectionArgs<object> args)
        {
            if (UberLocation.CodeKey > 11 && UberBU.CodeKey > 11)
            {
                appStateService.IsLoaded = true;
                //UberMenuHandler uberMenu = new UberMenuHandler(_codebaseManager,_apiManager,_orderManager);
                //IList<MenuFromUber> Menu = await uberMenu.GetUberItems(UberLocation.CodeKey, UberBU.CodeKey);


                RequestParameters rec = new RequestParameters()
                {
                    LocationKey = UberLocation.CodeKey,
                    PlatformName = BaseEndpoint.ProductImageURL

                };
                _ListOfUberMenu = await _orderManager.GetOrderItemsToUpload(rec);
                appStateService.IsLoaded = false;
                //if (_blTb != null)
                //{
                //    _blTb.Refresh();
                //}
                UIStateChanged();




            }
            else
            {
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("Please select Location & Business Unit", Severity.Info);
                UIStateChanged();
            }

        }
        private async void PushToUber(UIInterectionArgs<object> args)
        {
            PartnerMenuItem menu = args.DataObject as PartnerMenuItem;
            UberMenuHandler uberMenu = new UberMenuHandler(_codebaseManager, _apiManager, _orderManager);
            UberEatsItemsForMenu menudata = new UberEatsItemsForMenu();
            UberDiscontinueItem discontinue = new UberDiscontinueItem()
            {
                ItmCd = menu.ItemCode,
                isDiscontinue = menu.IsDiscontinued
            };
            _orderManager.UberMenu_DiscontinueWeb(discontinue);
            menudata.Price_info.Price = Convert.ToInt32(menu.OptionalSalesPrice * 100);
            if (menu.IsDiscontinued)
            {
                menudata.Suspension_info.Suspension.Suspend_until = 8640000000;
            }
            else
            {
                menudata.Suspension_info.Suspension.Suspend_until = -601249181;
            }

            bool success = await uberMenu.UpdateItemToUber(UberLocation.CodeKey, UberBU.CodeKey, menu.ItemCode, menudata);
            if (success)
            {
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("Item has been Updated Successfully ", Severity.Success);
                UIStateChanged();
            }
            else
            {
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("Item Update has been Failed. Please Try Again", Severity.Error);
                UIStateChanged();
            }

            //if (_blTb != null)
            //{
            //    _blTb.Refresh();
            //}

            UIStateChanged();
        }


        #endregion
    }
}
