using bluelotus360.com.razorComponents.MB.Settings.Theme.Models.Sections;
using Microsoft.AspNetCore.Components;

namespace bluelotus360.com.razorComponents.MB.Settings.Theme.Components;

public partial class BLTmSection
{
    private bool _open;

    [EditorRequired] [Parameter] public string? Title { set; get; }
    [EditorRequired] [Parameter] public SectionOptions? SectionOptions { set; get; }
    [Parameter] public RenderFragment? ChildContent { get; set; }

    protected override void OnInitialized()
    {
        if (SectionOptions is {DefaultOpen: true})
            _open = true;
    }
}