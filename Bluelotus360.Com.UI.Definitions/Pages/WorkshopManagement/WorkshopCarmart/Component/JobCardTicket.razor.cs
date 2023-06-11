using BL10.CleanArchitecture.Domain.Entities.WorkShopManagement;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.Com.UI.Definitions.Extensions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Com.UI.Definitions.Pages.WorkshopManagement.WorkshopCarmart.Component
{
    public partial class JobCardTicket
    {
        [Parameter] public EventCallback<int> Activate { get; set; }
        
        [Parameter] public BLUIElement UIScope { get; set; }

        [Parameter] public WorkOrder DataObject { get; set; }
        [Parameter] public EventCallback DisableTabs { get; set; }
        BLUIElement CustomerNotes;
        BLUIElement PlaningSection;
        protected override async Task OnParametersSetAsync()
        {
            CustomerNotes = SplitUIComponent("CustomerNotes");
            PlaningSection = SplitUIComponent("PlaningSection");
            base.OnParametersSetAsync();
        }

        private BLUIElement SplitUIComponent(string domName)
        {
            BLUIElement parent = new BLUIElement();
            if (UIScope != null && !string.IsNullOrEmpty(domName))
            {
                parent = UIScope.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals(domName)).FirstOrDefault();

            }

            if (parent != null)
            {
                parent.Children = UIScope.Children.Where(x => x.ParentKey == parent.ElementKey).ToList();
            }

            return parent;
        }
    }
}
