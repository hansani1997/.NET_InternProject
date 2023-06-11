using bluelotus360.com.razorComponents.Extensions;
using bluelotus360.Com.commonLib.Setting;
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
	public partial class BLToolButton : IBLUIOperationHelper
	{
		[Parameter]
		public BLUIElement FromSection { get; set; }

		[Parameter]
		public object DataObject { get; set; }

		[Parameter]
		public IDictionary<string, EventCallback> InteractionLogics { get; set; }
		[Parameter]
		public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }
		public BLUIElement LinkedUIObject => throw new NotImplementedException();
		private string btn_css = "";
		private string IconSvgCode = "";
		private bool IsComponentDisabled = false;

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
			throw new NotImplementedException();
		}

		protected override void OnInitialized()
		{
			btn_css = (FromSection.IsVisible ? FromSection.CssClass : "d-none");

			string[] path = this.FromSection.IconCss.Split('.');
			GetIconByStingName(this.FromSection.IconCss, typeof(Icons));
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
		private void OnToolButtonClick()
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

        private void GetIconByStingName(string PropertyName, Type t)
        {
            if (!string.IsNullOrEmpty(PropertyName))
            {

                Type type = t;
                string[] path = PropertyName.Split(".");
                string IconName = "";
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
					IconSvgCode = IconConversion.ConvertMudIconToFontAwesome(IconName);
					//type = iconObject.GetType();
     //               if (type != null && !string.IsNullOrEmpty(IconName))
     //               {
     //                   //PropertyInfo info = type.GetProperty(IconName);
     //                   FieldInfo info = type.GetField(IconName);
     //                   if (info != null)
     //                   {
     //                       string value = info.GetValue(iconObject) as string;
     //                       IconSvgCode = value;
     //                   }
     //               }

                }
                else
                {
                    IconSvgCode = PropertyName;
                }

                
            }

        }

        //void GetIconByStingName(string PropertyName, Type t)
        //{

        //	Type type = null;
        //	string[] path = PropertyName.Split('.');
        //	string IconName = null;
        //	object iconObject = Icons.Material.Filled;
        //	if (path.Length == 2)
        //	{
        //		//This will assume the Filled section
        //		if (path[0].Equals("Filled"))
        //		{
        //			type = Icons.Material.Filled.GetType();
        //			iconObject = Icons.Material.Filled;
        //		}
        //		//This will assume the Filled section
        //		if (path[0].Equals("Outlined"))
        //		{
        //			type = Icons.Material.Outlined.GetType();
        //			iconObject = Icons.Material.Outlined;
        //		}

        //		if (path[0].Equals("TwoTone"))
        //		{
        //			type = Icons.Material.TwoTone.GetType();
        //			iconObject = Icons.Material.TwoTone;
        //		}

        //		if (path[0].Equals("Sharp"))
        //		{
        //			type = Icons.Material.Sharp.GetType();
        //			iconObject = Icons.Material.Sharp;
        //		}


        //		if (path[0].Equals("Rounded"))
        //		{
        //			type = Icons.Material.Rounded.GetType();
        //			iconObject = Icons.Material.Rounded;
        //		}

        //		IconName = path[1];

        //	}
        //	else
        //	{
        //		type = Icons.Material.Filled.GetType();
        //		iconObject = Icons.Material.Filled;
        //		IconName = PropertyName;
        //	}


        //	if (type != null)
        //	{
        //		PropertyInfo info = type.GetProperty(IconName);
        //		if (info != null)
        //		{
        //			string value = info.GetValue(iconObject) as string;
        //			IconSvgCode = value;
        //		}
        //	}



        //}
    }
}
