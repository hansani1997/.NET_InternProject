using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.Com.UI.Definitions.Extensions;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlueLotus360.Com.UI.Definitions.MB.Shared.Components
{
    public partial class BLTimePicker : IBLUIOperationHelper
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
        

        private string css_class = "";

        TimeSpan? time;
        int hours, minutes, second;
        protected override void OnInitialized()
        {
           

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

        private async void OnTimeChanged(TimeSpan? value)
        {
            UIInterectionArgs<TimeSpan?> args = new UIInterectionArgs<TimeSpan?>();
            try
            {
                //DataObject.SetValueByObjectPath(UIElement.DefaultAccessPath, value);

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
                    time = value;
                }
                else
                {
                    if (args.OverrideValue)
                    {
                        time = args.OverriddenValue;
                        //DataObject.SetValueByObjectPath(UIElement.DefaultAccessPath, time);

                    }
                }

            }
            catch (Exception ex)
            {

            }
        }

        public Task FocusComponentAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task Refresh()
        {
            throw new System.NotImplementedException();
        }

        public void ResetToInitialValue()
        {
            time = TimeSpan.Zero;

            this.StateHasChanged();
        }

        public async Task SetValue(object value)
        {
            if (value!=null)
            {
                if (value.GetType() == typeof(DateTime))
                {
                    hours = ((DateTime)value).Hour;
                    minutes = ((DateTime)value).Minute;
                    second = ((DateTime)value).Second;

                    this.time = new TimeSpan(hours, minutes, second);
                }
            }
            

            this.StateHasChanged();
            await Task.CompletedTask;
        }

        public void ToggleEditable(bool IsEditable)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateVisibility(bool IsVisible)
        {
            this.UIElement.IsVisible = IsVisible;
            css_class = (UIElement.IsVisible ? "d-flex" : "d-none") + " align-end ";
            //PropertyConversionResponse<TimeSpan?> propertyConversion = DataObject.GetPropObject<TimeSpan?>(UIElement.DefaultAccessPath);//get property value equal name with this UIElement.DefaultAccessPath
            //if (propertyConversion.IsConversionSuccess)
            //{
            //    this.time = propertyConversion.Value;// get value of property 

            //}

            this.StateHasChanged();
        }
    }
}
