using BL10.CleanArchitecture.Domain.Entities.Theme;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.razorComponents.MB.Settings
{
    public class BL10LookAndFeel
    {
        //private static string[] font_family = new[] { "inter", "Montserrat", "Helvetica", "Arial", "sans-serif" };
        private static string[] font_family = new[] { "Cera_Round_Pro", "Montserrat", "Roboto", "sans-serif" };

        private static Typography DefaultTypography = new Typography()
        {
            Default = new Default()
            {
                FontFamily = font_family,
                FontSize = ".875rem",
                FontWeight = 400,
                LineHeight = 1.43,
                LetterSpacing = ".01071em"
            },
            H1 = new H1()
            {
                FontFamily = font_family,
                FontSize = "6rem",
                FontWeight = 300,
                LineHeight = 1.167,
                LetterSpacing = "-.01562em"
            },
            H2 = new H2()
            {
                FontFamily = font_family,
                FontSize = "3.75rem",
                FontWeight = 300,
                LineHeight = 1.2,
                LetterSpacing = "-.00833em"
            },
            H3 = new H3()
            {
                FontFamily = font_family,
                FontSize = "3rem",
                FontWeight = 400,
                LineHeight = 1.167,
                LetterSpacing = "0"
            },
            H4 = new H4()
            {
                FontFamily = font_family,
                FontSize = "2.125rem",
                FontWeight = 400,
                LineHeight = 1.235,
                LetterSpacing = ".00735em"
            },
            H5 = new H5()
            {
                FontFamily = font_family,
                FontSize = "1.5rem",
                FontWeight = 400,
                LineHeight = 1.334,
                LetterSpacing = "0"
            },
            H6 = new H6()
            {
                FontFamily = font_family,
                FontSize = "1.25rem",
                FontWeight = 400,
                LineHeight = 1.6,
                LetterSpacing = ".0075em"
            },
            Button = new Button()
            {
                FontFamily = font_family,
                FontSize = ".875rem",
                FontWeight = 500,
                LineHeight = 1.75,
                LetterSpacing = ".02857em"
            },
            Body1 = new Body1()
            {
                FontFamily = font_family,
                FontSize = "1rem",
                FontWeight = 400,
                LineHeight = 1.5,
                LetterSpacing = ".00938em"
            },
            Body2 = new Body2()
            {
                FontFamily = font_family,
                FontSize = ".875rem",
                FontWeight = 400,
                LineHeight = 1.43,
                LetterSpacing = ".01071em"
            },
            Caption = new Caption()
            {
                FontFamily = font_family,
                FontSize = ".75rem",
                FontWeight = 400,
                LineHeight = 1.66,
                LetterSpacing = ".03333em"
            },
            Subtitle2 = new Subtitle2()
            {
                FontFamily = font_family,
                FontSize = ".875rem",
                FontWeight = 500,
                LineHeight = 1.57,
                LetterSpacing = ".00714em"
            }
        };

        private static LayoutProperties DefaultLayoutProperties = new LayoutProperties()
        {
            DefaultBorderRadius = "20px",
            DrawerWidthLeft = "360px;",
        };

        public static MudTheme DefaultTheme = new MudTheme()
        {
            Palette = new Palette()
            {
                Primary = "#183153",
                Secondary = "#FF8A00",
                AppbarBackground = "#ffffff",
                Background = "#183153",
                DrawerBackground = "#ffffff",
                DrawerText = "rgba(255,255,255, 1)",
                DrawerIcon = "#ffffff",
                Success = "#007E33"
            },
            Typography = DefaultTypography,
            LayoutProperties = DefaultLayoutProperties

        };

        public static MudTheme DarkTheme = new MudTheme()
        {
            Palette = new Palette()
            {
                Primary = "#063B71",
                Success = "#007E33",
                Black = "#27272f",
                Background = "#161622",
                BackgroundGrey = "#27272f",
                Surface = "#373740",
                DrawerBackground = "#27272f",
                DrawerText = "rgba(255,255,255, 0.50)",
                AppbarBackground = "#373740",
                AppbarText = "rgba(255,255,255, 0.70)",
                TextPrimary = "rgba(255,255,255, 0.70)",
                TextSecondary = "rgba(255,255,255, 0.50)",
                ActionDefault = "#adadb1",
                ActionDisabled = "rgba(255,255,255, 0.26)",
                ActionDisabledBackground = "rgba(255,255,255, 0.12)",
                DrawerIcon = "rgba(255,255,255, 1)",

            },
            Typography = DefaultTypography,
            LayoutProperties = DefaultLayoutProperties,

        };
        public static MudTheme GetBlThemePreset(ClientThemePreference? theme)
        {
            var primaryColor = theme?.PrimaryColor ?? "#183153";
            var secondaryColor = theme?.SecondaryColor ?? "#FF8A00";
            var fontSize = theme?.FontSize ?? ".875rem";
            var borderRadius = theme?.BorderRadius ?? "20px";

            return new MudTheme
            {
                Palette = new Palette
                {
                    Primary = primaryColor,
                    Secondary = secondaryColor,
                    AppbarBackground = "#ffffff",
                    Background = "#183153",
                    DrawerBackground = "#ffffff",
                    DrawerText = "rgba(255,255,255, 1)",
                    DrawerIcon = "#ffffff",
                    Success = "#007E33"
                },
                Typography = DefaultTypography,
                LayoutProperties = new LayoutProperties()
                {
                    DefaultBorderRadius = borderRadius,
                    DrawerWidthLeft = "360px;",
                }
        };
 

        }

        
    }
}
