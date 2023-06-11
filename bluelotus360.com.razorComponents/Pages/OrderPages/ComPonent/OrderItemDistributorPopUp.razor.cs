using BlueLotus360.CleanArchitecture.Application.Validators.MessageSystem;
using BlueLotus360.CleanArchitecture.Application.Validators.SalesOrder;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bluelotus360.com.razorComponents.Extensions;

namespace bluelotus360.com.razorComponents.Pages.OrderPages.ComPonent
{
    public partial class OrderItemDistributorPopUp
    {
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

        [Parameter] public bool IsQuotation { get; set; }
        bool HideMinMax { get; set; } = false;
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
            Validaor = new SalesOrderValidatorV2(ModalUIElement,new Order() { OrderLocation = ParentLocation, SelectedOrderItem = OrderItem, IsFromQuotation = IsQuotation });
            if (Validaor != null)
            {
                if (Validaor.CanAddItemToGridGeneric())
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
