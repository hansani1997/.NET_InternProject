using BL10.CleanArchitecture.Domain.Entities.WorkShopManagement;
using BlueLotus360.CleanArchitecture.Application.Validators.MessageSystem;
using BlueLotus360.CleanArchitecture.Application.Validators.Transaction;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL10.CleanArchitecture.Application.Validators.WorkShopManagement
{
    public class WorkShopValidator : IWorkShopValidator
    {
        private WorkOrder _work_order;
        public UserMessageManager UserMessages { get; set; }
        public WorkShopValidator(WorkOrder workOrd)
        {
            this._work_order = workOrd;
            UserMessages = new UserMessageManager();
        }

        public bool CanAddItemtoEstimate()
        {
            UserMessages.UserMessages.Clear();
            if (_work_order.WorkOrderSimpleEstimation.EstimatedMaterials.ToList().Exists(x=>x.TransactionItem.ItemKey==_work_order.SelectedOrderItem.TransactionItem.ItemKey && x.IsActive==1))
            {
                UserMessages.AddErrorMessage("You have already added this item");
            }
            if (_work_order.SelectedOrderItem.TransactionQuantity > _work_order.SelectedOrderItem.AvailableStock)
            {
                UserMessages.AddErrorMessage("Can't add morer than !" + _work_order.SelectedOrderItem.AvailableStock);
            }
            return UserMessages.UserMessages.Count == 0;
        }
        public bool CanCreateWorkOrder()
        {
            UserMessages.UserMessages.Clear();

            if (_work_order.OrderCategory1!=null && _work_order.OrderCategory1.CodeKey<10)
            {
                UserMessages.AddErrorMessage("Please Select Work Order Category !");
            }
            if (_work_order.OrderCategory2 != null && _work_order.OrderCategory2.CodeKey <10)
            {
                UserMessages.AddErrorMessage("Please Select Work Order Type !");
            }
            //if (_work_order.SelectedVehicle.PreviousMilage > _work_order.MeterReading)
            //{
            //    UserMessages.AddErrorMessage($"Current Milage can't be less than {_work_order.SelectedVehicle.PreviousMilage}!");
            //}

            if (_work_order.OrderCategory1.Code.Equals("Good Will Warranty"))
            {
                if (_work_order.PrincipalPercentage + _work_order.CompanyPercentage + _work_order.CustomerPrecentage != 100)
                {
                    UserMessages.AddErrorMessage("Total of precentages should be 100!");
                }
                if (_work_order.PrincipalPercentage > 0 && _work_order.BaringHeaderPrincipleAccount.AccountKey < 11)
                {
                    UserMessages.AddErrorMessage("Please enter Principle account!");
                }
                if (_work_order.CompanyPercentage > 0 && _work_order.BaringHeaderCompanyAccount.AccountKey < 11)
                {
                    UserMessages.AddErrorMessage("Please enter Carmart account!");
                }
                if (_work_order.CustomerPrecentage > 0 && _work_order.OrderAccount.AccountKey < 11)
                {
                    UserMessages.AddErrorMessage("Please enter Customer account!");
                }
                
            }

            return UserMessages.UserMessages.Count == 0;
        }

        public bool CanAddToGridItem() 
        {
            UserMessages.UserMessages.Clear();

            if (_work_order.SelectedOrderItem != null)
            {
                if (_work_order.SelectedOrderItem.IsMaterialItem)
                {
                    if (_work_order.SelectedOrderItem.TransactionItem.ItemKey < 11)
                    {
                        UserMessages.AddErrorMessage("Please select a material before add !");
                    }
                    if (_work_order.SelectedOrderItem.TransactionUnit.UnitKey < 11)
                    {
                        UserMessages.AddErrorMessage("Please select a material unit before add !");
                    }
                    if (_work_order.SelectedOrderItem.TransactionRate <= 0)
                    {
                        UserMessages.AddErrorMessage("Please enter rate!");
                    }
                    if (_work_order.SelectedOrderItem.TransactionQuantity <= 0)
                    {
                        UserMessages.AddErrorMessage("Please enter quantity!");
                    }
                    if (_work_order.SelectedOrderItem.TransactionQuantity > _work_order.SelectedOrderItem.AvailableStock)
                    {
                        UserMessages.AddErrorMessage("Can't add morer than !"+ _work_order.SelectedOrderItem.AvailableStock);
                    }
                    if (_work_order.WorkOrderMaterials.ToList().Exists(x => (x.TransactionItem.ItemKey == _work_order.SelectedOrderItem.TransactionItem.ItemKey) && !x.IsInEditMode))
                    {
                        UserMessages.AddErrorMessage("You have already added this item");
                    }

                    if (_work_order.OrderCategory1.Code.Equals("Good Will Warranty"))
                    {
                        if (_work_order.SelectedOrderItem.PrinciplePrecentage + _work_order.SelectedOrderItem.CompanyPrecentage + _work_order.SelectedOrderItem.CustomerPrecentage != 100)
                        {
                            UserMessages.AddErrorMessage("Total of precentages should be 100!");
                        }

                        if (_work_order.SelectedOrderItem.PrinciplePrecentage > 0 && _work_order.SelectedOrderItem.BaringPrinciple.AccountKey < 11)
                        {
                            UserMessages.AddErrorMessage("Please enter Principle account!");
                        }
                        if (_work_order.SelectedOrderItem.CompanyPrecentage > 0 && _work_order.SelectedOrderItem.BaringCompany.AccountKey < 11)
                        {
                            UserMessages.AddErrorMessage("Please enter Carmart account!");
                        }
                        if (_work_order.SelectedOrderItem.CustomerPrecentage > 0 && _work_order.SelectedOrderItem.BaringCustomer.AccountKey < 11)
                        {
                            UserMessages.AddErrorMessage("Please enter Customer account!");
                        }
                       
                    }
                }
                else if (_work_order.SelectedOrderItem.IsServiceItem)
                {

                    if (_work_order.SelectedOrderItem.TransactionItem.ItemKey < 11)
                    {
                        UserMessages.AddErrorMessage("Please select a service before add !");
                    }
                    if (_work_order.SelectedOrderItem.TransactionUnit.UnitKey < 11)
                    {
                        UserMessages.AddErrorMessage("Please select a service unit before add !");
                    }
                    if (_work_order.SelectedOrderItem.TransactionRate <= 0)
                    {
                        UserMessages.AddErrorMessage("Please enter rate!");
                    }
                    if (_work_order.SelectedOrderItem.TransactionQuantity == 0)
                    {
                        UserMessages.AddErrorMessage("Please enter time!");
                    }
                    if (_work_order.WorkOrderServices.ToList().Exists(x => (x.TransactionItem.ItemKey == _work_order.SelectedOrderItem.TransactionItem.ItemKey) && !x.IsInEditMode))
                    {
                        UserMessages.AddErrorMessage("You have already added this service");
                    }
                    if (_work_order.OrderCategory1.Code.Equals("Good Will Warranty"))
                    {
                            if (_work_order.SelectedOrderItem.PrinciplePrecentage + _work_order.SelectedOrderItem.CompanyPrecentage + _work_order.SelectedOrderItem.CustomerPrecentage != 100)
                            {
                                UserMessages.AddErrorMessage("Total of precentages should be 100!");
                            }

                            if (_work_order.SelectedOrderItem.PrinciplePrecentage > 0 && _work_order.SelectedOrderItem.BaringPrinciple.AccountKey < 11)
                            {
                                UserMessages.AddErrorMessage("Please enter Principle account!");
                            }
                            if (_work_order.SelectedOrderItem.CompanyPrecentage > 0 && _work_order.SelectedOrderItem.BaringCompany.AccountKey < 11)
                            {
                                UserMessages.AddErrorMessage("Please enter Carmart account!");
                            }
                            if (_work_order.SelectedOrderItem.CustomerPrecentage > 0 && _work_order.SelectedOrderItem.BaringCustomer.AccountKey < 11)
                            {
                                UserMessages.AddErrorMessage("Please enter Customer account!");
                            }
                       
                    }
                }
                else if (_work_order.SelectedOrderItem.IsNoteItem)
                {
                    if (_work_order.SelectedOrderItem.TransactionItem.ItemKey < 11)
                    {
                        UserMessages.AddErrorMessage("Please select a note before add !");
                    }
                    if (string.IsNullOrEmpty(_work_order.SelectedOrderItem.Description))
                    {
                        UserMessages.AddErrorMessage("Please add a description before add !");
                    }
                }
                else { }
            }
           

            return UserMessages.UserMessages.Count == 0;
        }

        public bool CanSaveWorkOrder()
        {
            UserMessages.UserMessages.Clear();

            if (_work_order.WorkOrderMaterials.Count()+ _work_order.WorkOrderServices.Count()+ _work_order.OtherServices.Count()== 0)
            {
                UserMessages.AddErrorMessage("Please add at least one item before save !");
            }
            if (_work_order.OrderStatus != null && _work_order.OrderStatus.Code.Equals("Closed") )
            {
                UserMessages.AddErrorMessage("This work order is already closed!");
            }
            foreach (var itm in _work_order.WorkOrderMaterials.Where(x=>x.IsActive==1 && x.IsJustAdded))
            {
                    if (itm.TransactionItem.ItemKey < 11)
                    {
                        UserMessages.AddErrorMessage("Null Item has been added,delete that before save!");
                    }
                    if (itm.TransactionUnit.UnitKey < 11)
                    {
                        UserMessages.AddErrorMessage("Unit is not defined for the item of "+itm.TransactionItem.ItemName+"!");
                    }
                    if (itm.TransactionRate <= 0)
                    {
                        UserMessages.AddErrorMessage("Please enter rate for item of "+itm.TransactionItem.ItemName+"!");
                    }
                    if (itm.TransactionQuantity <= 0)
                    {
                        UserMessages.AddErrorMessage("Please enter quantity for item of" + itm.TransactionItem.ItemName+"!");
                    }
                    if (itm.TransactionQuantity > itm.AvailableStock)
                    {
                        UserMessages.AddErrorMessage("Can't add morer than " + itm.AvailableStock+" for the item of "+itm.TransactionItem.ItemName);
                    }
                    if (_work_order.OrderCategory1.Code.Equals("Good Will Warranty"))
                    {
                        if (itm.PrinciplePrecentage + itm.CompanyPrecentage + itm.CustomerPrecentage != 100)
                        {
                            UserMessages.AddErrorMessage("Total of precentages should be 100!");
                        }

                        if (itm.PrinciplePrecentage > 0 && itm.BaringPrinciple.AccountKey < 11)
                        {
                            UserMessages.AddErrorMessage("Please enter Principle account!");
                        }
                        if (itm.CompanyPrecentage > 0 && itm.BaringCompany.AccountKey < 11)
                        {
                            UserMessages.AddErrorMessage("Please enter Carmart account!");
                        }
                        if (itm.CustomerPrecentage > 0 && itm.BaringCustomer.AccountKey < 11)
                        {
                            UserMessages.AddErrorMessage("Please enter Customer account!");
                        }

                    }


            }
            foreach (var itm in _work_order.WorkOrderServices.Where(x => x.IsActive == 1 && x.IsJustAdded))
            {

                    if (itm.TransactionItem.ItemKey < 11)
                    {
                        UserMessages.AddErrorMessage("Null service has been added,delete that before save!");
                    }
                    if (itm.TransactionUnit.UnitKey < 11)
                    {
                        UserMessages.AddErrorMessage("Unit is not defined for the service of " + itm.TransactionItem.ItemName + "!");
                    }
                    if (itm.TransactionRate <= 0)
                    {
                        UserMessages.AddErrorMessage("Please enter rate for service of " + itm.TransactionItem.ItemName + "!");
                    }
                    if (itm.TransactionQuantity <= 0)
                    {
                        UserMessages.AddErrorMessage("Please enter quantity for service of" + itm.TransactionItem.ItemName + "!");
                    }
                if (_work_order.OrderCategory1.Code.Equals("Good Will Warranty"))
                {
                    if (itm.PrinciplePrecentage + itm.CompanyPrecentage + itm.CustomerPrecentage != 100)
                    {
                        UserMessages.AddErrorMessage("Total of precentages should be 100!");
                    }

                    if (itm.PrinciplePrecentage > 0 && itm.BaringPrinciple.AccountKey < 11)
                    {
                        UserMessages.AddErrorMessage("Please enter Principle account!");
                    }
                    if (itm.CompanyPrecentage > 0 && itm.BaringCompany.AccountKey < 11)
                    {
                        UserMessages.AddErrorMessage("Please enter Carmart account!");
                    }
                    if (itm.CustomerPrecentage > 0 && itm.BaringCustomer.AccountKey < 11)
                    {
                        UserMessages.AddErrorMessage("Please enter Customer account!");
                    }

                }

            }
            foreach (var itm in _work_order.OtherServices.Where(x => x.IsActive == 1 && x.IsJustAdded))
            {
                if (itm.TransactionItem.ItemKey < 11)
                {
                    UserMessages.AddErrorMessage("Null service has been added,delete that before save!");
                }
                if (itm.TransactionUnit.UnitKey < 11)
                {
                    UserMessages.AddErrorMessage("Unit is not defined for the service of " + itm.TransactionItem.ItemName + "!");
                }
                if (itm.TransactionRate <= 0)
                {
                    UserMessages.AddErrorMessage("Please enter rate for service of " + itm.TransactionItem.ItemName + "!");
                }
                if (itm.TransactionQuantity <= 0)
                {
                    UserMessages.AddErrorMessage("Please enter quantity for service of" + itm.TransactionItem.ItemName + "!");
                }
            }
            
            return UserMessages.UserMessages.Count == 0;
        }

        public bool CanAddItemToGrid(decimal? TransactionQuantiy = null)
        {
            UserMessages.UserMessages.Clear();

            if (_work_order.WorkOrderTransaction.Location.CodeKey < 10 && _work_order.WorkOrderTransaction.Location.IsMust)
            {
                UserMessages.AddErrorMessage("Transaction Location is required ");
            }
            if (_work_order.WorkOrderTransaction.SelectedLineItem.TransactionItem.ItemKey < 11)
            {
                UserMessages.AddErrorMessage("Transaction Item  is required ");
            }

            if (_work_order.WorkOrderTransaction.SelectedLineItem.TransactionItem.ItemName == "")
            {
                UserMessages.AddErrorMessage("Transaction Item Name is required ");
            }

            if (_work_order.WorkOrderTransaction.SelectedLineItem.TransactionQuantity <= 0)
            {
                UserMessages.AddErrorMessage("Transaction Quantity Cannot be Zero or less");
            }

            if (_work_order.WorkOrderTransaction.SelectedLineItem.TransactionUnit.UnitKey < 11)
            {
                UserMessages.AddErrorMessage("Transaction Unit Cannot be empty");
            }

            if (TransactionQuantiy.HasValue)
            {
                if (_work_order.WorkOrderTransaction.SelectedLineItem.TransactionQuantity > TransactionQuantiy.Value)
                {
                    UserMessages.AddErrorMessage($"Cannot Add {_work_order.WorkOrderTransaction.SelectedLineItem.TransactionQuantity} as Available stock is {TransactionQuantiy.Value.ToString()}");

                }
            }


            return UserMessages.UserMessages.Count == 0;
        }
       
        public bool CanSaveTransaction()
        {
            UserMessages.UserMessages.Clear();

            if (_work_order.WorkOrderTransaction != null && !BaseResponse.IsValidData(_work_order.WorkOrderTransaction.Location))
            {
                UserMessages.AddErrorMessage("Please Select a location");
            }
            //if (_transaction != null && !BaseResponse.IsValidData(_transaction.Address))
            //{
            //    UserMessages.AddErrorMessage("Please Select a Customer");
            //}

            if (_work_order.WorkOrderTransaction != null && !BaseResponse.IsValidData(_work_order.WorkOrderTransaction.Account) && _work_order.WorkOrderTransaction.Account.IsMust)
            {
                UserMessages.AddErrorMessage("Please Select a Customer Account");
            }
            if (_work_order.WorkOrderTransaction != null && !BaseResponse.IsValidData(_work_order.WorkOrderTransaction.ContraAccount) && _work_order.WorkOrderTransaction.ContraAccount.IsMust)
            {
                UserMessages.AddErrorMessage("Please Select a Sales Account");
            }
            if (_work_order.WorkOrderTransaction != null && !BaseResponse.IsValidData(_work_order.WorkOrderTransaction.PaymentTerm) && !_work_order.OrderCategory1.Code.Equals("Free"))
            {
                UserMessages.AddErrorMessage("Please select a payement Term");
            }

            if (_work_order.WorkOrderTransaction.InvoiceLineItems.Count == 0)
            {
                UserMessages.AddErrorMessage("No Details Found");
            }

            return UserMessages.UserMessages.Count == 0;
        }

        public bool CanUpdateEstimateGrid()
        {
            UserMessages.UserMessages.Clear();

            if (_work_order.SelectedOrderItem.TransactionQuantity > _work_order.SelectedOrderItem.AvailableStock)
            {
                UserMessages.AddErrorMessage("Can't add morer than !" + _work_order.SelectedOrderItem.AvailableStock);
            }

            return UserMessages.UserMessages.Count == 0;
        }
    }
}
