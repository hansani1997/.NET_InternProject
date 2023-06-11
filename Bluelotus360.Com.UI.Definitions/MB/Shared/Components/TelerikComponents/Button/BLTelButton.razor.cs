
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.Com.UI.Definitions.Extensions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazor.Utilities;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace BlueLotus360.Com.UI.Definitions.MB.Shared.Components.TelerikComponents.Button
{
    public partial class BLTelButton : IBLUIOperationHelper
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

            private bool IsComponentEnabled = true;
            public BLUIElement LinkedUIObject { get; private set; }
            private string IconSvgCode = "";
            protected override void OnInitialized()
            {
                css_class = (FromSection.IsVisible ? "d-flex" : "d-none") + " align-end";
                btn_css = (FromSection.IsVisible ? FromSection.CssClass : "") + " flex-grow-1";
                

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
                        else
                        {

                        }

                    }
                }



            }

        

        public void ResetToInitialValue()
            {
                throw new System.NotImplementedException();
            }

            public void UpdateVisibility(bool IsVisible)
            {
                throw new System.NotImplementedException();
            }

            public void ToggleEditable(bool IsEditable)
            {
                IsComponentEnabled = IsEditable;
                StateHasChanged();
            }

            public async Task Refresh()
            {
                await Task.CompletedTask;
            }


            public Task FocusComponentAsync()
            {
                throw new System.NotImplementedException();
            }

            public Task SetValue(object value)
            {
                throw new System.NotImplementedException();
            }
        }
}
