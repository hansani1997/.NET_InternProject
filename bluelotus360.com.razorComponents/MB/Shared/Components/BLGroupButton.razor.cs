using bluelotus360.com.razorComponents.Extensions;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.razorComponents.MB.Shared.Components
{
    public partial class BLGroupButton
    {
		private MudButtonGroup buttonGroupRef; // field to hold the reference to the MudButtonGroup component
		[Parameter]
        public BLUIElement FromSection { get; set; }

        [Parameter]
        public object DataObject { get; set; }

        [Parameter]
        public IDictionary<string, EventCallback> InteractionLogics { get; set; }

        [Parameter]
        public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }

        private string css_class = "";

        protected override async Task OnInitializedAsync()
        {
            css_class = (FromSection.IsVisible ? "d-flex " : "d-none ") + FromSection.CssClass;
            await base.OnInitializedAsync();
        }
		

	}
}
