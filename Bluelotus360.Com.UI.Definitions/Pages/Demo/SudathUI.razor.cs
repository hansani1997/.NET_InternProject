using BlueLotus360.CleanArchitecture.Client.Infrastructure.Helpers;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using BlueLotus360.Com.UI.Definitions.Extensions;
using BlueLotus360.Com.UI.Definitions.MB.Settings;
using BlueLotus360.Com.UI.Definitions.MB.Shared.Components;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Com.UI.Definitions.Pages.Demo
{
    public partial class SudathUI
    {
        #region parameter

        private BLUIElement formDefinition;
        private BLTransaction transaction = new();
        private BLUIElement findOrderUI;
        private IDictionary<string, EventCallback> _interactionLogic;
        private IDictionary<string, BLUIElement> _modalDefinitions;
        private IDictionary<string, BLUIElement> _activeModalDefinitions;

        BLUIElement modalUIElement;

        private IDictionary<string, IBLUIOperationHelper> _objectHelpers;

        private UIBuilder _refBuilder;
        private BLTabPage _bLTab;
        private bool FindOrderShown = false;

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

                if (formDefinition != null)
                {
                    formDefinition.IsDebugMode = false;
                    if (formDefinition.Children != null && formDefinition.Children.Count() > 0)
                    {
                        findOrderUI = formDefinition.Children.Where(x => x._internalElementName.Equals("DemoItemPopUp")).FirstOrDefault();
                    }

                }


            }

            _interactionLogic = new Dictionary<string, EventCallback>();
            _modalDefinitions = new Dictionary<string, BLUIElement>();
            _objectHelpers = new Dictionary<string, IBLUIOperationHelper>();
            _activeModalDefinitions = new Dictionary<string, BLUIElement>();

            HookInteractions();

        }

        private void HookInteractions()
        {

            InteractionHelper helper = new InteractionHelper(this, formDefinition);//formdefinition has all form objects 
            _interactionLogic = helper.GenerateEventCallbacks();// generate all event callbacks


        }

        #endregion

        private async void OnFindButtonClick(UIInterectionArgs<object> args)
        {
            FindOrderShown = true;
            this.StateHasChanged();
        }
        //private async void OnSelectedLocationChangedPopUp(UIInterectionArgs<CodeBaseResponse> args)
        //{

        //}

    }
}
