
using BlueLotus360.CleanArchitecture.Domain;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.Com.UI.Definitions.Extensions;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlueLotus360.Com.UI.Definitions.MB.Shared.Components
{
    public partial class BLMultiRadio: IBLUIOperationHelper
    {
        [Parameter]
        public BLUIElement UIElement {  get; set; }

        [Parameter]
        public object DataObject { get; set; }

        [Parameter]
        public IDictionary<string, EventCallback> InteractionLogics { get; set; }

        [Parameter]
        public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }

        public BLUIElement LinkedUIObject { get; private set; }
        private int Amount { get; set; } 

        private string css_class = "";

		protected override Task OnInitializedAsync()
		{
            css_class = (UIElement.IsVisible ? "d-flex" : "d-none") + " align-end ";
            if (ObjectHelpers.ContainsKey(UIElement.ElementName))
            {
                ObjectHelpers.Remove(UIElement.ElementName);

            }
            if (ObjectHelpers != null)
            {
                ObjectHelpers.Add(UIElement.ElementName, this);
            }

            PropertyConversionResponse<int> propertyConversion = DataObject.GetPropObject<int>(UIElement.DefaultAccessPath);
            if (propertyConversion.IsConversionSuccess)
            {
                this.Amount = propertyConversion.Value;


            }
            return base.OnInitializedAsync();
		}
		private void OnBLMultiRadioChange()
        {
            UIInterectionArgs<int> args = new UIInterectionArgs<int>();
            try
            {
                DataObject.SetValueByObjectPath(UIElement.DefaultAccessPath, Amount);
                if (InteractionLogics != null && UIElement.OnClickAction!=null && UIElement.OnClickAction.Length>3)
                {
                    EventCallback callback;
                    if (InteractionLogics.TryGetValue(UIElement.OnClickAction, out callback))
                    {
                        if (callback.HasDelegate)
                        {                        
                            args.Caller = this.UIElement.OnClickAction;
                            args.ObjectPath = this.UIElement.DefaultAccessPath;
                            args.DataObject = Amount;
                            args.sender = this;
                            args.InitiatorObject = UIElement;
                            callback.InvokeAsync(args);                           
                        }
                        else
                        {
                            
                        }

                    }
                }
                //if (!args.CancelChange)
                //{
                //    Amount = value;
                //}
                //else
                //{
                //    if (args.OverrideValue)
                //    {
                //        Amount = args.OverriddenValue;
                //    }
                //}

            }
            catch (Exception ex)
            {

            }

            
        }

		public void ResetToInitialValue()
		{
			throw new NotImplementedException();
		}

		public void UpdateVisibility(bool IsVisible)
		{
           
            this.UIElement.IsVisible = IsVisible;
            css_class = (UIElement.IsVisible ? "d-flex" : "d-none") + " align-end ";
            PropertyConversionResponse<int> propertyConversion = DataObject.GetPropObject<int>(UIElement.DefaultAccessPath);
            if (propertyConversion.IsConversionSuccess)
            {
                this.Amount = propertyConversion.Value;
               

            }

            this.StateHasChanged();
        }

        public void ToggleEditable(bool IsEditable)
        {
            throw new NotImplementedException();
        }

        public async Task Refresh()
        {
            await Task.CompletedTask;
        }


        public async Task FocusComponentAsync()
        {
           
        }

        public async Task SetValue(object value)
        {
            await Task.CompletedTask;
        }
    }

}
