
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace bluelotus360.com.razorComponents.Pages.Transaction.TransactionComponent
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
