
using BlueLotus360.CleanArchitecture.Domain;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.Com.UI.Definitions.Extensions;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlueLotus360.Com.UI.Definitions.MB.Shared.Components.TelerikComponents
{
    public partial class BLTelTextArea : IBLUIOperationHelper
    {
        [Parameter]
        public BLUIElement UIElement { get; set; }

        [Parameter]
        public object DataObject { get; set; }

        [Parameter]
        public IDictionary<string, EventCallback> InteractionLogics { get; set; }

        [Parameter]
        public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }

        public BLUIElement LinkedUIObject { get; private set; }

        private int width = 12;

        private string TextValue = "";

        private string css_class = "";

        private string txar_css = "";

        private bool IsEnabled = true;

        protected override void OnInitialized()
        {
            css_class = (UIElement.IsVisible ? "d-flex" : "d-none") + " align-end ";
            txar_css = (UIElement.IsVisible ? UIElement.CssClass : "");
            base.OnInitialized();
        }

        protected override void OnParametersSet()
        {
            this.LinkedUIObject = UIElement;

            if (ObjectHelpers != null)
            {

                if (ObjectHelpers.ContainsKey(UIElement.ElementName))
                {
                    ObjectHelpers.Remove(UIElement.ElementName);

                }
                ObjectHelpers.Add(UIElement.ElementName, this);
            }

            base.OnParametersSet();
        }

        private async void OnBlTelTextAreaChanged(string value)
        {
            UIInterectionArgs<string> args = new UIInterectionArgs<string>();
            try
            {
                DataObject.SetValueByObjectPath(UIElement.DefaultAccessPath, value);

                if (InteractionLogics != null && UIElement.OnClickAction != null && UIElement.OnClickAction.Length > 3)
                {
                    EventCallback callback;

                    if (InteractionLogics.TryGetValue(UIElement.OnClickAction, out callback))
                    {

                        if (callback.HasDelegate)
                        {

                            args.Caller = this.UIElement.OnClickAction;
                            args.ObjectPath = this.UIElement.DefaultAccessPath;
                            args.DataObject = value;
                            args.sender = this;
                            args.InitiatorObject = UIElement;
                            await callback.InvokeAsync(args);
                        }
                        else
                        {

                        }

                    }
                }
                if (!args.CancelChange)
                {
                    TextValue = value;
                }
                else
                {
                    if (args.OverrideValue)
                    {
                        TextValue = args.OverriddenValue;
                        DataObject.SetValueByObjectPath(UIElement.DefaultAccessPath, TextValue);

                    }
                }

            }
            catch (Exception ex)
            {

            }
        }

        public void ResetToInitialValue()
        {
            this.TextValue = string.Empty;

            this.StateHasChanged();
        }

        public void UpdateVisibility(bool IsVisible)
        {
            this.UIElement.IsVisible = IsVisible;
            css_class = (UIElement.IsVisible ? "d-flex" : "d-none") + " align-end ";
            PropertyConversionResponse<string> propertyConversion = DataObject.GetPropObject<string>(UIElement.DefaultAccessPath);//get property value equal name with this UIElement.DefaultAccessPath
            if (propertyConversion.IsConversionSuccess)
            {
                this.TextValue = propertyConversion.Value;// get value of property 

            }

            this.StateHasChanged();
        }

        public void ToggleEditable(bool IsEditable)
        {
            IsEnabled = IsEditable;
            this.StateHasChanged();
        }

        public async Task Refresh()
        {
            PropertyConversionResponse<string> propertyConversion = DataObject.GetPropObject<string>(UIElement.DefaultAccessPath);//get property value equal name with this UIElement.DefaultAccessPath
            if (propertyConversion.IsConversionSuccess)
            {
                this.TextValue = propertyConversion.Value;// get value of property 

            }
            StateHasChanged();
            await Task.CompletedTask;
        }

        public async Task SetValue(object value)
        {
            if (value != null) 
            {
                this.TextValue = value.ToString();
            }
            this.StateHasChanged();
            await Task.CompletedTask;
        }

        public Task FocusComponentAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}
