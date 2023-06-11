using BL10.CleanArchitecture.Domain.Entities.WorkShopManagement;
using BlueLotus360.CleanArchitecture.Client.Infrastructure.Helpers;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.Com.UI.Definitions.Extensions;
using BlueLotus360.Com.UI.Definitions.Pages.WorkshopManagement.WorkshopCarmart.Component;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace BlueLotus360.Com.UI.Definitions.Pages.WorkshopManagement.WorkshopCarmart
{
    public partial class WorkShopCarmart
    {
        #region parameter
        private BLUIElement formDefinition;
        private BLUIElement modalUIElement;
        private BLUIElement editUIElement;
        private IDictionary<string, EventCallback> _interactionLogic;
        private IDictionary<string, BLUIElement> _modalDefinitions;
        private IDictionary<string, IBLUIOperationHelper> _objectHelpers;
        private WorkOrder _workOrderObj;
        long elementKey;
        MudTabs tabs=new MudTabs();
        private bool IsTabDisabled1=true, IsTabDisabled2 = true, IsTabDisabled3 = true;
        JobMaterialServices jobms_ref = new JobMaterialServices();
        JobSummery job_sum = new JobSummery();
        private int _previousTabIndex = 0;

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


            HookInteractions();
            InitilizeWorkOrder();




        }

        private async void InitilizeWorkOrder()
        {

            _workOrderObj=new WorkOrder();  
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
                
            }
            if (index==2)
            {
                IsTabDisabled2 = false;
                
            }
            if (index==3)
            {
                tabs.ActivatePanel(index,true);
                IsTabDisabled3 = false;
                
            }

            
            StateHasChanged();
        }

        private async void TabActivator(int index)
        {

            if (index!=1)
            {
                jobms_ref.checkTabActivating(index);
            }
            if (index==1)
            {
                jobms_ref.VisibleElement();
            }
            if (index == 3)
            {
                job_sum.ShowSummery();
            }


        }

        private  void EnableAllTabs()
        {
            IsTabDisabled1 = false;
            IsTabDisabled2 = false;
            IsTabDisabled3 = false;

            StateHasChanged();
        }

        private void DisableAllTabs()
        {
            IsTabDisabled1 = true;
            IsTabDisabled2 = true;
            IsTabDisabled3 = true;

            StateHasChanged();
        }

        #endregion

        
    }
}
