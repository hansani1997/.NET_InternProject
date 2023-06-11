
using BlueLotus360.CleanArchitecture.Application.Validators.MessageSystem;
using Microsoft.AspNetCore.Components;

namespace BlueLotus360.Com.UI.Definitions.MB.Shared.Components
{
    public partial class UserMessageDialog
    {
        [Parameter]
        public UserMessageManager Messages { get; set; } = new();

        [Parameter]
        public bool  MessageShown { get; set; }


        public void ShowUserMessageWindow()
        {
            MessageShown = true;
            StateHasChanged(); 
        }

        public void HideWindow()
        {
            MessageShown = false;
            StateHasChanged();
        }

    }
}
