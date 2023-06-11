using BlueLotus360.CleanArchitecture.Application.Responses.ServerResponse;
using BlueLotus360.CleanArchitecture.Application.Validators.MasterData;
using BlueLotus360.CleanArchitecture.Client.Infrastructure.Helpers;
using BlueLotus360.CleanArchitecture.Domain.DTO.MasterData;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.Com.UI.Definitions.Extensions;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlueLotus360.Com.UI.Definitions.MB.Shared.Components.Popups.MasterDetailPopup
{
    public partial class AddNewAdvAnlPopup
    {
        [Parameter]
        public BLUIElement InitiatorElement { get; set; }

        [Parameter]
        public EventCallback<AddAdvAnl> OnAddressCreated { get; set; }


        AddAdvAnl addressMaster = new AddAdvAnl();
        private bool IsPopUpShown = false;
        IList<ServerMessage> Messages;
        IAddressMasterValidator validator;

        public IDictionary<string, EventCallback> InteractionLogics { get; set; }


        public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }




        //protected override async Task OnParametersSetAsync()
        //{
        //    addressMaster = new AddAdvAnl();
        //    validator = new AddressMasterValidator(addressMaster);
        //    Messages = new List<ServerMessage>();
        //    if (InitiatorElement != null && InitiatorElement.Children.Count == 0)
        //    {
        //        var formrequest = new ObjectFormRequest();
        //        formrequest.MenuKey = InitiatorElement.ReferenceElementKey;
        //        InitiatorElement.Children = (await _navManger.GetMenuUIElement(formrequest)).Children;//get ui elements
        //        InteractionHelper helper = new InteractionHelper(this, InitiatorElement);//formdefinition has all form objects 
        //        InteractionLogics = helper.GenerateEventCallbacks();//
        //        StateHasChanged();
        //    }

        //    await base.OnParametersSetAsync();
        //}



        //public override async Task SetParametersAsync(ParameterView parameters)
        //{

        //    await base.SetParametersAsync(parameters);
        //}


        //public void ShowPopUp()
        //{
        //    IsPopUpShown = true;
        //    StateHasChanged();
        //}


        //public void HidePopUp()
        //{
        //    IsPopUpShown = false;
        //    StateHasChanged();
        //}
    }
}
