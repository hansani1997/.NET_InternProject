using bluelotus360.com.razorComponents.Extensions;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.razorComponents.MB.Shared.Components
{
	public partial class BLHeaderTitle : IBLUIOperationHelper
	{
		[Parameter]
		public BLUIElement FromSection { get; set; }

		[Parameter]
		public object DataObject { get; set; }

		[Parameter]
		public IDictionary<string, EventCallback> InteractionLogics { get; set; }

		[Parameter]
		public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }

		private string css_class = "";

		private string Caption = "";
		private string MappedValue = "- New";
		public BLUIElement LinkedUIObject { get; private set; }
		protected override Task OnInitializedAsync()
		{
			css_class = (FromSection.IsVisible ? "d-flex" : "d-none");
			Caption = FromSection.ElementCaption;

			if (ObjectHelpers != null && ObjectHelpers.ContainsKey(FromSection.ElementName))
			{
				ObjectHelpers.Remove(FromSection.ElementName);
			}

			if (ObjectHelpers != null)
			{
				ObjectHelpers.Add(FromSection.ElementName, this);
			}

			return base.OnInitializedAsync();
		}

		public void ResetToInitialValue()
		{
			throw new NotImplementedException();
		}

		public void UpdateVisibility(bool IsVisible)
		{
			throw new NotImplementedException();
		}

		public void ToggleEditable(bool IsEditable)
		{
			throw new NotImplementedException();
		}

		public async Task Refresh()
		{
			await Task.CompletedTask;
		}


		public async Task SetValue(object value)
		{
			this.MappedValue = " - " + value.ToString();
			StateHasChanged();
			await Task.CompletedTask;
		}

		public Task FocusComponentAsync()
		{
			throw new NotImplementedException();
		}
	}
}
