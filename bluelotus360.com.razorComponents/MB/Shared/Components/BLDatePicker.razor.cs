using bluelotus360.com.razorComponents.Extensions;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Components;
using System;
using MudBlazor;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Newtonsoft.Json;

namespace bluelotus360.com.razorComponents.MB.Shared.Components
{
    public partial class BLDatePicker : IBLUIOperationHelper
    {
        [Parameter]
        public BLUIElement UIElement { get; set; }

        [Parameter]
        public object DataObject { get; set; }

        [Parameter]
        public IDictionary<string, EventCallback> InteractionLogics { get; set; }

        [Parameter]
        public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }
        [Parameter] public IDictionary<string, string> DynamicBindings { get; set; }

        private DateTime? dateValue = DateTime.Now;
        private bool editable = false;

        private MudDatePicker _picker;
        private PropertyConversionResponse<DateTime> conversionInfo;
        public DateTime? DateValue
        {
            get { return dateValue; }
            set
            {
                dateValue = value;
                this.OnDateChange(dateValue);
            }
        }

        private string css_class = "";

        public BLUIElement LinkedUIObject => new BLUIElement();

        private void OnDateChange(DateTime? date)
        {
            UIInterectionArgs<DateTime?> args = new UIInterectionArgs<DateTime?>();
            if (date.HasValue)
            {
                DataObject.SetValueByObjectPath(UIElement.DefaultAccessPath, date.Value);
            }
            if (InteractionLogics != null && InteractionLogics.Count > 0 && UIElement.OnClickAction != null)
            {
                EventCallback callback;
                if (InteractionLogics.TryGetValue(UIElement.OnClickAction, out callback))
                {
                    args.Caller = this.UIElement.OnClickAction;
                    args.ObjectPath = this.UIElement.DefaultAccessPath;
                    args.DataObject = date;
                    args.sender = this;
                    args.InitiatorObject = UIElement;
                    callback.InvokeAsync(args);
                }
            }
            else
            {
                if (UIElement.IsDynamicalyLoaded && DynamicBindings != null && DynamicBindings.ContainsKey(UIElement.ElementName))
                {
                    args.ObjectPath = this.UIElement.DefaultAccessPath;
                    args.DataObject = date;
                    args.InitiatorObject = UIElement;
                    DynamicBindings[UIElement.ElementName] = JsonConvert.SerializeObject(args);
                }
            }
        }

