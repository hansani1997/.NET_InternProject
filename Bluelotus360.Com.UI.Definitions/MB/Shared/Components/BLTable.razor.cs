
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.Com.UI.Definitions.Extensions;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlueLotus360.Com.UI.Definitions.MB.Shared.Components
{
    public partial class BLTable<T>
    {   
        [Parameter] public BLUIElement FormObject { get; set; }
        [Parameter] public IDictionary<string, EventCallback> InteractionLogics { get; set; }
        [Parameter] public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }
        [Parameter] public IList<T> DataObject { get; set; }
        [Parameter] public string Height { get; set; }

        private UIGrid<T> _refGrid;

        protected override async Task OnParametersSetAsync() { 
            await base.OnParametersSetAsync();
        }
    }
}
