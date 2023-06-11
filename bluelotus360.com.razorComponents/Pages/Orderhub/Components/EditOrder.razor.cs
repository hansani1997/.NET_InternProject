using BL10.CleanArchitecture.Domain.Entities;
using BlueLotus.Com.Domain.PartnerEntity;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using bluelotus360.Com.commonLib.Helpers;
using BlueLotus360.Com.Infrastructure.OrderPlatforms.Ubereats;
using bluelotus360.com.razorComponents.Extensions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bluelotus360.com.razorComponents.MB.Shared.Components;

namespace bluelotus360.com.razorComponents.Pages.Orderhub.Components
{
    public partial class EditOrder
    {
        [Parameter]
        public EventCallback OnCloseButtonClick { get; set; }
        [Parameter] public bool WindowIsVisible { get; set; } = true;
        [Parameter] public BLUIElement blElement { get; set; }
        [Parameter] public BLUIElement EditSection { get; set; }
        [Parameter] public BLUIElement EditGrid { get; set; }
        [Parameter] public int LocationKey { get; set; }
        public IDictionary<string, EventCallback> InteractionLogics { get; set; }
        public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }

        [Parameter] public IList<PartnerOrderDetails> _Order { get; set; }
        [Parameter] public PartnerOrder selectedOrder { get; set; }

        public BLTable<PartnerOrderDetails> GridRef;
        private decimal subTotal = 0;
        private decimal FinalTotal = 0;
        private PartnerOrderDetails SelectedItem;
        private PartnerOrderDetails EditedItem;
        private bool IsAmendOrder = false;
        private DialogOptions dialogOptions = new() { CloseButton = true };
        public EditOrder()
        {
            _Order = new List<PartnerOrderDetails>();
            selectedOrder = new PartnerOrder();
            SelectedItem = new PartnerOrderDetails();
            EditedItem = new PartnerOrderDetails();
            GridRef = new BLTable<PartnerOrderDetails>();
        }
        protected override async Task OnParametersSetAsync()
        {
            if (blElement != null)
            {
                ObjectHelpers = new Dictionary<string, IBLUIOperationHelper>();
                InteractionHelper helper = new InteractionHelper(this, blElement);
                InteractionLogics = helper.GenerateEventCallbacks();//
                StateHasChanged();
            }
            await base.OnParametersSetAsync();
        }

