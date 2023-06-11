using BlueLotus360.CleanArchitecture.Domain.Entities.HR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.razorComponents.MB.Shared.Components.Popups.HR
{
	public partial class LeaveBalanceZero
	{
		[CascadingParameter]
		MudDialogInstance MudDialog { get; set; }

		[Parameter]
		public string ButtonName { get; set; }

		[Parameter]
		public string HeadingPopUp { get; set; }

		[Parameter]
		public Leaverequest LeaveRequest { get; set; }
		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();
		}

		public void Cancel()
		{
			MudDialog.Cancel();
		}

		public void ClickYes()
		{
			MudDialog.Close(DialogResult.Ok(LeaveRequest));

		}
	}
}
