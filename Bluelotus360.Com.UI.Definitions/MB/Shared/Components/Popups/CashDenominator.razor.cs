using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlueLotus360.Com.UI.Definitions.MB.Shared.Components.Popups
{
    public partial class CashDenominator
    {
        [Parameter]
        public IList<DenominationEntry> Entries { get; set; }

        protected override async Task OnInitializedAsync()
        {
            DenominationRequest request=
                new DenominationRequest();
            Entries = await _transactionManager.ReadDenominationEntries(request);
            StateHasChanged();
            await base.OnInitializedAsync();
        }


    }
}
