using BL10.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Client.Infrastructure.Helpers;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using BlueLotus360.Com.UI.Definitions.Extensions;
using BlueLotus360.Com.UI.Definitions.MB.Shared.Components;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Com.UI.Definitions.Pages.OrderHub.Component
{
    public partial class FilterOrder
    {
        [Parameter]
        public EventCallback OnCloseButtonClick { get; set; }
        [Parameter] public bool WindowIsVisible { get; set; } = true;
        [Parameter] public BLUIElement blElement { get; set; }
        [Parameter] public RequestParameters request { get; set; }
        [Parameter] public IList<PartnerOrder> Order { get; set; }
        [Parameter] public int LocationKey { get; set; }
        public IDictionary<string, EventCallback> InteractionLogics { get; set; }

        [Parameter]
        public EventCallback<RequestParameters> OnOpenClick { get; set; }
        public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }

        protected override async Task OnParametersSetAsync() 
        {
            if (blElement != null)
            {
                InteractionHelper helper = new InteractionHelper(this, blElement);//formdefinition has all form objects 
                InteractionLogics = helper.GenerateEventCallbacks();//
                StateHasChanged();
            }
            await base.OnParametersSetAsync();
        }

        protected override async Task OnInitializedAsync()
        {
            request = new RequestParameters();
            request.FromDate = DateTime.Now.ToString("yyyy/MM/dd");
            request.ToDate = DateTime.Now.ToString("yyyy/MM/dd");
        }

        private async void OnCloseClick()
        {
            if (OnCloseButtonClick.HasDelegate)
            {
                await OnCloseButtonClick.InvokeAsync();
            }

        }
        private void OnFromDateChange(UIInterectionArgs<DateTime?> args)
        {
           request.FromDate = args.DataObject.Value.ToString("yyyy/MM/dd");
            StateHasChanged();
        }

        private void OnToDateChange(UIInterectionArgs<DateTime?> args)
        {
            request.ToDate = args.DataObject.Value.ToString("yyyy/MM/dd");
            StateHasChanged(); 
        }
        private async void FilterOrderData(UIInterectionArgs<object> args)
        {

            RequestParameters parameters = new RequestParameters();
            parameters.FromDate = request.FromDate;
            parameters.ToDate = request.ToDate;
            parameters.LocationKey = LocationKey;
            if (OnOpenClick.HasDelegate)
            {
                await OnOpenClick.InvokeAsync(parameters);

            }
        }

       


    }
}
