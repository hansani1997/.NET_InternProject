using ApexCharts;
using BL10.CleanArchitecture.Domain.Entities.WorkShopManagement;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.Com.UI.Definitions.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Com.UI.Definitions.Pages.WorkshopManagement.InsuranceCarmart.Component
{
    public partial class PendingIRN
    {
        [Parameter] public EventCallback<int> Activate { get; set; }
        [Parameter] public EventCallback EnableTabs { get; set; }
        [Parameter] public EventCallback DisableTabs { get; set; }
        [Parameter] public BLUIElement UIScope { get; set; }

        [Parameter] public WorkOrder DataObject { get; set; }
        [Parameter] public IDictionary<string, EventCallback> InteractionLogic { get; set; }

        [Parameter] public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }
		private string RowColor { get; set; } = "#EE3810";
		private string[] hiddenArray;
        private string[] displayedArray;
		private IList<WorkOrder> PendingIRNs =new List<WorkOrder>(); 

        #region general
        protected override async Task OnParametersSetAsync()
        {
			base.OnParametersSetAsync();
        }
        protected override async Task OnInitializedAsync()
        {
            InitializePendingIRN();           
        }
        private void InitializePendingIRN()
        {
            hiddenArray = new string[] { };
            displayedArray = new string[] { };
        }

		#endregion

		#region ui event
		private async void OnPendingIRNEditClick() 
        {
			PageShowHide(new string[] { "irn-pending-view", "irn-pending-finalized", "irn-pending-btn" }, new string[] { "irn-pending-edit", "add-materials-services-btn", "pending-proceed-btn" });
			StateHasChanged();
        }
        private async void OnPendingIRNDeleteClick()
        { 

            StateHasChanged();
        }
		private async void OnAddMaterialAndServices()
		{
			if (Activate.HasDelegate)
			{
				await Activate.InvokeAsync(1);
				StateHasChanged();
			}
			StateHasChanged();
		}
		private async void OnProceed()
		{
			PageShowHide(new string[] { "irn-pending-edit", "add-materials-services-btn", "pending-proceed-btn", "irn-pending-view" }, new string[] { "irn-pending-finalized", "irn-pending-btn" });
			DataObject.IsInWorkOrderEditMode = true;
            StateHasChanged();
		}
		private async void OnPendingPrint()
		{

			StateHasChanged();
		}
		private async void OnFinalizeforApproval()
		{
			if (Activate.HasDelegate)
			{
				await Activate.InvokeAsync(3);
				StateHasChanged();
			}
			StateHasChanged();
			StateHasChanged();
		}

		#endregion

		#region ui logics

		private async void PageShowHide(string[] hidden, string[] displayed)
        {
            foreach (string div in hidden)
            {
                await _jsRuntime.InvokeVoidAsync("HideDiv", div);
            }
            foreach (string div in displayed)
            {
                await _jsRuntime.InvokeVoidAsync("ShowDiv", div);
            }
            StateHasChanged();
        }

		#endregion

		public async void PendingIRNViewEvent() 
		{
			if (DataObject.IsInWorkOrderEditMode)
			{
                PageShowHide(new string[] { "irn-pending-view", "irn-pending-finalized", "irn-pending-btn" }, new string[] { "irn-pending-edit", "add-materials-services-btn", "pending-proceed-btn" });
            }
			else
			{
				PageShowHide(new string[] { "irn-pending-edit", "add-materials-services-btn", "pending-proceed-btn", "irn-pending-finalized", "irn-pending-btn" }, new string[] { "irn-pending-view" });
				WorkOrder wobj = new WorkOrder() { OrderStatus = new CodeBaseResponse() { OurCode = "Pending" } };
				wobj.FormObjectKey = UIScope.ElementKey;

				PendingIRNs = await _workshopManager.GetPendingIRNs(wobj);
			}
            
			StateHasChanged();
        }
    }
}
