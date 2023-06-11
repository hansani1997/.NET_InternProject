using BlueLotus360.CleanArchitecture.Domain;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.Com.UI.Definitions.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace BlueLotus360.Com.UI.Definitions.MB.Shared.Components
{
    public partial class BLTextBox : IBLUIOperationHelper
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

        [Parameter]
        public bool IsReadOnly { get; set; }  = false;
        public BLUIElement LinkedUIObject { get; private set; }

        private string TextValue = "";

        private string textAlignment = "";

        private string css_class = "";

        private string HeaderPostFix = "";
        private string IconSvgCode = "";

        private string LastKey = "NoKey";

        private bool editable = false;
        protected override void OnInitialized()
        {
            css_class = UIElement.IsVisible? UIElement.CssClass:"d-none";
            string[] path = this.UIElement.IconCss.Split('.');
            GetIconByStringName(this.UIElement.IconCss, typeof(Icons));
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
            //TextValue = UIElement.DefaultValue;
            base.OnParametersSet();
        }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
             //   await singleLineReference.FocusAsync();
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        private async void OnBlTextChanged(string value)
       {
          //  LastKey = value;
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


            LastKey = e.Key;

            UIInterectionArgs<object> args = new UIInterectionArgs<object>();

           

            if (InteractionLogics != null && UIElement.EnterKeyAction != null && UIElement.EnterKeyAction.Length > 3 && (e.Code.Equals("Enter") || e.Code.Equals("NumpadEnter")  || e.Key.Equals("Enter")))
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

        private void AdornmentClick() 
        {
            UIInterectionArgs<object> args = new UIInterectionArgs<object>();
            try
            {
                if (InteractionLogics != null && UIElement.OnAdornmentAction != null)
                {
                    EventCallback callback;


                    if (InteractionLogics.TryGetValue(UIElement.OnAdornmentAction, out callback))
                    {

                        if (callback.HasDelegate)
                        {
                            args.Caller = this.UIElement.OnAdornmentAction;
                            args.ObjectPath = this.UIElement.DefaultAccessPath;
                            args.DataObject = TextValue;
                            args.sender = this;
                            args.InitiatorObject = UIElement;
                            callback.InvokeAsync(args);
                        }
                        else
                        {

                        }



                    }
                }
            }
            catch (Exception ex)
            {

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
            PropertyConversionResponse<string> propertyConversion = DataObject.GetPropObject<string>(UIElement.DefaultAccessPath);//get property value equal name with this UIElement.DefaultAccessPath
            if (propertyConversion.IsConversionSuccess)
            {
                this.TextValue = propertyConversion.Value;// get value of property 

            }
            StateHasChanged();
            await Task.CompletedTask;
        }


        public async Task FocusComponentAsync()
        {

           await singleLineReference.FocusAsync();

        }

        public async Task SetValue(object value)
        {
            if (value!=null)
            {
                this.TextValue = value.ToString();
                await singleLineReference.SetText(value.ToString());
            }
            
            this.StateHasChanged();
            await Task.CompletedTask;
        }

        private void TextBoxFocus()
        {
            UIInterectionArgs<object> args = new UIInterectionArgs<object>();

            if (InteractionLogics != null)
            {
                EventCallback callback;


                if (InteractionLogics.TryGetValue("OnFocus_" + UIElement._internalElementName, out callback))
                {

                    if (callback.HasDelegate)
                    {
                        args.Caller = "OnFocus_" + UIElement._internalElementName;
                        args.ObjectPath = this.UIElement.DefaultAccessPath;
                        args.DataObject = TextValue;
                        args.sender = this;
                        args.InitiatorObject = UIElement;
                        callback.InvokeAsync(args);
                    }
                    else
                    {

                    }



                }
            }
        }
        private void GetIconByStringName(string PropertyName, Type t)
        {

            Type type = null;
            string[] path = PropertyName.Split('.');
            string IconName = null;
            object iconObject = Icons.Material.Filled;
            if (path.Length == 2)
            {
                //This will assume the Filled section
                if (path[0].Equals("Filled"))
                {
                    type = Icons.Material.Filled.GetType();
                    iconObject = Icons.Material.Filled;
                }
                //This will assume the Filled section
                if (path[0].Equals("Outlined"))
                {
                    type = Icons.Material.Outlined.GetType();
                    iconObject = Icons.Material.Outlined;
                }

                if (path[0].Equals("TwoTone"))
                {
                    type = Icons.Material.TwoTone.GetType();
                    iconObject = Icons.Material.TwoTone;
                }

                if (path[0].Equals("Sharp"))
                {
                    type = Icons.Material.Sharp.GetType();
                    iconObject = Icons.Material.Sharp;
                }


                if (path[0].Equals("Rounded"))
                {
                    type = Icons.Material.Rounded.GetType();
                    iconObject = Icons.Material.Rounded;
                }

                IconName = path[1];

            }
            else
            {
                type = Icons.Material.Filled.GetType();
                iconObject = Icons.Material.Filled;
                IconName = PropertyName;
            }


            if (type != null)
            {
                PropertyInfo info = type.GetProperty(IconName);
                if (info != null)
                {
                    string value = info.GetValue(iconObject) as string;
                    IconSvgCode = value;
                }
            }



        }
    }
}
