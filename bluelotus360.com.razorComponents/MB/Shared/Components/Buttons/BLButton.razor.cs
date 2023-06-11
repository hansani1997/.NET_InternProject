using bluelotus360.com.razorComponents.Extensions;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.razorComponents.MB.Shared.Components.Buttons
{
    public  partial class BLButton : IBLUIOperationHelper
    {
        [Parameter]
        public BLUIElement FromSection { get; set; }

        [Parameter]
        public object DataObject { get; set; }

        [Parameter]
        public IDictionary<string, EventCallback> InteractionLogics { get; set; }

        [Parameter]
        public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }

        private string css_class = "";
        private string btn_css = "";

        Color ControlColor = Color.Default;
        private string IconSvgCode = "";
        private bool IsComponentDisabled = false;
        public BLUIElement LinkedUIObject { get; private set; }

        public Task FocusComponentAsync()
        {
            throw new NotImplementedException();
        }

        public async Task Refresh()
        {
            await Task.CompletedTask;
        }

        public void ResetToInitialValue()
        {
            throw new NotImplementedException();
        }

        public Task SetValue(object value)
        {
            throw new NotImplementedException();
        }

        public void ToggleEditable(bool IsEditable)
        {
            IsComponentDisabled = !IsEditable;
            StateHasChanged();
        }

        public void UpdateVisibility(bool IsVisible)
        {
            this.FromSection.IsVisible = IsVisible;
            css_class = FromSection.IsVisible ? "d-flex" : "d-none" + " align-end";
        }

        protected override void OnInitialized()
        {
            css_class = (FromSection.IsVisible ? "d-flex" : "d-none") + " align-end";
            btn_css = (FromSection.IsVisible ? FromSection.CssClass : "") + " flex-grow-1";

            string[] path = this.FromSection.IconCss.Split('.');
            GetIconByStingName(this.FromSection.IconCss, typeof(Icons));

            StateHasChanged();
            base.OnInitialized();
        }
        protected override void OnParametersSet()
        {
            if (ObjectHelpers != null)
            {
                if (ObjectHelpers.ContainsKey(FromSection.ElementName))
                {
                    ObjectHelpers.Remove(FromSection.ElementName);

                }
                ObjectHelpers.Add(FromSection.ElementName, this);
            }

            Color BtnCp;
            Enum.TryParse(FromSection.CssClass, out BtnCp);
            ControlColor = BtnCp;
            base.OnParametersSet();
        }
        private void OnButtonClick()
        {
            UIInterectionArgs<object> args = new UIInterectionArgs<object>();
            if (InteractionLogics != null && FromSection.OnClickAction != null && FromSection.OnClickAction.Length > 3)
            {
                EventCallback callback;
                if (InteractionLogics.TryGetValue(FromSection.OnClickAction, out callback))
                {
                    if (callback.HasDelegate)
                    {
                        args.Caller = this.FromSection.OnClickAction;
                        args.ObjectPath = this.FromSection.DefaultAccessPath;
                        args.DataObject = (DataObject == null) ? new object() : DataObject;
                        args.sender = this;
                        args.InitiatorObject = FromSection;
                        callback.InvokeAsync(args);
                    }
                }
            }
        }

        private void GetIconByStingName(string PropertyName, Type t)
        {
            if (!string.IsNullOrEmpty(PropertyName))
            {

                Type type = null;
                string[] path = PropertyName.Split('.');
                string IconName = null;
                object iconObject = new Icons.Material.Filled();
                if (path.Length == 2)
                {
                    //This will assume the Filled section
                    if (path[0].Equals("Filled"))
                    {
                        iconObject = new Icons.Material.Filled();
                    }
                    //This will assume the Filled section
                    if (path[0].Equals("Outlined"))
                    {
                        iconObject = new Icons.Material.Outlined();
                    }

                    if (path[0].Equals("TwoTone"))
                    {
                        iconObject = new Icons.Material.TwoTone();

                    }

                    if (path[0].Equals("Sharp"))
                    {
                        iconObject = new Icons.Material.Sharp();
                    }


                    if (path[0].Equals("Rounded"))
                    {
                        iconObject = new Icons.Material.Rounded();
                    }

                    IconName = path[1];

                    type = iconObject.GetType();
                    if (type != null && !string.IsNullOrEmpty(IconName))
                    {
                        //PropertyInfo info = type.GetProperty(IconName);
                        FieldInfo info = type.GetField(IconName);
                        if (info != null)
                        {
                            string value = info.GetValue(iconObject) as string;
                            IconSvgCode = value;
                        }
                    }

                }
                else
                {
                    //iconObject = new Icons.Material.Filled();
                    //IconName = PropertyName;
                    IconSvgCode = PropertyName;
                }


            }

        }
    }
}
