using Microsoft.AspNetCore.Components;
using bluelotus360.com.razorComponents.MB.Settings.Theme.Enums;

namespace bluelotus360.com.razorComponents.MB.Settings.Theme.Components;

public partial class BLTmMode
{
    [EditorRequired] [Parameter] public Modes Mode { get; set; }
    [EditorRequired] [Parameter] public EventCallback<Modes> ModeChanged { get; set; }

    private void UpdateMode(Modes mode)
    {
        Mode = mode;
        ModeChanged.InvokeAsync(mode);
    }
}