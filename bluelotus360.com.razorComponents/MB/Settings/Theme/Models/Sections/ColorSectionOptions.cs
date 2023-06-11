using bluelotus360.com.razorComponents.MB.Settings.Theme.Enums;
using MudBlazor;
using MudBlazor.Utilities;

namespace bluelotus360.com.razorComponents.MB.Settings.Theme.Models.Sections;

public class ColorSectionOptions
{
    private static readonly List<MudColor> DefaultColors = new()
    {
        Colors.DeepPurple.Default,
        Colors.Green.Default,
        Colors.Blue.Default,
        Colors.Red.Default
    };

    public bool Display { get; set; }
    public List<MudColor> ColorOptions { get; set; } = DefaultColors;
    public MudColor SelectedColor { get; set; }
    public ColorSectionVariants Variant { get; set; } = ColorSectionVariants.Basic;
}