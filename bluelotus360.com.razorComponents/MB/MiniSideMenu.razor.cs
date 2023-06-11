using BlueLotus360.CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.razorComponents.MB
{
    public partial class MiniSideMenu
    {
		[Parameter] public MenuItem Menu { get; set; }
		public string IconSvgCode { get; private set; }
		[Parameter] public IDictionary<int, string> IconDictionary { get; set; }
		protected override Task OnParametersSetAsync()
		{
			//GetIconByStringName(this.Menu.MenuIcon, typeof(Icons));
			return base.OnParametersSetAsync();
		}


		public async Task NavigateToNewTab(string URL)
		{
			if (!string.IsNullOrEmpty(URL))
			{
				string url = URL;
				_navigationManager.NavigateTo(url);
				//await _jsRuntime.InvokeVoidAsync("open", url, "_blank");
			}


		}
	}
}
