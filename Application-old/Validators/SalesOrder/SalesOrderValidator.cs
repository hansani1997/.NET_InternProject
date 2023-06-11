using BlueLotus360.CleanArchitecture.Application.Validators.MessageSystem;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.CleanArchitecture.Application.Validators.SalesOrder
{
    public class SalesOrderValidator : IOrderValidator
    {

        private Order _order;
        public UserMessageManager UserMessages { get; set; }

        public bool CanAddItemToGrid()
        {
            UserMessages.UserMessages.Clear();
            if (_order.SelectedOrderItem.NeedToRequestFromAnotherLocation())
            {
                if (_order.SelectedOrderItem.OrderLineLocation.CodeKey == 1)
                {
                    UserMessages.AddErrorMessage("Please Select a  location for Item Requisition");
                }

                if (_order.OrderLocation.CodeKey == _order.SelectedOrderItem.OrderLineLocation.CodeKey)
                {
                    UserMessages.AddErrorMessage("Please Select a different location for Item Requisition");
                }

            }

            if (_order.SelectedOrderItem.TransactionQuantity <= 0)
            {
                UserMessages.AddErrorMessage("Transaction Quantity Cannot be Zero or less");
            }

            if (_order.SelectedOrderItem.TransactionUnit.UnitKey < 11)
            {
                UserMessages.AddErrorMessage("Transaction Unit Cannot be empty");
            }



            return UserMessages.UserMessages.Count == 0;
        }

        public bool CanRequestToAddItem()
        {
            throw new NotImplementedException();
        }

        public bool CanChangeHeaderInformatiom()
        {
            return _order.OrderItems.Count == 0;
        }

        public bool CanOrderSave()
        {
            UserMessages.UserMessages.Clear();


            if (_order.OrderLocation.CodeKey <= 1)
            {
                UserMessages.AddErrorMessage("Please Select Order Location");
            }

            if (_order.OrderCustomer.AddressKey <= 1)
            {
                UserMessages.AddErrorMessage("Please Select Customer");
            }

            if (_order.OrderRepAddress.AddressKey <= 1)
            {
                UserMessages.AddErrorMessage("Please Select Rep Address");
            }
            if (_order.OrderItems.Where(x => x.IsActive == 1).Count() <= 0)
            {
                UserMessages.AddErrorMessage("Please Add at least one item before save ");
            }
            return UserMessages.UserMessages.Count == 0;
        }

        public bool CanSelectItem()
        {
            UserMessages.UserMessages.Clear();


            if (_order.OrderLocation.CodeKey <= 1)
            {
                UserMessages.AddErrorMessage("Please Select Order Location");
            }

            if (_order.OrderCustomer.AddressKey <= 1)
            {
                UserMessages.AddErrorMessage("Please Select Customer");
            }

            if (_order.OrderRepAddress.AddressKey <= 1)
            {
                UserMessages.AddErrorMessage("Please Select Rep Address");
            }

            return UserMessages.UserMessages.Count == 0;
        }


        public SalesOrderValidator(Order order)
        {
            _order = order;
            UserMessages = new UserMessageManager();


        }
    }

    public class OrderValidatorV2 : IOrderValidator
    {
        private Order _order;
        public UserMessageManager UserMessages { get; set; }
        public IList<string> RequiredElements { get; set; }

        public bool CanSelectItem()
        {
            UserMessages.UserMessages.Clear();


            if (RequiredElements.Contains("HeaderSection1_Location") && !BaseResponse.IsValidData(_order.OrderLocation))
            {
                UserMessages.AddErrorMessage("Please Select Order Location");
            }

            if (RequiredElements.Contains("HeaderSection1_AddressCombo") && !BaseResponse.IsValidData(_order.OrderCustomer))
            {
                UserMessages.AddErrorMessage("Please Select Customer");
            }

            if (RequiredElements.Contains("HeaderSection1_RepCombo") && !BaseResponse.IsValidData(_order.OrderRepAddress))
            {
                UserMessages.AddErrorMessage("Please Select Rep Address");
            }

            return UserMessages.UserMessages.Count == 0;
        }

        public bool CanAddItemToGrid()
        {
            UserMessages.UserMessages.Clear();
            if (_order.SelectedOrderItem.NeedToRequestFromAnotherLocation())
            {
                UserMessages.AddErrorMessage("Insufficient Quantity is in the current location,Can't proceed");

                if (BaseResponse.IsValidData(_order.SelectedOrderItem.OrderLineLocation))
                {
                    UserMessages.AddErrorMessage("Please Select a  location for Item Requisition");
                }

                if (_order.OrderLocation.CodeKey == _order.SelectedOrderItem.OrderLineLocation.CodeKey)
                {
                    UserMessages.AddErrorMessage("Please Select a different location for Item Requisition");
                }

            }

            if (_order.SelectedOrderItem.TransactionQuantity <= 0)
            {
                UserMessages.AddErrorMessage("Transaction Quantity Cannot be Zero or less");
            }

            if (_order.SelectedOrderItem.TransactionUnit.UnitKey < 11)
            {
                UserMessages.AddErrorMessage("Transaction Unit Cannot be empty");
            }
            if (RequiredElements.Contains("HeaderSection2_DisPer") && _order.SelectedOrderItem.DiscountPercentage == 0)
            {
                UserMessages.AddErrorMessage("Transaction Discount Percentage is 0,can't proceed");
            }
            if (RequiredElements.Contains("HeaderSection2_ItmTaxTyp1Per") && _order.SelectedOrderItem.ItemTaxType1Per == 0)
            {
                UserMessages.AddErrorMessage("Transaction Vat Percentage is 0 ,can't proceed");
            }
            if (RequiredElements.Contains("HeaderSection2_ItmTaxTyp4Per") && _order.SelectedOrderItem.ItemTaxType4Per == 0)
            {
                UserMessages.AddErrorMessage("Transaction SVat Percentage is 0 ,can't proceed");
            }


            return UserMessages.UserMessages.Count == 0;
        }

        public bool CanRequestToAddItem()
        {
            throw new NotImplementedException();
        }

        public bool CanChangeHeaderInformatiom()
        {
            throw new NotImplementedException();
        }

        public bool CanOrderSave()
        {
            throw new NotImplementedException();
        }

        public OrderValidatorV2(BLUIElement uielement, Order order)
        {
            _order = order;
            RequiredElements = uielement.IsMustElements;
            UserMessages = new UserMessageManager();
        }
    }
}

