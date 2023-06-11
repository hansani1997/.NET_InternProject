using BlueLotus360.CleanArchitecture.Domain.Entities.HR;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlueLotus360.Com.UI.Definitions.MB.Shared.Components.Popups.HR
{
    public partial class PaySlipPopUp
    {
        [CascadingParameter]
        MudDialogInstance MudDialog { get; set; }

        [Parameter]
        public IList<PaySlipDetails> PaySlipInfo { get; set; } = new List<PaySlipDetails>();

        private async Task Print()
        {
            await _jsRuntime.InvokeVoidAsync("Print", "print-content");
        }
    }
}
