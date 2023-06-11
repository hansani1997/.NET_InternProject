using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.razorComponents.Pages.Transaction.TransactionComponent
{
    public partial class GenericRecieptEntryLine
    {
        [Parameter]
        public PaymentModeWiseAmount RecieptLine { get; set; }

        private MudNumericField<decimal> singleLineReferenceNumericBox;
        private MudTextField<string> singleLineReferenceTextBox;


        protected override Task OnParametersSetAsync()
        {

            return base.OnParametersSetAsync();
        }
    }
}
