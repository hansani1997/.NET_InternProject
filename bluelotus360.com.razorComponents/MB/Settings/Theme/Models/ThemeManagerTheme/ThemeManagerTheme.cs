using bluelotus360.com.razorComponents.MB.Settings.Theme.Enums;
using MudBlazor;

namespace bluelotus360.com.razorComponents.MB.Settings.Theme.Models.ThemeManagerTheme;

public class ThemeManagerTheme
{
    public string PresetThemes { get; set; }
    public Modes Mode { get; set; }
    public ThemeManagerThemePalette Palette { get; set; } = new();
    public int DefaultBorderRadiusAsInt { get; set; }
    public LayoutProperties LayoutProperties { get; set; }
}