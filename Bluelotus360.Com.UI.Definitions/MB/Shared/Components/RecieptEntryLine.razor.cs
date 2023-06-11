
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace BlueLotus360.Com.UI.Definitions.MB.Shared.Components
{
    public partial class RecieptEntryLine
    {
        
        [Parameter]
        public AccoutRecieptPayment RecieptLine { get; set; }


        protected override Task OnParametersSetAsync()
        {
            long c = RecieptLine.AccountTransactionKey;
            return base.OnParametersSetAsync();
        }




    }
}
