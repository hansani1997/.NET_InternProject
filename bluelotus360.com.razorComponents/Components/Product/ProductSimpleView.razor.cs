using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.razorComponents.Components.Product
{
    public partial class ProductSimpleView
    {

      
        protected override Task OnInitializedAsync()
        {
            var tranItem = _orderState.CurrentOrder.OrderItems.Where(x => x.TransactionItem.ItemKey == ItemRateResponse.RateItem.ItemKey).FirstOrDefault();
            if (tranItem != null)
            {
                thisProductQuantity = tranItem.TransactionQuantity;
            }
            return base.OnInitializedAsync();
        }
        [Parameter]
        public ItemRateResponse ItemRateResponse { get; set; }

        private decimal thisProductQuantity;

        private async Task TryAddProductToCart(ItemRateResponse response)
        {
            var tranItem = _orderState.CurrentOrder.OrderItems.Where(x => x.TransactionItem.ItemKey == response.RateItem.ItemKey).FirstOrDefault();
            if (tranItem == null)
            {
                OrderItem transactionLineItem = new OrderItem();
                transactionLineItem.Rate = response.Rate;
                transactionLineItem.TransactionRate = response.TransactionRate;
                transactionLineItem.TransactionUnit = new UnitResponse() { UnitKey = response.UnitKey };
                transactionLineItem.OrderLineLocation = _orderState.SelectedLocation;
                transactionLineItem.ItemTaxType1Per = response.ItemTaxType1;
                transactionLineItem.TransactionItem = new ItemResponse()
                {
                    ItemName = response.RateItem.ItemName,
                    ItemCode = response.RateItem.ItemCode,
                    ItemNameOnly = response.RateItem.ItemNameOnly,
                    ItemKey = response.RateItem.ItemKey,
                };

                tranItem = _orderState.CurrentOrder.CreateOrderItem(transactionLineItem);
                _orderState.CurrentOrder.AddGridItems(tranItem);
            }
            else
            {
                tranItem.TransactionQuantity++;
            }
            tranItem.CalculateLineBalance();
            thisProductQuantity = tranItem.TransactionQuantity;
            _taxCalculator.CalculateTaxes(tranItem);
            await _orderState.CalculateTotatls();
        }


        private async Task TryReduceItemFromCart(ItemRateResponse response)
        {
            var tranItem = _orderState.CurrentOrder.OrderItems.Where(x => x.TransactionItem.ItemKey == response.RateItem.ItemKey).FirstOrDefault();
            if (tranItem != null)
            {
                if (tranItem.TransactionQuantity > 0)
                {
                    tranItem.TransactionQuantity--;
                    thisProductQuantity = tranItem.TransactionQuantity;
                    tranItem.CalculateLineBalance();
                }
                else
                {
                    _orderState.CurrentOrder.OrderItems.Remove(tranItem);
                    thisProductQuantity = 0;
                }
              await  _orderState.CalculateTotatls();
                _taxCalculator.CalculateTaxes(tranItem);
            }
          
        }


        public async Task OnProductQuantityChange(decimal value)
        {
            var tranItem = _orderState.CurrentOrder.OrderItems.Where(x => x.TransactionItem.ItemKey == ItemRateResponse.RateItem.ItemKey).FirstOrDefault();
            if(tranItem != null)
            {
                if (value == 0)
                {
                    _orderState.CurrentOrder.OrderItems.Remove(tranItem);
                    thisProductQuantity = 0;
                    await _orderState.CalculateTotatls();
                    return;
                }
                 
                tranItem.TransactionQuantity= value;
                thisProductQuantity= value;
                await _orderState.CalculateTotatls();
                _taxCalculator.CalculateTaxes(tranItem);
                return;
               
            }

            if (value > 0)
            {
                OrderItem transactionLineItem = new OrderItem();
                transactionLineItem.Rate = ItemRateResponse.Rate;
                transactionLineItem.TransactionRate = ItemRateResponse.TransactionRate;
                transactionLineItem.TransactionUnit = new UnitResponse() { UnitKey = ItemRateResponse.UnitKey };
                transactionLineItem.ItemTaxType1Per = ItemRateResponse.ItemTaxType1;
                transactionLineItem.OrderLineLocation = _orderState.SelectedLocation;
              
                transactionLineItem.TransactionItem = new ItemResponse()
                {
                    ItemName = ItemRateResponse.RateItem.ItemName,
                    ItemCode = ItemRateResponse.RateItem.ItemCode,
                    ItemNameOnly = ItemRateResponse.RateItem.ItemNameOnly,
                    ItemKey = ItemRateResponse.RateItem.ItemKey,
                };
               
                tranItem = _orderState.CurrentOrder.CreateOrderItem(transactionLineItem);
                tranItem.TransactionQuantity= value;
                _orderState.CurrentOrder.AddGridItems(tranItem);
                tranItem.CalculateLineBalance();
                _taxCalculator.CalculateTaxes(tranItem);
                thisProductQuantity = value;
                await _orderState.CalculateTotatls();
            }
       
        }
    }
}
