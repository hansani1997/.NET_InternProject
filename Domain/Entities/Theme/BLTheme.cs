using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL10.CleanArchitecture.Domain.Entities.Theme
{
    public class BLPreference
    {
        ClientLanguagePrefernce Theme { get; set; }=new ClientLanguagePrefernce();
        ClientLanguagePrefernce LanguagePrefernce { get; set; }=new ClientLanguagePrefernce();
        public BLPreference() 
        {
            Theme= new ClientLanguagePrefernce();
            LanguagePrefernce= new ClientLanguagePrefernce();   
        }
    }
    public class ClientThemePreference
    {
        public int CKy { get; set; } = 1;
        public int UsrKy { get; set; } = 1;
        public long ThemeKey { get; set; } = 1;
		public int ThemeType { get; set; } = 0;
		public string? BorderRadius { get; set; } = "20px";
        public string? Elevation { get; set; } = "0";
        public string? PrimaryColor { get; set; } = "#183153";
        public string? SecondaryColor { get; set; } = "#0CA678";
        public string? FontFamily { get; set; } = "";
        public string? FontSize { get; set; } = "20px";
        public bool IsDarkMode { get; set; }
        public bool IsRTL { get; set; }
        public bool IsDrawerOpen { get; set; }
        public int IsDefault { get; set; } 

        public ClientThemePreference()
        {

        }
        public bool IsthemePersisted()
        {
            return ThemeKey > 1;
        }
    }

    public class ClientLanguagePrefernce
    {
        public string LanguageCode { get; set; }
    }
}
