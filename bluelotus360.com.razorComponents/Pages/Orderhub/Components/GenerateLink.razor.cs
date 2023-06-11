using BlueLotus360.Com.Infrastructure.OrderPlatforms.Ubereats;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.razorComponents.Pages.Orderhub.Components
{
    public partial class GenerateLink
    {
        [Parameter]
        public EventCallback OnCloseButtonClick { get; set; }
        [Parameter] public bool WindowIsVisible { get; set; } = true;
        private DialogOptions dialogOptions = new() { CloseButton = true };
        private async void OnCloseClick()
        {
            if (OnCloseButtonClick.HasDelegate)
            {
                await OnCloseButtonClick.InvokeAsync();
            }

        }

        private async Task CopyLink()
        {
            UberProvisionHandler uber = new UberProvisionHandler(_apiManager, _orderManager, _addressManager);
            string Link = await uber.SetupProvision();
            try
            {
                await ClipboardService.WriteTextAsync(Link);
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("Link Copied to the Clipboard", Severity.Info);
                OnCloseClick();
            }
            catch (Exception ex)
            {
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("Can't Copy the Link. Please Try Again", Severity.Error);
                OnCloseClick();
            }
            StateHasChanged();
        }
    }
}
