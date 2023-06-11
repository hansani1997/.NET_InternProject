using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using bluelotus360.com.razorComponents.Extensions;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bluelotus360.Com.commonLib.Helpers;
using MudBlazor;
using bluelotus360.com.razorComponents.MB.Shared.Components;

namespace bluelotus360.com.razorComponents.Pages.SetUp.Configuration
{
    public partial class UserConfiguration
    {
        [CascadingParameter(Name = "NavMenus")]
        protected MenuItem ChildMenu { get; set; }
        private BLUIElement formDefinition;
        private IDictionary<string, EventCallback> _interactionLogic;
        private IDictionary<string, BLUIElement> _modalDefinitions;
        private IDictionary<string, IBLUIOperationHelper> _objectHelpers;
        private long elementKey;
        private long selectedElementObjKy = 1;
        private UserConfigObjectsBlLite userConfigObject;
        BLUIElement userConfigTable;
        //private BLTelGrid<UserConfigObjectsBlLite> _blTb = new BLTelGrid<UserConfigObjectsBlLite>();
        private BLTable<UserConfigObjectsBlLite> _mudGrid = new BLTable<UserConfigObjectsBlLite>();

        protected override async Task OnInitializedAsync()
        {
            elementKey = 1;
            _navigationManager.TryGetQueryString<long>("ElementKey", out elementKey);
            if (elementKey > 10)
            {
                var formrequest = new ObjectFormRequest();
                formrequest.MenuKey = elementKey;
                formDefinition = await _navManger.GetMenuUIElement(formrequest);
            }
            if (formDefinition != null)
            {
                userConfigTable = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("DetailsTable")).FirstOrDefault();
                formDefinition.IsDebugMode = true;

            }
            if (userConfigTable != null)
            {
                userConfigTable.Children = formDefinition.Children.Where(x => x.ParentKey == userConfigTable.ElementKey).ToList();
            }

            _interactionLogic = new Dictionary<string, EventCallback>();
            _modalDefinitions = new Dictionary<string, BLUIElement>();
            _objectHelpers = new Dictionary<string, IBLUIOperationHelper>();
            HookInteractions();
        }

        private void HookInteractions()
        {
            InteractionHelper helper = new InteractionHelper(this, formDefinition);
            _interactionLogic = helper.GenerateEventCallbacks(); ;
            appStateService._AppBarName = "User Configuration";
        }

        private void InitializeForm()
        {
            userConfigObject = new UserConfigObjectsBlLite();
            userConfigObject.Children.Add(new UserConfigObjectsBlLite());
            ChildMenu = new MenuItem();
            userConfigTable = new BLUIElement();
        }


        #region ui events
        //save btn
        private async void OnSaveClick(UIInterectionArgs<object> args)
        {
            try
            {
                this.appStateService.IsLoaded = true;
                await _navManger.UpdateObjectsForUserConfiguration(userConfigObject);
                this.appStateService.IsLoaded = false;
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("Component is Updated Successfully", Severity.Success);
                userConfigObject = await LoadGrid(selectedElementObjKy);

                this.StateHasChanged();

            }
            catch (Exception ex)
            {
                this.appStateService.IsLoaded = false;
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("Error Occured", Severity.Error);
            }


        }

        private async void OnCodebaseChange(UIInterectionArgs<CodeBaseResponse> args)
        {
            if (userConfigObject != null && userConfigObject.Children != null)
            {
                int index = userConfigObject.Children.ToList().FindIndex(x => x.ElementKey == args.InitiatorObject.ElementKey);
                if (index > 0)
                {
                    userConfigObject.Children[index].DefaultValue = args.DataObject.CodeKey.ToString();
                }
            }
        }
        private async void OnItemChange(UIInterectionArgs<ItemResponse> args)
        {
            if (userConfigObject != null && userConfigObject.Children != null)
            {
                int index = userConfigObject.Children.ToList().FindIndex(x => x.ElementKey == args.InitiatorObject.ElementKey);
                if (index > 0)
                {
                    userConfigObject.Children[index].DefaultValue = args.DataObject.ItemKey.ToString();
                }


            }
        }
        private async void OnAddressChange(UIInterectionArgs<AddressResponse> args)
        {
            if (userConfigObject != null && userConfigObject.Children != null)
            {
                int index = userConfigObject.Children.ToList().FindIndex(x => x.ElementKey == args.InitiatorObject.ElementKey);
                if (index > 0)
                {
                    userConfigObject.Children[index].DefaultValue = args.DataObject.AddressKey.ToString();
                }
            }
        }

