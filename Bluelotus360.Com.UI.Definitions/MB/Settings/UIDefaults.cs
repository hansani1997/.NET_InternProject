
using BlueLotus360.Com.UI.Definitions.MB.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;

namespace BlueLotus360.Com.UI.Definitions.MB.Settings
{
    public class UIDefaults
    {
        //public static Variant DefaultControlVariants { get; set; } = Variant.Outlined;
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
            if (_miniDrawer!=null)
            {
                _miniDrawer.UpdateHeaderTitle();
            }
            

        }


    }

}
