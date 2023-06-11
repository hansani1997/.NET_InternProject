using BL10.CleanArchitecture.Domain.Entities.WorkShopManagement;
using BlueLotus360.CleanArchitecture.Client.Infrastructure.Helpers;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using BlueLotus360.Com.UI.Definitions.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Blazor.Components;
using static MudBlazor.Colors;
using static System.Collections.Specialized.BitVector32;

namespace BlueLotus360.Com.UI.Definitions.Pages.WorkshopManagement.WorkshopCarmart.Component
{
    public partial class JobSummery
    {
        [Parameter] public EventCallback<int> Activate { get; set; }
        [Parameter] public BLUIElement UIScope { get; set; }

        [Parameter] public WorkOrder DataObject { get; set; }

        private int pagePosition;
        private bool isBackButtonDisabled=true;
        private List<string> summerytypes=new List<string>() { "Service Item", "Spare Parts", "Hardware Items", "Labour Items", "Trading Items" };
        

        private async void OnBack()
        {
            if (pagePosition > 0)
            {
                pagePosition--;
            }
            if (pagePosition == 0)
            {
                isBackButtonDisabled = true;
                
                PageShowHide("job-summery-section2", "job-summery-section1");
            }
            else
            {
                isBackButtonDisabled = false;
            }
            this.StateHasChanged();
        }

        private async void PageShowHide(string hiddenDiv,string displayedDiv)
        {
            try
            {
                await _jsRuntime.InvokeVoidAsync("HideDiv", hiddenDiv);
                await _jsRuntime.InvokeVoidAsync("ShowDiv", displayedDiv);
            }
            catch (Exception ex)
            {

            }
           
            StateHasChanged();
        }

        private void ShowInvoiceSection()
        {
            pagePosition = 1;
            isBackButtonDisabled = false;
            PageShowHide("job-summery-section1", "job-summery-section2");
            StateHasChanged();
        }

        #region ERP Link Event

        private async void ViewEstimate() 
        {
            string URL = "https://bl360x.com/BL10/Object/LoadMenu?ObjKy=107752&InilizeRecordKey=1&RefOrdKy=1&Token=abscd";
            //_navigationManager.NavigateTo(URL);

            if (!string.IsNullOrEmpty(URL))
            {
                string url = URL;
                await _jsRuntime.InvokeVoidAsync("open", url, "_blank");
            }

            StateHasChanged();
            await Task.CompletedTask;
        }

        #endregion
        public void ShowSummery()
        {
            PageShowHide("job-summery-section2", "job-summery-section1");
            StateHasChanged();
        }
        protected override async Task OnParametersSetAsync()
        {
            
            base.OnParametersSetAsync();
        }



    }
}
