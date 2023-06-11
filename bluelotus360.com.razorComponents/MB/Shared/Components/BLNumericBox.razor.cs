using bluelotus360.com.razorComponents.Extensions;
using BlueLotus360.CleanArchitecture.Domain;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.razorComponents.MB.Shared.Components
{
    public partial class BLNumericBox : IBLUIOperationHelper
    {
        private MudNumericField<decimal> singleLineReference;
        [Parameter]
        public BLUIElement UIElement { get; set; }

        [Parameter]
        public object DataObject { get; set; }

        [Parameter]
        public IDictionary<string, EventCallback> InteractionLogics { get; set; }
        [Parameter]
        public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }

        [Parameter]
        public bool IsReadOnly { get; set; } = false;

        [Parameter]
        public IDictionary<string, string> DynamicBindings { get; set; }

        private decimal Amount;

        private string css_class = "";
        string numeric_css = "";

        private bool __forcerender = false;
        private bool IsComponentDisabled;
        public BLUIElement LinkedUIObject { get; private set; }
        

        protected override void OnInitialized()
        {
            css_class = (UIElement.IsVisible ? "d-flex" : "d-none") + " align-end";
            numeric_css= (UIElement.IsVisible ? UIElement.CssClass : "d-none") + " child-inp-right";
            if (UIElement.IsDynamicalyLoaded && DynamicBindings != null)
            {
                if (DynamicBindings.ContainsKey(UIElement.ElementName))
                {
                    DynamicBindings.Remove(UIElement.ElementName);

                }
                DynamicBindings.Add(UIElement.ElementName, "");
            }
            if (ObjectHelpers != null)
            {
                if (ObjectHelpers.ContainsKey(UIElement.ElementName))
                {
                    ObjectHelpers.Remove(UIElement.ElementName);

                }
                ObjectHelpers.Add(UIElement.ElementName, this);
            }

            //this.Amount = !string.IsNullOrEmpty(UIElement.DefaultValue)? Convert.ToDecimal(UIElement.DefaultValue):0;
            //OnBlNumercChanged(Amount);


            base.OnInitialized();
        }

        protected override void OnParametersSet()
        {
            PropertyConversionResponse<decimal> propertyConversion = DataObject.GetPropObject<decimal>(UIElement.DefaultAccessPath);

                if (propertyConversion.IsConversionSuccess)
                {
                    this.Amount = propertyConversion.Value;
                    this.StateHasChanged();
                }
          
            
            base.OnParametersSet();
        }
        private async void OnBlNumercChanged(decimal value)
        {
            UIInterectionArgs<decimal> args = new UIInterectionArgs<decimal>();

            try
            {

                if (InteractionLogics != null && UIElement.OnClickAction != null && UIElement.OnClickAction.Length > 3)
                {
                    EventCallback callback;

                    if (InteractionLogics.TryGetValue(UIElement.OnClickAction, out callback))
                    {

                        if (callback.HasDelegate)
                        {
                            DataObject.SetValueByObjectPath(UIElement.DefaultAccessPath, value);
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
                    Amount = value;
                    DataObject.SetValueByObjectPath(UIElement.DefaultAccessPath, value);

                    if (UIElement.IsDynamicalyLoaded && DynamicBindings != null && DynamicBindings.ContainsKey(UIElement.ElementName))
                    {
                        args.ObjectPath = this.UIElement.DefaultAccessPath;
                        args.DataObject = Amount;
                        args.InitiatorObject = UIElement;
                        DynamicBindings[UIElement.ElementName] = JsonConvert.SerializeObject(args);
                    }

                }
                else
                {
                    if (args.OverrideValue)
                    {
                        Amount = args.OverriddenValue;
                        value = Amount;
                        DataObject.SetValueByObjectPath(UIElement.DefaultAccessPath, Amount);

                        StateHasChanged();

                    }
                }

            }
            catch (Exception ex)
            {

            }
        }

        public async Task FocusComponentAsync()
        {
            await singleLineReference.FocusAsync();
            await singleLineReference.SelectAsync();
        }

        public async Task Refresh()
        {
            PropertyConversionResponse<decimal> propertyConversion = DataObject.GetPropObject<decimal>(UIElement.DefaultAccessPath);//get property value equal name with this UIElement.DefaultAccessPath
            if (propertyConversion.IsConversionSuccess)
            {
                this.Amount = propertyConversion.Value;// get value of property 

            }
            this.StateHasChanged();
            await Task.CompletedTask;
        }

        public void ResetToInitialValue()
        {
            this.Amount = 0;
            __forcerender = true;
            this.StateHasChanged();
            __forcerender = false;
        }

        public async Task SetValue(object value)
        {
            Amount = Convert.ToDecimal(value);
            StateHasChanged();
        }

        public void ToggleEditable(bool IsEditable)
        {
            IsComponentDisabled = !IsEditable;
            StateHasChanged();
        }

        public void UpdateVisibility(bool IsVisible)
        {
            this.UIElement.IsVisible = IsVisible;
            css_class = (UIElement.IsVisible ? "d-flex" : "d-none") + " align-end ";
            this.Refresh();

        }
        public async void OnNumericBoxKeyUp(KeyboardEventArgs e)
        {


            UIInterectionArgs<decimal> args = new UIInterectionArgs<decimal>();

            if (InteractionLogics != null && UIElement.EnterKeyAction != null && (e.Code.Equals("Enter") || e.Code.Equals("NumpadEnter")))
            {
                EventCallback callback;


                if (InteractionLogics.TryGetValue(UIElement.EnterKeyAction, out callback))
                {

                    if (callback.HasDelegate)
                    {

                        args.Caller = this.UIElement.EnterKeyAction;
                        args.ObjectPath = this.UIElement.DefaultAccessPath;
                        args.DataObject = Amount;
                        args.sender = this;
                        args.InitiatorObject = UIElement;
                        args.e = e;
                        await callback.InvokeAsync(args);


                    }
                    else
                    {

                    }



                }
            }
        }
    }
}
