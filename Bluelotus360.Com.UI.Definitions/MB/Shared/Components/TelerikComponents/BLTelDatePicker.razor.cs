using BlueLotus360.CleanArchitecture.Domain;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.Com.UI.Definitions.Extensions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlueLotus360.Com.UI.Definitions.MB.Shared.Components.TelerikComponents
{
    public partial class BLTelDatePicker: IBLUIOperationHelper
    {
        [Parameter]
        public BLUIElement UIElement { get; set; }

        [Parameter]
        public object DataObject { get; set; }

        [Parameter]
        public IDictionary<string, EventCallback> InteractionLogics { get; set; }

        [Parameter]
        public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }

        private DateTime? dateValue = DateTime.Now;
        private bool editable = false;

        private MudDatePicker _picker;

        public DateTime? DateValue
        {
            get { return dateValue; }
            set
            {
                dateValue = value;
               // this.OnDateChange(dateValue);
            }
        }

        public BLUIElement LinkedUIObject => throw new NotImplementedException();

        private string css_class = "";

        private string date_pic_css = "";
        private bool IsEnabled = true;
        private PropertyConversionResponse<DateTime> conversionInfo;
        private void OnDateChange(DateTime? date)
        {
            if (date.HasValue)
            {
                DataObject.SetValueByObjectPath(UIElement.DefaultAccessPath, date.Value);
                dateValue = date.Value;
            }
            if (InteractionLogics != null && InteractionLogics.Count > 0 && UIElement.OnClickAction != null)
            {
                EventCallback callback;
                if (InteractionLogics.TryGetValue(UIElement.OnClickAction, out callback))
                {
                    UIInterectionArgs<DateTime?> args = new UIInterectionArgs<DateTime?>();
                    args.Caller = this.UIElement.OnClickAction;
                    args.ObjectPath = this.UIElement.DefaultAccessPath;
                    args.DataObject = (DateTime?)date;
                    args.sender = this;
                    args.InitiatorObject = UIElement;
                    callback.InvokeAsync(args);
                }
            }
        }

        protected override Task OnParametersSetAsync()
        {
            conversionInfo = DataObject.GetPropObject<DateTime>(UIElement.DefaultAccessPath);
            if (conversionInfo.IsConversionSuccess)
            {
                dateValue = conversionInfo.Value;
            }
            //;
            return base.OnParametersSetAsync();
        }

        protected override Task OnInitializedAsync()
        {
            css_class = (UIElement.IsVisible ? $"d-flex {UIElement.ParentCssClass}" : "d-none") + " align-end ";
            date_pic_css= (UIElement.IsVisible ? UIElement.CssClass : "d-none");

            IsEnabled = IsEnabled && UIElement.IsEnable;

            if (!string.IsNullOrEmpty(UIElement.DefaultValue))
            {
                if (UIElement.DefaultValue.Equals("NODATE"))
                {
                    //dateValue = null;
                    editable = true;
                }
                else
                {
                    string[] default_date = new string[3];
                    int i = 0;
                    foreach(string dt in UIElement.DefaultValue.Split("/"))
                    {
                        default_date[i] = dt;
                        i++;
                    }
                    if (!string.IsNullOrEmpty(default_date[0]) && !string.IsNullOrEmpty(default_date[1]) && !string.IsNullOrEmpty(default_date[2]))
                    {
                        dateValue = new DateTime(Convert.ToInt32(default_date[0]), Convert.ToInt32(default_date[1]), Convert.ToInt32(default_date[2]));
                    }
                }
                
            }
            if (ObjectHelpers != null)
            {
                if (ObjectHelpers.ContainsKey(UIElement.ElementName))
                {
                    ObjectHelpers.Remove(UIElement.ElementName);

                }
                ObjectHelpers.Add(UIElement.ElementName, this);
            }
            return base.OnInitializedAsync();
        }

        public void ResetToInitialValue()
        {
           
        }

        public void UpdateVisibility(bool IsVisible)
        {
          
        }

        public void ToggleEditable(bool IsEditable)
        {
            editable = IsEditable;
            IsEnabled = IsEditable;
            StateHasChanged();
        }

        public async Task Refresh()
        {
            var val= DataObject.GetPropObject<DateTime>(this.UIElement.DefaultAccessPath);
            if (val.IsConversionSuccess)
            {
                dateValue = val.Value;
            }
            await Task.CompletedTask;
        }

        public  async Task SetValue(object value)
        {
            if (value!=null)
            {
                dateValue = DateTime.Parse(value.ToString());
                StateHasChanged();
            }
            
            await Task.CompletedTask;

        }

        public async Task FocusComponentAsync()
        {
            if (_picker != null)
            {
                await _picker.FocusAsync();
                _picker.Open();
            }
        }
    }

}
 