using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Collections.Generic;
using BL10.CleanArchitecture.Domain;
using System;
using System.Reflection;
using System.Linq;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;

namespace BlueLotus360.Com.UI.Definitions.MB.Shared.Components.Buttons
{
    public partial class BLToggleButton
    {
        [Parameter]
        public BLUIElement FromSection { get; set; }

        [Parameter]
        public object DataObject { get; set; }

        [Parameter]
        public IDictionary<string, EventCallback> InteractionLogics { get; set; }


        private string IconSvgCode = "";
        private string ToggledIconSvgCode = "";

        private bool Toggled { get; set; }

        protected override void OnInitialized()
        {
            string[] path = this.FromSection.IconCss.Split('.');
            IconSvgCode = GetIconByStingName(this.FromSection.IconCss, typeof(Icons));
            ToggledIconSvgCode = GetIconByStingName($"{this.FromSection.IconCss}Off", typeof(Icons));
            base.OnInitialized();
        }


        private void OnToggledChanged(bool toggled)
        {
            UIInterectionArgs<object> args = new UIInterectionArgs<object>();
            UIInterectionArgs<object> toggled_args = new UIInterectionArgs<object>();
            Toggled = toggled;



            if (InteractionLogics != null && FromSection.OnClickAction != null && FromSection.OnToggledAction != null &&  FromSection.OnClickAction.Length > 3 && FromSection.OnToggledAction.Length >3)
            {
                EventCallback callback;
                EventCallback toggled_callback;

                if (Toggled)
                {
                    if (InteractionLogics.TryGetValue(FromSection.OnToggledAction, out toggled_callback))
                    {
                        toggled_args.Caller = this.FromSection.OnToggledAction;
                        toggled_args.ObjectPath = this.FromSection.DefaultAccessPath;
                        toggled_args.DataObject = new object();
                        toggled_args.sender = this;
                        toggled_args.InitiatorObject = FromSection;
                        toggled_callback.InvokeAsync(toggled_args);


                    }
                }
                else
                {
                    if (InteractionLogics.TryGetValue(FromSection.OnClickAction, out callback))
                    {
                            args.Caller = this.FromSection.OnClickAction;
                            args.ObjectPath = this.FromSection.DefaultAccessPath;
                            args.DataObject = new object();
                            args.sender = this;
                            args.InitiatorObject = FromSection;
                            callback.InvokeAsync(args);


                    }
                }


            }



        }

        string GetIconByStingName(string PropertyName, Type t)
        {
            string svgcode="";
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
                    svgcode = value;
                }
            }

            return svgcode;

        }
    }
}
