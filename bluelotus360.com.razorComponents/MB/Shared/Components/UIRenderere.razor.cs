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
    public partial class UIRenderere : IBLUIOperationHelper
    {
        [Parameter]
        public BLUIElement FromSection { get; set; }

        [Parameter]
        public object DataObject { get; set; }

        [Parameter]
        public IDictionary<string, EventCallback> InteractionLogics { get; set; }

        [Parameter]
        public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }
        [Parameter] public IDictionary<string, string> DynamicBindings { get; set; }

        public BLUIElement LinkedUIObject { get; private set; }
        public Task FocusComponentAsync()
        {
            throw new System.NotImplementedException();
        }

        public void OnSectionOK()
        {

        }

        public async Task Refresh()
        {
            await Task.CompletedTask;
        }

        public void ResetToInitialValue()
        {
            throw new System.NotImplementedException();
        }

        public Task SetValue(object value)
        {
            throw new System.NotImplementedException();
        }

        public void ToggleEditable(bool IsEditable)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateVisibility(bool IsVisible)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnInitialized()
        {

            base.OnInitialized();
        }
    }
}
