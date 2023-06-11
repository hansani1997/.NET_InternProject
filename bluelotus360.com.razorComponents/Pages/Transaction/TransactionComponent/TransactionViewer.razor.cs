using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.razorComponents.Pages.Transaction.TransactionComponent
{
    public partial class TransactionViewer
    {
        [Parameter]
        public BLTransaction Transaction { get; set; }


        [Parameter]
        public InvoiceDetailsByHdrSerNo InvoiceResponse { get; set; }

        bool disableButton = false;
        public async Task Refesh()
        {
            StateHasChanged();
            await Task.CompletedTask;
        }

        public async Task OnCheckoutClick()
        {
            disableButton = true;
            var transaction = Transaction;
            await _transactionManager.SaveTransaction(Transaction);
            await Task.CompletedTask;

        }
    }
}
