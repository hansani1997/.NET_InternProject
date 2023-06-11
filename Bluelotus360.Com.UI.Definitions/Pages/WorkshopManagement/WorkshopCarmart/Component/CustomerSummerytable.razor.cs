using BL10.CleanArchitecture.Domain.Entities.WorkShopManagement;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Com.UI.Definitions.Pages.WorkshopManagement.WorkshopCarmart.Component
{
    public partial class CustomerSummerytable
    {
        [Parameter] public Vehicle SelectedVehicle { get; set; }
    }
}
