using System;
using System.Collections.Generic;
using MudBlazor;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.razorComponents.MB.Settings
{
	public class UIDefaults
	{
		public static Variant DefaultControlVariants { get; set; } = Variant.Text;
		public static PickerVariant DatePickerVariants { get; set; } = PickerVariant.Dialog;
	}
	public class AppSettings
	{

		public static string _AppBarName { get; set; } = "Home";

		public static BLMiniDrawer _miniDrawer { get; set; } = new BLMiniDrawer();

		public static void RefreshTopBar(string AppBarName)
		{
			_AppBarName = AppBarName;
			if (_miniDrawer != null)
			{
				_miniDrawer.UpdateHeaderTitle();
			}
		}
	}
}
