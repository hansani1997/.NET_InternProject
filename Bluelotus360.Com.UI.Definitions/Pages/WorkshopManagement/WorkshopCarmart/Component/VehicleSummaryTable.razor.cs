using BL10.CleanArchitecture.Domain.Entities.WorkShopManagement;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Com.UI.Definitions.Pages.WorkshopManagement.WorkshopCarmart.Component
{
    public partial class VehicleSummaryTable
    {
        [Parameter] public Vehicle SelectedVehicle { get; set; }
        CompletedUserAuth auth = new CompletedUserAuth();
        protected override async Task OnInitializedAsync()
        {
            auth = await _authenticationManager.GetUserInformation();
        }
    }
}
