using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain;
using bluelotus360.com.razorComponents.Extensions;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.razorComponents.MB.Shared.Components
{
    public partial class BLTextArea : IBLUIOperationHelper
    {
        [Parameter]
        public BLUIElement UIElement { get; set; }

        [Parameter]
        public object DataObject { get; set; }

        [Parameter]
        public IDictionary<string, EventCallback> InteractionLogics { get; set; }

        [Parameter]
        public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }

        private string TextValue = "";
        public BLUIElement LinkedUIObject { get; private set; }
        private string css_class = "";
        private MudTextField<string> multilineReference;
        private bool editable = false;
        private PropertyConversionResponse<string> conversionInfo;
        protected override void OnInitialized()
        {
            css_class = UIElement.CssClass;
            if (ObjectHelpers != null)
            {

                if (ObjectHelpers.ContainsKey(UIElement.ElementName))
                {
                    ObjectHelpers.Remove(UIElement.ElementName);

                }
                ObjectHelpers.Add(UIElement.ElementName, this);
            }

            base.OnInitialized();
        }

        protected override void OnParametersSet()
        {
            this.LinkedUIObject = UIElement;

            conversionInfo = DataObject.GetPropObject<string>(UIElement.DefaultAccessPath);
            if (conversionInfo.IsConversionSuccess)
            {
                TextValue = conversionInfo.Value;
            }
            base.OnParametersSet();
        }

        private async void OnBlTextChanged(string value)
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


        private void OnTextBoxKeyDown(KeyboardEventArgs e)
        {



            UIInterectionArgs<object> args = new UIInterectionArgs<object>();

            if (InteractionLogics != null && UIElement.EnterKeyAction != null && UIElement.EnterKeyAction.Length > 3 && (e.Code.Equals("Enter") || e.Code.Equals("NumpadEnter")))
            {
                EventCallback callback;


                if (InteractionLogics.TryGetValue(UIElement.EnterKeyAction, out callback))
                {

                    if (callback.HasDelegate)
                    {

                        args.Caller = this.UIElement.EnterKeyAction;
                        args.ObjectPath = this.UIElement.DefaultAccessPath;
                        args.DataObject = TextValue;
                        args.sender = this;
                        args.InitiatorObject = UIElement;
                        args.e = e;
                        callback.InvokeAsync(args);


                    }
                    else
                    {

                    }



                }
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
            editable = !IsEditable;
            StateHasChanged();
        }

        public async Task Refresh()
        {
            StateHasChanged();
        }

        public Task FocusComponentAsync()
        {
            throw new NotImplementedException();
        }

        public async Task SetValue(object value)
        {
            this.TextValue = value.ToString();
            this.StateHasChanged();
            await Task.CompletedTask;
        }
    }
}