        protected override Task OnParametersSetAsync()
        {
            int c = this.DataObject.GetHashCode();
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
            if (UIElement != null)
            {
                css_class = (UIElement.IsVisible ? "d-flex" : "d-none") + " align-end " + UIElement.ParentCssClass;
                
                if (UIElement.DefaultValue != null)
                {
                    if (!string.IsNullOrEmpty(UIElement.DefaultValue))
                    {
                        if (UIElement.DefaultValue.Equals("NODATE"))
                        {
                            //dateValue = null;
                            editable = true;
                        }
                        else
                        {
                            if (DateTime.TryParse(UIElement.DefaultValue, out DateTime dt)) 
                            { Console.WriteLine("Parsing successful. The DateTime value is: " + dt);}
                            else
                            {
                                dt=DateTime.Now;
                            }

                            string[] default_date = new string[7];
                            int i = 0;
                            if (UIElement.DefaultValue.Contains('/') && UIElement.DefaultValue.Contains(':'))
                            {
                                    dateValue = dt;
                                
                            }
                            else if (UIElement.DefaultValue.Contains('/'))
                            {
                                foreach (string dts in dt.ToString("yyyy/MM/dd").Split("/"))
                                {
                                    default_date[i] = dts;
                                    i++;
                                }
                                default_date[3] = DateTime.Now.ToString("HH", CultureInfo.InvariantCulture);
                                default_date[4] = DateTime.Now.ToString("mm", CultureInfo.InvariantCulture);
                                default_date[5] = DateTime.Now.ToString("ss", CultureInfo.InvariantCulture);
                                default_date[6] = DateTime.Now.ToString("tt", CultureInfo.InvariantCulture);
                                //default_date[5] = DateTime.Now.ToString("ss", CultureInfo.InvariantCulture);

                                if (!string.IsNullOrEmpty(default_date[0]) && !string.IsNullOrEmpty(default_date[1]) && !string.IsNullOrEmpty(default_date[2]) && !string.IsNullOrEmpty(default_date[3]) && !string.IsNullOrEmpty(default_date[4]) && !string.IsNullOrEmpty(default_date[5]) && !string.IsNullOrEmpty(default_date[6]))
                                {
                                    dateValue = new DateTime(Convert.ToInt32(default_date[0]), Convert.ToInt32(default_date[1]), Convert.ToInt32(default_date[2]), Convert.ToInt32(default_date[3]), Convert.ToInt32(default_date[4]), Convert.ToInt32(default_date[5]));
                                }
                            }
                            else if (UIElement.DefaultValue.Contains(':'))
                            {
                                foreach (string dts in dt.ToString("HH:mm").Split(":"))
                                {
                                    default_date[i] = dts;
                                    i++;
                                }
                                if (string.IsNullOrEmpty(default_date[2]))
                                {
                                    default_date[2] = "00";
                                }
                                default_date[3] = dt.ToString("tt", CultureInfo.InvariantCulture);
                                default_date[4] = DateTime.Now.ToString("dd", CultureInfo.InvariantCulture);
                                default_date[5] = DateTime.Now.ToString("MM", CultureInfo.InvariantCulture);
                                default_date[6] = DateTime.Now.ToString("yyyy", CultureInfo.InvariantCulture);
                                //default_date[5] = DateTime.Now.ToString("ss", CultureInfo.InvariantCulture);

                                if (!string.IsNullOrEmpty(default_date[0]) && !string.IsNullOrEmpty(default_date[1]) && !string.IsNullOrEmpty(default_date[2]) && !string.IsNullOrEmpty(default_date[3]) && !string.IsNullOrEmpty(default_date[4]) && !string.IsNullOrEmpty(default_date[5]) && !string.IsNullOrEmpty(default_date[6]))
                                {
                                    dateValue = new DateTime(Convert.ToInt32(default_date[6]), Convert.ToInt32(default_date[5]), Convert.ToInt32(default_date[4]), Convert.ToInt32(default_date[0]), Convert.ToInt32(default_date[1]), Convert.ToInt32(default_date[2]));
                                }
                            }
                            else
                            {

                            }
                        }

                    }
                }
            }

            if (UIElement.IsDynamicalyLoaded && DynamicBindings != null)
            {
                if (DynamicBindings.ContainsKey(UIElement.ElementName))
                {
                    DynamicBindings.Remove(UIElement.ElementName);

                }
                DynamicBindings.Add(UIElement.ElementName, "");
            }

            if (dateValue.HasValue)
            {
				DataObject.SetValueByObjectPath(UIElement.DefaultAccessPath, dateValue.Value);
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

        public async Task FocusComponentAsync()
        {
            if (_picker != null)
            {
                await _picker.FocusAsync();
                _picker.Open();
            }
        }

        public async Task Refresh()
        {
            var val = DataObject.GetPropObject<DateTime>(this.UIElement.DefaultAccessPath);
            if (val.IsConversionSuccess)
            {
                dateValue = val.Value;
            }
            await Task.CompletedTask;
        }

        public async void ResetToInitialValue()
        {
            if (!string.IsNullOrEmpty(UIElement.DefaultValue))
            {
                if (UIElement.DefaultValue.Equals("NODATE"))
                {
                    //dateValue = null;
                    editable = true;
                }
                else
                {
                    DateTime dt = DateTime.Parse(UIElement.DefaultValue);

                    string[] default_date = new string[7];
                    int i = 0;
                    if (UIElement.DefaultValue.Contains('/') && UIElement.DefaultValue.Contains(':'))
                    {
                        dateValue = dt;
                    }
                    else if (UIElement.DefaultValue.Contains('/'))
                    {
                        foreach (string dts in dt.ToString("yyyy/MM/dd").Split("/"))
                        {
                            default_date[i] = dts;
                            i++;
                        }
                        default_date[3] = DateTime.Now.ToString("HH", CultureInfo.InvariantCulture);
                        default_date[4] = DateTime.Now.ToString("mm", CultureInfo.InvariantCulture);
                        default_date[5] = DateTime.Now.ToString("ss", CultureInfo.InvariantCulture);
                        default_date[6] = DateTime.Now.ToString("tt", CultureInfo.InvariantCulture);
                        //default_date[5] = DateTime.Now.ToString("ss", CultureInfo.InvariantCulture);

                        if (!string.IsNullOrEmpty(default_date[0]) && !string.IsNullOrEmpty(default_date[1]) && !string.IsNullOrEmpty(default_date[2]) && !string.IsNullOrEmpty(default_date[3]) && !string.IsNullOrEmpty(default_date[4]) && !string.IsNullOrEmpty(default_date[5]) && !string.IsNullOrEmpty(default_date[6]))
                        {
                            dateValue = new DateTime(Convert.ToInt32(default_date[0]), Convert.ToInt32(default_date[1]), Convert.ToInt32(default_date[2]), Convert.ToInt32(default_date[3]), Convert.ToInt32(default_date[4]), Convert.ToInt32(default_date[5]));
                        }
                    }
                    else if (UIElement.DefaultValue.Contains(':'))
                    {
                        foreach (string dts in dt.ToString("HH:mm").Split(":"))
                        {
                            default_date[i] = dts;
                            i++;
                        }
                        if (string.IsNullOrEmpty(default_date[2]))
                        {
                            default_date[2] = "00";
                        }
                        default_date[3] = dt.ToString("tt", CultureInfo.InvariantCulture);
                        default_date[4] = DateTime.Now.ToString("dd", CultureInfo.InvariantCulture);
                        default_date[5] = DateTime.Now.ToString("MM", CultureInfo.InvariantCulture);
                        default_date[6] = DateTime.Now.ToString("yyyy", CultureInfo.InvariantCulture);
                        //default_date[5] = DateTime.Now.ToString("ss", CultureInfo.InvariantCulture);

                        if (!string.IsNullOrEmpty(default_date[0]) && !string.IsNullOrEmpty(default_date[1]) && !string.IsNullOrEmpty(default_date[2]) && !string.IsNullOrEmpty(default_date[3]) && !string.IsNullOrEmpty(default_date[4]) && !string.IsNullOrEmpty(default_date[5]) && !string.IsNullOrEmpty(default_date[6]))
                        {
                            dateValue = new DateTime(Convert.ToInt32(default_date[6]), Convert.ToInt32(default_date[5]), Convert.ToInt32(default_date[4]), Convert.ToInt32(default_date[0]), Convert.ToInt32(default_date[1]), Convert.ToInt32(default_date[2]));
                        }
                    }
                    else
                    {

                    }
                }

            }
            else
            {
                dateValue = DateTime.Now;
            }

            if (dateValue.HasValue)
            {
                DataObject.SetValueByObjectPath(UIElement.DefaultAccessPath, dateValue.Value);
            }
            StateHasChanged();
            await Task.CompletedTask;
        }

        public async Task SetValue(object value)
        {
            if (value != null)
            {
                dateValue = DateTime.Parse(value.ToString());
                StateHasChanged();
            }

            await Task.CompletedTask;
        }

        public void ToggleEditable(bool IsEditable)
        {
            editable = IsEditable;
            StateHasChanged();
        }

        public void UpdateVisibility(bool IsVisible)
        {
        }
    }
}
