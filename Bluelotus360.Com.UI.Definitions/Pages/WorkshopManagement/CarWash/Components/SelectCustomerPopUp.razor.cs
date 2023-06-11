using BL10.CleanArchitecture.Domain.Entities.Booking;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.Com.UI.Definitions.Extensions;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Com.UI.Definitions.Pages.WorkshopManagement.CarWash.Components
{
    public partial class SelectCustomerPopUp
    {
        [Parameter] public IList<CustomerDetailsByVehicle> CusList { get; set; }

        [Parameter] public BLUIElement ModalUIElement { get; set; }
        [Parameter] public IDictionary<string, EventCallback> InteractionLogics { get; set; }

        [Parameter] public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }

        [Parameter] public bool IsPopShown { get; set; }
        bool HideMinMax { get; set; } = false;
        protected override async Task OnParametersSetAsync()
        {

            await base.OnParametersSetAsync();
        }
        protected override async Task OnInitializedAsync()
        {

            await base.OnInitializedAsync();
        }
        private async void OnCloseClick()
        {
            //if (OnCloseButtonClick.HasDelegate)
            //{
            //    await OnCloseButtonClick.InvokeAsync();
            //}
            IsPopShown = false;
            StateHasChanged();
        }
    }
}
