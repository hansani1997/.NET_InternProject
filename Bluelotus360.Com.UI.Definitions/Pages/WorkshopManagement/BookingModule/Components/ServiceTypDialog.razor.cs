using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.Com.UI.Definitions.Extensions;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Com.UI.Definitions.Pages.WorkshopManagement.BookingModule.Components
{
    public partial class ServiceTypDialog
    {
        [Parameter] public CodeBaseResponse Other { get; set; }

        [Parameter] public BLUIElement ModalUIElement { get; set; }
        [Parameter] public IDictionary<string, EventCallback> InteractionLogics { get; set; }

        [Parameter] public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }

        [Parameter] public bool IsPopShown { get; set; }
        protected override async Task OnParametersSetAsync()
        {

            await base.OnParametersSetAsync();
        }
        protected override async Task OnInitializedAsync()
        {

            await base.OnInitializedAsync();
        }
    }
}
