using BlueLotus360.CleanArchitecture.Application.Validators.MessageSystem;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.razorComponents.MB.Shared.Components
{
	public partial class UserMessageDialog
	{
		[Parameter]
		public UserMessageManager Messages { get; set; } = new();

		[Parameter]
		public bool MessageShown { get; set; }

		public void ShowUserMessageWindow()
		{
			MessageShown = true;
			StateHasChanged();
		}
		public void HideWindow()
		{
			MessageShown = false;
			StateHasChanged();
		}
	}
}
