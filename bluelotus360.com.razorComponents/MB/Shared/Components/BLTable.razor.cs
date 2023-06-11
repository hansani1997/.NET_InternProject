using BlueLotus360.CleanArchitecture.Domain.Entities;
using bluelotus360.com.razorComponents.Extensions;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MudBlazor;

namespace bluelotus360.com.razorComponents.MB.Shared.Components
{
    public partial class BLTable<T>
    {
        [Parameter] public BLUIElement FormObject { get; set; }
        [Parameter] public IDictionary<string, EventCallback> InteractionLogics { get; set; }
        [Parameter] public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }
        [Parameter] public IList<T> DataObject { get; set; }
        [Parameter]public SortDirection SortDirection { get; set; }
        [Parameter] public string Height { get; set; }
        [Parameter] public int VersionNumber { get; set; } = 1;
        [Parameter] public bool IsServerFilterEnabled { get; set; }
        //[Parameter] public int GridCount { get; set; } = 0;
        private UIGrid<T> _refGrid;
		private UIGridV2<T> _refGridV2;

		protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
        }

        public void ServerFilterRefrersh()
        {
            _refGridV2.ServerFilter();
        }
    }
}
