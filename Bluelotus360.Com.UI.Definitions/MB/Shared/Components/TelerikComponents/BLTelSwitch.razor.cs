
using BlueLotus360.CleanArchitecture.Domain;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.Com.UI.Definitions.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlueLotus360.Com.UI.Definitions.MB.Shared.Components.TelerikComponents
{
    public partial class BLTelSwitch:IBLUIOperationHelper
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
        private string swt_css = "";
        private bool IsSwitchOn;

        protected override void OnInitialized()
        {
            css_class = (FromSection.IsVisible ? "d-flex" : "d-none") ;
            swt_css = (FromSection.IsVisible ? FromSection.CssClass : "");
            IsSwitchOn = Convert.ToBoolean(FromSection.DefaultValue); ;

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
            DataObject.SetValueByObjectPath(FromSection.DefaultAccessPath, value);

            if (InteractionLogics != null && FromSection.OnClickAction != null && FromSection.OnClickAction.Length > 3)
            {
                EventCallback callback;

                if (InteractionLogics.TryGetValue(FromSection.OnClickAction, out callback))
                {

                    if (callback.HasDelegate)
                    {
                        IsSwitchOn = !IsSwitchOn;
                        args.Caller = this.FromSection.OnClickAction;
                        args.ObjectPath = this.FromSection.DefaultAccessPath;
                        args.DataObject = value;
                        args.sender = this;
                        args.InitiatorObject = FromSection;
                        await callback.InvokeAsync(args);

                        if (!args.CancelChange)
                        {
                            IsSwitchOn = value;
                        }
                    }
                    else
                    {
                        IsSwitchOn = value;
                    }

                }
            }
            IsSwitchOn = value;
        }

        public void ResetToInitialValue()
        {
           
        }

        public void UpdateVisibility(bool IsVisible)
        {
            
        }

        public void ToggleEditable(bool IsEditable)
        {
        
        }

        public async Task Refresh()
        {
            PropertyConversionResponse<bool> propertyConversion = DataObject.GetPropObject<bool>(FromSection.DefaultAccessPath);
            if (propertyConversion.IsConversionSuccess)
            {
                IsSwitchOn = propertyConversion.Value;
            }
            await Task.CompletedTask;
        }

        public async Task SetValue(object value)
        {
            this.IsSwitchOn = bool.Parse(value.ToString());
            this.StateHasChanged();
            await Task.CompletedTask;
        }

        public async Task FocusComponentAsync()
        {
            await Task.CompletedTask;
        }
    }
}
