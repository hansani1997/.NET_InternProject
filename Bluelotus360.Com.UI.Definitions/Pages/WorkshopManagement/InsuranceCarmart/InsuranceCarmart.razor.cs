using BL10.CleanArchitecture.Domain.Entities.WorkShopManagement;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.Com.UI.Definitions.Extensions;
using BlueLotus360.Com.UI.Definitions.Pages.WorkshopManagement.InsuranceCarmart.Component;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Com.UI.Definitions.Pages.WorkshopManagement.InsuranceCarmart
{
    public partial class InsuranceCarmart
    {
        #region parameter

        private BLUIElement formDefinition;
        private IDictionary<string, EventCallback> _interactionLogic;
        private IDictionary<string, BLUIElement> _modalDefinitions;
        private IDictionary<string, IBLUIOperationHelper> _objectHelpers;
        private WorkOrder _insurenceOrderObj;
        MudTabs tabs = new MudTabs();
        private bool IsTabDisabled0 = false;
        private bool IsTabDisabled1 = true;
        private bool IsTabDisabled2 = false;
        long elementKey;

        PendingIRN _irnPending = new PendingIRN();
        ProcessingIRN _irnProcessing = new ProcessingIRN();
        FinalizedIRN _irnFinalized = new FinalizedIRN();

        #endregion

        #region General

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
                formDefinition.IsDebugMode = true;

            }

            _interactionLogic = new Dictionary<string, EventCallback>();
            _modalDefinitions = new Dictionary<string, BLUIElement>();
            _objectHelpers = new Dictionary<string, IBLUIOperationHelper>();

            InitilizeWorkOrder();
            HookInteractions();
        }
        private async void InitilizeWorkOrder()
        {

            _insurenceOrderObj = new WorkOrder();
        }

        private void HookInteractions()
        {
            //InteractionHelper helper = new InteractionHelper(this, formDefinition);
            //_interactionLogic = helper.GenerateEventCallbacks();
            appStateService._AppBarName = "Work Order";

        }

        #endregion

        #region tab activate 
        private async void Activate(int index)
        {
            if (index == 0) 
            {
                tabs.ActivatePanel(index, true);
            }
            if (index == 1)
            {
                tabs.ActivatePanel(index, true);
                IsTabDisabled1 = false;
                IsTabDisabled0 = true;
            }
            if (index == 2) 
            {
                tabs.ActivatePanel(index, true);
            }
            if (index == 3)
            {
                tabs.ActivatePanel(index, true);
                
            }
            if (index == 4)
            {
                tabs.ActivatePanel(index, true);
            }

            StateHasChanged();
        }
        private async void TabActivator(int index)
        {
            if (index == 2)
            {
                _irnPending.PendingIRNViewEvent();
            }
            if (index == 3)
            {
                _irnProcessing.ProcessingIRNViewEvent();
            }
            if (index == 4)
            {
				_irnFinalized.FinalizedIRNViewEvent();
			}
        }
        private void EnableAllTabs()
        {
           
            StateHasChanged();
        }

        private void DisableAllTabs()
        {

            StateHasChanged();
        }

        #endregion

    }
}
