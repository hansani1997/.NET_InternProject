using BlueLotus360.CleanArchitecture.Domain;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.Com.UI.Definitions.Extensions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlueLotus360.Com.UI.Definitions.MB.Shared.Components
{
    public partial class BLLabel : IBLUIOperationHelper
    {
        private MudTextField<string> singleLineReference;
        [Parameter]
        public BLUIElement UIElement { get; set; }

        [Parameter]
        public object DataObject { get; set; }

        [Parameter]
        public IDictionary<string, EventCallback> InteractionLogics { get; set; }

        [Parameter]
        public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }

        bool __forcerender;
        private string TextValue;

        private string textAlignment = "";

        private string css_class = "";
        public BLUIElement LinkedUIObject { get; private set; }
        protected override void OnInitialized()
        {
            Type t = DataObject.GetObjectType(UIElement.DefaultAccessPath);
            string displayValue = null;
            textAlignment = "left";
            if (t != null && t != typeof(void))
            {
                TypeCode typeCode = Type.GetTypeCode(t);

                if (typeCode == TypeCode.String)
                {
                    var conversionInfo = DataObject.GetPropObject<string>(UIElement.DefaultAccessPath);
                    if (conversionInfo.IsConversionSuccess)
                    {
                        displayValue = conversionInfo.Value;
                    }
                }

                if (typeCode == TypeCode.Decimal)
                {
                    textAlignment = "right";
                    var conversionInfo = DataObject.GetPropObject<decimal>(UIElement.DefaultAccessPath);
                    if (conversionInfo.IsConversionSuccess)
                    {
                        displayValue = conversionInfo.Value.ToString(UIElement.Format);
                    }
                }



            }

            TextValue = displayValue;
            css_class = (UIElement.IsVisible ? "d-flex" : "d-none") + " align-end ";



            base.OnInitialized();
        }

        protected override void OnParametersSet()
        {


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

        public void ResetToInitialValue()
        {
            this.TextValue = string.Empty;
            __forcerender = true;
            this.StateHasChanged();
            __forcerender = false;
        }




        public void UpdateVisibility(bool IsVisible)
        {
            this.UIElement.IsVisible = IsVisible;
            css_class = (UIElement.IsVisible ? "d-flex" : "d-none") + " align-end ";
            Refresh();
        }

        public void ToggleEditable(bool IsEditable)
        {
            throw new NotImplementedException();
        }

        public async Task Refresh()
        {

            PropertyConversionResponse<string> propertyConversion = DataObject.GetPropObject<string>(UIElement.DefaultAccessPath);//get property value equal name with this UIElement.DefaultAccessPath
            if (propertyConversion.IsConversionSuccess)
            {
                this.TextValue = propertyConversion.Value;// get value of property 

            }
            await Task.CompletedTask;

            this.StateHasChanged();
        }

        public Task FocusComponentAsync()
        {
            throw new NotImplementedException();
        }

        public async Task SetValue(object value)
        {
            if (value != null)
            {
                this.TextValue = value.ToString();
                await singleLineReference.SetText(value.ToString());
            }

            this.StateHasChanged();
            await Task.CompletedTask;
        }
    }

}
