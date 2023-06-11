
using BlueLotus360.CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections.Generic;

namespace BlueLotus360.Com.UI.Definitions.MB.Shared;
public partial class SideMenu
{
    [Parameter]
    public MenuItem Menu { get; set; }
    [Parameter] public IDictionary<string, string> IconDictionary { get; set; }

    public async Task NavigateToNewTab(string URL)
    {
        if (!string.IsNullOrEmpty(URL))
        {
            string url = URL;
            await _jsRuntime.InvokeVoidAsync("open", url, "_blank");
        }
            
    }
}

