using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.razorComponents.MB.Shared.Components.Popups
{
    public partial class CashDenominator
    {
        [Parameter]
        public IList<DenominationEntry> Entries { get; set; }

        protected override async Task OnInitializedAsync()
        {
            DenominationRequest request =
                new DenominationRequest();
            Entries = await _transactionManager.ReadDenominationEntries(request);
            StateHasChanged();
            await base.OnInitializedAsync();
        }
    }
}
