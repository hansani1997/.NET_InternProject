using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using BL10.CleanArchitecture.Domain.Entities.WorkShopManagement;
using BlueLotus360.CleanArchitecture.Client.Infrastructure.Helpers;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using BlueLotus360.Com.UI.Definitions.Extensions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Telerik.DataSource.Extensions;
using static System.Formats.Asn1.AsnWriter;
using static Telerik.Blazor.ThemeConstants;


namespace BlueLotus360.Com.UI.Definitions.Pages.WorkshopManagement.WorkshopCarmart.Component
{
    public partial class AddCustomerComplaintPopup
    {
        [Parameter] public BLUIElement PopUI { get; set; }
        [Parameter]public EventCallback OnCloseButtonClick { get; set; }
        [Parameter] public bool WindowIsVisible { get; set; }
        [Parameter] public WorkOrder WorkOrder { get; set; }
        [Parameter] public EventCallback SuccessfullPopUpShow { get; set; }
        [Parameter] public EventCallback<IList<OrderItem>> RefreshComplains { get; set; }
        [Parameter] public IEnumerable<OrderItem> SelectedItems { get; set; } = Enumerable.Empty<OrderItem>();
        [Parameter] public long ObjectKey { get; set; }
        private IDictionary<string, EventCallback> InteractionLogic { get; set; }

        private IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }

        string Complain = "";
        bool HideMinMax { get; set; } = false;
        CustomerComplainSection cusComplainsSection = new CustomerComplainSection();
        MudMessageBox mbox { get; set; }
        protected override async Task OnParametersSetAsync()
        {
            ObjectHelpers= new Dictionary<string, IBLUIOperationHelper>();
            InteractionHelper helper = new InteractionHelper(this, PopUI);
            InteractionLogic = helper.GenerateEventCallbacks();

            WorkOrder.SelectedOrderItem = new OrderItem();
            await base.OnParametersSetAsync();
        }

        private async void OnCloseClick()
        {
            
                if (OnCloseButtonClick.HasDelegate)
                {
                    await OnCloseButtonClick.InvokeAsync();
                }
                //need to close project
            
        }

        
        private async void OnFinishClick()
        {
            WorkOrder.OrderItems.AddRange(WorkOrder.CustomerComplains);
            WorkOrder.OrderStatus = new CodeBaseResponse() { OurCode = "WIP" };
            
            WorkOrder.FormObjectKey = ObjectKey;

            if (WorkOrder.OrderKey==1)
            {
                await _workshopManager.SaveWorkOrder(WorkOrder);
            }
            else
            {
                await _workshopManager.EditWorkOrder(WorkOrder);
            }
            

            if (OnCloseButtonClick.HasDelegate)
            {
                await OnCloseButtonClick.InvokeAsync();
            }
            if (SuccessfullPopUpShow.HasDelegate)
            {
               await  SuccessfullPopUpShow.InvokeAsync();
            }
            if(RefreshComplains.HasDelegate){
                await RefreshComplains.InvokeAsync(WorkOrder.CustomerComplains);
            }
            
            
        }

        private async void OnNotesChange(UIInterectionArgs<ItemResponse> args)
        {
            WorkOrder.SelectedOrderItem.TransactionItem = args.DataObject;
            WorkOrder.SelectedOrderItem.LineNumber = WorkOrder.OrderItems.Count()+1;
            

            StateHasChanged();
        }
        private async void OnNotesDescriptionChange(UIInterectionArgs<string> args)
        {
            WorkOrder.SelectedOrderItem.Description = args.DataObject;
            StateHasChanged();
            await Task.CompletedTask;
        }
        private async void OnAddToGridNotesClick(UIInterectionArgs<object> args)
        {
            WorkOrder.SelectedOrderItem.BaringPrinciple = new AccountResponse();
            WorkOrder.SelectedOrderItem.BaringCompany = new AccountResponse();
            WorkOrder.CustomerComplains.Add(WorkOrder.SelectedOrderItem) ;
            WorkOrder.SelectedOrderItem = new OrderItem();


            await SetValue("Description","");
            StateHasChanged();
            await Task.CompletedTask;
        }
       
        
        #region Object Helpers

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
        private async Task SetDataSource(string name, object dataSource)
        {
            IBLUIOperationHelper helper;

            if (ObjectHelpers.TryGetValue(name, out helper))
            {
                await (helper as IBLServerDependentComponent).SetDataSource(dataSource);
                StateHasChanged();
            }
        }

        private void ToggleViisbility(string name, bool visible)
        {
            IBLUIOperationHelper helper;

            if (ObjectHelpers.TryGetValue(name, out helper))
            {
                helper.UpdateVisibility(visible);
                StateHasChanged();
            }
        }
        private async Task ReadData(string name, bool UseLocalStorage = false)
        {
            IBLUIOperationHelper helper;

            if (ObjectHelpers.TryGetValue(name, out helper))
            {
                await (helper as IBLServerDependentComponent).FetchData(UseLocalStorage);

                StateHasChanged();
            }
        }
        #endregion
    }
}
