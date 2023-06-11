using BL10.CleanArchitecture.Domain.Entities.Booking;
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
    public partial class TbTable
    {
        [Parameter] public BLUIElement FormObject { get; set; }
        [Parameter] public object DataObject { get; set; }
        [Parameter] public IDictionary<string, EventCallback> InteractionLogics { get; set; }
        [Parameter] public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }

        private BookingDetails bookingDetails { get; set; }

        protected override void OnInitialized()
        {
            bookingDetails = (BookingDetails)DataObject;
            base.OnInitialized();
        }
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
        }
    }
}
