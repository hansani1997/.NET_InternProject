using BL10.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using bluelotus360.Com.commonLib.Helpers;
using BlueLotus360.Com.Infrastructure.OrderPlatforms.PickMe;
using bluelotus360.com.razorComponents.Extensions;
using bluelotus360.com.razorComponents.MB.Shared.Components;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BlueLotus360.Com.Infrastructure.OrderPlatforms.PickMe.PickmeEntity;
using Newtonsoft.Json;

namespace bluelotus360.com.razorComponents.Pages.Orderhub
{
    public partial class PickmeOrderManager
    {
        #region parameter

        private BLUIElement formDefinition;
        private IDictionary<string, EventCallback> _interactionLogic;
        private IDictionary<string, IBLUIOperationHelper> _objectHelpers;
        private UIBuilder _refBuilder;
        private CodeBaseResponse Location = new CodeBaseResponse();
        private CodeBaseResponse BU = new CodeBaseResponse();
        private decimal Duration;
        private int Page = 1;
        public GetOrder getOrder;
        private long ObjKy = 1;
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
                ObjKy = formrequest.MenuKey;
                formDefinition = await _navManger.GetMenuUIElement(formrequest);//get ui elements
            }

            if (formDefinition != null)
            {
                formDefinition.IsDebugMode = true;
            }


            _interactionLogic = new Dictionary<string, EventCallback>();
            _objectHelpers = new Dictionary<string, IBLUIOperationHelper>();
            _refBuilder = new UIBuilder();
            getOrder = new GetOrder();
            HookInteractions();

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
            appStateService._AppBarName = "View Orders";
        }



        #endregion

        #region UI Events

        private void LocationOnChange(UIInterectionArgs<CodeBaseResponse> args)
        {
            Location = args.DataObject;
            UIStateChanged();
        }

        private void BUOnChange(UIInterectionArgs<CodeBaseResponse> args)
        {
            BU = args.DataObject;
            UIStateChanged();
        }

        private void FromDateOnChange(UIInterectionArgs<decimal> args)
        {
            Duration = args.DataObject;
            UIStateChanged();
        }
        private void OnPageChange(UIInterectionArgs<decimal> args)
        {
            if (args.DataObject < 1)
            {
                args.CancelChange = true; ;
                args.OverrideValue = true;
                args.OverriddenValue = 1M;
            }
            Page = (int)args.DataObject;
            UIStateChanged();
        }

        private async void LoadOrders(UIInterectionArgs<object> args)
        {
            if (Location.CodeKey > 11 && BU.CodeKey > 11 && Duration > 0)
            {
                //appStateService.IsLoaded = true;
                //PickmeAPIHandler pickmeAPIHandler = new PickmeAPIHandler(_apiManager, _orderManager, _addressManager, _codebaseManager);
                //getOrder = await pickmeAPIHandler.GetAllPickMeOrderByDuration(Location, BU, Duration, Page);
                RequestParameters parameters = new RequestParameters();
                parameters.pagination.Page = Page;
                parameters.LocationKey= Location.CodeKey;
                parameters.BUKy = BU.CodeKey;
                parameters.Duration= Duration;
                string orderdata = await _orderManager.GetPickmeOrdersByDuration(parameters);
                getOrder=JsonConvert.DeserializeObject<GetOrder>(orderdata);
                //appStateService.IsLoaded = false;
                StateHasChanged();
            }
            else
            {
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("Please fill all the fields", Severity.Warning);
            }

        }
        private async void SaveOrder(PickmeEntity.data getOrder)
        {
            appStateService.IsLoaded = true;
            RequestParameters request = new RequestParameters()
            {
                PlatformName = "Pickme",
                OrderID = getOrder.PickmeJobID
            };
            int OrderKey = await _orderManager.GetPickMeOrderByOrderID(request);
            if (OrderKey > 11)
            {
                appStateService.IsLoaded = false;
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("Order already available in Order Hub", Severity.Info);
            }
            else
            {
                PickmeAPIHandler pickme = new PickmeAPIHandler(_apiManager, _orderManager, _addressManager, _codebaseManager);
                bool success = await pickme.SaveSingleOrder(Location.CodeKey, BU.CodeKey, getOrder, ObjKy);
                if (success)
                {
                    appStateService.IsLoaded = false;
                    _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                    _snackBar.Add("Order has been saved successfully", Severity.Success);
                }
                else
                {
                    appStateService.IsLoaded = false;
                    _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                    _snackBar.Add("Something went wrong", Severity.Error);
                }
            }
        }

        #endregion
    }
}
