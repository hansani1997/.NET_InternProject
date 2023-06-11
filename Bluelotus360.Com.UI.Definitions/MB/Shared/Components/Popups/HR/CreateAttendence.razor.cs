using BlueLotus360.CleanArchitecture.Application.Validators.HR;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain.Entities.HR;
using BlueLotus360.Com.UI.Definitions.Extensions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlueLotus360.Com.UI.Definitions.MB.Shared.Components.Popups.HR
{
    public partial class CreateAttendence
    {
        [CascadingParameter]
        MudDialogInstance MudDialog { get; set; }

        [Parameter]
        public BLUIElement ModalUIElement { get; set; }

        [Parameter]
        public IDictionary<string, EventCallback> InteractionLogics { get; set; }

        [Parameter]
        public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }

        [Parameter]
        public AddManualAdt AttendenceRequest { get; set; }

        [Parameter]
        public string ButtonName { get; set; }

        [Parameter]
        public string HeadingPopUp { get; set; }

        [Parameter]
        public IHRValidator Validaor { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        protected override void  OnAfterRender(bool firstRender)
        {
            if (!firstRender)
            {
                return;
            }
            else
            {
                
            }

        }


        MudMessageBox mbox;

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        public void AddAttendence()
        {
            if (Validaor.CanAddTimeToGrid())
            {
                MudDialog.Close(DialogResult.Ok(AttendenceRequest));
            }
            
        }


    }
}
