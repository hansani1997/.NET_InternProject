using bluelotus360.com.razorComponents.Extensions;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.razorComponents.MB.Shared.Components
{
    public partial class BLChip : IBLUIOperationHelper
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

        private string IconSvgCode = "";

        private string css_class = "";

        private string btn_css = "";

        private string Caption = "";

        private object IconColor = Color.Error;

        private PropertyConversionResponse<string> conversionInfo;
        protected override async void OnInitialized()
        {
            css_class = (UIElement.IsVisible ? UIElement.CssClass : "d-none") + " align-end";
            btn_css = (UIElement.IsVisible ? UIElement.CssClass : "d-none") + " flex-grow-1";

            await SetCaption();

            IconSvgCode = GetIconByStingName(this.UIElement.IconCss, typeof(Icons));
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

        private async Task SetCaption()
        {
            if (!string.IsNullOrEmpty(UIElement.ElementCaption))
            {

                Type type = DataObject.GetType();
                PropertyInfo[] properties = type.GetProperties();

                foreach (PropertyInfo property in properties)
                {
                    if (property.Name.Equals(UIElement.DefaultAccessPath))
                    {
                        Caption = property.GetValue(DataObject).ToString() ?? UIElement.ElementCaption;
                    }

                }
                if (string.IsNullOrEmpty(Caption))
                {
                    Caption = UIElement.ElementCaption;
                }
                if (!string.IsNullOrEmpty(UIElement.DefaultValue))
                {
                    Caption = Caption + UIElement.DefaultValue;
                }

            }

        }

        //string GetIconByStingName(string PropertyName, Type t)
        //{
        //    string svgcode = "";
        //    Type type = null;
        //    string[] path = PropertyName.Split('.');
        //    string IconName = null;
        //    object iconObject = new Icons.Material.Filled();
        //    if (path.Length == 2)
        //    {
        //        //This will assume the Filled section
        //        if (path[0].Equals("Filled"))
        //        {
        //            //type = Icons.Material.Filled.GetType();
        //            //iconObject = Icons.Material.Filled;
        //            type =typeof(Icons.Material.Filled);
        //            iconObject =new Icons.Material.Filled();
        //        }
        //        //This will assume the Filled section
        //        if (path[0].Equals("Outlined"))
        //        {
        //            //type = Icons.Material.Outlined.GetType();
        //            //iconObject = Icons.Material.Outlined;
        //            type = typeof(Icons.Material.Outlined);
        //            iconObject = new Icons.Material.Outlined();
        //        }

        //        if (path[0].Equals("TwoTone"))
        //        {
        //            //    type = Icons.Material.TwoTone.GetType();
        //            //    iconObject = Icons.Material.TwoTone;
        //            type = typeof(Icons.Material.TwoTone);
        //            iconObject = new Icons.Material.TwoTone();
        //        }

        //        if (path[0].Equals("Sharp"))
        //        {
        //            //type = Icons.Material.Sharp.GetType();
        //            //iconObject = Icons.Material.Sharp;
        //            type = typeof(Icons.Material.Sharp);
        //            iconObject = new Icons.Material.Sharp();
        //        }


        //        if (path[0].Equals("Rounded"))
        //        {
        //            //type = Icons.Material.Rounded.GetType();
        //            //iconObject = Icons.Material.Rounded;
        //            type = typeof(Icons.Material.Rounded);
        //            iconObject = new Icons.Material.Rounded();
        //        }

        //        IconName = path[1];

        //    }
        //    else
        //    {
        //        //type = Icons.Material.Filled.GetType();
        //        //iconObject = Icons.Material.Filled;
        //        type = typeof(Icons.Material.Filled);
        //        iconObject = new Icons.Material.Filled();
        //        IconName = PropertyName;
        //    }


        //    if (type != null)
        //    {
        //        PropertyInfo info = type.GetProperty(IconName);
        //        if (info != null)
        //        {
        //            string value = info.GetValue(iconObject) as string;
        //            svgcode = value;
        //        }
        //    }

        //    return svgcode;

        //}

        private string GetIconByStingName(string PropertyName, Type t)
        {
            string svgcode = "";
            Type type = null;
            string[] path = PropertyName.Split('.');
            string IconName = null;
            object iconObject = new Icons.Material.Filled();
            if (path.Length == 2)
            {
                //This will assume the Filled section
                if (path[0].Equals("Filled"))
                {
                    //type = Icons.Material.Filled.GetType();
                    //iconObject = Icons.Material.Filled;

                    iconObject = new Icons.Material.Filled();
                }
                //This will assume the Filled section
                if (path[0].Equals("Outlined"))
                {
                    //type = Icons.Material.Outlined.GetType();
                    //iconObject = Icons.Material.Outlined;

                    iconObject = new Icons.Material.Outlined();
                }

                if (path[0].Equals("TwoTone"))
                {
                    //    type = Icons.Material.TwoTone.GetType();
                    //    iconObject = Icons.Material.TwoTone;

                    iconObject = new Icons.Material.TwoTone();

                }

                if (path[0].Equals("Sharp"))
                {
                    //type = Icons.Material.Sharp.GetType();
                    //iconObject = Icons.Material.Sharp;

                    iconObject = new Icons.Material.Sharp();
                }


                if (path[0].Equals("Rounded"))
                {
                    //type = Icons.Material.Rounded.GetType();
                    //iconObject = Icons.Material.Rounded;

                    iconObject = new Icons.Material.Rounded();
                }

                IconName = path[1];

            }
            else
            {
                //type = Icons.Material.Filled.GetType();
                //iconObject = Icons.Material.Filled;

                iconObject = new Icons.Material.Filled();
                IconName = PropertyName;
            }

            type = iconObject.GetType();
            if (type != null)
            {
                //PropertyInfo info = type.GetProperty(IconName);
                FieldInfo info = type.GetField(IconName);
                if (info != null)
                {
                    string value = info.GetValue(iconObject) as string;
                    svgcode = value;
                }
            }

            return svgcode;

        }

        private async void OnFuntion()
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
                        args.sender = this;
                        args.DataObject = this.DataObject;
                        args.InitiatorObject = UIElement;
                        callback.InvokeAsync(args);
                    }
                    else
                    {

                    }

                }
            }

        }
        public async Task Refresh()
        {
            await SetCaption();
        }

        public void ResetToInitialValue()
        {
            throw new NotImplementedException();
        }

        public void UpdateVisibility(bool IsVisible)
        {
            throw new NotImplementedException();
        }
        public void ToggleEditable(bool IsEditable)
        {
            throw new NotImplementedException();
        }

        public async Task SetValue(object value)
        {
            if (value != null)
            {
                Caption = UIElement.ElementCaption + value.ToString();
            }
            StateHasChanged();
            await Task.CompletedTask;
        }

        public Task FocusComponentAsync()
        {
            throw new NotImplementedException();
        }
    }
}
