using BL10.CleanArchitecture.Domain.Entities.WorkShopManagement;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.Com.UI.Definitions.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Com.UI.Definitions.Pages.WorkshopManagement.InsuranceCarmart.Component
{
    public partial class FinalizedIRN
    {
        [Parameter] public EventCallback<int> Activate { get; set; }
        [Parameter] public EventCallback EnableTabs { get; set; }
        [Parameter] public EventCallback DisableTabs { get; set; }
        [Parameter] public BLUIElement UIScope { get; set; }

        [Parameter] public WorkOrder DataObject { get; set; }
        [Parameter] public IDictionary<string, EventCallback> InteractionLogic { get; set; }

        [Parameter] public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }
		private string[] hiddenArray;
		private string[] displayedArray;
		private IList<WorkOrder> FinalizedIRNs = new List<WorkOrder>();
		private string RowColor { get; set; } = "#1DA92B";
		#region general
		protected override async Task OnParametersSetAsync()
        {
            base.OnParametersSetAsync();
        }
        protected override async Task OnInitializedAsync()
        {
			InitializeFinalizedIRN();		

		}
		private void InitializeFinalizedIRN()
		{
			hiddenArray = new string[] { };
			displayedArray = new string[] { };
		}
		

		

		#endregion

		#region ui event
		private async void OnFinalizedIRNEditClick()
		{
			PageShowHide(new string[] { "irn-Finalized-view" }, new string[] { "irn-Finalized-edit" });
			StateHasChanged();
		}
		private async void OnFinalizedIRNDeleteClick()
		{

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

		public async void FinalizedIRNViewEvent()
		{
			if (DataObject.IsInWorkOrderEditMode)
			{
                PageShowHide(new string[] { "irn-Finalized-view" }, new string[] { "irn-Finalized-edit" });
            }
			else
			{
				PageShowHide(new string[] { "irn-Finalized-edit" }, new string[] { "irn-Finalized-view" });
				WorkOrder wobj = new WorkOrder() { OrderStatus = new CodeBaseResponse() { OurCode = "Finalized" } };
				wobj.FormObjectKey = UIScope.ElementKey;

				FinalizedIRNs = await _workshopManager.GetPendingIRNs(wobj);
			}
			StateHasChanged();
		}
	}
}
