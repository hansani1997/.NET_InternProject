using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Com.UI.Definitions.MB.Shared.Components
{
    public partial class GenericRecieptEntryLine
    {
        [Parameter]
        public PaymentModeWiseAmount RecieptLine { get; set; }


        protected override Task OnParametersSetAsync()
        {

            return base.OnParametersSetAsync();
        }
    }
}
