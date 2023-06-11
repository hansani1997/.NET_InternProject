using Microsoft.AspNetCore.Components;

namespace BlueLotus360.Com.UI.Definitions.MB.Shared.Components.Chart;
    public partial class BLChartContainer
    {
        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter] public double Height { get; set; } = 400;
    }