        private async void OnUnitChange(UIInterectionArgs<UnitResponse> args)
        {
            if (userConfigObject != null && userConfigObject.Children != null)
            {
                int index = userConfigObject.Children.ToList().FindIndex(x => x.ElementKey == args.InitiatorObject.ElementKey);
                if (index > 0)
                {
                    userConfigObject.Children[index].DefaultValue = args.DataObject.UnitKey.ToString();
                }
            }
        }

        private async void OnTextBoxChange(UIInterectionArgs<string> args)
        {
            if (userConfigObject != null && userConfigObject.Children != null)
            {
                int index = userConfigObject.Children.ToList().FindIndex(x => x.ElementKey == args.InitiatorObject.ElementKey);
                if (index > 0)
                {
                    userConfigObject.Children[index].DefaultValue = args.DataObject.ToString();
                }
            }
        }
        private async void OnNumericBoxChange(UIInterectionArgs<decimal> args)
        {
            if (userConfigObject != null && userConfigObject.Children != null)
            {
                int index = userConfigObject.Children.ToList().FindIndex(x => x.ElementKey == args.InitiatorObject.ElementKey);
                if (index > 0)
                {
                    userConfigObject.Children[index].DefaultValue = args.DataObject.ToString();
                }
            }
        }
        private async void OnDateChange(UIInterectionArgs<DateTime?> args)
        {
            if (userConfigObject != null && userConfigObject.Children != null)
            {
                int index = userConfigObject.Children.ToList().FindIndex(x => x.ElementKey == args.InitiatorObject.ElementKey);
                if (index > 0)
                {
                    userConfigObject.Children[index].DefaultValue = args.DataObject.ToString();
                }
            }
        }
        private async void IsVisibleChange(UIInterectionArgs<bool> args)
        {
            if (userConfigObject != null && userConfigObject.Children != null)
            {
                int index = userConfigObject.Children.ToList().FindIndex(x => x.ElementKey == args.InitiatorObject.ElementKey);
                if (index > 0)
                {
                    userConfigObject.Children[index].IsVisible = args.DataObject;
                }
            }

            this.StateHasChanged();
        }
        private async void IsEnableChange(UIInterectionArgs<bool> args)
        {
            if (userConfigObject != null && userConfigObject.Children != null)
            {
                int index = userConfigObject.Children.ToList().FindIndex(x => x.ElementKey == args.InitiatorObject.ElementKey);
                if (index > 0)
                {
                    userConfigObject.Children[index].IsEnable = args.DataObject;
                }
            }
            this.StateHasChanged();
        }
        private async void IsCd01Change(UIInterectionArgs<bool> args)
        {
            if (userConfigObject != null && userConfigObject.Children != null)
            {
                int index = userConfigObject.Children.ToList().FindIndex(x => x.ElementKey == args.InitiatorObject.ElementKey);
                if (index > 0)
                {
                    userConfigObject.Children[index].IsCd01 = args.DataObject;
                }
            }
            this.StateHasChanged();
        }
        private async void IsCd02Change(UIInterectionArgs<bool> args)
        {
            if (userConfigObject != null && userConfigObject.Children != null)
            {
                int index = userConfigObject.Children.ToList().FindIndex(x => x.ElementKey == args.InitiatorObject.ElementKey);
                if (index > 0)
                {
                    userConfigObject.Children[index].IsCd02 = args.DataObject;
                }
            }
            this.StateHasChanged();
        }

        #endregion

        #region gridEvents

        //searching menu item
        private async void MenuSearchComboChanged(MenuItem menu)
        {
            selectedElementObjKy = menu.MenuId;
            userConfigObject = await LoadGrid(selectedElementObjKy);


            this.StateHasChanged();
        }

        //after searching, grid laoding
        private async Task<UserConfigObjectsBlLite> LoadGrid(long objKy)
        {
            ObjectFormRequest obreq = new ObjectFormRequest() { MenuKey = objKy };
            return await _navManger.LoadObjectsForUserConfiguration(obreq);

        }

        #endregion
    }


}
