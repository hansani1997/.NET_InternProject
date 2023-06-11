using System.Collections.Generic;
using bluelotus360.com.razorComponents.MB.Settings.Theme.Enums;
using bluelotus360.com.razorComponents.MB.Settings.Theme.Models.Sections;
using MudBlazor;
using MudBlazor.Utilities;

namespace bluelotus360.com.razorComponents.MB.Settings.Theme.Models;

public static class ThemeManagerPresetConfigurations
{
    public static ThemeManagerConfiguration GetPresetConfigOne(MudTheme? theme = null)
    {
        var themeManagerOptions = new ThemeManagerConfiguration
        {
            ShowLayoutSection = true,
            ShowColorSections = true,
            ShowPresetThemeSection= false,
        };

        themeManagerOptions.UpdateColorSection(ColorTitles.Primary, new ColorSectionOptions
        {
            Display = true,
            SelectedColor = theme is null ? Colors.Green.Default : theme.Palette.Primary.Value,
			Variant = ColorSectionVariants.Advanced
		});

        themeManagerOptions.UpdateColorSection(ColorTitles.Secondary, new ColorSectionOptions
        {
            Display = true,
            ColorOptions = new List<MudColor>
            {
                Colors.Pink.Default,
                Colors.Amber.Default,
                Colors.Teal.Lighten1,
                Colors.Lime.Default
            },
            SelectedColor = theme is null ? Colors.Amber.Default : theme.Palette.Secondary.Value,
			Variant = ColorSectionVariants.Advanced
        });

        themeManagerOptions.UpdateColorSection(ColorTitles.Tertiary, new ColorSectionOptions
        {
            Display = false,
            Variant = ColorSectionVariants.Advanced,
            SelectedColor = theme is null ? Colors.BlueGrey.Default : theme.Palette.Tertiary.Value
        });

        return themeManagerOptions;
    }

    public static ThemeManagerConfiguration GetPresetConfigTwo()
    {
        var themeManagerOptions = new ThemeManagerConfiguration
        {
            ShowPresetThemeSection = false,
            ShowColorSections = true,
        };

        return themeManagerOptions;
    }
}