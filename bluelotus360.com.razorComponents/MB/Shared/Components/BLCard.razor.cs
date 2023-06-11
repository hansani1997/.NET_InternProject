using bluelotus360.com.razorComponents.Extensions;
using BlueLotus360.CleanArchitecture.Domain;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.razorComponents.MB.Shared.Components
{
	public partial class BLCard : IBLUIOperationHelper
	{
		[Parameter]
		public BLUIElement UIElement { get; set; }

		[Parameter]
		public object DataObject { get; set; }

		[Parameter]
		public IDictionary<string, EventCallback> InteractionLogics { get; set; }

		[Parameter]
		public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }

		public BLUIElement LinkedUIObject => throw new System.NotImplementedException();
		private string css_class = "";
		private string card_content = "";
		private string card_value = "";

		protected override Task OnParametersSetAsync()
		{
			if (ObjectHelpers != null)
			{
				if (ObjectHelpers.ContainsKey(UIElement.ElementName))
				{
					ObjectHelpers.Remove(UIElement.ElementName);

				}
				ObjectHelpers.Add(UIElement.ElementName, this);
			}

			return base.OnParametersSetAsync();
		}
		protected override async Task OnInitializedAsync()
		{
			css_class = UIElement.IsVisible ? "d-flex" : "d-none";
			await SetCardContent();
            this.StateHasChanged();
			await base.OnInitializedAsync();
        }

        private async Task SetCardContent()
        {
            try
            {
                if (DataObject != null)
                {
                    Type t = DataObject.GetType();
                    PropertyInfo info = t.GetProperty(UIElement.DefaultAccessPath, BindingFlags.Public | BindingFlags.Instance);

                    if (info == null)
                    {
                        card_value = String.Empty;
                    }
                    else if ((info?.GetValue(DataObject, null)).IsNumericType())
                    {
                        if (info?.GetValue(DataObject, null).GetType() == typeof(int))
                        {
                            card_value = ((int)info?.GetValue(DataObject, null)).ToString("D2");
                        }
                        else
                        {
                            decimal value = decimal.Parse(info?.GetValue(DataObject, null).ToString());
                            string unit = "";

                            if (value >= 1000000000)
                            {
                                value /= 1000000000;
                                unit= "G";
                            }
                            else if(value >= 1000000 && value < 1000000000)
                            {
                                value /= 1000000;
                                unit = "M";
                            }
                            else if(value >= 1000 && value < 1000000)
                            {
                                value /= 1000;
                                unit= "k";
                            }

                            card_value = value.ToString("N0") + unit;
                            //card_value = decimal.Parse(info?.GetValue(DataObject, null).ToString()).ToString("N2");
                          }
                    }
                    else
                    {
                        card_value = info?.GetValue(DataObject, null).ToString();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void OnCardClick()
        {
            UIInterectionArgs<object> args = new UIInterectionArgs<object>();
            if (InteractionLogics != null && UIElement.OnClickAction != null && UIElement.OnClickAction.Length > 3)
            {
                EventCallback callback;
                if (InteractionLogics.TryGetValue(UIElement.OnClickAction, out callback))
                {
                    if (callback.HasDelegate)
                    {
                        args.Caller = this.UIElement.OnClickAction;
                        args.ObjectPath = this.UIElement.DefaultAccessPath;
                        args.DataObject = (DataObject == null) ? new object() : DataObject;
                        args.sender = this;
                        args.InitiatorObject = UIElement;
                        callback.InvokeAsync(args);
                    }
                }
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
			throw new System.NotImplementedException();
		}

        public async Task SetValue(object value)
        {
            if (value == null)
            {
                card_value = String.Empty;

            }
            else if (value.IsNumericType())
            {
                if(value.GetType() == typeof(int))
                {
                    card_value = ((int)value).ToString("D2");
                }
                else
                {
                    //card_value = decimal.Parse(value.ToString()).ToString("N2");
                    decimal result = decimal.Parse(value.ToString());
                    string unit = "";

                    if (result >= 1000000000)
                    {
                        result /= 1000000000;
                        unit = "G";
                    }
                    else if (result >= 1000000 && result < 1000000000)
                    {
                        result /= 1000000;
                        unit = "M";
                    }
                    else if (result >= 1000 && result < 1000000)
                    {
                        result /= 1000;
                        unit = "k";
                    }

                    card_value = result.ToString("N0") + unit;
                }
            }
            else
            {
                card_value = value.ToString();
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
			throw new System.NotImplementedException();
		}
	}
}
