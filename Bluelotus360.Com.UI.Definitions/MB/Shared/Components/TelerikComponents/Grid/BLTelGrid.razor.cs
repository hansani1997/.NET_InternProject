using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.Com.UI.Definitions.Extensions;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlueLotus360.Com.UI.Definitions.MB.Shared.Components.TelerikComponents.Grid
{
    public partial class BLTelGrid<T>
    {
        [Parameter] public BLUIElement FormObject { get; set; }
        [Parameter] public IDictionary<string, EventCallback> InteractionLogics { get; set; }
        [Parameter] public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }
        [Parameter] public IList<T> DataObject { get; set; }
        [Parameter] public string Height { get; set; }

        private BLTelerikKendoGrid<T> _refGrid;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
        }

        public void Refresh()
        {
            _refGrid?.Refresh();


        }
    }
}
