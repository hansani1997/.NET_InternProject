
using BlueLotus360.CleanArchitecture.Domain;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.Com.UI.Definitions.Extensions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlueLotus360.Com.UI.Definitions.MB.Shared.Components
{
    public partial class BLDatePicker: IBLUIOperationHelper
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
                this.OnDateChange(dateValue);
            }
        }

        public BLUIElement LinkedUIObject => throw new NotImplementedException();

        private string css_class = "";


        private void OnDateChange(DateTime? date)
        {
            if (date.HasValue)
            {
                DataObject.SetValueByObjectPath(UIElement.DefaultAccessPath, date.Value);
            }
            if (InteractionLogics != null && InteractionLogics.Count > 0 && UIElement.OnClickAction != null)
            {
                EventCallback callback;
                if (InteractionLogics.TryGetValue(UIElement.OnClickAction, out callback))
                {
                    UIInterectionArgs<DateTime?> args = new UIInterectionArgs<DateTime?>();
                    args.Caller = this.UIElement.OnClickAction;
                    args.ObjectPath = this.UIElement.DefaultAccessPath;
                    args.DataObject = date;
                    args.sender = this;
                    args.InitiatorObject = UIElement;
                    callback.InvokeAsync(args);
                }
            }
        }

        protected override Task OnParametersSetAsync()
        {
           
            //;
            return base.OnParametersSetAsync();
        }

        protected override Task OnInitializedAsync()
        {
            if (UIElement!=null)
            {
                css_class = (UIElement.IsVisible ? "d-flex" : "d-none") + " align-end " + UIElement.ParentCssClass;
            }
            
            if (UIElement.DefaultValue!=null)
            {
                if (UIElement.DefaultValue.Equals("NODATE"))
                {
                    dateValue = null;
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
                    dateValue = new DateTime(Convert.ToInt32(default_date[0]), Convert.ToInt32(default_date[1]), Convert.ToInt32(default_date[2]));
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
 