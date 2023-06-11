using BlueLotus360.CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.razorComponents.MB
{
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
                Debug.WriteLine(url);
                _navigationManager.NavigateTo(url);
                //await _jsRuntime.InvokeVoidAsync("open", url, "_blank");
            }
            StateHasChanged();
        }
    }
}
