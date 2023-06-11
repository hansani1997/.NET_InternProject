using BL10.CleanArchitecture.Domain.Entities.WorkShopManagement;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Com.UI.Definitions.Pages.WorkshopManagement.InsuranceCarmart.Component
{
	public partial class IRNJobHistory
	{
		[Parameter] public WorkOrder DataObject { get; set; }
		[Parameter] public long ObjectKey { get; set; }
		[Parameter] public EventCallback<OrderOpenRequest> LoadWorkOrder { get; set; }

		bool isloading;

		private async void OpenWorkOrder(long ordKy)
		{
			OrderOpenRequest req = new OrderOpenRequest() { OrderKey = ordKy, ObjKy = ObjectKey };

			if (LoadWorkOrder.HasDelegate)
			{
				await LoadWorkOrder.InvokeAsync(req);
			}

		}

	}
}
