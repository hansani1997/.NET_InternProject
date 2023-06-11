using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using BL10.CleanArchitecture.Domain.Entities.WorkShopManagement;
using BlueLotus360.Com.UI.Definitions.Extensions;
using BlueLotus360.CleanArchitecture.Client.Infrastructure.Helpers;
using Microsoft.JSInterop;

namespace BlueLotus360.Com.UI.Definitions.Pages.WorkshopManagement.BookingModule
{
    public partial class Booking
    {
        #region parameter

        [Parameter] public BLUIElement UIScope { get; set; }
        [Parameter] public Booking _bookingObject { get; set; }
        [Parameter] public IDictionary<string, EventCallback> InteractionLogic { get; set; }
        [Parameter] public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }

        private string[] hiddenArray;
        private string[] displayedArray;

        long elementKey;

        #endregion

        #region General

        protected override void OnParametersSet()
        {
            if (UIScope != null)
            {
                InteractionHelper helper = new InteractionHelper(this, UIScope);
                InteractionLogic = helper.GenerateEventCallbacks();
            }

            base.OnParametersSet();
        }
        protected override async Task OnInitializedAsync() 
        {
            elementKey = 1;
            RefreshGrid();
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
        private async void PageShowHide(string[] hidden, string[] displayed)
        {
            foreach (string div in hidden)
            {
                await _jsRuntime.InvokeVoidAsync("HideDiv", div);
            }
            foreach (string div in displayed)
            {
                await _jsRuntime.InvokeVoidAsync("ShowDiv", div);
            }
            StateHasChanged();
        }
        private void RefreshGrid() 
        {
            hiddenArray = new string[] { };
            displayedArray = new string[] { };
        }

        #endregion
    }
}