        protected override async Task OnInitializedAsync()
        {
            EditSection = blElement.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("OrderAmendment")).FirstOrDefault();
            EditGrid = blElement.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("EditOrderTable")).FirstOrDefault();
        }

        private async void OnCloseClick()
        {
            if (OnCloseButtonClick.HasDelegate)
            {
                await OnCloseButtonClick.InvokeAsync();
            }

        }

        //private void OnRowRenderHandler(GridRowRenderEventArgs args)
        //{
        //    PartnerOrderDetails item = args.Item as PartnerOrderDetails;
        //    if (item.SpecialInstructions == "Unavailable")
        //    {
        //        args.Class = "UnavailableRowFormatting";
        //    }
        //}

        public void GetSingleOrder(PartnerOrder Order)
        {
            selectedOrder = Order;
            _Order = selectedOrder.OrderItemDetails.Where(x => x.OrderItem.ItemType.CodeName != "SERVICE").ToList();

            PartnerOrderDetails deliveryCharge = selectedOrder.OrderItemDetails.Where(x => x.OrderItem.ItemType.CodeName == "SERVICE").FirstOrDefault();
            if (deliveryCharge != null)
            {
                selectedOrder.DeliveryCharges = deliveryCharge.TransactionPrice;
                subTotal = _Order.Sum(x => x.BaseTotalPrice);
                selectedOrder.TotalWithDiscount = subTotal - selectedOrder.DiscountAmount;
                FinalTotal = selectedOrder.TotalWithDiscount + selectedOrder.DeliveryCharges;
            }
            else
            {
                selectedOrder.DeliveryCharges = 0;
                subTotal = _Order.Sum(x => x.BaseTotalPrice);
                selectedOrder.TotalWithDiscount = subTotal - selectedOrder.DiscountAmount;
                FinalTotal = selectedOrder.TotalWithDiscount;
            }
        }

        private async void ItemOnChange(UIInterectionArgs<ItemResponse> args)
        {
            SelectedItem.OrderItem = args.DataObject;
            StateHasChanged();
        }

        private async void QtyOnChange(UIInterectionArgs<decimal> args)
        {
            SelectedItem.ItemQuantity = args.DataObject;
            StateHasChanged();
        }

        private async void AddToGrid(UIInterectionArgs<object> args)
        {
            if (SelectedItem != null && SelectedItem.OrderItem.ItemKey > 11 && SelectedItem.ItemQuantity > 0)
            {
                //if (SelectedItem.isEditModeOn)
                //{
                //    if (EditedItem != null)
                //    {
                //        SelectedItem.isEditModeOn = false;
                //        SelectedItem.BaseTotalPrice = SelectedItem.ItemQuantity * SelectedItem.TransactionPrice;
                //        EditedItem.CopyFrom(SelectedItem);

                //        SelectedItem = new PartnerOrderDetails();
                //    }
                //}
                //else
                //{
                SelectedItem.PartnerOrderDetailsId = 1;
                SelectedItem.TransactionPrice = await GetRateByItmKy(Convert.ToInt32(SelectedItem.OrderItem.ItemKey));
                SelectedItem.BaseTotalPrice = SelectedItem.ItemQuantity * SelectedItem.TransactionPrice;
                SelectedItem.OrderItem.LineNumber = selectedOrder.OrderItemDetails.Count + 1;
                selectedOrder.OrderItemDetails.Add(SelectedItem);
                SelectedItem = new PartnerOrderDetails();
                //}

               // GridRef.Rebind();
                StateHasChanged();
            }
        }


        private async void AmendOrderData(UIInterectionArgs<object> args)
        {
            appStateService.IsLoaded = true;

            PartnerOrder partnerOrder = new PartnerOrder();
            partnerOrder.CopyFrom(selectedOrder);
            partnerOrder.OrderItemDetails = new();
            foreach (PartnerOrderDetails item in selectedOrder.OrderItemDetails)
            {
                if (item.PartnerOrderDetailsId == 1)
                {
                    partnerOrder.OrderItemDetails.Add(item);
                }
            }
            partnerOrder.IsActive = 1;
            partnerOrder.IsApproved = 1;
            partnerOrder.Amount = selectedOrder.OrderItemDetails.Sum(x => x.BaseTotalPrice);
            bool data = await _orderManager.UnmappedSKUUpdate(partnerOrder);
            appStateService.IsLoaded = false;
            if (data)
            {
                OnCloseClick();
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("Updated Successfully", Severity.Success);
            }
            else
            {
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("Something went wrong", Severity.Error);
            }


        }

        //private async void EditSingleLineItem(PartnerOrderDetails partnerOrderDetails)
        //{
        //    partnerOrderDetails.isEditModeOn = true;
        //    EditedItem = new();
        //    EditedItem = partnerOrderDetails;
        //    SelectedItem.CopyFrom(partnerOrderDetails);

        //    StateHasChanged();
        //    await Task.CompletedTask;
        //}
        private async void DeleteSingleLineItem(UIInterectionArgs<object> args)
        {
            PartnerOrderDetails partnerOrderDetails = args.DataObject as PartnerOrderDetails;
            List<FulfillmentIssue> fulfillment_issues = new List<FulfillmentIssue>();
            if (selectedOrder.Platforms.AccountName == "PickMe")
            {
                bool? result = await _dialogService.ShowMessageBox(
               "Warning",
               $"Do you want to remove Item",
               yesText: "Delete!", cancelText: "Cancel");
                if (result.HasValue && result.Value)
                {
                    appStateService.IsLoaded = true;
                    RequestParameters request = new RequestParameters()
                    {
                        OrderKey = Convert.ToInt32(partnerOrderDetails.PartnerOrderDetailsId)
                    };
                    bool success = await _orderManager.OrderItem_DeleteWeb(request);
                    appStateService.IsLoaded = false;
                    if (success)
                    {
                        selectedOrder.OrderItemDetails.RemoveAll(r => r.PartnerOrderDetailsId == Convert.ToInt32(partnerOrderDetails.PartnerOrderDetailsId));
                        StateHasChanged();
                        _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                        _snackBar.Add("Item has been deleted", Severity.Success);

                    }
                    else
                    {
                        _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                        _snackBar.Add("Something went wrong", Severity.Error);
                    }

                   // GridRef.Rebind();
                    StateHasChanged();
                }

            }
            else
            {
                bool cartupdate = false;
                bool? resultx = await _dialogService.ShowMessageBox(
               "Warning",
               $"Do you want to Update Uber Cart?",
               yesText: "Yes!", cancelText: "No");

                if (resultx.HasValue && resultx.Value)
                {
                    appStateService.IsLoaded = true;
                    UberOrderHandler uberOrderHandler = new UberOrderHandler(_apiManager, _orderManager);
                    UberOrder uberOrder = await uberOrderHandler.GetUberDetailsByOrderID(selectedOrder.OrderId);
                    if (uberOrder != null && uberOrder.Cart.Items.Count > 0)
                    {
                        foreach (UberItem item in uberOrder.Cart.Items)
                        {
                            if (item.Id == partnerOrderDetails.OrderItem.ItemCode)
                            {
                                FulfillmentIssue fulfillmentIssue = new FulfillmentIssue();
                                fulfillmentIssue.fulfillment_issue_type = "OUT_OF_ITEM";
                                fulfillmentIssue.fulfillment_action_type = "REMOVE_ITEM";
                                fulfillmentIssue.root_item.instance_id = item.Instance_id;
                                fulfillment_issues.Add(fulfillmentIssue);
                                UpdateCart cart = new UpdateCart();
                                cart.fulfillment_issues = fulfillment_issues;
                                cartupdate = await uberOrderHandler.UpdateUberCart(cart, selectedOrder.OrderId);
                            }
                        }

                        RequestParameters request = new RequestParameters()
                        {
                            OrderKey = Convert.ToInt32(partnerOrderDetails.PartnerOrderDetailsId)
                        };
                        bool success = await _orderManager.OrderItem_DeleteWeb(request);
                        appStateService.IsLoaded = false;
                        if (success && cartupdate)
                        {
                            selectedOrder.OrderItemDetails.RemoveAll(r => r.PartnerOrderDetailsId == Convert.ToInt32(partnerOrderDetails.PartnerOrderDetailsId));
                            StateHasChanged();
                            _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                            _snackBar.Add("Item has been deleted", Severity.Success);

                        }
                        else
                        {
                            _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                            _snackBar.Add("Something went wrong", Severity.Error);
                        }

                       // GridRef.Rebind();
                        StateHasChanged();
                    }
                    appStateService.IsLoaded = false;
                }

                else
                {
                    bool? resulty = await _dialogService.ShowMessageBox(
               "Warning",
               $"Do you want to remove Item",
               yesText: "Delete!", cancelText: "Cancel");

                    if (resulty.HasValue && resulty.Value)
                    {
                        appStateService.IsLoaded = true;
                        RequestParameters request = new RequestParameters()
                        {
                            OrderKey = Convert.ToInt32(partnerOrderDetails.PartnerOrderDetailsId)
                        };
                        bool success = await _orderManager.OrderItem_DeleteWeb(request);
                        appStateService.IsLoaded = false;
                        if (success)
                        {
                            selectedOrder.OrderItemDetails.RemoveAll(r => r.PartnerOrderDetailsId == Convert.ToInt32(partnerOrderDetails.PartnerOrderDetailsId));
                            StateHasChanged();
                            _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                            _snackBar.Add("Item has been deleted", Severity.Success);

                        }
                        else
                        {
                            _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                            _snackBar.Add("Something went wrong", Severity.Error);
                        }

                       // GridRef.Rebind();
                        StateHasChanged();
                    }
                }

            }



        }



        private void SaveSingleLineItem(PartnerOrderDetails partnerOrderDetails)
        {

        }

        private async Task<decimal> GetRateByItmKy(int ItemKey)
        {
            RequestParameters request = new RequestParameters()
            {
                LocationKey = LocationKey,
                ItemKey = ItemKey
            };
            decimal rate = await _orderManager.GetOrderHubItemRateByItemKy(request);
            return rate;
        }

        private async void OnOptionChanged(UIInterectionArgs<bool> args)
        {
            IsAmendOrder = args.DataObject;
        }


    }
}
