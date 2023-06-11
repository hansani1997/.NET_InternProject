using BL10.CleanArchitecture.Domain.Entities.WorkShopManagement;
using BlueLotus360.CleanArchitecture.Application.Validators.MessageSystem;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL10.CleanArchitecture.Application.Validators.WorkShopManagement
{
    public class InsuranceModuleValidator : IInsuranceModuleValidator
    {
        private WorkOrder _insurenceOrder;
        public UserMessageManager UserMessages { get; set; }
        public InsuranceModuleValidator(WorkOrder InsurenceOrder)
        {
            this._insurenceOrder = InsurenceOrder;
            UserMessages = new UserMessageManager();
        }
        public bool CanAddToGridServiceItem()
        {
            UserMessages.UserMessages.Clear();

            if (!(_insurenceOrder.SelectedServiceItem.TransactionItem.ItemKey > 1))
            {
                UserMessages.AddErrorMessage("Please Select Item !");
            }
            if (!(_insurenceOrder.SelectedServiceItem.AnalysisType4.CodeKey > 1))
            {
                UserMessages.AddErrorMessage("Please Select Category !");
            }

            int index = _insurenceOrder.OrderItems.ToList().FindIndex(x => x.TransactionItem.ItemKey == _insurenceOrder.SelectedServiceItem.TransactionItem.ItemKey);
            if (index != -1)
            {
                UserMessages.AddErrorMessage("Already added !");
            }

            return UserMessages.UserMessages.Count == 0;
        }
        public bool CanAddToGridMaterialItem()
        {
            UserMessages.UserMessages.Clear();

            if (!(_insurenceOrder.SelectedMaterialItem.TransactionItem.ItemKey > 1))
            {
                UserMessages.AddErrorMessage("Please Select Item !");
            }
            if (!(_insurenceOrder.SelectedMaterialItem.AnalysisType4.CodeKey > 1))
            {
                UserMessages.AddErrorMessage("Please Select Category !");
            }

            int index = _insurenceOrder.OrderItems.ToList().FindIndex(x => x.TransactionItem.ItemKey == _insurenceOrder.SelectedMaterialItem.TransactionItem.ItemKey);
            if (index != -1)
            {
                UserMessages.AddErrorMessage("Already added !");
            }

            return UserMessages.UserMessages.Count == 0;
        }
        public bool CanAddToGridMiscellaneousItem()
        {
            UserMessages.UserMessages.Clear();

            if (!(_insurenceOrder.SelectedMiscellaneousItem.TransactionItem.ItemKey > 1))
            {
                UserMessages.AddErrorMessage("Please Select Item !");
            }
            if (!(_insurenceOrder.SelectedMiscellaneousItem.AnalysisType4.CodeKey > 1))
            {
                UserMessages.AddErrorMessage("Please Select Category !");
            }

            int index = _insurenceOrder.OrderItems.ToList().FindIndex(x => x.TransactionItem.ItemKey == _insurenceOrder.SelectedMiscellaneousItem.TransactionItem.ItemKey);
            if (index != -1)
            {
                UserMessages.AddErrorMessage("Already added !");
            }

            return UserMessages.UserMessages.Count == 0;
        }
        public bool CanAddToGridMaterialItemInEstimate()
        {
            UserMessages.UserMessages.Clear();

            if (!(_insurenceOrder.SelectedOrderItem.TransactionItem.ItemKey > 1)) 
            {
                UserMessages.AddErrorMessage("Please Select Item !");
            }
            
            int index = _insurenceOrder.WorkOrderSimpleEstimation.EstimatedMaterials.ToList().FindIndex(x => x.TransactionItem.ItemKey == _insurenceOrder.SelectedOrderItem.TransactionItem.ItemKey);
            if (index != -1)
            {
                UserMessages.AddErrorMessage("Already added !");
            }
            
            return UserMessages.UserMessages.Count == 0;
        }
        public bool CanAddToGridServiceItemInEstimate()
        {
            UserMessages.UserMessages.Clear();

            if (!(_insurenceOrder.SelectedOrderItem.TransactionItem.ItemKey > 1))
            {
                UserMessages.AddErrorMessage("Please Select Item !");
            }
            int index = _insurenceOrder.WorkOrderSimpleEstimation.EstimatedMaterials.ToList().FindIndex(x => x.TransactionItem.ItemKey == _insurenceOrder.SelectedOrderItem.TransactionItem.ItemKey);
            if (index != -1)
            {
                UserMessages.AddErrorMessage("Already added !");
            }
            
            return UserMessages.UserMessages.Count == 0;
        }

        public bool CanCreateIRNOrder()
        {
            UserMessages.UserMessages.Clear();

            if (_insurenceOrder.OrderAccount != null && _insurenceOrder.OrderAccount.AccountKey < 10)
            {
                UserMessages.AddErrorMessage("Please Select Insurance !");
            }
            if (_insurenceOrder.OrderCategory2 != null && _insurenceOrder.OrderCategory2.CodeKey < 10)
            {
                UserMessages.AddErrorMessage("Please Select IRN Type !");
            }
            if (_insurenceOrder.MeterReading < _insurenceOrder.SelectedVehicle.PreviousMilage)
            {
                UserMessages.AddErrorMessage($"Current Milage can't be less than " + _insurenceOrder.SelectedVehicle.PreviousMilage);
            }

            return UserMessages.UserMessages.Count == 0;
        }

        public bool CanSaveWorkOrder()
        {
            throw new NotImplementedException();
        }

        public bool CanAddQtyInEstimate()
        {
            UserMessages.UserMessages.Clear();

            if (_insurenceOrder.EditingLineItem.AvailableStock < _insurenceOrder.EditingLineItem.TransactionQuantity)
            {
                UserMessages.AddErrorMessage("Available Quantity is " + ((int)_insurenceOrder.EditingLineItem.AvailableStock));
            }

            return UserMessages.UserMessages.Count == 0;
        }

        public bool CanSaveTransaction()
        {
            UserMessages.UserMessages.Clear();

            if (_insurenceOrder.WorkOrderTransaction != null && !BaseResponse.IsValidData(_insurenceOrder.WorkOrderTransaction.Location))
            {
                UserMessages.AddErrorMessage("Please Select a location");
            }   
            if (_insurenceOrder.WorkOrderTransaction != null && !BaseResponse.IsValidData(_insurenceOrder.WorkOrderTransaction.Account) && _insurenceOrder.WorkOrderTransaction.Account.IsMust)
            {
                UserMessages.AddErrorMessage("Please Select a Customer Account");
            }
            if (_insurenceOrder.WorkOrderTransaction != null && !BaseResponse.IsValidData(_insurenceOrder.WorkOrderTransaction.ContraAccount) && _insurenceOrder.WorkOrderTransaction.ContraAccount.IsMust)
            {
                UserMessages.AddErrorMessage("Please Select a Sales Account");
            }
            if (_insurenceOrder.WorkOrderTransaction != null && !BaseResponse.IsValidData(_insurenceOrder.WorkOrderTransaction.PaymentTerm))
            {
                UserMessages.AddErrorMessage("Please select a payement Term");
            }

            if (_insurenceOrder.WorkOrderTransaction.InvoiceLineItems.Count == 0)
            {
                UserMessages.AddErrorMessage("No Details Found");
            }

            return UserMessages.UserMessages.Count == 0;
        }
    }
}
