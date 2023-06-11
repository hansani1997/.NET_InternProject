using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.commonLib.Setting
{
	public static class IconConversion
	{
		public static string ConvertMudIconToFontAwesome(string iconCss)
		{
			switch (iconCss)
			{
				case "Home":return "fa-solid fa-house";
				case "Settings": return "fa-solid fa-gears";
				case "Splitscreen": return "fa-solid fa-boxes-stacked";
				case "Build": return "fa-solid fa-wrench";
				case "HomeRepairService": return "fa-solid fa-warehouse";
				case "CreditCard": return "fa-solid fa-cash-register";
				case "Balance": return "fa-solid fa-file-invoice-dollar";
				case "Group": return "fa-solid fa-user-group";
				case "SpaceDashboard": return "fa-solid fa-cube";
				case "Dashboard": return "fa-solid fa-chart-line";
				case "Android":return "fa-solid fa-cube";
				case "CreateNewFolder": return "fa-solid fa-file";
				case "Save":return "fa-solid fa-floppy-disk";
				case "Search": return "fa-solid fa-solid fa-magnifying-glass";
				case "Print":return "fa-solid fa-print";
				case "Cancel": return "fa-solid fa-xmark";
				case "QrCodeScanner": return "fa-solid fa-qrcode";
				case "FilterAlt": return "fa-solid fa-filter";
				case "Add": return "fa-solid fa-plus";
				case "Reply":return "fa-solid fa-circle-arrow-left";
				case "PersonAdd":return "fa-solid fa-user-plus";
				case "Receipt": return "fa-solid fa-receipt";
				case "Money": return "fa-solid fa-money-check";
				case "Checklist": return "fa-solid fa-list-check";
				case "RequestQuote": return "fa-brands fa-get-pocket";
				case "FilterAltOff":return "fa-solid fa-filter-circle-xmark";
				case "RemoveRedEye": return "fa-solid fa-eye";
            }
			return string.Empty;
		}
	}
}
