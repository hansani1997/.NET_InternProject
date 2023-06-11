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
	public partial class BLTypo : IBLUIOperationHelper
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
		Headings _heading;
		private string Caption = "";
		//private string MappedValue = "- New";
		private string MappedValue = "";
		public BLUIElement LinkedUIObject { get; private set; }
		protected override Task OnInitializedAsync()
		{
			css_class = (FromSection.IsVisible ? "d-flex" : "d-none");
			Caption = FromSection.ElementCaption;
			MappedValue = FromSection.DefaultValue ?? "";
			Enum.TryParse(FromSection.ElementType, out Headings heading1);
			_heading = heading1;
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

		MudBlazor.Typo Typos => _heading switch
		{
			Headings.Heading1 => MudBlazor.Typo.h1,
			Headings.Heading2 => MudBlazor.Typo.h2,
			Headings.Heading3 => MudBlazor.Typo.h3,
			Headings.Heading4 => MudBlazor.Typo.h4,
			Headings.Heading5 => MudBlazor.Typo.h5,
			Headings.Heading6 => MudBlazor.Typo.h6,
			_ => MudBlazor.Typo.overline,

		};

		public enum Headings
		{
			Heading1,
			Heading2,
			Heading3,
			Heading4,
			Heading5,
			Heading6,

		}
	}
}
