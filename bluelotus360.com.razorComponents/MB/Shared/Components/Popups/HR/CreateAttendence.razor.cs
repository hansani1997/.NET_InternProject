using BlueLotus360.CleanArchitecture.Domain.Entities.HR;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using bluelotus360.com.razorComponents.Extensions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueLotus360.CleanArchitecture.Application.Validators.HR;

namespace bluelotus360.com.razorComponents.MB.Shared.Components.Popups.HR
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

        protected async override Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        protected override void OnAfterRender(bool firstRender)
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
