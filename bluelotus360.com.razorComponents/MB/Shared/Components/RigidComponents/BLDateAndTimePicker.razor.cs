using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain;
using bluelotus360.Com.commonLib.Routes;
using bluelotus360.com.razorComponents.Extensions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace bluelotus360.com.razorComponents.MB.Shared.Components.RigidComponents
{
    public partial class BLDateAndTimePicker : IBLUIOperationHelper
    {
        [Parameter] public BLUIElement UIElement { get; set; }

        [Parameter] public object DataObject { get; set; }

        [Parameter] public IDictionary<string, EventCallback> InteractionLogics { get; set; }

        [Parameter] public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }

        DateTime? dateValue = DateTime.Now;
        TimeSpan? time = new TimeSpan(00, 00, 00);

        DateTime? _dateTime= DateTime.Now;

        private MudDatePicker _picker;
        private bool editable = false;
        MudDialog _mdComboDataPick;

        bool IsDialogVisible = false;
        DialogOptions _dialogOption = new DialogOptions() { CloseButton = true };
        public BLUIElement LinkedUIObject => throw new NotImplementedException();

        private string css_class = "";
        private string combo_css = "";
        private PropertyConversionResponse<DateTime> conversionInfo;
        string format= "dd/MM/yyyy HH:mm:ss";
        private void OnDateAndTimeChange()
        {
            if (dateValue.HasValue && time.HasValue)
            {
                _dateTime = dateValue.Value.Date + time.Value;
            }
            else
            {
                _dateTime= DateTime.Now;    
            }

            if (_dateTime.HasValue)
            {
                DataObject.SetValueByObjectPath(UIElement.DefaultAccessPath, _dateTime.Value);
            }
            if (InteractionLogics != null && InteractionLogics.Count > 0 && UIElement.OnClickAction != null)
            {
                EventCallback callback;
                if (InteractionLogics.TryGetValue(UIElement.OnClickAction, out callback))
                {
                    UIInterectionArgs<DateTime?> args = new UIInterectionArgs<DateTime?>();
                    args.Caller = this.UIElement.OnClickAction;
                    args.ObjectPath = this.UIElement.DefaultAccessPath;
                    args.DataObject = _dateTime;
                    args.sender = this;
                    args.InitiatorObject = UIElement;
                    callback.InvokeAsync(args);
                }
            }

            _mdComboDataPick.Close();
            StateHasChanged();
        }

        protected override Task OnParametersSetAsync()
        {
            int c = this.DataObject.GetHashCode();
            conversionInfo = DataObject.GetPropObject<DateTime>(UIElement.DefaultAccessPath);
            if (conversionInfo.IsConversionSuccess)
            {
                _dateTime = conversionInfo.Value;
                dateValue = _dateTime.Value.Date;
                time = ((DateTime)_dateTime).TimeOfDay;
            }

            //;
            return base.OnParametersSetAsync();
        }

        protected override Task OnInitializedAsync()
        {
            css_class = (UIElement.IsVisible ? "d-flex" : "d-none") ;
            combo_css = (UIElement.IsVisible ? UIElement.CssClass : "d-none") ;
            format = !string.IsNullOrEmpty(UIElement.Format) ? UIElement.Format : "dd/MM/yyyy HH:mm:ss";
            if (!string.IsNullOrEmpty(UIElement.DefaultValue))
            {
                if (UIElement.DefaultValue.Equals("NODATE"))
                {
                    _dateTime = null;
                    editable = true;
                }
                else
                {
                    if (DateTime.TryParse(UIElement.DefaultValue, out DateTime dt))
                    { Console.WriteLine("Parsing successful. The DateTime value is: " + dt); }
                    else
                    {
                        dt = DateTime.Now;
                    }

                    string[] default_date = new string[7];
                    int i = 0;
                    if (UIElement.DefaultValue.Contains('/') && UIElement.DefaultValue.Contains(':'))
                    {
                        _dateTime = dt;
                            
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
                            _dateTime = new DateTime(Convert.ToInt32(default_date[0]), Convert.ToInt32(default_date[1]), Convert.ToInt32(default_date[2]), Convert.ToInt32(default_date[3]), Convert.ToInt32(default_date[4]), Convert.ToInt32(default_date[5]));
                            dateValue= _dateTime.Value.Date; 
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
                            _dateTime = new DateTime(Convert.ToInt32(default_date[6]), Convert.ToInt32(default_date[5]), Convert.ToInt32(default_date[4]), Convert.ToInt32(default_date[0]), Convert.ToInt32(default_date[1]), Convert.ToInt32(default_date[2]));
                            time = ((DateTime)_dateTime).TimeOfDay;
                        }
                    }
                    else
                    {

                    }

                }

            }

            if (_dateTime.HasValue)
            {
                DataObject.SetValueByObjectPath(UIElement.DefaultAccessPath, _dateTime.Value);
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
                    { Console.WriteLine("Parsing successful. The DateTime value is: " + dt); }
                    else
                    {
                        dt = DateTime.Now;
                    }

                    string[] default_date = new string[7];
                    int i = 0;
                    if (UIElement.DefaultValue.Contains('/') && UIElement.DefaultValue.Contains(':'))
                    {
                          _dateTime = dt;
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
            var val = DataObject.GetPropObject<DateTime>(this.UIElement.DefaultAccessPath);
            if (val.IsConversionSuccess)
            {
                _dateTime = val.Value;
            }
            await Task.CompletedTask;
        }

        public async Task SetValue(object value)
        {
            if (value != null)
            {
                _dateTime = DateTime.Parse(value.ToString());
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

        private async Task OnDatePickerClick()
        {
            IsDialogVisible = true;
            await Task.CompletedTask;
        }
    }
}
