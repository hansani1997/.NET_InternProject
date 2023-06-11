
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.Com.UI.Definitions.Extensions;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlueLotus360.Com.UI.Definitions.MB.Shared.Components
{
    public partial class BLSwitch:IBLUIOperationHelper
    {
        [Parameter]
        public BLUIElement FromSection { get; set; }

        [Parameter]
        public object DataObject { get; set; }

        [Parameter]
        public IDictionary<string, EventCallback> InteractionLogics { get; set; }

        [Parameter]
        public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }

        public BLUIElement LinkedUIObject => throw new System.NotImplementedException();

        private string css_class = "";
        private bool check;

        protected override void OnInitialized()
        {
            css_class = (FromSection.IsVisible ? "d-flex" : "d-none") ;
            check = false;

            base.OnInitialized();
        }

        protected override void OnParametersSet()
        {
            if (ObjectHelpers != null)
            {
                if (ObjectHelpers.ContainsKey(FromSection.ElementName))
                {
                    ObjectHelpers.Remove(FromSection.ElementName);

                }
                ObjectHelpers.Add(FromSection.ElementName, this);
            }

            base.OnParametersSet();
        }

        private async void SwitchValueChanged(bool value)
        {
            UIInterectionArgs<bool> args = new UIInterectionArgs<bool>();

            if (InteractionLogics != null && FromSection.OnClickAction != null && FromSection.OnClickAction.Length > 3)
            {
                EventCallback callback;

                if (InteractionLogics.TryGetValue(FromSection.OnClickAction, out callback))
                {

                    if (callback.HasDelegate)
                    {
                        check = !check;
                        args.Caller = this.FromSection.OnClickAction;
                        args.ObjectPath = this.FromSection.DefaultAccessPath;
                        args.DataObject = value;
                        args.sender = this;
                        args.InitiatorObject = FromSection;
                        await callback.InvokeAsync(args);
                    }
                    else
                    {

                    }

                }
            }
        }

        public void ResetToInitialValue()
        {
            throw new System.NotImplementedException();
        }

        public void UpdateVisibility(bool IsVisible)
        {
            throw new System.NotImplementedException();
        }

        public void ToggleEditable(bool IsEditable)
        {
            throw new System.NotImplementedException();
        }

        public Task Refresh()
        {
            throw new System.NotImplementedException();
        }

        public async Task SetValue(object value)
        {
            this.check = bool.Parse(value.ToString());
            this.StateHasChanged();
            await Task.CompletedTask;
        }

        public Task FocusComponentAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}
