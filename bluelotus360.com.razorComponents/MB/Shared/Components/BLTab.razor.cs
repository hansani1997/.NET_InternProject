using BlueLotus360.CleanArchitecture.Domain.Entities;
using bluelotus360.com.razorComponents.Extensions;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bluelotus360.Com.MauiSupports.Services.Detectors;
using Microsoft.JSInterop;
using bluelotus360.com.razorComponents.StateManagement;

namespace bluelotus360.com.razorComponents.MB.Shared.Components
{
    public partial class BLTab
    {
        [Parameter] public BLUIElement FormObject { get; set; }
        [Parameter] public object DataObject { get; set; }
        [Parameter] public IDictionary<string, EventCallback> InteractionLogics { get; set; }
        [Parameter] public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }

        double ScreenWidth;
        WindowSize ws = new WindowSize();
        BLBreakpoint breakpointMargin = new BLBreakpoint();

        
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
        }



        protected override async void OnParametersSet()
        {
            ws = await JS.InvokeAsync<WindowSize>("getWindowSize");

            if (ws.Width<600)
            {
                breakpointMargin.BreakpointMargin = MudBlazor.Breakpoint.Xs;
            }
            else if (600<=ws.Width && ws.Width<960  )
            {
                breakpointMargin.BreakpointMargin = MudBlazor.Breakpoint.Sm;
            }
            else if (960 <= ws.Width && ws.Width < 1280)
            {
                breakpointMargin.BreakpointMargin = MudBlazor.Breakpoint.Md;
            }
            else if (1280 <= ws.Width && ws.Width < 1920)
            {
                breakpointMargin.BreakpointMargin = MudBlazor.Breakpoint.Lg;
            }
            else if (1920 <= ws.Width && ws.Width < 2560)
            {
                breakpointMargin.BreakpointMargin = MudBlazor.Breakpoint.Xl;
            }

            StateHasChanged();
        }
    }
}
