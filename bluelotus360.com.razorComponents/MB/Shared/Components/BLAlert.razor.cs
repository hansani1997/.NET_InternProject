using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.razorComponents.MB.Shared.Components
{
    public partial class BLAlert
    {
        [Parameter]
        public string AlertContent { get; set; }

        [Parameter]
        public string AlertStatus { get; set; }


        [Parameter]
        public EventCallback CloseAlert { get; set; }

        Status _servrity;

        protected override async Task OnInitializedAsync()
        {
            Enum.TryParse(AlertStatus, out Status sts);
            _servrity = sts;
            await base.OnInitializedAsync();
        }

        private async void Close()
        {
            await CloseAlert.InvokeAsync(null);
        }

        MudBlazor.Severity severity => _servrity switch
        {
            Status.Sucess => MudBlazor.Severity.Success,
            Status.Info => MudBlazor.Severity.Info,
            Status.Warning => MudBlazor.Severity.Warning,
            Status.Error => MudBlazor.Severity.Error,
            _ => MudBlazor.Severity.Normal,
        };

        private enum Status
        {
            Sucess,
            Info,
            Warning,
            Error,
            Normal,
            
        }
    }
}
