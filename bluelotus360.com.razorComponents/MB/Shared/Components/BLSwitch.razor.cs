using BL10.CleanArchitecture.Domain.Entities.ProjectProfileMobile;
using bluelotus360.com.razorComponents.Extensions;
using BlueLotus360.CleanArchitecture.Domain;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.razorComponents.MB.Shared.Components
{
    public partial class BLSwitch : IBLUIOperationHelper
    {
        [Parameter]
        public BLUIElement FromSection { get; set; }

        [Parameter]
        public object DataObject { get; set; }

        [Parameter]
        public IDictionary<string, EventCallback> InteractionLogics { get; set; }

        [Parameter]
        public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }

        public BLUIElement LinkedUIObject => throw new NotImplementedException();

        private string css_class = "";
        private bool IsSwitchOn;
        private string swt_css = "";
        private bool __forcerender = false;
        private bool IsSwitchDisable;
        private PropertyConversionResponse<bool> conversionInfo;

        protected override void OnInitialized()
        {
            css_class = (FromSection.IsVisible ? "d-flex" : "d-none");
            swt_css = (FromSection.IsVisible ? FromSection.CssClass : "");
            if (!string.IsNullOrEmpty(FromSection.DefaultValue))
            {
                IsSwitchOn = Convert.ToBoolean(FromSection.DefaultValue);
            }
            else
            {
                IsSwitchOn = false;
            }
            if (!string.IsNullOrEmpty(FromSection.DefaultAccessPath))
            {
                DataObject.SetValueByObjectPath(FromSection.DefaultAccessPath, IsSwitchOn);
            }
            
            
            if (ObjectHelpers != null)
            {
                if (ObjectHelpers.ContainsKey(FromSection.ElementName))
                {
                    ObjectHelpers.Remove(FromSection.ElementName);

                }
                ObjectHelpers.Add(FromSection.ElementName, this);
            }
            base.OnInitialized();
        }

        protected override void OnParametersSet()
        {
            conversionInfo = DataObject.GetPropObject<bool>(FromSection.DefaultAccessPath);
            if (conversionInfo.IsConversionSuccess)
            {
                IsSwitchOn = conversionInfo.Value;
            }

            base.OnParametersSet();
        }

        private async void SwitchValueChanged(bool value)
        {
            UIInterectionArgs<bool> args = new UIInterectionArgs<bool>();
            try
            {
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

                if (!args.CancelChange)
                {
                    IsSwitchOn = value;
                }
                else
                {
                    if (args.OverrideValue)
                    {
                        IsSwitchOn = args.OverriddenValue;
                        DataObject.SetValueByObjectPath(FromSection.DefaultAccessPath, IsSwitchOn);
                    }
                }
            }
            catch(Exception ex)
            {

            }
            
        }

        public Task FocusComponentAsync()
        {
            throw new NotImplementedException();
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

        public void ResetToInitialValue()
        {
            if (!string.IsNullOrEmpty(FromSection.DefaultValue))
            {
                IsSwitchOn = Convert.ToBoolean(FromSection.DefaultValue);
            }
            else
            {
                IsSwitchOn = false;
            }
        }

        public async Task SetValue(object value)
        {
            this.IsSwitchOn = bool.Parse(value.ToString());
            this.StateHasChanged();
            await Task.CompletedTask;
        }

        public void ToggleEditable(bool IsEditable)
        {
            IsSwitchDisable = !IsEditable;
            this.StateHasChanged();
        }

        public void UpdateVisibility(bool IsVisible)
        {
            this.FromSection.IsVisible = IsVisible;
            css_class = FromSection.IsVisible ? "d-flex" : "d-none" + " align-end";
        }
    }
}
