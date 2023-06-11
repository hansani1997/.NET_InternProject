using BlueLotus360.CleanArchitecture.Application.Validators.MessageSystem;
using BlueLotus360.CleanArchitecture.Application.Validators.SalesOrder;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using BlueLotus360.Com.UI.Definitions.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlueLotus360.Com.UI.Definitions.MB.Shared.Components.Dialogs
{
    public partial class OrderItemDialog
    {
        //[CascadingParameter]
        //MudDialogInstance MudDialog { get; set; }

        [Parameter]
        public OrderItem OrderItem { get; set; }

        [Parameter] public BLUIElement ModalUIElement { get; set; }
        [Parameter] public IDictionary<string, EventCallback> InteractionLogics { get; set; }

        [Parameter] public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }

        [Parameter] public CodeBaseResponse ParentLocation { get; set; }


        [Parameter]
        public IOrderValidator Validaor { get; set; }

        [Parameter]
        public string ButtonName { get; set; }

        [Parameter]
        public string HeadingPopUp { get; set; }

        [Parameter] public EventCallback LineItemEdit { get; set; }
        [Parameter] public EventCallback ClosePopUp { get; set; }
        [Parameter] public bool IsEditPopShown { get; set; }

        protected override async Task OnParametersSetAsync()
        {

            await base.OnParametersSetAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {

            //if (firstRender)
            //{
            //    RefreshComponent("LineLevelLocation");
            //}


        }

        protected override async Task OnInitializedAsync()
        {
            Validaor.UserMessages.UserMessages = new List<UserMessage>();

            await base.OnInitializedAsync();
        }


        MudMessageBox mbox;

        public void Cancel()
        {
            //MudDialog.Cancel();
            ClosePopUp.InvokeAsync(null);
        }

        public async void AddItem()
        {
            Validaor = new SalesOrderValidator(new Order() { OrderLocation = ParentLocation, SelectedOrderItem = OrderItem });
            if (Validaor != null)
            {
                if (Validaor.CanAddItemToGrid())
                {
                    //MudDialog.Close(DialogResult.Ok(OrderItem));
                    await LineItemEdit.InvokeAsync(null);
                }
                else
                {


                }
            }

        }

        public async void Refresh()
        {
            await Task.CompletedTask;
        }
        private void RefreshComponent(string name)
        {
            IBLUIOperationHelper helper;

            if (ObjectHelpers.TryGetValue(name, out helper))
            {
                helper.Refresh();
                StateHasChanged();
            }
        }

        private void ToggleViisbility(string name, bool visible)
        {
            IBLUIOperationHelper helper;

            if (ObjectHelpers.TryGetValue(name, out helper))
            {
                helper.UpdateVisibility(visible);
                this.StateHasChanged();
            }
        }



    }
}
