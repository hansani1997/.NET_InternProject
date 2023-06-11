using bluelotus360.Com.commonLib.Setting;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.razorComponents.MB
{
    public partial class MiniNavMenu
    {
        [Parameter]
        public EventCallback DrawerToggle { get; set; }
        [Parameter] public MenuItem Menus { get; set; } = new();

        public IDictionary<int, string> icon_dict;
        protected override async Task OnInitializedAsync()
        {
            icon_dict = new Dictionary<int, string>();
            //menuItem = await _navManger.GetNavOrPinnedMenus("nav-menu");

            if (Menus != null && Menus.SubMenus != null)
            {
                SetIcon(Menus.SubMenus);
            }
        }

        private void SetIcon(IList<MenuItem> list)
        {
            foreach (var itm in list)
            {
                icon_dict[itm.MenuId] = GetIconByStringName(itm.MenuIcon, typeof(Icons));
                SetIcon(itm.SubMenus);
            }
        }

        private string GetIconByStringName(string PropertyName, Type t)
        {
            string svgcode = "";
            if (string.IsNullOrEmpty(PropertyName))
            {
                PropertyName = "Filled.SpaceDashboard";
            }
            Type type = t;
            string[] path = PropertyName.Split(".");
            string IconName = "";
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
				svgcode=IconConversion.ConvertMudIconToFontAwesome(IconName);

				//type = iconObject.GetType();
    //            if (type != null)
    //            {
    //                //PropertyInfo info = type.GetProperty(IconName);
    //                FieldInfo info = type.GetField(IconName);
    //                if (info != null)
    //                {
    //                    string value = info.GetValue(iconObject) as string;
    //                    svgcode = value;
    //                }
    //            }

            }
            else
            {
                svgcode= PropertyName;
            }

            

            return svgcode;

        }

        //string GetIconFromFontAwesome(string PropertyName, Type t)
        //{
        //    string IconSvgCode = "";

        //    if (string.IsNullOrEmpty(PropertyName))
        //    {
        //        PropertyName = "Filled.SpaceDashboard";
        //    }
        //    Type type = t;
        //    string[] path = PropertyName.Split('.');
        //    string IconName = null;
        //    object iconObject = new Icons.Material.TwoTone();
        //    if (path.Length == 2)
        //    {
        //        //This will assume the Filled section
        //        if (path[0].Equals("Filled"))
        //        {
        //            type = Icons.Material.Filled.GetType();
        //            iconObject = Icons.Material.Filled;
        //        }
        //        //This will assume the Filled section
        //        if (path[0].Equals("Outlined"))
        //        {
        //            type = Icons.Material.Outlined.GetType();
        //            iconObject = Icons.Material.Outlined;
        //        }

        //        if (path[0].Equals("TwoTone"))
        //        {
        //            type = Icons.Material.TwoTone.GetType();
        //            iconObject = Icons.Material.TwoTone;
        //        }

        //        if (path[0].Equals("Sharp"))
        //        {
        //            type = Icons.Material.Sharp.GetType();
        //            iconObject = Icons.Material.Sharp;
        //        }


        //        if (path[0].Equals("Rounded"))
        //        {
        //            type = Icons.Material.Rounded.GetType();
        //            iconObject = Icons.Material.Rounded;
        //        }

        //        IconName = path[1];

        //    }
        //    else
        //    {
        //        type = Icons.Material.Filled.GetType();
        //        iconObject = Icons.Material.Filled;
        //        IconName = PropertyName;
        //    }


        //    if (type != null)
        //    {
        //        PropertyInfo info = type.GetProperty(IconName);
        //        if (info != null)
        //        {
        //            string value = info.GetValue(iconObject) as string;
        //            IconSvgCode = value;
        //        }
        //    }

        //    return IconSvgCode;

        //}
    }
}
