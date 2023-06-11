
using BlueLotus360.CleanArchitecture.Application.Validators.HR;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain.Entities.HR;
using BlueLotus360.Com.UI.Definitions.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlueLotus360.Com.UI.Definitions.MB.Shared.Components.Popups.HR
{
    public partial class AddEditAttendence
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
        public MultiAtnAnlysis_Response AttendenceDetails { get; set; }

        [Parameter]
        public string ButtonName { get; set; }

        [Parameter]
        public string HeadingPopUp { get; set; }

        [Parameter]
        public IHREditFormValidator Validaor { get; set; }

        public UpdateAttendence updateAttendence;

        protected override async Task OnInitializedAsync()
        {
            updateAttendence = new UpdateAttendence();
            updateAttendence.InDtm = AttendenceDetails.InDtm;
            updateAttendence.OutDtm = AttendenceDetails.OutDtm;
            updateAttendence.MultiAtnDetKy=AttendenceDetails.MultiAtnDetKy;
            updateAttendence.AtnDetKy = AttendenceDetails.AtnDetKy;

            
            await base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
            { 
                return;
            }
            else
            {
                await SetValue("InTime", updateAttendence.InDtm);
                await SetValue("OutTime", updateAttendence.OutDtm);
            }

        }


        MudMessageBox mbox;

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        public async void AddOrEditAttendence()
        {
            if (Validaor.CanEditTimeToGrid())
            {
                MudDialog.Close(DialogResult.Ok(updateAttendence));
            }
             
        }

        private async Task SetValue(string name, object value)
        {
            IBLUIOperationHelper helper;

            if (ObjectHelpers.TryGetValue(name, out helper))
            {
                await helper.SetValue(value);
                StateHasChanged();
                await Task.CompletedTask;
            }
        }
    }
}
